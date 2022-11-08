using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using Lamb.UI.FollowerSelect;
using UnityEngine;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class HealingBay
{
    private static FollowerBrainInfo _fi;
    private static readonly bool Run = Plugin.AddExhaustedToHealingBay.Value;
    private static bool _isHealingBay;

    private static IEnumerator StartHeal(FollowerBrainInfo followerBrain)
    {
        if (!Run) yield break;
        followerBrain._info.Exhaustion = 0f;
        followerBrain._brain.Stats.Exhaustion = 0f;
        var onExhaustionStateChanged2 = FollowerBrainStats.OnExhaustionStateChanged;
        onExhaustionStateChanged2?.Invoke(followerBrain._info.ID, FollowerStatState.Off, FollowerStatState.On);
        yield return new WaitForSeconds(1);
        NotificationCentre.Instance.PlayGenericNotification($"{followerBrain._info.Name} is no longer exhausted!");
        _fi = null;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIFollowerSelectMenuController), nameof(UIFollowerSelectMenuController.Show),
        typeof(List<FollowerInfo>),
        typeof(List<FollowerInfo>),
        typeof(bool),
        typeof(UpgradeSystem.Type),
        typeof(bool),
        typeof(bool),
        typeof(bool)
    )]
    public static void UIFollowerSelectMenuController_Show(ref List<FollowerInfo> blackList)
    {
        if (!Run) return;
        Plugin.L($"IsHealingBay: {_isHealingBay}");
        if (!_isHealingBay) return;
        blackList.RemoveAll(follower => follower.CursedState is Thought.TiredFromMissionary or Thought.TiredFromMissionaryHappy or Thought.TiredFromMissionaryScared ||
                                        follower.Exhaustion > 0);
    }


    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnInteract))]
    public static class InteractionHealingBayOnInteractPatches
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (!Run) return;
            _isHealingBay = true;
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (!Run) return;
            _isHealingBay = false;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.HealingRoutine))]
    public static void Interaction_HealingBay_HealingRoutine(ref Follower follower)
    {
        if (!Run) return;
        if (follower.Brain._directInfoAccess.CursedState is Thought.TiredFromMissionary or Thought.TiredFromMissionaryHappy or Thought.TiredFromMissionaryScared ||
            follower.Brain._directInfoAccess.Exhaustion > 0f)
            _fi = follower.Brain.Info;
    }


    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayFollowerNotification))]
    public static class NotificationCentrePlayFollowerNotificationPatches
    {
        [HarmonyPrefix]
        public static bool Prefix(ref NotificationCentre.NotificationType type, ref FollowerBrainInfo info, ref bool __state)
        {
            if (!Run) return true;
            Plugin.L($"{type.ToString()} : {info.Name}");
            if (type == NotificationCentre.NotificationType.NoLongerIll && info == _fi)
            {
                Plugin.L("Skipping main method!");
                __state = true;
                return false;
            }

            Plugin.L("NOT skipping main method!");
            __state = false;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(ref FollowerBrainInfo info, ref bool __state)
        {
            if (!Run) return;
            if (!__state) return;
            Plugin.L("Running postfix");

            GameManager.GetInstance().StartCoroutine(StartHeal(info));
        }
    }
}