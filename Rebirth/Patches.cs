using System.Collections;
using System.Globalization;
using COTL_API.CustomInventory;
using COTL_API.CustomObjectives;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
[HarmonyWrapSafe]
public static class Patches
{
    private static readonly RebirthItem RebirthItem = new();

    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    [HarmonyPrefix]
    public static void Prefix(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team == Health.Team.Team2)
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (CustomItemManager.DropLoot(RebirthItem))
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            Plugin.Log.LogWarning($"Victim: {__instance.name}, Team: {Victim.team}");
            if (CustomItemManager.DropLoot(RebirthItem))
            {
                Plugin.Log.LogWarning($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }
    }

    private static IEnumerator GiveLoot(Vector3 position)
    {
        yield return new WaitForSeconds(2f);
        for (var i = 0; i < 25; i++)
        {
            ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, position, Plugin.RebirthItem, null);
            yield return new WaitForSeconds(0.05f);
        }

        Inventory.AddItem(Plugin.RebirthItem, 25);
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract), typeof(StateMachine))]
    [HarmonyPostfix]
    public static void Postfix(interaction_FollowerInteraction __instance)
    {
        var questInstance = Quests.QuestsAll.Find(a => a.ID == Plugin.RebirthSacrificeFollowerQuest.ObjectiveData.ID);

        if (questInstance is {IsComplete: true})
        {
            __instance.StartCoroutine(GiveLoot(__instance.follower.transform.position));
        }
    }


    [HarmonyPatch(typeof(Quests), nameof(Quests.GetRandomBaseQuest))]
    public static class QuestsPatches
    {
        [HarmonyPostfix]
        public static void QuestsGetRandomBaseQuest(ref ObjectivesData __result)
        {
            if (__result != null && !Quests.ObjectiveAlreadyActive(Plugin.RebirthSacrificeFollowerQuest.ObjectiveData))
            {
                Plugin.Log.LogWarning($"Overriding Quest: {__result.Type.ToString()}");
                //TODO: Check Quest ID's arent matching
                Plugin.RebirthSacrificeFollowerQuest.ObjectiveData.ResetInitialisation();
                Plugin.RebirthSacrificeFollowerQuest.ObjectiveData.Follower = __result.Follower;
                __result = Plugin.RebirthSacrificeFollowerQuest.ObjectiveData;
            }
        }
    }
}