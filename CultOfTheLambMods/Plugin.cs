﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace CultOfTheLambMods
{
    [BepInPlugin("com.p1xel8ted.CultOfTheLamb.CultOfTheLambMods", "p1xel8ted's Cult of the Lamb mods!", "1.1")]
    public class Plugin : BaseUnityPlugin
    {
        private static ManualLogSource _log;
        private static ConfigEntry<bool> _modEnabled;
        public static ConfigEntry<bool> SkipIntros;
        public static ConfigEntry<bool> EasyFishing;
        public static ConfigEntry<bool> RemoveMenuClutter;
        public static ConfigEntry<bool> RemoveTwitchButton;
        public static ConfigEntry<bool> BulkInspireAndExtort;
        public static ConfigEntry<bool> ReverseGoldenFleeceDamageChange;
        public static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate;

        private void Awake()
        {
            _log = new ManualLogSource("CultOfTheLambMods-Log");
            BepInEx.Logging.Logger.Sources.Add(_log);

            _modEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");
            SkipIntros = Config.Bind("General", "Skip Intros", true, "Skip splash screens.");
            EasyFishing = Config.Bind("General", "Cheese Fishing Mini-Game", true, "Fishing mini-game cheese.");
            RemoveMenuClutter = Config.Bind("General", "Remove Extra Menu Buttons", true, "Removes credits/road-map/discord buttons from the menus.");
            RemoveTwitchButton = Config.Bind("General", "Remove Twitch Buttons", true, "Removes twitch buttons from the menus.");
            BulkInspireAndExtort = Config.Bind("General", "Bulk Inspire/Extort", true, "When collecting tithes, or inspiring, all followers are done at once.");
            ReverseGoldenFleeceDamageChange = Config.Bind("General", "Reverse Golden Fleece Change", true, "Removes the 200% cap on damage multiplier.");
            IncreaseGoldenFleeceDamageRate = Config.Bind("General", "Increase Golden Fleece Rate", true, "Increases the damage gain by 20% per kill instead of 10%.");

            if (_modEnabled.Value)
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
                _log.LogInfo($"Plugin p1xel8ted's CultOfTheLambMods is loaded!");
            }
            else
            {
                _log.LogInfo($"Plugin p1xel8ted's CultOfTheLambMods is disabled!");
            }
        }
    }
}