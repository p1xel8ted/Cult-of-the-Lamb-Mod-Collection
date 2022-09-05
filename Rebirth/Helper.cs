using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MonoMod.Utils;

namespace Rebirth;

[HarmonyPatch]
public static class Helper
{
    [HarmonyPatch(typeof(FollowerRecruit), nameof(FollowerRecruit.Update))]
    public static class FollowerRecruitUpdate
    {
        // [HarmonyPrefix]
        // public static void Prefix(ref FollowerRecruit __instance)
        // {
        //     Plugin.Log.LogWarning($"Triggered: {__instance.triggered}");
        //     __instance.triggered = false;
        // }
        //
        [HarmonyDebug]
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(FollowerRecruit), nameof(FollowerRecruit.Update))]
        public static IEnumerable<CodeInstruction> InteractionOuthouseTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
           
            var codes = new List<CodeInstruction>(instructions);
 
            var newValue = 20f;
            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Call && 
                    codes[i + 1].opcode == OpCodes.Callvirt && 
                    codes[i + 2].opcode == OpCodes.Call && 
                    codes[i + 3].opcode == OpCodes.Ldc_R4 && 
                    codes[i + 4].opcode == OpCodes.Bge_Un)
                {
                    editIndex = i + 3;

                    codes[editIndex].operand = newValue;

                    break;
                }
            }

            Plugin.Log.LogWarning(editIndex != -1 ? $"Found distance position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {newValue}" : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");

            return codes.AsEnumerable();
        }
    }
}