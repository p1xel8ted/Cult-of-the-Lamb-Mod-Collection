using System.IO;
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
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
            if (TooOld)
            {
                return "Not enough life essence left to satisfy those below.";
            }

            if (BornAgain)
            {
                return $"{follower.name} has already been born again!";

            }

            return string.Empty;
        }

        private static bool TooOld { get; set; }
        private static bool BornAgain { get; set; }

        public override bool IsAvailable(Follower follower)
        {
            BornAgainFollower = SaveData.GetBornAgainFollowerData(follower.Brain._directInfoAccess);

            TooOld= IsOld(follower);
            BornAgain = BornAgainFollower is {BornAgain: true};
            
            if (TooOld)
            {
                return false;
            }

            return !BornAgain;
        }
        
        private static SaveData.BornAgainFollowerData BornAgainFollower { get; set; }
        private static bool IsOld(Follower follower)
        {
            return follower.Outfit.CurrentOutfit == FollowerOutfitType.Old && (follower.Brain.Info.OldAge || follower.Brain.HasThought(Thought.OldAge));
        }

        private static void SpawnRecruit(Follower follower)
        {
            NotificationCentre.NotificationsEnabled = false;
            var name = follower.name;
            var oldId = follower.Brain.Info.ID;
            follower.Die(NotificationCentre.NotificationType.None, force: true);


            FollowerManager.CreateNewRecruit(FollowerLocation.Base, NotificationCentre.NotificationType.None);
            var newFollower = DataManager.Instance.Followers_Recruit.LastElement();
            if (newFollower != null)
            {
                Plugin.Log.LogWarning($"New follower: {newFollower.Name}");
                var bornAgainFollower = new SaveData.BornAgainFollowerData(newFollower, true);
                SaveData.SetBornAgainFollowerData(bornAgainFollower);
            }

            NotificationCentre.NotificationsEnabled = true;
            NotificationCentreScreen.Play($"{name} died to be reborn! All hail {name}!");
            RemoveFromDeadLists(oldId);
        }

        //this is stop being able to resurrect the old dead body of a born-again follower
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
                SpawnRecruit(interaction.follower);
            }

            // interaction.Close(true, true);
        }
    }
}