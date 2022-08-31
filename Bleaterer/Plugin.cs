using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using COTL_API.CustomInventory;
using System.IO;
using Bleaterer.CustomFollowerCommands;
using Bleaterer.Items;
using static InventoryItem;
using COTL_API.CustomFollowerCommand;

namespace Bleaterer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "IngoH.cotl.$safeprojectname$";
        public const string PluginName = "$safeprojectname$";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        internal static ITEM_TYPE ExampleItem;

        internal static FollowerCommands FollowerCommand;

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            ExampleItem = CustomItemManager.Add(new ExampleItem());
            FollowerCommand = CustomFollowerCommandManager.Add(new ExampleFollowerCommand());
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