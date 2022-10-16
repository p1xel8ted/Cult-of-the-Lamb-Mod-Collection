using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using UnityEngine;

namespace CultOfQoL;

[HarmonyPatch]
public static class FollowerPatches
{
    private enum CommandType
    {
        Dance,
        ExtortMoney,
        Intimidate,
        Bless,
        Bribe
    }

    private static IEnumerator GiveRewards(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType, CommandType type)
    {
        switch (type)
        {
            case CommandType.ExtortMoney:
                yield return Extort.ExtortMoneyRoutine(follower, instance);
                break;
            case CommandType.Bless:
                yield return Bless.BlessRoutine(follower, instance, previousTaskType);
                break;
            case CommandType.Dance:
                yield return Dance.DanceRoutine(follower, instance, previousTaskType);
                break;
            case CommandType.Intimidate:
                yield return Intimidate.IntimidateRoutine(follower, instance, previousTaskType);
                break;
            case CommandType.Bribe:
                yield return Bribe.BribeRoutine(follower, instance, previousTaskType);
                break;
        }

        // yield return new WaitForSeconds(5f);
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
            if (!Plugin.BulkFollowerCommands.Value) return;

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
                            __instance.StartCoroutine(Extort.ExtortMoneyRoutine(follower, __instance));
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
                            __instance.StartCoroutine(GiveRewards(follower, __instance, __instance.follower.Brain.CurrentTask.Type, CommandType.Dance));
                            break;
                    }
                }
            }

            if (followerCommands[0] == FollowerCommands.Intimidate)
            {
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.Intimidated))
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
                            __instance.StartCoroutine(GiveRewards(follower, __instance, __instance.follower.Brain.CurrentTask.Type, CommandType.Intimidate));
                            break;
                    }
                }
            }

            if (followerCommands[0] == FollowerCommands.Bless)
            {
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.BlessedToday))
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
                            __instance.StartCoroutine(GiveRewards(follower, __instance, __instance.follower.Brain.CurrentTask.Type, CommandType.Bless));
                            break;
                    }
                }
            }

            if (followerCommands[0] == FollowerCommands.Bribe)
            {
                foreach (var follower in Follower.Followers.Where(follower => !follower.Brain.Stats.Bribed))
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
                            __instance.StartCoroutine(GiveRewards(follower, __instance, __instance.follower.Brain.CurrentTask.Type, CommandType.Bribe));
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