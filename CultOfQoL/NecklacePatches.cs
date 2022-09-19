using HarmonyLib;

namespace CultOfQoL;

[HarmonyPatch]
public static class NecklacePatches
{
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GiveItemRoutine))]
    public static class NecklacePatch
    {
        [HarmonyPrefix]
        public static void Prefix(ref interaction_FollowerInteraction __instance, ref InventoryItem.ITEM_TYPE itemToGive)
        {
            if (!itemToGive.ToString().Contains("Necklace")) return;
            InventoryItem.Spawn(__instance.follower.Brain.Info.Necklace, 1, __instance.follower.transform.position);
            __instance.follower.Brain.Info.Necklace = InventoryItem.ITEM_TYPE.NONE;
            __instance.follower.SetOutfit(FollowerOutfitType.Follower, false);
        }
    }
}