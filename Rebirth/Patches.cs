using System.Globalization;
using COTL_API.CustomInventory;
using HarmonyLib;
using UnityEngine;

namespace Rebirth;

[HarmonyPatch]
[HarmonyWrapSafe]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    public static void DropLootOnDeath_OnDie(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team == Health.Team.Team2)
        {
            // Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (CustomItemManager.DropLoot(Plugin.RebirthItemInstance))
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            // Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (CustomItemManager.DropLoot(Plugin.RebirthItemInstance))
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }
    }
}