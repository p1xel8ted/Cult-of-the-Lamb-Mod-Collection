using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using BepInEx.Configuration;

namespace GoatOuthouses
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.cotl.goatouthouses";
        private const string PluginName = "G.O.A.T Outhouses";
        private const string PluginVer = "1.0.0";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
        private static string PluginPath { get; set; }
        internal static ConfigEntry<int> OuthouseAttemptsWhenInQueue;
        internal static ConfigEntry<bool> AlwaysGoForOuthouseWithLeastPoop;

        private void Awake()
        {
            Log = Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);
            
            //Outhouse
            OuthouseAttemptsWhenInQueue = Config.Bind("Outhouse", "Queue Attempts", 6, "By default, Followers try 3 times to use an outhouse before they move on to crapping in the bushes. Increase this to make them wait longer.");
            AlwaysGoForOuthouseWithLeastPoop = Config.Bind("Outhouse", "Prioritize Less Poopy Outhouses", true, "Where possible, followers will go to the outhouse that has the least amount of poop.");
        }

        private void OnEnable()
        {
         //  Harmony.PatchAll();
            Log.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Log.LogInfo($"Unloaded {PluginName}!");
        }
        
        public static void L(string message)
        {
            Log.LogWarning(message);
        }

    }
}