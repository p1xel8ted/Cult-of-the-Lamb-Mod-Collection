using System.Collections.Generic;
using HarmonyLib;

namespace CultOfQoL;

[HarmonyPatch]
public static class Notifications
{
    private static readonly List<int> StructureID = new();

    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.ShutTrap))]
    public static class ScarecrowPatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref Scarecrow __instance)
        {
            if (Plugin.NotifyOfScarecrowTraps.Value && !NotificationCentre.Instance.notificationsThisFrame.Contains("Scarecrow"))
            {
                NotificationCentre.Instance.PlayGenericNotification($"{__instance.Structure.Structure_Info.GetLocalizedName()} has caught a {__instance.Bird.name}!");
            }
        }
    }

    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    public static class InteractionAddFuelPatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref Interaction_AddFuel __instance)
        {
            if (!Plugin.NotifyOfNoFuel.Value) return;
            if (!NotificationCentre.Instance.notificationsThisFrame.Contains("fuel") && __instance.Structure.Structure_Info.Fuel <= 0 && !StructureID.Contains(__instance.Structure.Structure_Info.ID))
            {
                NotificationCentre.Instance.PlayGenericNotification($"Structure {__instance.Structure.Structure_Info.GetLocalizedName()} has no fuel!");
                StructureID.Add(__instance.Structure.Structure_Info.ID);
                //return;
            }

            if (StructureID.Contains(__instance.Structure.Structure_Info.ID) && __instance.Structure.Structure_Info.Fuel > 0)
            {
                StructureID.Remove(__instance.Structure.Structure_Info.ID);
            }
        }
    }
}