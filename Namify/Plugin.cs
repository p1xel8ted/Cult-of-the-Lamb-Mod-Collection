namespace Namify
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.cotl.namify";
        private const string PluginName = "Namify";
        private const string PluginVer = "0.1.1";

        public static ManualLogSource Log = null!;
        private readonly static Harmony Harmony = new(PluginGuid);

        internal static ConfigEntry<string> PersonalApiKey = null!;
        
        private void Awake()
        {
            Log = Logger;
            PersonalApiKey = Config.Bind("API", "Personal API Key", "ee5f806e1c1d458b99c934c0eb3de5b8", "The default API Key is mine, limited to 1000 requests per day. You can get your own at https://randommer.io/");
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Log.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
        }

      
    }
}