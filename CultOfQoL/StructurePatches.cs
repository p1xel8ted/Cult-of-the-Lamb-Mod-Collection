using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL
{
    internal static class StructurePatches
    {
        [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.IncreaseAge))]
        public static class StructuresLumberjackStationAgePatches
        {
            [HarmonyPrefix]
            public static void Prefix(ref Structures_LumberjackStation __instance)
            {
                if (!Plugin.LumberAndMiningStationsDontAge.Value) return;
                __instance.Data.Age = 0;
                Plugin.Log.LogWarning($"Resetting age of lumber/mining station to 0!");

            }
        }
        
        [HarmonyPatch(typeof(Structures_SiloFertiliser))]
        public static class StructuresSiloFertiliserPatches
        {
            [HarmonyPrefix]
            public static void Prefix(ref float ___Capacity)
            {
                if (!Plugin.JustRightSiloCapacity.Value) return;
                ___Capacity = 32f;
            }
        }

        [HarmonyPatch(typeof(Structures_SiloSeed))]
        public static class StructuresSiloSeedPatches
        {
            [HarmonyPrefix]
            public static void Prefix(ref float ___Capacity)
            {
                if (!Plugin.JustRightSiloCapacity.Value) return;
                ___Capacity = 32f;
            }
        }
        
        [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost), typeof(InventoryItem.ITEM_TYPE))]
        public static class RefineryItemCheckCanAffordPatches
        {
            [HarmonyPostfix]
            public static void Postfix(ref List<StructuresData.ItemCost> __result)
            {
                if (!Plugin.AdjustRefineryRequirements.Value) return;
                foreach (var item in __result)
                {
                    item.CostValue = Mathf.CeilToInt(item.CostValue / 2f);
                }
            }
        }
    }
}