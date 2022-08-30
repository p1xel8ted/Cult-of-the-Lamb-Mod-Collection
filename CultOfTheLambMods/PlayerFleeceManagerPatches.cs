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
                if (playerFleece == 1)
                {
                    if (Plugin.ReverseGoldenFleeceDamageChange.Value)
                    {
                        ___damageMultiplier += Plugin.IncreaseGoldenFleeceDamageRate.Value ? 0.2f : 0.1f;
                    }
                    else
                    {
                        ___damageMultiplier += Plugin.IncreaseGoldenFleeceDamageRate.Value ? 0.1f : 0.05f;
                    }
                }
                ___OnDamageMultiplierModified?.Invoke(___damageMultiplier);
            }
        }
    }
}