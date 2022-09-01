using System.Collections;
using System.IO;
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using I2.Loc;
using UnityEngine;

namespace Rebirth
{
    internal class RebirthFollowerCommand : CustomFollowerCommand
    {
        public override string InternalName => "REBIRTH_COMMAND";

        public override Sprite CommandIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));

        public override string GetTitle(Follower follower)
        {
            return "Rebirth";
        }

        public RebirthFollowerCommand()
        {
            SubCommands = FollowerCommandGroups.AreYouSureCommands();
        }

        public override string GetDescription(Follower follower)
        {
            return "Tired of looking at this sheep? Order a rebirth!";
        }

        public override string GetLockedDescription(Follower follower)
        {
            return IsOld(follower) ? "Not enough life essence left to satisfy those below." : $"{follower.name} has already been born again!";
        }

        private static bool IsOld(Follower follower) 
        {
            Plugin.Log.LogWarning($"Follower null: {follower == null}");
            return follower.Outfit.CurrentOutfit == FollowerOutfitType.Old || follower.Brain.Info.OldAge || follower.Brain.HasThought(Thought.OldAge);
        }

        public override bool ShouldAppearFor(Follower follower)
        {
            Plugin.Log.LogWarning($"Follower task: {follower.Brain.CurrentTask.Type}");
            Plugin.Log.LogWarning($"Follower outfit: {follower.Outfit.CurrentOutfit}");
            Plugin.Log.LogWarning($"Follower old: {follower.Brain.Info.OldAge}");
            Plugin.Log.LogWarning($"Follower age: {follower.Brain.Info.Age}");
      
            return !IsOld(follower);
        }

        private IEnumerator SpawnRecruit(Follower follower)
        {
            var name = follower.name;
            var oldId = follower.Brain.Info.ID;
            follower.Die(deathNotificationType:NotificationCentre.NotificationType.None, force:true);
            FollowerManager.ConsumeFollower(follower.Brain.Info.ID);
            var newFollower = FollowerManager.CreateNewRecruit(FollowerLocation.Base, "",NotificationCentre.NotificationType.NewRecruit);
            yield return new WaitForSeconds(5f);
            NotificationCentre.Instance.PlayGenericNotification($"{name} died to be reborn as {newFollower.Name}! All hail {newFollower.Name}!");
            RemoveFromDeadLists(oldId);
        }

        //this is stop being able to resurrect a born-again follower
        private static void RemoveFromDeadLists(int id)
        {
            for (var i = 0; i < DataManager.Instance.Followers_Dead.Count; i++)
            {
                if (DataManager.Instance.Followers_Dead[i].ID != id) continue;
                DataManager.Instance.Followers_Dead.RemoveAt(i);
                DataManager.Instance.Followers_Dead_IDs.RemoveAt(i);
            }
        }

        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            if (finalCommand == FollowerCommands.AreYouSureYes)
            {
                GameManager.GetInstance().StartCoroutine(SpawnRecruit(interaction.follower));
            }
        }
    }
}