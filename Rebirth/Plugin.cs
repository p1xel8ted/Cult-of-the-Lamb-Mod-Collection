using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomMission;
using COTL_API.CustomObjectives;
using COTL_API.CustomSettings;
using COTL_API.Saves;
using HarmonyLib;
using Random = UnityEngine.Random;

namespace Rebirth
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [BepInDependency("io.github.xhayper.COTL_API", "0.1.12")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PluginGuid = "p1xel8ted.cotl.rebirth";
        private const string PluginName = "Rebirth";
        private const string PluginVer = "1.0.2";

        public static ManualLogSource Log { get; private set; } = null!;
        private static readonly Harmony Harmony = new(PluginGuid);
        public static string PluginPath = null!;

        private static ConfigEntry<bool> _modEnabled = null!;
        internal static ConfigEntry<bool> RebirthOldFollowers = null!;
        public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }
        private CustomObjective RebirthCollectItemQuest { get; set; } = null!;
        internal static RebirthItem RebirthItemInstance { get; private set; } = null!;

        public static InventoryItem.ITEM_TYPE RebirthMission { get; private set; }

        
        public static readonly ModdedSaveData<List<int>> RebirthSaveData = new(PluginGuid);

        private void Awake()
        {
          
            RebirthSaveData.LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE;
            ModdedSaveManager.RegisterModdedSave(RebirthSaveData);
            
            _modEnabled = Config.Bind("General", "Enabled", true, "Enable/disable this mod.");
            RebirthOldFollowers = Config.Bind("General", "RebirthOldFollowers", false, "Allow old followers to be reborn.");
            Log = Logger;

            PluginPath = Path.GetDirectoryName(Info.Location) ?? throw new DirectoryNotFoundException();

            CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
            CustomFollowerCommandManager.Add(new RebirthSubCommand());
            RebirthItem = CustomItemManager.Add(new RebirthItem());
            RebirthMission = CustomMissionManager.Add(new MissionItem());

            RebirthItemInstance = new RebirthItem();

            RebirthCollectItemQuest = CustomObjectiveManager.CollectItem(RebirthItem, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
            RebirthCollectItemQuest.InitialQuestText = $"Please Leader, please! I'm {"weary of this existence".Wave()} and seek to be reborn! I will do anything for you! Can you please help me?";
            CustomSettingsManager.AddBepInExConfig("Rebirth", "Rebirth Old Followers",RebirthOldFollowers, b =>
            {
                Log.LogWarning("Setting RebirthOldFollowers to " + b);
            });

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