using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using COTL_API.CustomFollowerCommand;
using UnityEngine;


namespace Rebirth
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.cotl.rebirth";
        private const string PluginName = "Rebirth";
        private const string PluginVer = "1.0.0";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);

        public static string PluginPath { get; private set; }

        private static FollowerCommands _followerCommand;

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            _followerCommand = CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
    }
}