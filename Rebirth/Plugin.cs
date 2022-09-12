using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Linq;
using BepInEx.Configuration;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using Map;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rebirth
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PluginGuid = "p1xel8ted.cotl.rebirth";
        private const string PluginName = "Rebirth";
        private const string PluginVer = "0.1.1";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
        public static string PluginPath { get; private set; }

        private static ConfigEntry<bool> _modEnabled;

        public static AssetBundle Assets { get; private set; }
        public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }
        private static bool _objectiveAdded = false;

        private void Awake()
        {
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable/disable this mod.");

            Log = Logger;
            Log.LogInfo($"Loaded {PluginName}!");

            PluginPath = Path.GetDirectoryName(Info.Location);
             Assets = AssetBundle.LoadFromFile(Path.Combine(PluginPath!, "assets", "rebirth", "rebirth"));
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

        // private void Update()
        // {
        //     if (GameManager.GetInstance() == null) return;
        //     if (_objectiveAdded) return;
        //     _objectiveAdded = true;
        //     var followerLocations = (FollowerLocation[]) Enum.GetValues(typeof(FollowerLocation));
        //     foreach (var location in followerLocations.Where(a => a.ToString().Contains("Dungeon") && a.ToString().Length == 10)) //restricts to DungeonX_X
        //     {
        //         var customObjective = new Objectives_CollectItem("Objectives/GroupTitles/Quest", RebirthItem, Random.Range(15, 26), false, location, 4800f)
        //         {
        //             TargetFollowerAllowOldAge = false,
        //         };
        //         Quests.QuestsAll.Add(customObjective);
        //         DataManager.Instance.Objectives.Add(customObjective);
        //         Log.LogWarning($"Added Rebirth Objective to Quests - Amount: {customObjective.Target}, Location: {customObjective.TargetLocation}");
        //     }
        // }
    }
}