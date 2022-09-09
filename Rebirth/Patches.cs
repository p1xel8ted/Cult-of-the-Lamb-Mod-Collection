using System.Linq;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;

namespace Rebirth;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPatch(typeof(Lamb.UI.InventoryMenu), nameof(Lamb.UI.InventoryMenu.OnShowStarted))]
    public static class InventoryMenuPatches
    {
        [HarmonyPrefix]
        public static void Prefix(ref Lamb.UI.InventoryMenu __instance)
        {
            __instance._currencyFilter.Add(Plugin.RebirthItem);
        }
    }

    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    public static void Postfix(Health Victim)
    {
        if (Victim.team != Health.Team.Team2) return; //Team2 = stuff that want's to kill the player
        if (Random.Range(0f, 1f) <= 0.1f) //10% chance to drop between 1 and 3 items on death of any enemy (not a critter/bone pile/grass etc)
        {
            Inventory.AddItem(Plugin.RebirthItem, Random.Range(1, 4), true);
        }
    }
}