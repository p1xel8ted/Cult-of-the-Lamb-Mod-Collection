using System.Collections;
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

            TooOld = IsOld(follower);
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
        
        private static IEnumerator GiveFollowerIE(FollowerInfo f, Follower old)
        {
            yield return DieRoutine(old);
            yield return new WaitForSeconds(3f);
            BiomeBaseManager.Instance.SpawnExistingRecruits = false;
            yield return new WaitForEndOfFrame();
           // GameManager.GetInstance().OnConversationNew(false, false, false);
            //GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation);
            yield return new WaitForSeconds(3f);
            DataManager.Instance.Followers_Recruit.Add(f);
            FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
            UnityEngine.Object.FindObjectOfType<FollowerRecruit>().ManualTriggerAnimateIn();
            BiomeBaseManager.Instance.SpawnExistingRecruits = true;
            yield return new WaitForSeconds(2f);
            //GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation);
            //yield return new WaitForSeconds(1f);
            //GameManager.GetInstance().OnConversationEnd();
        }


        private static void SpawnRecruit(Follower follower)
        {
            BiomeBaseManager.Instance.SpawnExistingRecruits = true;
            NotificationCentre.NotificationsEnabled = false;
            var name = follower.name;
            var oldId = follower.Brain.Info.ID;
            var newXp = Mathf.CeilToInt(follower.Brain.Info.XPLevel / 2f);
          // // follower.Die(NotificationCentre.NotificationType.None, force: true);
          //  GameManager.GetInstance().StartCoroutine(DieRoutine(follower));
            var fi = FollowerInfo.NewCharacter(FollowerLocation.Base);
           
            if (fi != null)
            {
                GameManager.GetInstance().StartCoroutine(GiveFollowerIE(fi, follower));
                Plugin.Log.LogWarning($"New follower: {fi.Name}");
                var bornAgainFollower = new SaveData.BornAgainFollowerData(fi, true);
                SaveData.SetBornAgainFollowerData(bornAgainFollower);
                bornAgainFollower.FollowerInfo.XPLevel = newXp;
            }
            else
            {
                Plugin.Log.LogWarning($"New follower is null!");
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
        
        private static IEnumerator DieRoutine(Follower follower)
        {
            follower.HideAllFollowerIcons();
            yield return new WaitForSeconds(0.5f);
      
            follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        
            // yield return new WaitForSeconds(0.1f);
            // follower.SetOutfit(FollowerOutfitType.Old, false, Thought.None);
            yield return new WaitForSeconds(1f);
            follower.SetBodyAnimation("wave", true);
            yield return new WaitForSeconds(0.75f);
      //      yield return new WaitForSeconds(1.8f);
            //follower.AddBodyAnimation("idle", false, 0f);
           
            //follower.State.CURRENT_STATE = StateMachine.State.Idle;
            
            follower.Die(NotificationCentre.NotificationType.None, force: true);
            yield break;
        }


        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            if (finalCommand == FollowerCommands.AreYouSureYes)
            {
                SpawnRecruit(interaction.follower);
                //interaction.Close(true, true);
            }
        }

        // interaction.Close(true, true);
    }
}