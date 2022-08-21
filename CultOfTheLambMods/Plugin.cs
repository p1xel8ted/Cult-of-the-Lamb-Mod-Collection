using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace CultOfTheLambMods
{
    [BepInPlugin("com.p1xel8ted.CultOfTheLamb.CultOfTheLambMods", "p1xel8ted's Cult of the Lamb mods!", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static ManualLogSource _log;
        private static ConfigEntry<bool> _modEnabled;
        public static ConfigEntry<bool> SkipIntros;
        public static ConfigEntry<bool> EasyFishing;
        public static ConfigEntry<bool> RemoveMenuClutter;
        public static ConfigEntry<bool> RemoveTwitchButton;

        private void Awake()
        {
            _log = new ManualLogSource("CultOfTheLambMods-Log");
            BepInEx.Logging.Logger.Sources.Add(_log);

            _modEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");
            SkipIntros = Config.Bind("General", "Skip Intros", true, "Skip splash screens.");
            EasyFishing = Config.Bind("General", "Cheese Fishing Mini-Game", true, "Fishing mini-game cheese.");
            RemoveMenuClutter = Config.Bind("General", "Remove Extra Menu Buttons", true, "Removes credits/road-map/discord buttons from the menus.");
            RemoveTwitchButton = Config.Bind("General", "Remove Twitch Buttons", true, "Removes twitch buttons from the menus.");

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