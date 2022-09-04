using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine.ProBuilder;

namespace CultOfQoL
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
        private const string PluginName = "Cult of QoL Collection";
        private const string PluginVer = "1.6";

        internal static ManualLogSource Log;
        private static readonly Harmony Harmony = new(PluginGuid);

        internal static ConfigEntry<bool> SkipIntros;
        internal static ConfigEntry<bool> EasyFishing;
        internal static ConfigEntry<bool> FastCollecting;
        internal static ConfigEntry<bool> RemoveMenuClutter;
        internal static ConfigEntry<bool> RemoveTwitchButton;
        internal static ConfigEntry<bool> BulkInspireAndExtort;
        internal static ConfigEntry<bool> ReverseGoldenFleeceDamageChange;
        internal static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate;
        internal static ConfigEntry<bool> AdjustRefineryRequirements;
        internal static ConfigEntry<bool> AlwaysGoForOuthouseWithLeastPoop;
        internal static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp;
        internal static ConfigEntry<bool> UnlockTwitchStuff;
        internal static ConfigEntry<bool> LumberAndMiningStationsDontAge;
        internal static ConfigEntry<bool> CollectTitheFromOldFollowers;
        internal static ConfigEntry<bool> EnableGameSpeedManipulation;
        internal static ConfigEntry<bool> JustRightSiloCapacity;
        internal static ConfigEntry<bool> DoubleSoulCapacity;
        internal static ConfigEntry<bool> ShortenGameSpeedIncrements;
        internal static ConfigEntry<bool> SlowDownTime;
        internal static ConfigEntry<float> SlowDownTimeMultiplier;
        internal static ConfigEntry<bool> SlowDownAgingInstead;
        internal static ConfigEntry<int> OuthouseAttemptsWhenInQueue;
        private static ConfigEntry<bool> _modEnabled;

        private void Awake()
        {
            Log = new ManualLogSource("Cult-of-QoL-Collection");
            BepInEx.Logging.Logger.Sources.Add(Log);

            _modEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");
            
            //General
            SkipIntros = Config.Bind("General", "Skip Intros", true, "Skip splash screens.");
            RemoveMenuClutter = Config.Bind("General", "Remove Extra Menu Buttons", true, "Removes credits/road-map/discord buttons from the menus.");
            RemoveTwitchButton = Config.Bind("General", "Remove Twitch Buttons", true, "Removes twitch buttons from the menus.");
            
            //Game Mechanics
            
            CleanseIllnessAndExhaustionOnLevelUp = Config.Bind("Game Mechanics", "Cleanse Illness and Exhaustion", true, "When a follower 'levels up', if they are sick or exhausted, the status is cleansed.");
            UnlockTwitchStuff = Config.Bind("Game Mechanics", "Unlock Twitch Stuff", true, "Unlock pre-order DLC, Twitch plush, and three drops.");
            CollectTitheFromOldFollowers = Config.Bind("Game Mechanics", "Collect Tithe From Old Followers", true, "Re-enable collecting tithe from the elderly. Brutal.");
            ReverseGoldenFleeceDamageChange = Config.Bind("Game Mechanics", "Reverse Golden Fleece Change", true, "Reverts the default damage increase to 10% instead of 5%.");
            IncreaseGoldenFleeceDamageRate = Config.Bind("Game Mechanics", "Increase Golden Fleece Rate", true, "Doubles the damage increase.");
            AdjustRefineryRequirements = Config.Bind("Game Mechanics", "Adjust Refinery Requirements", true, "Where possible, halves the materials needed to convert items in the refinery. Rounds up.");
            BulkInspireAndExtort = Config.Bind("Game Mechanics", "Bulk Inspire/Extort", true, "When collecting tithes, or inspiring, all followers are done at once.");
            EasyFishing = Config.Bind("Game Mechanics", "Cheese Fishing Mini-Game", true, "Fishing mini-game cheese. Just cast and let the mod do the rest.");
            
            //Outhouse
            OuthouseAttemptsWhenInQueue = Config.Bind("Outhouse", "Queue Attempts", 6, "By default, Followers try 3 times to use an outhouse before they move on to crapping in the bushes. Increase this to make them wait longer.");
                AlwaysGoForOuthouseWithLeastPoop = Config.Bind("Outhouse", "Prioritize Less Poopy Outhouses", true, "Where possible, followers will go to the outhouse that has the least amount of poop.");
            
            //Lumber/mining
            LumberAndMiningStationsDontAge = Config.Bind("Lumber/Mine Mods", "Infinite Lumber & Mining Stations", true, "Lumber and mining stations should never run out and collapse.");
            SlowDownAgingInstead = Config.Bind("Lumber/Mine Mods", "Slow Down Aging Instead", true, "Slows down aging of lumber/mining stations by half. This setting overrides the above.");

            //Speed
            EnableGameSpeedManipulation = Config.Bind("Speed", "Enable Game Speed Manipulation", true, "Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.");
            ShortenGameSpeedIncrements = Config.Bind("Speed", "Shorten Game Speed Increments", false, "Increments in steps of 1, instead of 0.25.");
            FastCollecting = Config.Bind("Speed", "Speed Up Collection", true, "Increases the rate you can collect from the shrines, and other structures.");
            SlowDownTime = Config.Bind("Speed", "Slow Down Time", true, "Enables the ability to slow down time. This is different to the increase speed implementation. This will make the days longer, but not slow down animations.");
            SlowDownTimeMultiplier = Config.Bind("Speed", "Slow Down Time Multiplier", 2f, "Increases the rate you can collect from the shrines, and other structures.");
                
            //Capacity
            JustRightSiloCapacity = Config.Bind("Capacity", "Set Silo Capacity to 32", true, "Set silo capacity for seed and fertilizer at 32.");
            DoubleSoulCapacity = Config.Bind("Capacity", "Double Soul Capacity", true, "Doubles the soul capacity of applicable structures.");

        }
        
        private void OnEnable()
        {
            if (!_modEnabled.Value) return;
            Harmony.PatchAll();
            L($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            L($"Unloaded {PluginName}!");
        }

        public static void L(string message)
        {
            Log.LogWarning(message);
        }
    }
}