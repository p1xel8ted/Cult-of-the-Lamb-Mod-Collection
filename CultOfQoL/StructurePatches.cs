using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

[HarmonyPatch]
internal static class StructurePatches
{
    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.LifeSpawn), MethodType.Getter)]
    public static class StructuresLumberjackStationGetAgePatches
    {
        [HarmonyPostfix]
        public static void Postfix(Structures_LumberjackStation __instance, ref int __result)
        {
            var old = __result;
            var newSpan = 0;
            if (Plugin.LumberAndMiningStationsDontAge.Value)
            {
                __result = newSpan;
                return;
            }

            if (Plugin.DoubleLifespanInstead.Value)
            {
                newSpan = old * 2;
                __result = newSpan;
                Plugin.L($"DOUBLE: Lumber/mining old lifespan {old}, new lifespan: {newSpan}. Current age: {__instance.Data.Age}.");
                return;
            }

            if (!Plugin.FiftyPercentIncreaseToLifespanInstead.Value) return;

            newSpan = (int) (old * 1.5f);
            __result = newSpan;
            Plugin.L($"50%: Lumber/mining old lifespan {old}, new lifespan: {newSpan}. Current age: {__instance.Data.Age}.");
        }
    }


    [HarmonyPatch(typeof(PropagandaSpeaker), nameof(PropagandaSpeaker.Update))]
    public static class PropagandaSpeakerUpdate
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (!Plugin.TurnOffSpeakersAtNight.Value) return true;
            return !TimeManager.IsNight;
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

    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static class TimeManagerOnNewPhaseStarted
    {
        [HarmonyPostfix]
        public static void Postfix(DayPhase phase)
        {
            if (!Plugin.TurnOffSpeakersAtNight.Value) return;

            if (phase != DayPhase.Night) return;

            var structures = Object.FindObjectsOfType<PropagandaSpeaker>();
            foreach (var structure in structures)
            {
                structure.StopAllCoroutines();
                structure.Spine.AnimationState.SetAnimation(0, "off", true);
                var fireOff = structure.onFireOff;
                fireOff?.Invoke();
                AudioManager.Instance.StopLoop(structure.loopedInstance);
                structure.VOPlaying = false;
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
            if (Plugin.UseCustomSoulCapacity.Value)
            {
                __instance.SoulMax = Mathf.CeilToInt(__instance.SoulMax * Plugin.CustomSoulCapacityMulti.Value);
                return;
            }

            if (!Plugin.DoubleSoulCapacity.Value) return;
            __instance.SoulMax *= 2;
        }
    }

    [HarmonyPatch(typeof(Structures_Shrine), "SoulMax", MethodType.Getter)]
    public static class StructuresShrinesSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result)
        {
            if (Plugin.UseCustomSoulCapacity.Value)
            {
                __result = Mathf.CeilToInt(__result * Plugin.CustomSoulCapacityMulti.Value);
                return;
            }

            if (!Plugin.DoubleSoulCapacity.Value) return;

            __result *= 2;
        }
    }


    [HarmonyPatch(typeof(Structures_Shrine_Misfit), "SoulMax", MethodType.Getter)]
    public static class StructuresShrineMisfitSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result)
        {
            if (Plugin.UseCustomSoulCapacity.Value)
            {
                __result = Mathf.CeilToInt(__result * Plugin.CustomSoulCapacityMulti.Value);
                return;
            }

            if (!Plugin.DoubleSoulCapacity.Value) return;

            __result *= 2;
        }
    }

    [HarmonyPatch(typeof(Structures_Shrine_Ratau), "SoulMax", MethodType.Getter)]
    public static class StructuresShrineRatauSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result)
        {
            if (Plugin.UseCustomSoulCapacity.Value)
            {
                __result = Mathf.CeilToInt(__result * Plugin.CustomSoulCapacityMulti.Value);
                return;
            }


            if (!Plugin.DoubleSoulCapacity.Value) return;
            __result *= 2;
        }
    }

    [HarmonyPatch(typeof(Structures_Shrine_Passive), "SoulMax", MethodType.Getter)]
    public static class StructuresShrinePassiveSoulMax
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result)
        {
            if (Plugin.UseCustomSoulCapacity.Value)
            {
                __result = Mathf.CeilToInt(__result * Plugin.CustomSoulCapacityMulti.Value);
                return;
            }

            if (!Plugin.DoubleSoulCapacity.Value) return;
            __result *= 2;
        }
    }

    [HarmonyPatch(typeof(Interaction_SiloFertilizer), nameof(Interaction_SiloFertilizer.OnInteract))]
    [HarmonyPatch(typeof(Interaction_SiloFertilizer), nameof(Interaction_SiloFertilizer.UpdateCapacityIndicators))]
    [HarmonyPrefix]
    public static void Interaction_SiloFertilizer_DepositItem(ref Interaction_SiloFertilizer __instance)
    {
        if (Plugin.UseCustomSiloCapacity.Value)
        {
            __instance.StructureBrain.Capacity = Mathf.Ceil(15 * Plugin.CustomSiloCapacityMulti.Value);
            return;
        }

        if (Plugin.JustRightSiloCapacity.Value)
        {
            __instance.StructureBrain.Capacity = 32f;
        }
    }


    [HarmonyPatch(typeof(Interaction_SiloSeeder), nameof(Interaction_SiloSeeder.OnInteract))]
    [HarmonyPatch(typeof(Interaction_SiloSeeder), nameof(Interaction_SiloSeeder.UpdateCapacityIndicators))]
    [HarmonyPrefix]
    public static void Interaction_SiloSeeder_DepositItem(ref Interaction_SiloSeeder __instance)
    {
        if (Plugin.UseCustomSiloCapacity.Value)
        {
            __instance.StructureBrain.Capacity = Mathf.Ceil(15 * Plugin.CustomSiloCapacityMulti.Value);
            return;
        }

        if (Plugin.JustRightSiloCapacity.Value)
        {
            __instance.StructureBrain.Capacity = 32f;
        }
    }


    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPostfix]
    public static void Structures_Refinery_GetCost(ref List<StructuresData.ItemCost> __result)
    {
        if (!Plugin.AdjustRefineryRequirements.Value) return;
        foreach (var item in __result) item.CostValue = Mathf.CeilToInt(item.CostValue / 2f);
    }
}