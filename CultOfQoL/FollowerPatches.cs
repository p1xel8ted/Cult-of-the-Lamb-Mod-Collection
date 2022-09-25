﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using UnityEngine;

namespace CultOfQoL;

public static class FollowerPatches
{
    private static Follower _follower;

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
        follower.Brain.Stats.Inspired = true;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, delegate
        {
            Plugin.L($"Adding Adoration thoughts to {follower.name}");
            follower.Brain.AddThought(Thought.DancedWithLeader, forced: true);
            instance.eventListener.PlayFollowerVO(instance.bowVO);
            CultFaithManager.AddThought(Thought.Cult_Inspire, follower.Brain.Info.ID, 1f, null);
            if (instance.follower.Brain.Stats.Adoration >= instance.follower.Brain.Stats.MAX_ADORATION)
            {
                instance.follower = follower;
                follower.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, null, false));
            }
        });
        Plugin.L($"Resetting {follower.name} and sending to next task.");
        follower.Dropped();
        follower.ResetStateAnimations();
        follower.Brain.ContinueToNextTask();
    }

    private static IEnumerator ExtortMoneyRoutine(Follower follower, Interaction instance)
    {
        follower.Brain.Stats.PaidTithes = true;
        var position = follower.transform.position;
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
        yield return new WaitForSeconds(0.2f);
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
    }

    private static IEnumerator GiveRewards(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {
        yield return DanceRoutine(follower, instance, previousTaskType);
        //  yield return new WaitForSeconds(5f);
        if (follower == instance.follower) yield break;
        if (follower.Brain.Stats.Adoration >= follower.Brain.Stats.MAX_ADORATION)
        {
            Plugin.L($"Adoration >= Max adoration for {follower.name}. Beginning reward process.");
            follower.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, delegate
            {
                follower.Brain.Stats.Adoration = 0f;
                var info = follower.Brain.Info;
                var xplevel = info.XPLevel;
                info.XPLevel = xplevel + 1;
                var speedUpSequenceMultiplier = 0.75f;
                follower.AdorationUI.BarController.ShrinkBarToEmpty(2f * speedUpSequenceMultiplier);
                follower.Dropped();
                follower.ResetStateAnimations();
                follower.Brain.ContinueToNextTask();
            }, false));
        }
    }


    [HarmonyPatch(typeof(interaction_FollowerInteraction))]
    [HarmonyWrapSafe]
    public static class InteractionFollowerInteraction
    {

        [HarmonyPatch("OnFollowerCommandFinalized")]
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
        {
            if (!Plugin.BulkInspireAndExtort.Value) return;

            if (followerCommands[0] == FollowerCommands.ExtortMoney)
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.PaidTithes))
                {
                    switch (follower.Brain.CurrentTask)
                    {
                        case FollowerTask_Sleep:
                        case FollowerTask_Dissent:
                        case FollowerTask_Imprisoned:
                        case FollowerTask_Bathroom:
                        case FollowerTask_OnMissionary:
                            continue;
                        default:
                            follower.StartCoroutine(ExtortMoneyRoutine(follower, __instance));
                            break;
                    }
                }


            if (followerCommands[0] == FollowerCommands.Dance)
            {
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.Inspired))
                {
                    switch (follower.Brain.CurrentTask)
                    {
                        case FollowerTask_Bathroom:
                        case FollowerTask_Sleep:
                        case FollowerTask_Dissent:
                        case FollowerTask_Imprisoned:
                        case FollowerTask_OnMissionary:
                            continue;
                        default:
                            __instance.StartCoroutine(GiveRewards(follower, __instance, __instance.follower.Brain.CurrentTask.Type));
                            break;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GiveDiscipleRewardRoutine))]
    [HarmonyWrapSafe]
    public static class GiveDiscipleRewardRoutinePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref interaction_FollowerInteraction __instance)
        {
            if (!Plugin.CleanseIllnessAndExhaustionOnLevelUp.Value) return;
            if (__instance.follower.Brain.Stats.Exhaustion > 0)
            {
                __instance.follower.Brain._directInfoAccess.Exhaustion = 0f;
                __instance.follower.Brain.Stats.Exhaustion = 0f;
                var onExhaustionStateChanged = FollowerBrainStats.OnExhaustionStateChanged;
                onExhaustionStateChanged?.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
                Plugin.L($"Resetting follower {__instance.follower.name} from exhaustion!");
            }

            if (__instance.follower.Brain.Stats.Illness > 0)
            {
                __instance.follower.Brain._directInfoAccess.Illness = 0f;
                __instance.follower.Brain.Stats.Illness = 0f;
                var onIllnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
                onIllnessStateChanged.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
                Plugin.L($"Resetting follower {__instance.follower.name} from illness!");
            }
        }
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands), typeof(Follower))]
    [HarmonyWrapSafe]
    public static class FollowerCommandGroupsOldAgeCommands
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CommandItem> __result)
        {
            if (!Plugin.CollectTitheFromOldFollowers.Value) return;
            if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes)) __result.Add(FollowerCommandItems.Extort());
        }
    }
}