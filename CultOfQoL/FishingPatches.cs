using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace CultOfQoL;

[HarmonyPatch]
public static class FishingPatches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.SetState))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> TranspilerOne(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        if (!Plugin.EasyFishing.Value) return instructions;

        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.reelingCanvasGroup))),
                new CodeMatch(OpCodes.Ldc_R4),
                new CodeMatch(OpCodes.Ldc_R4))
            .Repeat((matcher => matcher
                .Advance(1)
                .SetOperandAndAdvance(0f)
                .SetOperandAndAdvance(0f)))
            .InstructionEnumeration();
    }


    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.IsNeedleWithinSection))]
    [HarmonyPostfix]
    public static void UIFishingOverlayController_IsNeedleWithinSection(ref bool __result)
    {
        if (!Plugin.EasyFishing.Value) return;
        __result = true;
    }
}