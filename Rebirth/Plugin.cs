using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using BepInEx.Configuration;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomObjectives;
using COTL_API.Helpers;
using Socket.Newtonsoft.Json.Utilities.LinqBridge;
using UnityEngine;

namespace Rebirth
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API", "0.1.6")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PluginGuid = "p1xel8ted.cotl.rebirth";
        private const string PluginName = "Rebirth";
        private const string PluginVer = "1.0.0";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
        public static string PluginPath { get; private set; }

        private static ConfigEntry<bool> _modEnabled;
        public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }
        private CustomObjective RebirthCollectItemQuest { get; set; }
        internal static RebirthItem RebirthItemInstance { get; private set; }

        public static InventoryItem.ITEM_TYPE RebirthFoodItem { get; private set; }
        internal static FoodItem RebirthFoodItemInstance { get; private set; }

        private void Awake()
        {
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable/disable this mod.");

            Log = Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
            CustomFollowerCommandManager.Add(new RebirthSubCommand());
            RebirthItem = CustomItemManager.Add(new RebirthItem());
            RebirthItemInstance = new RebirthItem();

            RebirthFoodItem = CustomItemManager.Add(new FoodItem());
            RebirthFoodItemInstance = new FoodItem();

            RebirthCollectItemQuest = CustomObjectiveManager.CollectItem(RebirthItem, UnityEngine.Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
            RebirthCollectItemQuest.InitialQuestText = $"Please Leader, please! I'm {"weary of this existence".Wave()} and seek to be reborn! I will do anything for you! Can you please help me?";
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