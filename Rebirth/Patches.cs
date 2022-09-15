using System;
using System.Globalization;
using System.Reflection;
using HarmonyLib;
using MMBiomeGeneration;
using MMRoomGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
[HarmonyWrapSafe]
public static class Patches
{
    private const float Chance = 0.25f;

    private static bool DropLoot()
    {
        return true;
        //return Random.Range(0f, 1f) <= Chance * DataManager.Instance.GetLuckMultiplier();
    }

    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    [HarmonyWrapSafe]
    [HarmonyPrefix]
    public static void Prefix(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team == Health.Team.Team2)
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                SpawnTokens(1, PlayerFarming.Instance.transform.position);
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                SpawnTokens(1, PlayerFarming.Instance.transform.position);
            }
        }
    }

    private static void SpawnTokens(int quantity, Vector3 position)
    {
        var gameObject = GameObject.FindGameObjectWithTag("Unit Layer");
        var transform = gameObject != null ? gameObject.transform : null;
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


            var mySprite = new RebirthItem().GameObject.GetComponent<SpriteRenderer>().sprite;

            var copyObj = Resources.Load("Prefabs/Resources/BlackGold") as GameObject;

            copyObj!.GetComponentInChildren<SpriteRenderer>().sprite = mySprite;

            copyObj.GetComponent<PickUp>().type = Plugin.RebirthItem;

            copyObj.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
            
            ObjectPool.Spawn(copyObj, transform, position, Quaternion.identity);
            
        }
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