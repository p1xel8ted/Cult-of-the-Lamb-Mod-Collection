﻿using System.Collections.Generic;
using HarmonyLib;

namespace CultOfQoL;

internal static class TwitchJunk
{
    [HarmonyPatch(typeof(BuildingShrine))]
    public static class BuildingShrinePatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(BuildingShrine.OnEnableInteraction))]
        public static void Postfix()
        {
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

            if (Plugin.UnlockTwitchStuff.Value)
            {
               // if (!DataManager.TwitchTotemRewardAvailable()) return;
                foreach (var availableTwitchTotemDecoration in availableTwitchTotemDecorations)
                {
                    StructuresData.CompleteResearch(availableTwitchTotemDecoration);
                    StructuresData.SetRevealed(availableTwitchTotemDecoration);
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
        }
    }

    [HarmonyPatch(typeof(GameManager))]
    public static class GameManagerPatches
    {
        //[HarmonyPatch(nameof(GameManager.AuthenticateCultistDLC))] -- pay the $6
        [HarmonyPatch(nameof(GameManager.AuthenticatePrePurchaseDLC))]
        [HarmonyPatch(nameof(GameManager.AuthenticatePlushBonusDLC))]
        [HarmonyPatch(nameof(GameManager.AuthenticateTwitchDrop1))]
        [HarmonyPatch(nameof(GameManager.AuthenticateTwitchDrop2))]
        [HarmonyPatch(nameof(GameManager.AuthenticateTwitchDrop3))]
        public static void Postfix(ref bool __result)
        {
            if (Plugin.UnlockTwitchStuff.Value) __result = true;
        }
    }
}