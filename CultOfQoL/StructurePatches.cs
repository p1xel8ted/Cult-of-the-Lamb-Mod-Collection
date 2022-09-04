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
                if (Plugin.SlowDownAgingInstead.Value)
                {
                    __instance.Data.Age /= 2;
                    Plugin.L($"Halving new age of lumber/mining station!");
                    return;
                }

                if (!Plugin.LumberAndMiningStationsDontAge.Value) return;
                
                __instance.Data.Age = 0;
                Plugin.L($"Resetting age of lumber/mining station to 0!");

            }
        }
        
        
        [HarmonyPatch(typeof(Structures_Bed), MethodType.Constructor)]
        public static class StructuresBedSoulMax
        {
            [HarmonyPostfix]
            public static void Postfix(ref Structures_Bed __instance)
            {
                if (!Plugin.DoubleSoulCapacity.Value) return;
                __instance.SoulMax *= 2;

            }
        }

        
        [HarmonyPatch(typeof(Structures_Shrine), "SoulMax", MethodType.Getter)]
        [HarmonyPatch(typeof(Structures_Shrine_Misfit), "SoulMax", MethodType.Getter)]
        [HarmonyPatch(typeof(Structures_Shrine_Passive), "SoulMax", MethodType.Getter)]
        [HarmonyPatch(typeof(Structures_Shrine_Ratau), "SoulMax", MethodType.Getter)]
        public static class StructuresShrinesSoulMax
        {
            [HarmonyPostfix]
            public static void Postfix(ref int __result)
            {
                if (!Plugin.DoubleSoulCapacity.Value) return;
                __result *= 2;

            }
        }
  
        //original author is Matthew-X, I just refactored.
        [HarmonyPatch(typeof(Structures_SiloFertiliser), MethodType.Constructor)]
        public static class StructuresSiloFertiliserBrain
        {
            [HarmonyPostfix]
            public static void Postfix(ref Structures_SiloFertiliser __instance)
            {
                if (!Plugin.JustRightSiloCapacity.Value) return;
                __instance.Capacity = 32f;

            }
        }
        
        //original author is Matthew-X, I just refactored.
        [HarmonyPatch(typeof(Structures_SiloSeed), MethodType.Constructor)]
        public static class StructureBrainCreateBrainPatches
        {
            [HarmonyPostfix]
            public static void Postfix(ref Structures_SiloSeed __instance)
            {
                if (!Plugin.JustRightSiloCapacity.Value) return;
                __instance.Capacity = 32f;

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