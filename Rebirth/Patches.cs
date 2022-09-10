using HarmonyLib;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    public static void Postfix(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team != Health.Team.Team2) return;
        if (Random.Range(0f, 1f) <= 0.2f * DataManager.Instance.GetLuckMultiplier())
        {
            var qty = Random.Range(1, 4);
            Inventory.AddItem(Plugin.RebirthItem, qty);
        }
    }
}