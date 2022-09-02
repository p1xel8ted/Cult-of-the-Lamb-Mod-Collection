using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using src.Extensions;
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
    
    private static IEnumerator ExtortMoneyRoutine(Follower follower, Interaction instance)
    {
        follower.Brain.Stats.PaidTithes = true;
        var position = follower.transform.position;
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
        yield return new WaitForSeconds(0.2f);
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
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
            {
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.PaidTithes))
                {
                    follower.StartCoroutine(ExtortMoneyRoutine(follower, __instance));
                }
            }
            
            if (followerCommands[0] == FollowerCommands.Dance)
            {
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.Inspired))
                {
                    __instance.StartCoroutine(DanceRoutine(follower, __instance, __instance.follower.Brain.CurrentTask.Type));
                    
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
    [HarmonyWrapSafe]
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