﻿
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;

namespace CultOfQoL
{
    [HarmonyPatch]
    public static class OuthouseQueuePatches
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(FollowerTask_Bathroom), nameof(FollowerTask_Bathroom.OnDoingBegin))]
        public static IEnumerable<CodeInstruction> InteractionOuthouseTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            var codes = new List<CodeInstruction>(instructions);
            if (Plugin.OuthouseAttemptsWhenInQueue.Value <= 3) return codes.AsEnumerable();
            var tryUseField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "tryUseOuthouseCounter");

            // var startAgain = AccessTools.Method(typeof(FollowerTask), nameof(FollowerTask.StartAgain), new[] {typeof(Follower)});
            // var onDoingBegin = AccessTools.Method(typeof(FollowerTask_Bathroom), nameof(FollowerTask_Bathroom.OnDoingBegin), new[] {typeof(Follower)});
            
            var editIndex = -1;
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldfld && codes[i + 1].operand.Equals(tryUseField))
                {
                    editIndex = i + 2;
        
                    codes[editIndex].operand = Plugin.OuthouseAttemptsWhenInQueue.Value;
        
                    break;
                }
            }

            // var newCodes = new List<CodeInstruction>
            // {
            //     new(OpCodes.Ldarg_0),
            //     new(OpCodes.Ldloc_0),
            //     codes[59],
            //     new(OpCodes.Callvirt, onDoingBegin)
            // };
            //
            // codes.InsertRange(107, newCodes);
            Plugin.Log.LogWarning(editIndex != -1 ? $"Found outhouse queue position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name} at line {editIndex}. New value: {Plugin.OuthouseAttemptsWhenInQueue.Value}" : $"Did not find transpiler position for {originalMethod.GetRealDeclaringType().Name}.{originalMethod.Name}!");
        
            return codes.AsEnumerable();
        }

        private static Interaction_Outhouse _outhouse;

        private class GoldenToilet
        {
            public Structures_Outhouse StructuresOuthouse { get; }
            public Interaction_Outhouse InteractionOuthouse { get; }

            public int ToiletStructureID => StructuresOuthouse.Data.ID;

            public GoldenToilet(Structures_Outhouse structuresOuthouse, Interaction_Outhouse interactionOuthouse)
            {
                StructuresOuthouse = structuresOuthouse;
                InteractionOuthouse = interactionOuthouse;
            }
        }

        private static GoldenToilet FindGoldenToilet()
        {
            var toilets = StructureManager.GetAllStructuresOfType<Structures_Outhouse>(FollowerLocation.Base);
            // foreach (var t in toilets)
            // {
            //     t.ReservedForTask = false;
            // }
            toilets.RemoveAll(a => a.ReservedForTask || a.IsFull);
            toilets.Sort((x, y) => x.GetPoopCount().CompareTo(y.GetPoopCount()));
            
            if (toilets.Count <= 1) return null;
            
            if (toilets[0] == null)
            {
                return null;
            }

            var message = $"\nToilets\n";
            message += "-----------";

            foreach (var t in toilets.ToList())
            {
                message += "-----------\n";
                message += $"ID: {t.Data.ID}\n";
                message += $"Poop: {t.GetPoopCount()}\n";
                message += $"Full: {t.IsFull}\n";
                message += $"TaskReserved: {t.ReservedForTask}\n";
                message += $"PlayerReserved:{t.ReservedByPlayer}\n";
                message += "-----------";
            }

            Plugin.L(message);
            Plugin.L($"Setting toilet to new toilet, id {toilets[0].Data.ID}. Poop count: {toilets[0].GetPoopCount()}.");

            _outhouse = Interaction_Outhouse.Outhouses.Find(a => a.StructureInfo.ID == toilets[0].Data.ID);

            return new GoldenToilet(toilets[0], _outhouse);
        }
        
        private static GoldenToilet _theToilet;
        
        [HarmonyPatch(typeof(FollowerTask_Bathroom),nameof(FollowerTask_Bathroom.OnAbort))]
        [HarmonyPatch(typeof(FollowerTask_Bathroom),nameof(FollowerTask_Bathroom.OnEnd))]
        public static class FollowerTaskBathroomCleanupPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref FollowerTask_Bathroom __instance)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                Plugin.L("Released toilet reservations via OnAbort & OnEnd");
                __instance.ReleaseReservations();
              
            }
        }
        
        [HarmonyPatch(typeof(FollowerTask_Bathroom),"DoorCoroutine")]
        public static class FollowerTaskDoorCoroutinePatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref FollowerTask_Bathroom __instance)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                if (__instance._doorCoroutine == null)
                {
                    Plugin.L("DoorCoroutine is null, releasing toilet reservations.");
                    __instance.ReleaseReservations();
                    __instance._toilet.ReservedForTask = false;
                }
              
              
            }
        }


        
        [HarmonyPatch(typeof(FollowerTask_Bathroom),nameof(FollowerTask_Bathroom.ClaimReservations))]
        public static class FollowerTaskBathroomClaimReservationsPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ref FollowerTask_Bathroom __instance)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                __instance.ReleaseReservations();
                _theToilet = FindGoldenToilet();
                if (_theToilet == null) return;
                
                _theToilet.StructuresOuthouse.ReservedForTask = false;
                __instance._toilet = _theToilet.StructuresOuthouse;
                __instance._toiletID = _theToilet.ToiletStructureID;
            }
        }
        
        [HarmonyPatch(typeof(FollowerTask_Bathroom), nameof(FollowerTask_Bathroom.OnDoingBegin))]
        public static class FollowerTaskBathroomOnDoingBeginPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ref FollowerTask_Bathroom __instance)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                __instance.ReleaseReservations();
                _theToilet = FindGoldenToilet();
                if (_theToilet == null) return;
                __instance._toilet = _theToilet.StructuresOuthouse;
                __instance._toiletID = _theToilet.ToiletStructureID;
                
            }
        }



        [HarmonyPatch(typeof(FollowerTask_Bathroom), MethodType.Constructor, typeof(int))]
        public static class FollowerTaskBathroomConstructorPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref FollowerTask_Bathroom __instance)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                __instance.ReleaseReservations();
               _theToilet = FindGoldenToilet();
                if (_theToilet == null) return;
                __instance._toilet = _theToilet.StructuresOuthouse;
                __instance._toiletID = _theToilet.ToiletStructureID;
                
            }
        }

        [HarmonyPatch(typeof(FollowerTask_Bathroom), "FindOuthouse")]
        public static class FollowerTaskBathroomOnStartPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref FollowerTask_Bathroom __instance, ref Interaction_Outhouse __result)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                __instance.ReleaseReservations();
                _theToilet = FindGoldenToilet();
                if (_theToilet == null) return;
                 __instance._toilet = _theToilet.StructuresOuthouse;
                 __instance._toiletID = _theToilet.ToiletStructureID;
                // __instance.tryUseOuthouseCounter = 0;
                _theToilet.InteractionOuthouse.StructureInfo.FollowerID = __instance.Brain.Info.ID;
                __result = _theToilet.InteractionOuthouse;
            }
        }
    }
}