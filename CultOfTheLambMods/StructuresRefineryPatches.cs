using HarmonyLib;
using System.Collections.Generic;
using src.UI.Testing;
using UnityEngine;
using UnityEngine.UI;

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
                foreach (var item in __result)
                {
                    item.CostValue = Mathf.CeilToInt(item.CostValue / 2f);
                }
            }
        }

        //[HarmonyPatch(typeof(RefineryItem), nameof(RefineryItem.UpdateQuantity))]
        //public static class RefineryItemUpdateQuantityPatches
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(ref MMButton ____button, ref Image ____canAffordIcon,
        //        ref Image ____cantAffordIcon)
        //    {
        //        ____button.Confirmable = true;
        //        ____canAffordIcon.gameObject.SetActive(true);
        //        ____cantAffordIcon.gameObject.SetActive(false);
        //    }
        //}
    }
}
