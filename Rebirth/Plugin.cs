using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Linq;
using BepInEx.Configuration;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomObjectives;

namespace Rebirth
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API", "0.1.4")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PluginGuid = "p1xel8ted.cotl.rebirth";
        private const string PluginName = "Rebirth";
        private const string PluginVer = "0.1.3";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
        public static string PluginPath { get; private set; }

        private static ConfigEntry<bool> _modEnabled;
        public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }

        private static (Objectives.CustomQuestTypes ObjectiveKey, ObjectivesData ObjectiveData) RebirthCollectItemQuest { get; set; }

        //private readonly RebirthCollectItemQuest _rebirthCollectItemQuest = new();
        private void Awake()
        {
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable/disable this mod.");

            Log = Logger;
            Log.LogInfo($"Loaded {PluginName}!");

            PluginPath = Path.GetDirectoryName(Info.Location);

            CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
            CustomFollowerCommandManager.Add(new RebirthSubCommand());
            RebirthItem = CustomItemManager.Add(new RebirthItem());
           // RebirthCollectItemQuest = CustomObjectiveManager.Add(new RebirthCollectItemQuest());

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

       // private bool _addedQuest;

        // private void Update()
        // {
        //     if (GameManager.GetInstance() == null) return;
        //     if (_addedQuest) return;
        //     var locations = (FollowerLocation[]) Enum.GetValues(typeof(FollowerLocation));
        //
        //     foreach (var location in locations.Where(a => a.ToString().StartsWith("Dungeon1_") || a.ToString().StartsWith("Dungeon2_") || a.ToString().StartsWith("Dungeon3_") || a.ToString().StartsWith("Dungeon4_") || a.ToString().StartsWith("Dungeon5_")))
        //     {
        //         if (!DataManager.Instance.DungeonCompleted(locations.RandomElement())) continue;
        //         Log.LogWarning($"Added Rebirth quest at {location}.");
        //         var data = new Objectives_CollectItem("Objectives/GroupTitles/Quest", RebirthItem, UnityEngine.Random.Range(15, 26), false, location, 4800f);
        //         _rebirthCollectItemQuest.QuestData = data;
        //         RebirthCollectItemQuest = CustomObjectiveManager.Add(_rebirthCollectItemQuest);
        //         Quests.QuestsAll.Add(data);
        //         _addedQuest = true;
        //         break;
        //     }
        // }
    }
}