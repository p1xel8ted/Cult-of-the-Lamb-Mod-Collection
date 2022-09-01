using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CultOfQoL;

public static class FollowerPatches
{
    private static IEnumerator DanceRoutine(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {
        Plugin.Log.LogInfo($"Follower: {follower.name}, Instance follower: {instance.follower.name}");
        var position = PlayerFarming.Instance.transform.position;
        follower.FacePosition(position);
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.State.CURRENT_STATE = StateMachine.State.Dancing;
        yield return null;
        PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "dance", true);
        follower.SetBodyAnimation("dance", true);
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", position);
        AudioManager.Instance.SetFollowersDance(1f);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.SetFollowersDance(0f);
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", instance.follower.transform.position);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, delegate
        {
            Plugin.Log.LogInfo($"Adding Adoration thoughts to {follower.name}");
            follower.Brain.AddThought(Thought.DancedWithLeader, forced:true);
            instance.eventListener.PlayFollowerVO(instance.bowVO);
            CultFaithManager.AddThought(Thought.Cult_Inspire, follower.Brain.Info.ID, 1f, Array.Empty<string>());
            if (instance.follower.Brain.Stats.Adoration >= instance.follower.Brain.Stats.MAX_ADORATION)
            {
                Plugin.Log.LogInfo($"Adoration >= Max adoration for {follower.name}. Beginning reward process.");
                instance.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, null, false));
            }
        });
        Plugin.Log.LogInfo($"Resetting {follower.name} and sending to next task.");
        follower.Dropped();
        follower.ResetStateAnimations();
        follower.Brain.ContinueToNextTask();
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction))]
    public static class DanceRoutinePatch
    {
        [HarmonyPatch("DanceRoutine")]
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance, ref string ___CacheAnimation,
            ref bool ___CacheLoop,
            ref SimpleSpineAnimator ___CacheSpineAnimator,
            ref float ___CacheFacing,
            ref float ___CacheAnimationProgress)
        {
            if (!Plugin.BulkInspireAndExtort.Value) return;

            var originalFollower = __instance.follower;
            foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.Inspired))
            {
                // if (DataManager.Instance.Followers_Recruit.Contains(FollowerInfo.GetInfoByID(follower.Brain.Info.ID)))
                //     continue;

                __instance.follower = follower;
                __instance.follower.Brain.Stats.Inspired = true;
                // if (follower == originalFollower)
                // {
                //     //GameManager.GetInstance().StartCoroutine(nameof(interaction_FollowerInteraction.DanceRoutine));
                //     __instance.StartCoroutine(nameof(interaction_FollowerInteraction.DanceRoutine));
                // }
                // else
                // {
                    GameManager.GetInstance().StartCoroutine(DanceRoutine(follower, __instance, __instance.follower.Brain.CurrentTask.Type));
                   // __instance.StartCoroutine(DanceRoutine(follower, __instance, __instance.follower.Brain.CurrentTask.Type));
                // }

                __instance.eventListener.PlayFollowerVO(
                    "event:/dialogue/followers/general_acknowledge");
                AudioManager.Instance.PlayOneShot("event:/followers/pop_in",
                    __instance.follower.transform.position);
                Plugin.Log.LogMessage($"Inspired {__instance.follower.name}");

                ___CacheSpineAnimator.enabled = true;
                __instance.follower.SetBodyAnimation(___CacheAnimation, ___CacheLoop);
                __instance.follower.Spine.Skeleton.ScaleX = ___CacheFacing;
                __instance.follower.Spine.AnimationState.GetCurrent(1).TrackTime = ___CacheAnimationProgress;
                if (__instance.follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
                {
                    __instance.follower.Brain.CompleteCurrentTask();
                }
            }

            __instance.follower = originalFollower;
        }
    }

    private static IEnumerator ExtortMoneyRoutine(Follower follower, interaction_FollowerInteraction instance)
    {
        var position = follower.transform.position;
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
        yield return new WaitForSeconds(0.2f);
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction))]
    public static class InteractionFollowerInteraction
    {
        [HarmonyPatch("OnFollowerCommandFinalized")]
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance, ref string ___CacheAnimation,
            ref bool ___CacheLoop,
            ref SimpleSpineAnimator ___CacheSpineAnimator,
            ref float ___CacheFacing,
            ref float ___CacheAnimationProgress,
            params FollowerCommands[] followerCommands)
        {
            if (!Plugin.BulkInspireAndExtort.Value) return;
            var originalFollower = __instance.follower;
            var followerCommands2 = followerCommands[0];

            switch (followerCommands2)
            {
                case FollowerCommands.ExtortMoney:

                    foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.PaidTithes))
                    {
                        // if (DataManager.Instance.Followers_Recruit.Contains(FollowerInfo.GetInfoByID(follower.Brain.Info.ID)))
                        //     continue;

                        __instance.follower = follower;
                        __instance.follower.Brain.Stats.PaidTithes = true;
                        GameManager.GetInstance().StartCoroutine(ExtortMoneyRoutine(follower, __instance));
                       // __instance.StartCoroutine(ExtortMoneyRoutine(follower, __instance));

                        __instance.eventListener.PlayFollowerVO(
                            "event:/dialogue/followers/general_acknowledge");
                        AudioManager.Instance.PlayOneShot("event:/followers/pop_in",
                            __instance.follower.transform.position);
                        Plugin.Log.LogMessage($"Collected tithe from {__instance.follower.name}");

                        ___CacheSpineAnimator.enabled = true;
                        __instance.follower.SetBodyAnimation(___CacheAnimation, ___CacheLoop);
                        __instance.follower.Spine.Skeleton.ScaleX = ___CacheFacing;
                        __instance.follower.Spine.AnimationState.GetCurrent(1).TrackTime = ___CacheAnimationProgress;
                        if (__instance.follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
                        {
                            __instance.follower.Brain.CompleteCurrentTask();
                        }
                    }

                    break;
            }

            __instance.follower = originalFollower;
        }
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GiveDiscipleRewardRoutine))]
    public static class GiveDiscipleRewardRoutinePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance)
        {
            if (!Plugin.CleanseIllnessAndExhaustionOnLevelUp.Value) return;
            if (__instance.follower.Brain.Stats.Exhaustion > 0)
            {
                __instance.follower.Brain.Stats.Exhaustion = 0;
                Plugin.Log.LogMessage($"Resetting follower {__instance.follower.name} from exhaustion!");
            }

            if (__instance.follower.Brain.Stats.Illness > 0)
            {
                __instance.follower.Brain.Stats.Illness = 0f;
                Plugin.Log.LogMessage($"Resetting follower {__instance.follower.name} from illness!");
            }
        }
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands), typeof(Follower))]
    public static class FollowerCommandGroupsOldAgeCommands
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CommandItem> __result)
        {
            if (!Plugin.CollectTitheFromOldFollowers.Value) return;
            if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
            {
                __result.Add(FollowerCommandItems.Extort());
            }
        }
    }
    
}