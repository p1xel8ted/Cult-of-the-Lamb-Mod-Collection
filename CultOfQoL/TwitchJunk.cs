using System.Collections.Generic;
using HarmonyLib;

namespace CultOfQoL;

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

        var totemDecorations = new List<StructureBrain.TYPES>(6)
        {
            StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
            StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
            StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
            StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
            StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
            StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN
        };

        var twitchSkins = new List<string>(5) {"TwitchCat", "TwitchMouse", "TwitchPoggers", "TwitchDog", "TwitchDogAlt"};

        // if (!DataManager.TwitchTotemRewardAvailable()) return;
        foreach (var totem in availableTwitchTotemDecorations)
        {
            StructuresData.CompleteResearch(totem);
            StructuresData.SetRevealed(totem);
        }

        foreach (var totem in totemDecorations)
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


    //[HarmonyPatch(nameof(GameManager.AuthenticateCultistDLC))] -- pay the $6
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticatePrePurchaseDLC))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticatePlushBonusDLC))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop1))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop2))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AuthenticateTwitchDrop3))]
    public static void GameManager_Postfix(ref bool __result)
    {
        __result = Plugin.UnlockTwitchStuff.Value;
    }
}