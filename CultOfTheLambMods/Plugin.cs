using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace CultOfTheLambMods
{
    [BepInPlugin("com.p1xel8ted.CultOfQoLCollection", "Cult of QoL Collection", "1.2")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        private static ConfigEntry<bool> _modEnabled;
        public static ConfigEntry<bool> SkipIntros;
        public static ConfigEntry<bool> EasyFishing;
        public static ConfigEntry<bool> RemoveMenuClutter;
        public static ConfigEntry<bool> RemoveTwitchButton;
        public static ConfigEntry<bool> BulkInspireAndExtort;
        public static ConfigEntry<bool> ReverseGoldenFleeceDamageChange;
        public static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate;
        public static ConfigEntry<bool> AdjustRefineryRequirements;
        public static ConfigEntry<bool> AlwaysGoForOuthouseWithLeastPoop;
        public static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp;

        private void Awake()
        {
            Log = new ManualLogSource("CultOfQoLCollection-Log");
            BepInEx.Logging.Logger.Sources.Add(Log);

            _modEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");
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

            if (_modEnabled.Value)
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