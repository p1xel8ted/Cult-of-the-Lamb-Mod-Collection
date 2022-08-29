using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CultOfTheLambMods
{
    public static class StructuresRefineryPatches
    {
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
