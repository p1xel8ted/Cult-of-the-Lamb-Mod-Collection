using HarmonyLib;
using System;
using System.CodeDom;
using System.Collections;
using Spine.Unity.Examples;
using UnityEngine;

namespace CultOfTheLambMods;

public static class FollowerPatches
{
    private static IEnumerator DanceRoutine(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {
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
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", follower.transform.position);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower, -1);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, delegate ()
        {
            follower.Brain.AddThought(Thought.DancedWithLeader, true, true);
            instance.eventListener.PlayFollowerVO(instance.bowVO);
            CultFaithManager.AddThought(Thought.Cult_Inspire, -1, 1f, Array.Empty<string>());
            if (follower.Brain.Stats.Adoration >= follower.Brain.Stats.MAX_ADORATION)
            {
                instance.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, null, false));
            }
        });
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction))]
    public static class DanceRoutinePatch
    {
        [HarmonyPatch("DanceRoutine")]
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance, ref FollowerTaskType ___previousTaskType, ref string ___CacheAnimation,
            ref bool ___CacheLoop,
            ref SimpleSpineAnimator ___CacheSpineAnimator,
            ref float ___CacheFacing,
            ref float ___CacheAnimationProgress)
        {
            if (!Plugin.BulkInspireAndExtort.Value) return;

            var originalFollower = __instance.follower;
            foreach (var follower in Follower.Followers)
            {

                if (!follower.Brain.Stats.Inspired)
                {
                    __instance.follower = follower;
                    __instance.follower.Brain.Stats.Inspired = true;
                    __instance.StartCoroutine(DanceRoutine(follower, __instance, ___previousTaskType));
                    __instance.eventListener.PlayFollowerVO(
                        "event:/dialogue/followers/general_acknowledge");
                    AudioManager.Instance.PlayOneShot("event:/followers/pop_in",
                        __instance.follower.transform.position);
                    Debug.Log($"[CultOfQoL-OnFollowerCommandFinalized]: Inspired {__instance.follower.name}");

                    ___CacheSpineAnimator.enabled = true;
                    __instance.follower.SetBodyAnimation(___CacheAnimation, ___CacheLoop);
                    __instance.follower.Spine.Skeleton.ScaleX = ___CacheFacing;
                    __instance.follower.Spine.AnimationState.GetCurrent(1).TrackTime = ___CacheAnimationProgress;
                    //if (__instance.follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
                    //{
                    //    __instance.follower.Brain.CompleteCurrentTask();
                    //}
                }
            }

            __instance.follower = originalFollower;
            //AccessTools.Method(typeof(interaction_FollowerInteraction), "Close").Invoke(__instance, null);
            //__instance.DestroyAll();

            // 

        }
    }

    private static IEnumerator ExtortMoneyRoutine(Follower follower, interaction_FollowerInteraction instance)
    {
        ResourceCustomTarget.Create(instance.state.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate ()
        {
            Inventory.AddItem(20, 1, false);
        }, true);
        yield return new WaitForSeconds(0.2f);
        ResourceCustomTarget.Create(instance.state.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate ()
        {
            Inventory.AddItem(20, 1, false);
        }, true);
        yield break;
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction))]
    public static class InteractionFollowerInteraction
    {
        [HarmonyPatch("OnFollowerCommandFinalized")]
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance, ref FollowerTaskType ___previousTaskType, ref string ___CacheAnimation,
            ref bool ___CacheLoop,
            ref SimpleSpineAnimator ___CacheSpineAnimator,
            ref float ___CacheFacing,
            ref float ___CacheAnimationProgress,
            params FollowerCommands[] followerCommands)
        {
            if (!Plugin.BulkInspireAndExtort.Value) return;
            var originalFollower = __instance.follower;
            var followerCommands2 = followerCommands[0];
            // var instance = __instance;
            switch (followerCommands2)
            {
                case FollowerCommands.ExtortMoney:

                    foreach (var follower in Follower.Followers)
                    {
                        
                        if (!follower.Brain.Stats.PaidTithes)
                        {
                            __instance.follower = follower;
                            __instance.follower.Brain.Stats.PaidTithes = true;
                            __instance.StartCoroutine(ExtortMoneyRoutine(follower, __instance));
                            
                            __instance.eventListener.PlayFollowerVO(
                                "event:/dialogue/followers/general_acknowledge");
                            AudioManager.Instance.PlayOneShot("event:/followers/pop_in",
                                __instance.follower.transform.position);
                            Debug.Log($"[CultOfQoL-OnFollowerCommandFinalized]: Extorted money from {__instance.follower.name}");

                            ___CacheSpineAnimator.enabled = true;
                            __instance.follower.SetBodyAnimation(___CacheAnimation, ___CacheLoop);
                            __instance.follower.Spine.Skeleton.ScaleX = ___CacheFacing;
                            __instance.follower.Spine.AnimationState.GetCurrent(1).TrackTime = ___CacheAnimationProgress;
                            if (__instance.follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
                            {
                                __instance.follower.Brain.CompleteCurrentTask();
                            }
                        }
                    }

                    break;

            }

            __instance.follower = originalFollower;
            //AccessTools.Method(typeof(interaction_FollowerInteraction), "Close").Invoke(__instance, null);
            //__instance.DestroyAll();

            // AccessTools.Method(typeof(interaction_FollowerInteraction), "Close").Invoke(__instance, null);
        }
    }
}