using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
    
    //removes "tween is invalid" log spam
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debugger), nameof(Debugger.LogInvalidTween))]
    public static bool Debugger_LogInvalidTween()
    {
        return false;
    }
    
    //removes "steam informs us controller is null" log spam
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(ControlUtilities), nameof(ControlUtilities.GetCurrentInputType))]
    public static IEnumerable<CodeInstruction> ControlUtilities_GetCurrentInputType(IEnumerable<CodeInstruction> instructions,
        MethodBase originalMethod)
    {
        var codes = new List<CodeInstruction>(instructions);
        codes.RemoveRange(44,7);
        return codes.AsEnumerable();
    }
    
}