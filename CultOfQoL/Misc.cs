using HarmonyLib;

namespace CultOfQoL;

[HarmonyPatch]
public static class Misc
{
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.ShutTrap))]
    public static class ScarecrowPatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref Scarecrow __instance)
        {
            if (Plugin.NotifyOfScarecrowTraps.Value && !NotificationCentre.Instance.notificationsThisFrame.Contains("Scarecrow"))
            {
                NotificationCentre.Instance.PlayGenericNotification($"Scarecrow has caught a {__instance.Bird.name}!");
            }
        }
    }
}