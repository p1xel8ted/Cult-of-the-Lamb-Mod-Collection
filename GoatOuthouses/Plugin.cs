using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace GoatOuthouses
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.cotl.goatouthouses";
        private const string PluginName = "G.O.A.T Outhouses";
        private const string PluginVer = "0.1.0";

        public static ManualLogSource Log { get; private set; }
        private static readonly Harmony Harmony = new(PluginGuid);
        private static string PluginPath { get; set; }

        private void Awake()
        {
            Log = Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);
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
        
        public static void L(string message)
        {
            Log.LogWarning(message);
        }
        
        private void Update()
        {
            if (GameManager.instance is null) return;
            
            var structures = StructureManager.GetAllStructuresOfType<Structures_SiloFertiliser>();
            var fullPoop = StructureManager.GetAllStructuresOfType<Structures_Outhouse>().Exists(a => a.Data.Inventory.Count>=1);
            if (structures.Count <= 0 || !fullPoop) return;
            structures.Sort((a,b) => a.Data.Inventory.Count.CompareTo(b.Data.Inventory.Count));
            var task = new FollowerTaskOuthouse(structures[0].Data.ID);
        }

    }
}