using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

public static class TarotCardPatches
{
    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.DrawRandomCard))]
    public static class TarotCardDrawRandomCardPatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref TarotCards.TarotCard __result)
        {
            if (!Plugin.ThriceMultiplyTarotCardLuck.Value) return;
            //Plugin.L($"TarotCardDraw: Original upgrade index (rarity): {__result.UpgradeIndex}. Original random limit: {0.275f * DataManager.Instance.GetLuckMultiplier()}");
            var num = 0;

            while (Random.Range(0f, 1f) < 0.275f * (DataManager.Instance.GetLuckMultiplier() * 3f)) num++;

            num = Mathf.Min(num, TarotCards.GetMaxTarotCardLevel(__result.CardType));
       
            __result.UpgradeIndex = num;
           // Plugin.L($"TarotCardDraw: New upgrade index (rarity): {__result.UpgradeIndex}. New random limit: {0.275f * (DataManager.Instance.GetLuckMultiplier() * 3f)}");
        }
    }
}