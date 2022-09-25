using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

internal static class StructurePatches
{
    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.LifeSpawn), MethodType.Getter)]
    public static class StructuresLumberjackStationGetAgePatches
    {
        [HarmonyPostfix]
        public static void Postfix(Structures_LumberjackStation __instance, ref int __result)
        {
            if (Plugin.LumberAndMiningStationsDontAge.Value) return;
            var old = __result;
            var newSpan = 0;
            __result = newSpan;

            if (Plugin.DoubleLifespanInstead.Value)
            {
                newSpan = old * 2;
                __result *= 2;
                Plugin.L($"Lumber/mining old lifespan {old}, new lifespan: {newSpan}. Current age: {__instance.Data.Age}.");
                return;
            }

            if (!Plugin.FiftyPercentIncreaseToLifespanInstead.Value) return;
            
            newSpan = (int) (old * 1.5f);
            __result = newSpan;
            Plugin.L($"Lumber/mining old lifespan {old}, new lifespan: {newSpan}. Current age: {__instance.Data.Age}.");
        }
    }


    [HarmonyPatch(typeof(PropagandaSpeaker), nameof(PropagandaSpeaker.Update))]
    public static class PropagandaSpeakerUpdate
    {
        [HarmonyPostfix]
        public static void Postfix(ref PropagandaSpeaker __instance)
        {
            if (!Plugin.TurnOffSpeakersAtNight.Value) return;
            if (!__instance.gameObject.activeSelf)
            {
                Plugin.L("Turning speakers on!");
                __instance.gameObject.SetActive(true);
                __instance.OnEnable();
                __instance.OnEnableInteraction();
                
                AudioManager.Instance.PlayLoop(__instance.loopedInstance);
                __instance.VOPlaying = true;
                var fireOn = __instance.onFireOn;
                fireOn?.Invoke();
            }

            if (!TimeManager.IsNight) return;

            __instance.Spine.AnimationState.SetAnimation(0, "off", true);
            var fireOff = __instance.onFireOff;
            fireOff?.Invoke();

            AudioManager.Instance.StopLoop(__instance.loopedInstance);
            __instance.VOPlaying = false;
            __instance.OnDisable();
        }
    }

    [HarmonyPatch(typeof(Structures_PropagandaSpeaker), nameof(Structures_PropagandaSpeaker.OnNewPhaseStarted))]
    public static class StructuresFullUpdate
    {
        //stop fuel being taken when speakers are off
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (!Plugin.TurnOffSpeakersAtNight.Value) return true;
            return !TimeManager.IsNight;
        }
    }
    
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewDay))]
    public static class TimeManagerOnNewPhaseStarted
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (!Plugin.TurnOffSpeakersAtNight.Value) return;
            if (TimeManager.IsNight) return;
            
            var structures = Object.FindObjectsOfType<PropagandaSpeaker>();
            foreach (var structure in structures)
            {
                // if (!structure.enabled || !structure.gameObject.activeSelf)
                // {
                    Plugin.L($"Found sleepy propaganda speaker! {structure.name}. Turning it on!");
                    structure.enabled = true;
                    structure.gameObject.SetActive(true);
                    structure.structure.enabled = true;
                    structure.structure.OnEnable();
                // }
            }
        }
    }


    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.IncreaseAge))]
    public static class StructuresLumberjackStationAgePatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_LumberjackStation __instance)
        {
            if (!Plugin.LumberAndMiningStationsDontAge.Value) return;

            __instance.Data.Age = 0;
            Plugin.L("Resetting age of lumber/mining station to 0!");
        }
    }

    [HarmonyPatch(typeof(Structures_Bed), MethodType.Constructor)]
    public static class StructuresBedSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_Bed __instance)
        {
            if (!Plugin.DoubleSoulCapacity.Value) return;
            __instance.SoulMax *= 2;
        }
    }

    [HarmonyPatch(typeof(Structures_Shrine), "SoulMax", MethodType.Getter)]
    public static class StructuresShrinesSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_Shrine __instance, ref int __result)
        {
            if (!Plugin.DoubleSoulCapacity.Value) return;
            __result *= 2;
        }
    }

    [HarmonyPatch(typeof(Structures_Shrine_Misfit), "SoulMax", MethodType.Getter)]
    public static class StructuresShrineMisfitSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_Shrine_Misfit __instance, ref int __result)
        {
            if (!Plugin.DoubleSoulCapacity.Value) return;
            __result *= 2;
        }
    }


    [HarmonyPatch(typeof(Structures_Shrine_Ratau), "SoulMax", MethodType.Getter)]
    public static class StructuresShrineRatauSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_Shrine_Ratau __instance, ref int __result)
        {
            if (!Plugin.DoubleSoulCapacity.Value) return;
            __result *= 2;
        }
    }


    [HarmonyPatch(typeof(Structures_Shrine_Passive), "SoulMax", MethodType.Getter)]
    public static class StructuresShrinePassiveSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_Shrine_Passive __instance, ref int __result)
        {
            if (!Plugin.DoubleSoulCapacity.Value) return;
            __result *= 2;
        }
    }


    //original author is Matthew-X, I just refactored.
    [HarmonyPatch(typeof(Structures_SiloFertiliser), MethodType.Constructor)]
    public static class StructuresSiloFertiliserBrain
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_SiloFertiliser __instance)
        {
            if (!Plugin.JustRightSiloCapacity.Value) return;
            __instance.Capacity = 32f;
        }
    }

    //original author is Matthew-X, I just refactored.
    [HarmonyPatch(typeof(Structures_SiloSeed), MethodType.Constructor)]
    public static class StructureBrainCreateBrainPatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref Structures_SiloSeed __instance)
        {
            if (!Plugin.JustRightSiloCapacity.Value) return;
            __instance.Capacity = 32f;
        }
    }

    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost), typeof(InventoryItem.ITEM_TYPE))]
    public static class RefineryItemCheckCanAffordPatches
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<StructuresData.ItemCost> __result)
        {
            if (!Plugin.AdjustRefineryRequirements.Value) return;
            foreach (var item in __result) item.CostValue = Mathf.CeilToInt(item.CostValue / 2f);
        }
    }
}