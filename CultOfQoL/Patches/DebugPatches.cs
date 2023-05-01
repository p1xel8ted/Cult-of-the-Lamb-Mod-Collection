using System.Diagnostics;
using HarmonyLib;
using Debugger = DG.Tweening.Core.Debugger;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class DebugPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LocationManager), nameof(LocationManager.CheckExistingStructure))]
    public static bool LocationManager_CheckExistingStructure(Structure? s)
    {
        if (s is not null) return true;
        Plugin.L("LocationManager_CheckExistingStructure: Found null structure. Skipping check.");
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debugger), nameof(Debugger.LogInvalidTween))]
    public static bool Debugger_LogInvalidTween()
    {
        return false;
    }

}