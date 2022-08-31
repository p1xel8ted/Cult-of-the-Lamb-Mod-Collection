using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace CultOfQoL;

public static class BuildingShrinePatches
{
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnInteract))]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.Update))]
    public static class BuildingShrineBuildingShrineOnInteract
    {
        [HarmonyPrefix]
        public static void Prefix(ref float ___ReduceDelay, ref float ___Delay)
        {
            if (!Plugin.FastCollecting.Value) return;
            ___ReduceDelay = 0.0f;
            ___Delay = 0.0f;
        }
    }

    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.OnInteract))]
    [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.OnInteract))]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.OnInteract))]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.Update))]
    public static class BuildingShrinePassiveOnInteractPatches
    {
        [HarmonyPrefix]
        public static void Prefix(ref float ___Delay)
        {
            if (!Plugin.FastCollecting.Value) return;
            ___Delay = 0.0f;
        }
    }

    [HarmonyPatch(typeof(Interaction_Bed))]
    public static class InteractionBedGiveRewardPatch
    {
        internal static IEnumerable<MethodBase> TargetMethods()
        {
            var inner = typeof(Interaction_Bed).GetNestedType("<GiveReward>d__29", AccessTools.all)
                        ?? throw new Exception("Inner Not Found");

            foreach (var method in inner.GetMethods(AccessTools.all))
            {
                if (method.Name.Contains("MoveNext"))
                {
                    yield return method;
                }
            }
        }

        [HarmonyTranspiler]
        [CanBeNull]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldc_R4 && codes[i + 2].opcode == OpCodes.Newobj && codes[i + 3].opcode == OpCodes.Stfld)
                {
                    editIndex = i + 1;
                    codes[editIndex].operand = 0f;

                    break;
                }
            }

            Plugin.Log.LogWarning(editIndex != -1 ? $"Found transpiler position for BedGiveReward at line {editIndex}." : "Did not find transpiler position for Interaction_Bed GiveReward!");

            return codes.AsEnumerable();
        }
    }
}