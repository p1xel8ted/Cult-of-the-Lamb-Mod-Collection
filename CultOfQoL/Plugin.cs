using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace CultOfQoL
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "com.p1xel8ted.CultOfQoLCollection";
        private const string PluginName = "Cult of QoL Collection";
        private const string PluginVer = "1.4";

        internal static ManualLogSource Log;

        internal static ConfigEntry<bool> SkipIntros;
        internal static ConfigEntry<bool> EasyFishing;
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

        private void Awake()
        {
            Log = new ManualLogSource("Cult-of-QoL-Collection");
            BepInEx.Logging.Logger.Sources.Add(Log);

            var modEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");
            SkipIntros = Config.Bind("General", "Skip Intros", true, "Skip splash screens.");
            EasyFishing = Config.Bind("General", "Cheese Fishing Mini-Game", true, "Fishing mini-game cheese.");
            RemoveMenuClutter = Config.Bind("General", "Remove Extra Menu Buttons", true, "Removes credits/road-map/discord buttons from the menus.");
            RemoveTwitchButton = Config.Bind("General", "Remove Twitch Buttons", true, "Removes twitch buttons from the menus.");
            BulkInspireAndExtort = Config.Bind("General", "Bulk Inspire/Extort", true, "When collecting tithes, or inspiring, all followers are done at once.");
            ReverseGoldenFleeceDamageChange = Config.Bind("General", "Reverse Golden Fleece Change", true, "Removes the 200% cap on damage multiplier.");
            IncreaseGoldenFleeceDamageRate = Config.Bind("General", "Increase Golden Fleece Rate", true, "Increases the damage gain by 20% per kill instead of 10%.");
            AdjustRefineryRequirements = Config.Bind("General", "Adjust Refinery Requirements", true, "Where possible, halves the materials needed to convert items in the refinery. Rounds up.");
            AlwaysGoForOuthouseWithLeastPoop = Config.Bind("General", "Prioritize Less Poopy Outhouses", true, "Where possible, followers will go to the outhouse that has the least amount of poop.");
            CleanseIllnessAndExhaustionOnLevelUp = Config.Bind("General", "Cleanse Illness and Exhaustion", true, "When a follower 'levels up', if they are sick or exhausted, the status is cleansed.");
            UnlockTwitchStuff = Config.Bind("General", "Unlock Twitch Stuff", true, "Unlock pre-order DLC, Twitch plush, and three drops.");
            LumberAndMiningStationsDontAge = Config.Bind("General", "Infinite Lumber & Mining Stations", true, "Lumber and mining stations should never run out and collapse.");
            CollectTitheFromOldFollowers = Config.Bind("General", "Collect Tithe From Old Followers", true, "Re-enable collecting tithe from the elderly. Brutal.");
            EnableGameSpeedManipulation = Config.Bind("General", "Enable Game Speed Manipulation", true, "Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.");

            if (modEnabled.Value)
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
                Log.LogMessage($"Plugin p1xel8ted's Cult of QoL Collection is enabled!");
            }
            else
            {
                Log.LogMessage($"Plugin p1xel8ted's Cult of QoL Collection is disabled!");
            }
        }
    }
}