using System.Globalization;
using HarmonyLib;
using Map;
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
                Inventory.AddItem(Plugin.RebirthItem, Random.Range(1, 4));
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (DropLoot())
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                Inventory.AddItem(Plugin.RebirthItem, 1);
            }
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