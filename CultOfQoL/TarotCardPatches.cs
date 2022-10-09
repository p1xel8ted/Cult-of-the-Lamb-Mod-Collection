using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

public static class TarotCardPatches
{
    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.DrawRandomCard))]
    public static class TarotCardDrawRandomCardPatches
    {
        [HarmonyPrefix]
        public static bool Prefix(ref TarotCards.TarotCard __result)
        {
            if (!Plugin.ThriceMultiplyTarotCardLuck.Value) return true;
            
            // Plugin.L($"Current Luck Multi: {DataManager.Instance.GetLuckMultiplier()}");
            // Plugin.L($"Current Range Check: {0.275f * DataManager.Instance.GetLuckMultiplier()}");
            //
            // Plugin.L($"New Luck Multi: {DataManager.Instance.GetLuckMultiplier() * 3}");
            // Plugin.L($"New Range Check: {0.275f * DataManager.Instance.GetLuckMultiplier() * 3}");
            
            var unusedFoundTrinkets = TarotCards.GetUnusedFoundTrinkets();
            if (unusedFoundTrinkets.Count <= 0)
            {
                __result = null;
            }

            var card = unusedFoundTrinkets[Random.Range(0, unusedFoundTrinkets.Count)];
            var num = 0;
            
                while (Random.Range(0f, 1f) < 0.275f * DataManager.Instance.GetLuckMultiplier() * 3)
                {
                    num++;
                }
            

            num = Mathf.Min(num, TarotCards.GetMaxTarotCardLevel(card));
            __result = new TarotCards.TarotCard(card, num);
            return false;

        }
    }
}