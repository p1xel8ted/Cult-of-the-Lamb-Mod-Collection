using HarmonyLib;
using UnityEngine;

namespace ShamurasRevenge;

[HarmonyPatch]
public static class Revenge
{
    public static GameObject SpiderPrefab = null!;

    public static Transform SpawnParent = null!;

    public static Vector3 RandomPointInCollider { get; set; }
    public static CritterSpawner CritterSpawner = null!;
    

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
                RandomPointInCollider = __instance.RandomPointInCollider();
                SpiderPrefab.Spawn(SpawnParent, RandomPointInCollider, Quaternion.identity);
            }
            Plugin.Log.LogWarning($"Spawned: { SpiderPrefab.CountSpawned()}");

           // Plugin.MakeThemAllPoop();
           FollowerManager.MakeAllFollowersPoopOrVomit();
           NotificationCentre.Instance.PlayGenericNotification("All the followers have spoiled themselves in terror!");
        }
    }
}