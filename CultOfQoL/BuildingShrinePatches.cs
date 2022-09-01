using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using MonoMod.Utils;

namespace CultOfQoL;

[HarmonyPatch]
public static class BuildingShrinePatches
{
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.Update))]
    public static class BuildingShrineOnInteractPatches
    {
        [HarmonyPrefix]
        public static void Prefix(ref float ___ReduceDelay, ref float ___Delay)
        {
            if (!Plugin.FastCollecting.Value) return;
            ___ReduceDelay = 0.0f;
            ___Delay = 0.0f;
        }
        
        [HarmonyPostfix]
        public static void Postfix(ref float ___ReduceDelay, ref float ___Delay)
        {
            if (!Plugin.FastCollecting.Value) return;
            ___ReduceDelay = 0.0f;
            ___Delay = 0.0f;
        }
    }
    
    
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.Update))]
    public static class BuildingShrinePassiveOnInteractPatches
    {
        [HarmonyPrefix]
        public static void Prefix(ref float ___Delay)
        {
            if (!Plugin.FastCollecting.Value) return;
            ___Delay = 0.0f;
        }
        
        [HarmonyPostfix]
        public static void Postfix(ref float ___Delay)
        {
            if (!Plugin.FastCollecting.Value) return;
            ___Delay = 0.0f;
        }
    }

    [HarmonyPatch]
    public static class LootDelayTranspilers
    {
        internal static IEnumerable<MethodBase> TargetMethods()
        {

            var innersBeds = typeof(Interaction_Bed).GetNestedTypes(AccessTools.all);
            var innersCollect = typeof(Interaction_CollectedResources).GetNestedTypes(AccessTools.all);
            
            foreach (var type in innersBeds)
            {
                foreach (var method in type.GetMethods(AccessTools.all))
                {
                    if (method.Name.Contains("MoveNext"))
                    {
                        yield return method;
                    }
                }
            }
            
            foreach (var type in innersCollect)
            {
                foreach (var method in type.GetMethods(AccessTools.all))
                {
                    if (method.Name.Contains("MoveNext"))
                    {
                        yield return method;
                    }
                }
            }
        }

        [HarmonyTranspiler]
        [CanBeNull]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var codes = new List<CodeInstruction>(instructions);

            var editIndex = -1;
            var newValue = 0f;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldc_R4 && codes[i + 2].opcode == OpCodes.Newobj && codes[i + 3].opcode == OpCodes.Stfld)
                {
                    editIndex = i + 1;
                    if (originalMethod.GetRealDeclaringType().Name.Contains("Resources"))
                    {
                        //this sets the loot speed from the chest near the portal. Original value is 0.05f
                        newValue = 0.01f;
                    }
                    codes[editIndex].operand = 0f; //this sets the loot speed of devotion from beds

                    break;
                }
            }
            
            Plugin.Log.LogWarning(editIndex != -1 ? $"Found delay position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {newValue}." : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");

            return codes.AsEnumerable();
        }
    }
    
    [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.Update))]
    public static class InteractionOuthouseLootDelayTranspiler
    {
        [HarmonyTranspiler]
        [CanBeNull]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var codes = new List<CodeInstruction>(instructions);
            var newValue = 0.05f;
            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldc_R4 && codes[i + 2].opcode == OpCodes.Stfld && codes[i + 3].opcode == OpCodes.Ret)
                {
                    editIndex = i + 1;
                    codes[editIndex].operand = newValue; //this sets the poop collect speed from the outhouses. Original value 0.2f

                    break;
                }
            }

            Plugin.Log.LogWarning(editIndex != -1 ? $"Found delay position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {newValue}" : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");

            return codes.AsEnumerable();
        }
    }
}