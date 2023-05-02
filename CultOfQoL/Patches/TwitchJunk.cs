using System.Collections.Generic;
using HarmonyLib;

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


   
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticatePrePurchaseDLC))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticatePlushBonusDLC))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticatePAXDLC))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop1))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop2))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop3))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop4))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop5))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop6))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop7))]
    public static void GameManager_Postfix(ref bool __result)
    {
        __result = Plugin.UnlockTwitchStuff.Value;
    }
}