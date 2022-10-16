using System.Collections.Generic;
using HarmonyLib;

namespace CultOfQoL;

[HarmonyPatch]
public static class Notifications
{
    private static readonly List<int> StructureID = new();

    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.ShutTrap))]
    [HarmonyPostfix]
    public static void Scarecrow_ShutTrap(ref Scarecrow __instance)
    {
        var name = __instance.Structure.Structure_Info.GetLocalizedName();
        if (Plugin.NotifyOfScarecrowTraps.Value && !NotificationCentre.Instance.notificationsThisFrame.Contains(name))
        {
            NotificationCentre.Instance.PlayGenericNotification($"{name} has caught a {__instance.Bird.name}!");
        }
    }


    [HarmonyPatch(typeof(Structures_Bed), nameof(Structures_Bed.Collapse))]
    [HarmonyPostfix]
    public static void Structures_Bed_Collapse(ref Structures_Bed __instance)
    {
        var name = __instance.Data.GetLocalizedName();
        if (Plugin.NotifyOfBedCollapse.Value && !NotificationCentre.Instance.notificationsThisFrame.Contains(name))
        {
            NotificationCentre.Instance.PlayGenericNotification($"{name} has collapsed!");
        }
    }


    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    [HarmonyPostfix]
    public static void Interaction_AddFuel_Update(ref Interaction_AddFuel __instance)
    {
        if (!Plugin.NotifyOfNoFuel.Value) return;
        var name = __instance.Structure.Structure_Info.GetLocalizedName();
        if (!NotificationCentre.Instance.notificationsThisFrame.Contains(name) && __instance.Structure.Structure_Info.Fuel <= 0 && !StructureID.Contains(__instance.Structure.Structure_Info.ID))
        {
            NotificationCentre.Instance.PlayGenericNotification($"{name} has no fuel!");
            StructureID.Add(__instance.Structure.Structure_Info.ID);
            //return;
        }

        if (StructureID.Contains(__instance.Structure.Structure_Info.ID) && __instance.Structure.Structure_Info.Fuel > 0)
        {
            StructureID.Remove(__instance.Structure.Structure_Info.ID);
        }
    }
}