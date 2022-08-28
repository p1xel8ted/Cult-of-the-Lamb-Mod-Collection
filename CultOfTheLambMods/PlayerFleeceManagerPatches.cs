using System;
using FMOD;
using HarmonyLib;

namespace CultOfTheLambMods
{
    public static class PlayerFleeceManagerPatches
    {
        [HarmonyPatch(typeof(PlayerFleeceManager), nameof(PlayerFleeceManager.IncrementDamageModifier))]
        public static class PlayerFleeceManagerIncrementDamageModifierPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (Plugin.ReverseGoldenFleeceDamageChange.Value || Plugin.IncreaseGoldenFleeceDamageRate.Value) return false;
                return true;
            }

            [HarmonyPostfix]
            public static void Postfix(ref float ___damageMultiplier,
                ref PlayerFleeceManager.DamageEvent ___OnDamageMultiplierModified)
            {
                var playerFleece = DataManager.Instance.PlayerFleece;

                if (Plugin.ReverseGoldenFleeceDamageChange.Value)
                {
                    if (playerFleece == 1)
                    {
                        UnityEngine.Debug.Log($"[Fleece-OldDamage-Reverse]: {___damageMultiplier}");
                        ___damageMultiplier += Plugin.IncreaseGoldenFleeceDamageRate.Value ? 0.2f : 0.1f;
                        UnityEngine.Debug.Log($"[Fleece-NewDamage-Reverse]: {___damageMultiplier}");
                    }
                }
                else
                {
                    if (playerFleece == 1 && ___damageMultiplier < 2f)
                    {
                        UnityEngine.Debug.Log($"[Fleece-OldDamage-NoReverse]: {___damageMultiplier}");
                        ___damageMultiplier += Plugin.IncreaseGoldenFleeceDamageRate.Value ? 0.2f : 0.1f;
                        UnityEngine.Debug.Log($"[Fleece-NewDamage-NoReverse]: {___damageMultiplier}");
                    }
                }
   

                if (___OnDamageMultiplierModified == null)
                {
                    return;
                }

                ___OnDamageMultiplierModified(___damageMultiplier);
            }
        }
    }
}