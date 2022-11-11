// using System.Collections.Generic;
// using HarmonyLib;
// using src.UI.Menus;
// using UnityEngine;
// using UnityEngine.ProBuilder;
// using Object = UnityEngine.Object;
// using Random = UnityEngine.Random;
//
// namespace Rebirth;
//
// [HarmonyPatch]
// public static class MissionBoard
// {
//     private static readonly IntRange TokenRange = new(15, 25);
//
//     [HarmonyPrefix]
//     [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetReward))]
//     public static bool GetReward(ref InventoryItem.ITEM_TYPE type, ref float chance, ref int followerID, ref InventoryItem[] __result)
//     {
//         if (type != Plugin.RebirthItem) return true;
//         var num = Random.Range(0f, 1f);
//         foreach (var objective in DataManager.Instance.CompletedObjectives)
//         {
//             if (objective.Follower != followerID) continue;
//             chance = float.MaxValue;
//             break;
//         }
//
//         if (chance > num)
//         {
//             __result = new[] {new InventoryItem(Plugin.RebirthItem, TokenRange.Random())};
//         }
//
//         return false;
//     }
//
//     [HarmonyPostfix]
//     [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetRewardRange))]
//     public static void MissionaryManager_GetRewardRange(ref IntRange __result, InventoryItem.ITEM_TYPE type)
//     {
//         if (type == Plugin.RebirthItem)
//         {
//             __result = TokenRange;
//         }
//     }
//
//     private static readonly Dictionary<int, MissionButton> MissionButtons = new();
//     
//     [HarmonyPostfix]
//     [HarmonyPatch(typeof(MissionInfoCard), nameof(MissionInfoCard.Configure))]
//     public static void MissionInfoCard_Configure(ref MissionInfoCard __instance, ref FollowerInfo config)
//     {
//         if (MissionButtons.ContainsKey(__instance.GetInstanceID()))
//         {
//             MissionButtons[__instance.GetInstanceID()].Configure(config);
//             MissionButtons[__instance.GetInstanceID()].Start();
//             return;
//         }
//
//         var mission = __instance._missionButtons.RandomElement();
//
//         var newMission = Object.Instantiate(mission, mission.transform.parent);
//         __instance._missionButtons.Add(newMission);
//
//         newMission.name = $"Rebirth Mission";
//         newMission._type = Plugin.RebirthItem;
//         newMission.Configure(config);
//         newMission.Start();
//         var card = __instance;
//         newMission.OnMissionSelected += delegate(InventoryItem.ITEM_TYPE itemType)
//         {
//             var onMissionSelected = card.OnMissionSelected;
//             if (onMissionSelected == null)
//             {
//                 return;
//             }
//
//             onMissionSelected(itemType);
//         };
//
//         MissionButtons.Add(__instance.GetInstanceID(), newMission);
//     }
//
//
//     [HarmonyPostfix]
//     [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetChance))]
//     public static void MissionaryManager_GetChance(ref float __result, InventoryItem.ITEM_TYPE type, FollowerInfo followerInfo, StructureBrain.TYPES missionaryType)
//     {
//         var baseChanceMultiplier = MissionaryManager.GetBaseChanceMultiplier(type, followerInfo);
//         var random = new System.Random((int) (followerInfo.ID + type));
//         if (type == Plugin.RebirthItem)
//         {
//             __result = Mathf.Clamp((MissionaryManager.BaseChance_Bones + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * baseChanceMultiplier, 0f, 0.95f);
//         }
//     }
// }