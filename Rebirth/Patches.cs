using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Map;
using MMBiomeGeneration;
using MMRoomGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
public static class Patches
{
    private const float Chance = 0.15f;

    private static bool DropLoot()
    {
        return Random.Range(0f, 1f) <= Chance * DataManager.Instance.GetLuckMultiplier();
    }

    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    [HarmonyPostfix]
    public static void Postfix(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team == Health.Team.Team2)
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                SpawnTokens(Random.Range(1, 4), __instance.gameObject.transform.position, 4f, delegate
                {
                    // Inventory.AddItem(Plugin.RebirthItem, 1);
                    
                });
                //Inventory.AddItem(Plugin.RebirthItem, Random.Range(1, 4));
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
               // Inventory.AddItem(Plugin.RebirthItem, 1);
               SpawnTokens(1, __instance.gameObject.transform.position, 4f, delegate
               {
                   //Inventory.AddItem(Plugin.RebirthItem, 1);
               });
            }
        }
    }

    private static void SpawnTokens(int quantity, Vector3 position, float startSpeed = 4f, Action<PickUp> result = null)
    {
        var gameObject = GameObject.FindGameObjectWithTag("Unit Layer");
        var transform = (gameObject != null) ? gameObject.transform : null;
        while (--quantity >= 0)
        {
            var instance = BiomeGenerator.Instance;
            if (((instance != null) ? instance.CurrentRoom : null) != null)
            {
                transform = BiomeGenerator.Instance.CurrentRoom.GameObject.transform;
            }

            if (transform == null && GenerateRoom.Instance != null)
            {
                transform = GenerateRoom.Instance.transform;
            }

            if (transform == null)
            {
                break;
            }

            var myObj = new RebirthItem().GameObject.Spawn(transform, position, Quaternion.identity);
            myObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            var copyObj = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/BlackGold") as GameObject);
            var copyP = copyObj!.GetComponent<PickUp>();
           var p = myObj.AddComponent(copyP);
           //var p = copy!.GetComponent<PickUp>();
            if (p != null)
            {
                //p.Bounce = true;
                p.type = Plugin.RebirthItem;
                p.Speed = startSpeed;
                p.SetImage(Plugin.RebirthItem);
                p.child = copyP.child;

            }

            if (result == null)
            {
                return;
            }

            result(p);
            
            // ObjectPool.Spawn("BlackGold", position, Quaternion.identity, transform, delegate(GameObject obj)
            // {
            //     var p = obj.GetComponent<PickUp>();
            //     if (p != null)
            //     {
            //         p.type = Plugin.RebirthItem;
            //         p.Speed = StartSpeed;
            //     }
            //
            //     Action<PickUp> result2 = result;
            //     if (result2 == null)
            //     {
            //         return;
            //     }
            //
            //     result2(p);
            // });
            // var myCoin = new RebirthItem().GameObject;
            //
            // var coinToCopy = Resources.Load("Prefabs/Resources/BlackGold") as GameObject;
            // var pToCopy = coinToCopy!.GetComponent<PickUp>();
            // var colliderToCopy = coinToCopy!.GetComponent<CircleCollider2D>();
            // pToCopy.type = Plugin.RebirthItem;
            // pToCopy.Speed = StartSpeed;
            // pToCopy.child = myCoin;
            // myCoin.AddComponent(pToCopy);
            // myCoin.AddComponent(colliderToCopy);


            //  // var spawnedCoin = myCoin.Spawn(transform, position, Quaternion.identity);
            //  // spawnedCoin.transform.position = position;
            //  // spawnedCoin.transform.eulerAngles = Vector3.zero;
            // var spawnedCoin = ObjectPool.Spawn(coinToCopy, transform, position, Quaternion.identity);
            //  var p = spawnedCoin.GetComponent<PickUp>();
            //
            //  p.type = Plugin.RebirthItem;
            //  if (result == null)
            //  {
            //      return null;
            //  }
            //
            //  result(p);

        }
    }

    //thank unity forum person
    private static T AddComponent<T>(this GameObject game, T duplicate) where T : Component
    {
        var target = game.AddComponent<T>();
        foreach (var x in typeof(T).GetProperties())
            if (x.CanWrite)
                x.SetValue(target, x.GetValue(duplicate));
        return target;
    }

    [HarmonyPatch(typeof(Interaction_Chest))]
    public static class DungeonChestPatches
    {
        [HarmonyPatch(nameof(Interaction_Chest.Reveal))]
        [HarmonyPostfix]
        public static void RevealPostfix(Interaction_Chest __instance)
        {
            Plugin.Log.LogWarning($"Regular Room Chest: {__instance.name}, Location: {PlayerFarming.Location.ToString()}");

            if (__instance.MyState == Interaction_Chest.State.Open && DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                Inventory.AddItem(Plugin.RebirthItem, Random.Range(2, 5));
            }
        }

        [HarmonyPatch(nameof(Interaction_Chest.RevealBossReward))]
        [HarmonyPostfix]
        public static void RevealBossRewardPostfix(Interaction_Chest __instance)
        {
            Plugin.Log.LogWarning($"Boss Room Chest: {__instance.name}, Location: {PlayerFarming.Location.ToString()}");

            if (__instance.MyState == Interaction_Chest.State.Open && DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                Inventory.AddItem(Plugin.RebirthItem, Random.Range(3, 6));
            }
        }
    }
}