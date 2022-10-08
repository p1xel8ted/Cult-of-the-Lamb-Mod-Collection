using HarmonyLib;
using COTL_API.CustomInventory;
using src.UI.Menus;
using UnityEngine;

namespace Rebirth;

[HarmonyPatch]
public static class MissionBoard
{
    private static bool _chanceTaken;
    private static bool _unlockedThisPhase;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(TimeManager __instance, ref DayPhase phase)
    {
        _chanceTaken = false;
        _unlockedThisPhase = false;
        Plugin.Log.LogWarning($"Resetting Rebirth Mission Chance & Unlock To False.");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionInfoCard), nameof(MissionInfoCard.Start))]
    public static void MissionInfoCard_Start(ref MissionInfoCard __instance)
    {
        if (_unlockedThisPhase)
        {
            Plugin.Log.LogWarning($"Token Mission Already Unlocked This Phase.");
            AddMission(ref __instance);
        }
        else
        {
            if (_chanceTaken)
            {
                Plugin.Log.LogWarning($"Chance already taken this phase.");
                return;
            }
            _chanceTaken = true;
            if (CustomItemManager.DropLoot(Plugin.RebirthItemInstance))
            {
                AddMission(ref __instance);
            }
        }
    }

    private static void AddMission(ref MissionInfoCard __instance)
    {
        var mission = __instance._missionButtons.RandomElement();
        mission._type = Plugin.RebirthItem;
        mission._titleText.text = "Rebirth Tokens";
        _unlockedThisPhase = true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetRewardRange))]
    public static void MissionInfoCard_Start(ref IntRange __result, InventoryItem.ITEM_TYPE type)
    {
        if (type == Plugin.RebirthItem)
        {
            __result = new IntRange(15, 25);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetChance))]
    public static void MissionInfoCard_Start(ref float __result, InventoryItem.ITEM_TYPE type, FollowerInfo followerInfo, StructureBrain.TYPES missionaryType)
    {
        var baseChanceMultiplier = MissionaryManager.GetBaseChanceMultiplier(type, followerInfo);
        var random = new System.Random((int) (followerInfo.ID + type));
        if (type == Plugin.RebirthItem)
        {
            __result = Mathf.Clamp((MissionaryManager.BaseChance_Bones + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * baseChanceMultiplier, 0f, 0.95f);
        }
    }
}