using HarmonyLib;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class DebugPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LocationManager), nameof(LocationManager.CheckExistingStructure))]
    public static bool LocationManager_CheckExistingStructure(Structure s)
    {
        if (s is not null) return true;
        Plugin.L($"LocationManager_CheckExistingStructure: Found null structure. Skipping check.");
        return false;
    }

}