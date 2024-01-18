namespace CultOfQoL.Patches;

[HarmonyPatch]
internal static class TwitchJunk
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnEnableInteraction))]
    public static void BuildingShrine_OnEnableInteraction()
    {
        if (!Plugin.UnlockTwitchStuff.Value) return;

        var availableTwitchTotemDecorations = DataManager.GetAvailableTwitchTotemDecorations();
        var availableTwitchTotemSkins = DataManager.GetAvailableTwitchTotemSkins();

        var twitchSkins = new List<string>(5) {"TwitchCat", "TwitchMouse", "TwitchPoggers", "TwitchDog", "TwitchDogAlt"};

        // if (!DataManager.TwitchTotemRewardAvailable()) return;
        foreach (var totem in availableTwitchTotemDecorations)
        {
            StructuresData.CompleteResearch(totem);
            StructuresData.SetRevealed(totem);
        }

        foreach (var skin in twitchSkins)
        {
            DataManager.SetFollowerSkinUnlocked(skin);
        }

        foreach (var availableTwitchTotemSkin in availableTwitchTotemSkins)
        {
            DataManager.SetFollowerSkinUnlocked(availableTwitchTotemSkin);
        }
    }


    [Harmony]
    [UsedImplicitly]
    public static class Drops
    {
        private const string AuthenticateMethod = "Authenticate";
        private const string Heretic = "Heretic";
        private const string Cultist = "Cultist";
        private const string Sinful = "Sinful";
        
        [UsedImplicitly]
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(typeof(GameManager)))
            {
                if (method.Name.Contains(Sinful, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (method.Name.Contains(Heretic, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (method.Name.Contains(Cultist, StringComparison.OrdinalIgnoreCase))
                { 
                    continue;
                }
                if (!method.Name.StartsWith(AuthenticateMethod)) continue;
                
                Plugin.L($"[AuthenticateOverride] Overriding {method.Name}");
                yield return method;
            }
        }

        [UsedImplicitly]
        [HarmonyPostfix]
        public static void Authenticate(ref bool __result, MethodBase __originalMethod)
        {
            __result = Plugin.UnlockTwitchStuff.Value;
        }

    }
}