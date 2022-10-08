using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

[HarmonyPatch]
public static class Extort
{
    internal static IEnumerator ExtortMoneyRoutine(Follower follower, interaction_FollowerInteraction instance)
    {
        follower.Brain.Stats.PaidTithes = true;
        instance.eventListener.PlayFollowerVO(instance.generalAcknowledgeVO);
        var position = follower.transform.position;
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
        yield return new WaitForSeconds(0.2f);
        ResourceCustomTarget.Create(instance.state.gameObject, position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
    }
}