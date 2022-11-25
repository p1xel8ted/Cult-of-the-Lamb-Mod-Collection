﻿using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class FishingPatches
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.SetState))]
    public static IEnumerable<CodeInstruction> TranspilerOne(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        if (Plugin.EasyFishing is {Value: false}) return instructions;

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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.IsNeedleWithinSection))]
    public static void UIFishingOverlayController_IsNeedleWithinSection(ref bool __result)
    {
        if (Plugin.EasyFishing is {Value: false}) return;
        __result = true;
    }
}