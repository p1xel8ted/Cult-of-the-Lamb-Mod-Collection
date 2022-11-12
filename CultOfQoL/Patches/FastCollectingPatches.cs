using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class FastCollectingPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    public static void Interaction_CollectResourceChest_Update(ref Interaction_CollectResourceChest __instance)
    {
        // L($"Distance to trigger other collect: {__instance.DistanceToTriggerDeposits}");
        var triggerExists = __instance.StructureInfo.Inventory.Exists(a => a.quantity >= Mathf.Abs(Plugin.TriggerAmount.Value));
        __instance.AutomaticallyInteract = false;
        if (Plugin.EnableAutoInteract.Value && (__instance.StructureInfo.Inventory.Count >= Mathf.Abs(Plugin.TriggerAmount.Value) || triggerExists))
        {
            __instance.Activating = true;
            if (Plugin.UseCustomRange.Value)
            {
                Plugin.OtherFastCollect.Value = __instance.DistanceToTriggerDeposits;


                __instance.DistanceToTriggerDeposits = Plugin.OtherFastCollect.Value * Mathf.Abs(Plugin.CustomRangeMulti.Value);
            }
            else
            {
                __instance.DistanceToTriggerDeposits = Plugin.IncreaseRange.Value ? 10f : 5f;
            }

            __instance.AutomaticallyInteract = true;
        }
    }



    [HarmonyPrefix]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
    public static void LumberjackStation_Update(ref LumberjackStation __instance)
    {
        // L($"Distance to trigger lumber collect: {__instance.DistanceToTriggerDeposits}");

        var triggerExists = __instance.StructureInfo.Inventory.Exists(a => a.quantity >= Mathf.Abs(Plugin.TriggerAmount.Value));
        __instance.AutomaticallyInteract = false;
        if (Plugin.EnableAutoInteract.Value && (__instance.StructureInfo.Inventory.Count >= Mathf.Abs(Plugin.TriggerAmount.Value) || triggerExists))
        {
            __instance.Activating = true;
            if (Plugin.UseCustomRange.Value)
            {
                Plugin.LumberFastCollect.Value = __instance.DistanceToTriggerDeposits;


                __instance.DistanceToTriggerDeposits = Plugin.LumberFastCollect.Value * Mathf.Abs(Plugin.CustomRangeMulti.Value);
            }
            else
            {
                __instance.DistanceToTriggerDeposits = Plugin.IncreaseRange.Value ? 10f : 5f;
            }

            __instance.AutomaticallyInteract = true;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.Update))]
    public static void BuildingShrine_Update(ref float ___ReduceDelay, ref float ___Delay)
    {
        if (!Plugin.FastCollecting.Value) return;
        ___ReduceDelay = 0.0f;
        ___Delay = 0.0f;
    }

    [HarmonyPrefix]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.Update))]
    public static void BuildingShrinePassive_Update(ref float ___Delay)
    {
        if (!Plugin.FastCollecting.Value) return;
        ___Delay = 0.0f;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Bed), nameof(Interaction_Bed.OnSecondaryInteract))]
    public static void Interaction_Bed_OnSecondaryInteract(ref Interaction_Bed __instance)
    {
        if (!Plugin.MassCollecting.Value) return;
        var interactions = Object.FindObjectsOfType<Interaction_Bed>().ToList();
        foreach (var interaction in interactions)
        {
            GameManager.GetInstance().StartCoroutine(interaction.GiveReward());
        }
    }


    [HarmonyPatch]
    public static class LootDelayTranspilers
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_Bed), "GiveReward", MethodType.Enumerator)]
        [HarmonyPatch(typeof(Interaction_CollectedResources), "GiveResourcesRoutine", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> TranspilerOne(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            if (!Plugin.FastCollecting.Value) return instructions;

            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_R4),
                    new CodeMatch(OpCodes.Newobj, AccessTools.Constructor(typeof(WaitForSeconds), new[] {typeof(float)})))
                .SetOperandAndAdvance(originalMethod.GetRealDeclaringType().Name.Contains("Resources") ? 0.01f : 0f)
                .InstructionEnumeration();
        }

        //collection speed for Interaction_CollectResourceChest - default speed is 0.1f
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
        [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
        public static IEnumerable<CodeInstruction> InteractionCollectResourceChestTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            if (!Plugin.FastCollecting.Value) return instructions;
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_R4),
                    new CodeMatch(OpCodes.Stfld, delayField))
                .SetOperandAndAdvance(originalMethod.GetRealDeclaringType().Name.Contains("Lumber") ? 0.025f : 0.01f)
                .InstructionEnumeration();
        }


        //collection speed for Interaction_EntranceShrine (dungeon shrines) - default speed is 0.1f
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_RatauShrine), nameof(Interaction_RatauShrine.Update))]
        [HarmonyPatch(typeof(Interaction_EntranceShrine), nameof(Interaction_EntranceShrine.Update))]
        [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.Update))]
        public static IEnumerable<CodeInstruction> InteractionEntranceShrineTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            if (!Plugin.FastCollecting.Value) return instructions;
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_R4),
                    new CodeMatch(OpCodes.Stfld, delayField))
                .SetOperandAndAdvance(originalMethod.GetRealDeclaringType().Name.Contains("Outhouse") ? 0.025f : 0f)
                .InstructionEnumeration();
        }
    }
}