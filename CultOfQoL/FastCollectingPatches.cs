using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MonoMod.Utils;

namespace CultOfQoL;

[HarmonyPatch]
public static class FastCollectingPatches
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
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_Bed), "GiveReward", MethodType.Enumerator)]
        [HarmonyPatch(typeof(Interaction_CollectedResources), "GiveResourcesRoutine", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> TranspilerOne(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var codes = new List<CodeInstruction>(instructions);
            if (!Plugin.FastCollecting.Value) return codes.AsEnumerable();
            var editIndex = -1;
            var newValue = 0f;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldc_R4 && codes[i + 2].opcode == OpCodes.Newobj && codes[i + 2].operand.ToString().Contains("ctor") && codes[i + 3].opcode == OpCodes.Stfld)
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

        //collection speed for Interaction_Outhouse - default speed is 0.2f
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.Update))]
        public static IEnumerable<CodeInstruction> InteractionOuthouseTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            var codes = new List<CodeInstruction>(instructions);
            if (!Plugin.FastCollecting.Value) return codes.AsEnumerable();
            var newValue = 0.05f;
            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldc_R4 && codes[i + 2].opcode == OpCodes.Stfld && codes[i + 2].operand.Equals(delayField) && codes[i + 3].opcode == OpCodes.Ret)
                {
                    editIndex = i + 1;

                    codes[editIndex].operand = newValue;

                    break;
                }
            }

            Plugin.L(editIndex != -1 ? $"Found delay position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {newValue}" : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");

            return codes.AsEnumerable();
        }
        
        
        [HarmonyPatch(typeof(Interaction_CollectResourceChest), "Update")]
        [HarmonyPrefix]
        public static void InteractionCollectResourceChestPrefix(ref Interaction_CollectResourceChest __instance)
        {
            var triggerExists = __instance.StructureInfo.Inventory.Exists(a => a.quantity >= Plugin.TriggerAmount.Value);
            __instance.AutomaticallyInteract = false;
            if (Plugin.EnableAutoInteract.Value && (__instance.StructureInfo.Inventory.Count >= Plugin.TriggerAmount.Value || triggerExists))
            {
                __instance.Activating = true;
                __instance.DistanceToTriggerDeposits = Plugin.IncreaseRange.Value ? 10f : 5f;
                __instance.AutomaticallyInteract = true;
            }
        }
        
        
        [HarmonyPatch(typeof(LumberjackStation), "Update")]
        [HarmonyPrefix]
        public static void LumberjackStationPrefix(ref LumberjackStation __instance)
        {
            var triggerExists = __instance.StructureInfo.Inventory.Exists(a => a.quantity >= Plugin.TriggerAmount.Value);
            __instance.AutomaticallyInteract = false;
            if (Plugin.EnableAutoInteract.Value && (__instance.StructureInfo.Inventory.Count >= Plugin.TriggerAmount.Value || triggerExists))
            {
                __instance.Activating = true;
                __instance.DistanceToTriggerDeposits = Plugin.IncreaseRange.Value ? 10f : 5f;
                __instance.AutomaticallyInteract = true;
            }
        }

        //collection speed for Interaction_CollectResourceChest - default speed is 0.1f
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
        [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
        public static IEnumerable<CodeInstruction> InteractionCollectResourceChestTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            var codes = new List<CodeInstruction>(instructions);
            if (!Plugin.FastCollecting.Value) return codes.AsEnumerable();
            var newValue = 0.01f;
            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Call && codes[i + 1].opcode == OpCodes.Ldarg_0 && codes[i + 2].opcode == OpCodes.Ldc_R4 && codes[i + 3].opcode == OpCodes.Stfld && codes[i + 3].operand.Equals(delayField))
                {
                    editIndex = i + 2;
                    if (originalMethod.GetRealDeclaringType().Name.Contains("Lumber"))
                    {
                        newValue = 0.025f;
                    }

                    codes[editIndex].operand = newValue;

                    break;
                }
            }

            Plugin.L(editIndex != -1 ? $"Found delay position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {newValue}" : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");

            return codes.AsEnumerable();
        }

        //collection speed for Interaction_RatauShrine - default speed is 0.2f
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_RatauShrine), nameof(Interaction_RatauShrine.Update))]
        public static IEnumerable<CodeInstruction> InteractionRatauShrineTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            var codes = new List<CodeInstruction>(instructions);
            if (!Plugin.FastCollecting.Value) return codes.AsEnumerable();
            var newValue = 0f;
            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Callvirt && codes[i + 1].opcode == OpCodes.Ldarg_0 && codes[i + 2].opcode == OpCodes.Ldc_R4 && codes[i + 3].opcode == OpCodes.Stfld && codes[i + 3].operand.Equals(delayField))
                {
                    editIndex = i + 2;

                    codes[editIndex].operand = newValue;

                    break;
                }
            }

            Plugin.L(editIndex != -1 ? $"Found delay position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {newValue}" : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");

            return codes.AsEnumerable();
        }
    }
}