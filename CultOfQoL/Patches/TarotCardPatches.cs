using HarmonyLib;
using UnityEngine;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class TarotCardPatches
{
    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.DrawRandomCard))]
    public static class TarotCardDrawRandomCardPatches
    {
        [HarmonyPrefix]
        public static bool Prefix(ref TarotCards.TarotCard __result)
        {
            if (!Plugin.ThriceMultiplyTarotCardLuck.Value) return true;
            
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