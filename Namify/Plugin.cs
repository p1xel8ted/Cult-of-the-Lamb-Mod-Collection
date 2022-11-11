using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using BepInEx.Configuration;
using UnityEngine;

namespace Namify
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PluginGuid = "p1xel8ted.cotl.namify";
        private const string PluginName = "Namify";
        private const string PluginVer = "0.1.1";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
  
        internal static ConfigEntry<string> PersonalApiKey;
        private static ConfigEntry<string> _nothing;

        private void Awake()
        {
            Log = Logger;
            PersonalApiKey = Config.Bind("API", "Personal API Key", "ee5f806e1c1d458b99c934c0eb3de5b8", "The default API Key is mine, limited to 1000 requests per day. You can get your own at https://randommer.io/");
            _nothing = Config.Bind("API", "INFO", "INFO", "When you load your save game, it will fetch 2000 names and store them in a file, which will be loaded next time around instead of wasting API calls. When you name a follower, it will remove that name from the stored list.");
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Log.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Log.LogInfo($"Unloaded {PluginName}!");
        }

      
    }
}