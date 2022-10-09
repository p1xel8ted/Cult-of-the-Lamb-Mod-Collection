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
            if (Plugin.ThriceMultiplyTarotCardLuck.Value || Plugin.UseCustomLuckMultiplier.Value)
            {
                var newRangeCheck = 0.275f * (DataManager.Instance.GetLuckMultiplier() * (Plugin.UseCustomLuckMultiplier.Value ? Plugin.CustomLuckMultiplier.Value : 3f));
                if (newRangeCheck > 1)
                {
                    newRangeCheck = 1;
                }

                Plugin.L($"Original rangeCheck: {0.275f * DataManager.Instance.GetLuckMultiplier()}, New rangeCheck: {newRangeCheck}");
                var num = 0;

                while (Random.Range(0f, 1f) < newRangeCheck) num++;

                num = Mathf.Min(num, TarotCards.GetMaxTarotCardLevel(__result.CardType));

                __result.UpgradeIndex = num;
            }
        }
    }
}