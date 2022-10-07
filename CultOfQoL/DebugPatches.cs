using System.Collections.Generic;
using HarmonyLib;
using Lamb.UI.FollowerSelect;

namespace CultOfQoL;

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