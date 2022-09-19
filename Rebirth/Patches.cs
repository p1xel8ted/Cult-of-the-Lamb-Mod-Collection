using System.Globalization;
using HarmonyLib;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
[HarmonyWrapSafe]
public static class Patches
{
    private const float Chance = 1f;

    private static bool DropLoot()
    {
        var roll = Random.Range(0f, 1f);
        var chance = Chance * DataManager.Instance.GetLuckMultiplier();
        Plugin.Log.LogWarning($"Rebirth Coin Chance: {roll} / {chance}: {roll <= chance}");
        return roll <= chance;
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
                // SpawnLoot(Random.Range(1, 3), __instance.transform.position);
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                //SpawnLoot(Random.Range(1, 3), __instance.transform.position);
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }
    }

    [HarmonyPatch(typeof(Structures_OfferingShrine), nameof(Structures_OfferingShrine.Complete))]
    public static class StructuresOfferingShrineCompletePatches
    {
        [HarmonyPrefix]
        public static void Prefix(ref Structures_OfferingShrine __instance)
        {
            __instance.Offerings.Add(Plugin.RebirthItem);
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
                //SpawnLoot(Random.Range(2, 5), __instance.transform.position);
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(2, 5), __instance.transform.position);
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
                //SpawnLoot(Random.Range(3, 6), __instance.transform.position);
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(3, 6), __instance.transform.position);
            }
        }
    }
}