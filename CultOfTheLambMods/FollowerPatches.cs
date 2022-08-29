using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CultOfTheLambMods;

public static class FollowerPatches
{
    private static IEnumerator DanceRoutine(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {

        Plugin.Log.LogInfo($"[CultOfQoL]: Follower: {follower.name}, Instance follower: {instance.follower.name}");
        follower.FacePosition(PlayerFarming.Instance.transform.position);
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.State.CURRENT_STATE = StateMachine.State.Dancing;
        yield return null;
        PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "dance", true);
        follower.SetBodyAnimation("dance", true);
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
        AudioManager.Instance.SetFollowersDance(1f);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.SetFollowersDance(0f);
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", instance.follower.transform.position);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, delegate ()
        {
            Plugin.Log.LogInfo($"[CultOfQoL]: Adding Adoration thoughts to {follower.name}");
            follower.Brain.AddThought(Thought.DancedWithLeader, true, true);
            instance.eventListener.PlayFollowerVO(instance.bowVO);
            CultFaithManager.AddThought(Thought.Cult_Inspire, -1, 1f, Array.Empty<string>());
            if (instance.follower.Brain.Stats.Adoration >= instance.follower.Brain.Stats.MAX_ADORATION)
            {
                Plugin.Log.LogInfo($"[CultOfQoL]: Adoration >= Max adoration for {follower.name}. Beginning reward process.");
                instance.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, null, false));
            }
        });
        Plugin.Log.LogInfo($"[CultOfQoL]: Resetting {follower.name} and sending to next task.");
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
                __instance.follower = follower;
                __instance.follower.Brain.Stats.Inspired = true;
                if (follower == originalFollower)
                {
                    __instance.StartCoroutine("DanceRoutine");
                }
                else
                {
                    __instance.StartCoroutine(DanceRoutine(follower, __instance, __instance.follower.Brain.CurrentTask.Type));
                }
                
                __instance.eventListener.PlayFollowerVO(
                    "event:/dialogue/followers/general_acknowledge");
                AudioManager.Instance.PlayOneShot("event:/followers/pop_in",
                    __instance.follower.transform.position);
                Plugin.Log.LogInfo($"[CultOfQoL]: Inspired {__instance.follower.name}");

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
        ResourceCustomTarget.Create(instance.state.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate ()
        {
            Inventory.AddItem(20, 1);
        });
        yield return new WaitForSeconds(0.2f);
        ResourceCustomTarget.Create(instance.state.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate ()
        {
            Inventory.AddItem(20, 1);
        });
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
                        __instance.follower = follower;
                        __instance.follower.Brain.Stats.PaidTithes = true;
                        __instance.StartCoroutine(ExtortMoneyRoutine(follower, __instance));

                        __instance.eventListener.PlayFollowerVO(
                            "event:/dialogue/followers/general_acknowledge");
                        AudioManager.Instance.PlayOneShot("event:/followers/pop_in",
                            __instance.follower.transform.position);
                        Plugin.Log.LogInfo($"[CultOfQoL]: Extorted money from {__instance.follower.name}");

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
}