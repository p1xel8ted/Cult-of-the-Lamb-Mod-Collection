using HarmonyLib;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;

namespace ShamurasRevenge;

[HarmonyPatch]
public static class Revenge
{
    [field: FormerlySerializedAs("SpiderPrefab")]
    public static GameObject SpiderPrefab { get; set; }

    [field: FormerlySerializedAs("SpawnParent")]
    public static Transform SpawnParent { get; set; }

    public static Vector3 RandomPointInCollider { get; set; }
    public static CritterSpawner CritterSpawner { get; set; }
    
    
    public static void OnNewPhaseStarted()
    {
        if (CritterSpawner is null) return;
        if (TimeManager.CurrentPhase != DayPhase.Night) return;
        var min = -1;
        const int max = 100;
        while (++min < max)
        {
            SpiderPrefab.Spawn(SpawnParent, RandomPointInCollider, Quaternion.identity);
        }
    }

    [HarmonyPatch(typeof(CritterSpawner),nameof(CritterSpawner.OnNewPhaseStarted))]
    public static class CritterSpawnerOnNewPhaseStartedHook
    {
        [HarmonyPostfix]
        public static void Postfix(CritterSpawner __instance)
        {
            if (TimeManager.CurrentPhase != DayPhase.Night) return;
            
            Plugin.PoopList.Clear();
            
            CritterSpawner = __instance;
            SpiderPrefab = __instance.SpiderPrefab;
            SpawnParent = __instance.SpawnParent;
           
            var min = -1;
            const int max = 100;
            while (++min < max)
            {
                RandomPointInCollider = (Vector3) AccessTools.Method(typeof(CritterSpawner), "RandomPointInCollider").Invoke(__instance,null);
                SpiderPrefab.Spawn(SpawnParent, RandomPointInCollider, Quaternion.identity);
            }
            Plugin.Log.LogWarning($"Spawned: { SpiderPrefab.CountSpawned()}");

           // Plugin.MakeThemAllPoop();
           FollowerManager.MakeAllFollowersPoopOrVomit();
           NotificationCentre.Instance.PlayGenericNotification($"All the followers have spoiled themselves in terror!");
        }
    }
}