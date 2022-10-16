using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Lamb.UI;
using Rewired.Data;
using src.UI.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
public static class MissionBoard
{
    private static readonly IntRange TokenRange = new(15, 25);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetReward))]
    public static bool GetReward(ref InventoryItem.ITEM_TYPE type, ref float chance, ref int followerID, ref InventoryItem[] __result)
    {
        if (type != Plugin.RebirthItem) return true;
        var num = Random.Range(0f, 1f);
        foreach (var objective in DataManager.Instance.CompletedObjectives)
        {
            if (objective.Follower != followerID) continue;
            chance = float.MaxValue;
            break;
        }

        if (chance > num)
        {
            __result = new[] {new InventoryItem(Plugin.RebirthItem, TokenRange.Random())};
        }

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetRewardRange))]
    public static void MissionaryManager_GetRewardRange(ref IntRange __result, InventoryItem.ITEM_TYPE type)
    {
        if (type == Plugin.RebirthItem)
        {
            __result = TokenRange;
        }
    }

    private static readonly Dictionary<int, MissionButton> MissionButtons = new();


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionInfoCard), nameof(MissionInfoCard.Start))]
    public static void MissionInfoCard_MissionInfoCard(ref MissionInfoCard __instance)
    {
        // var contentParent = GameObject.Find("Missionary Menu(Clone)/FollowerSelectContainer/Right/CardContainer/Mission Follower Info/Transform/Content/Copy");
        var content = GameObject.Find("Missionary Menu(Clone)/FollowerSelectContainer/Right/CardContainer/Mission Follower Info/Transform/Content/Copy/Options");

        var newParent = new GameObject();
        var parent = content.transform.parent;
        newParent.transform.SetParent(parent);
        var scroll = newParent.AddComponent<ScrollRect>();

        scroll.viewport = parent.GetComponent<RectTransform>();

        content.transform.SetParent(newParent.transform);
        scroll.content = content.GetComponent<RectTransform>();
        scroll.vertical = true;
        scroll.horizontal = true;
        scroll.verticalScrollbar = newParent.AddComponent<Scrollbar>();
        scroll.movementType = ScrollRect.MovementType.Clamped;
        // scroll.SetLayoutVertical();
        //scroll.

        // var scroll = contentParent.AddComponent<ScrollRect>();
        // var scrollBar = new GameObject();
        // var bar = scrollBar.AddComponent<Scrollbar>();
        // scroll.content = content.GetComponent<RectTransform>();
        // scroll.verticalScrollbar = bar;

        //
        // foreach (var content in __instance.GetComponentsInChildren<Component>().Where(a => a.name == "Content"))
        // {
        //     var scroll = content.gameObject.AddComponent<ScrollRect>();
        //     scroll.transform.SetParent(content.transform);
        //
        //     scroll.content = content.GetComponentInChildren<RectTransform>();
        // }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionInfoCard), nameof(MissionInfoCard.Configure))]
    public static void MissionInfoCard_Configure(ref MissionInfoCard __instance, ref FollowerInfo config)
    {
        // if (MissionButtons.ContainsKey(__instance.GetInstanceID()))
        // {
        //     MissionButtons[__instance.GetInstanceID()].Configure(config);
        //     MissionButtons[__instance.GetInstanceID()].Start();
        //     return;
        // }

        var mission = __instance._missionButtons.RandomElement();
        // var newMission = Object.Instantiate(mission, __instance._missionButtons[0].transform.parent);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        // __instance._missionButtons.Add(newMission);
        for (var i = 0; i < 20; i++)
        {
            var newMission = Object.Instantiate(mission, __instance._missionButtons[0].transform.parent);
            __instance._missionButtons.Add(newMission);

            newMission.name = $"Rebirth Mission {i}";
            newMission._type = Plugin.RebirthItem;
            newMission.Configure(config);
            newMission.Start();
            var card = __instance;
            newMission.OnMissionSelected += delegate(InventoryItem.ITEM_TYPE itemType)
            {
                var onMissionSelected = card.OnMissionSelected;
                if (onMissionSelected == null)
                {
                    return;
                }

                onMissionSelected(itemType);
            };
        }
        // MissionButtons.Add(__instance.GetInstanceID(), newMission);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetChance))]
    public static void MissionaryManager_GetChance(ref float __result, InventoryItem.ITEM_TYPE type, FollowerInfo followerInfo, StructureBrain.TYPES missionaryType)
    {
        var baseChanceMultiplier = MissionaryManager.GetBaseChanceMultiplier(type, followerInfo);
        var random = new System.Random((int) (followerInfo.ID + type));
        if (type == Plugin.RebirthItem)
        {
            __result = Mathf.Clamp((MissionaryManager.BaseChance_Bones + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * baseChanceMultiplier, 0f, 0.95f);
        }
    }
}