using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ShamurasRevenge
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.cotl.shamurasrevenge";
        private const string PluginName = "Shamura's Revenge";
        private const string PluginVer = "1.0.0";

        public static ManualLogSource Log = null!;
        private static readonly Harmony Harmony = new(PluginGuid);
        public static string PluginPath = null!;


        private void Awake()
        {
            Log = Logger;
            PluginPath = Path.GetDirectoryName(Info.Location) ?? throw new DirectoryNotFoundException();
        }

        private void OnEnable()
        {
           // Harmony.PatchAll();
            Log.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Log.LogInfo($"Unloaded {PluginName}!");
        }

        private void Update()
        {
            if (GameManager.instance is null) return;
            var count = Revenge.SpiderPrefab.CountSpawned();
            if (count > 0)
            {
                Log.LogWarning($"Spider count: {Revenge.SpiderPrefab.CountSpawned()}");
            }

    
        }

        internal static void MakeThemAllPoop()
        {
            foreach (var follower in Follower.Followers)
            {
                GameManager.GetInstance().StartCoroutine(PoopAll(follower));
            }
            NotificationCentre.Instance.PlayGenericNotification("All the followers have pooped themselves in terror!");
        }

        public static List<Follower> PoopList { get; } = new();
        

        private static IEnumerator PoopAll(Follower follower)
        {
          //  if (Revenge.SpiderPrefab.CountSpawned() < 104) yield break;
          //  if(PoopList.Contains(follower)) yield break;
            yield return new WaitForSeconds(2f);
            follower.Brain.HardSwapToTask(new FollowerTask_InstantPoop());
            if (!PoopList.Contains(follower))
            {
                PoopList.Add(follower);
            }
        }
    }
}