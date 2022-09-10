using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using BepInEx.Configuration;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using UnityEngine;

namespace Rebirth
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PluginGuid = "p1xel8ted.cotl.rebirth";
        private const string PluginName = "Rebirth";
        private const string PluginVer = "0.1";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
        public static string PluginPath { get; private set; }
        private static ConfigEntry<bool> _modEnabled;
        //public static AssetBundle Assets { get; private set; }
        public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }

        private void Awake()
        {
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable/disable this mod.");

            Log = Logger;
            Log.LogInfo($"Loaded {PluginName}!");

            PluginPath = Path.GetDirectoryName(Info.Location); 
           // Assets = AssetBundle.LoadFromFile(Path.Combine(PluginPath!, "assets", "rebirth", "rebirth"));
            //Assets.Unload(false);
            
            CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
            CustomFollowerCommandManager.Add(new RebirthSubCommand());
            RebirthItem = CustomItemManager.Add(new RebirthItem());
            
        }

        private void OnEnable()
        {
            if (_modEnabled.Value)
            {
                Harmony.PatchAll();
                Log.LogInfo($"Loaded {PluginName}!");
            }
            else
            {
                Log.LogInfo($"{PluginName} is disabled in config!"); 
            }
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Log.LogInfo($"Unloaded {PluginName}!");
        }
    }
}