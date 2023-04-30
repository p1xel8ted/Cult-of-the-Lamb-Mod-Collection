// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection.Emit;
// using HarmonyLib;
// using UnityEngine;
//
// namespace CultOfQoL.Patches;
//
// [HarmonyPatch]
// public static class Weather
// {
//     [HarmonyPostfix]
//     [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
//     public static void TimeManager_StartNewPhase(TimeManager __instance, ref DayPhase phase)
//     {
//         if (Plugin.MoreDynamicWeather.Value)
//         {
//             if (Plugin.ChangeWeatherOnPhaseChange.Value)
//             {
//                 WeatherController.Instance.CheckWeather();
//                 Plugin.L($"New phase: {phase.ToString()}, changing weather!");
//             }
//         }
//
//         if (Plugin.ShowPhaseNotifications.Value && phase is not DayPhase.Count or DayPhase.None)
//         {
//             NotificationCentre.Instance.PlayGenericNotification($"{phase.ToString()} has started!");
//         }
//     }
//
//
//     /*[HarmonyTranspiler]
//     [HarmonyPatch(typeof(BiomeBaseManager), nameof(BiomeBaseManager.ActivateRoom))]
//     [HarmonyPatch(typeof(RoomSwapManager), nameof(RoomSwapManager.ActivateRoom))]
//     public static IEnumerable<CodeInstruction> ActivateRoom_Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         var instructionList = instructions.ToList();
//         if (!Plugin.MoreDynamicWeather.Value) return instructionList.AsEnumerable();
//         for (var i = 0; i < instructionList.Count; i++)
//         {
//             if (instructionList[i].Calls(typeof(WeatherController).GetMethod(nameof(WeatherController.CheckWeather))))
//             {
//                 instructionList.RemoveRange(i - 1, 2);
//             }
//         }
//
//         return instructionList.AsEnumerable();
//     }*/
//
//
//     // [HarmonyPatch(typeof(WeatherController), nameof(WeatherController.CheckWeather))]
//     // public static class WeatherControllerCheckWeather
//     // {
//     //     [HarmonyPrefix]
//     //     public static void Prefix(ref WeatherController __instance)
//     //     {
//     //         if (!Plugin.MoreDynamicWeather.Value) return;
//     //         __instance.chanceOfRain = 100;
//     //         __instance.chanceOfWind = 100;
//     //         WeatherController.InWeatherOverride = false;
//     //     }
//     //
//     //     [HarmonyTranspiler]
//     //     public static IEnumerable<CodeInstruction> CheckWeather_Transpiler(IEnumerable<CodeInstruction> instructions)
//     //     {
//     //         var instructionList = instructions.ToList();
//     //         if (!Plugin.MoreDynamicWeather.Value) return instructionList.AsEnumerable();
//     //         var countSixteen = 0;
//     //         var countSeventeen = 0;
//     //
//     //         for (var i = 0; i < instructionList.Count; i++)
//     //         {
//     //             if (instructionList[i].opcode != OpCodes.Ldc_I4_S) continue;
//     //             if (instructionList[i].operand is sbyte value)
//     //             {
//     //                 switch (value)
//     //                 {
//     //                     case 16:
//     //                         countSixteen++;
//     //                         instructionList[i].operand = countSixteen switch
//     //                         {
//     //                             1 => Mathf.Abs(Plugin.RainLowerChance.Value),
//     //                             2 => Mathf.Abs(Plugin.WindLowerChance.Value),
//     //                             _ => instructionList[i].operand
//     //                         };
//     //                         instructionList[i + 1].opcode = OpCodes.Bgt_S;
//     //                         break;
//     //                     case 17:
//     //                         countSeventeen++;
//     //                         instructionList[i].operand = countSeventeen switch
//     //                         {
//     //                             1 => Mathf.Abs(Plugin.RainUpperChance.Value),
//     //                             2 => Mathf.Abs(Plugin.WindUpperChance.Value),
//     //                             _ => instructionList[i].operand
//     //                         };
//     //                         instructionList[i + 1].opcode = OpCodes.Blt_S;
//     //                         break;
//     //                 }
//     //             }
//     //         }
//     //
//     //         return instructionList.AsEnumerable();
//     //     }
//     //
//     //     [HarmonyPostfix]
//     //     public static void Postfix(ref WeatherController __instance)
//     //     {
//     //         if (!Plugin.ShowWeatherChangeNotifications.Value) return;
//     //         if (WeatherController.insideBuilding) return;
//     //
//     //         var lightRain = Math.Abs(__instance.weatherManager.RainIntensity - 0.15f) < 0.01f && WeatherController.isRaining && WeatherController.IsActive;
//     //         var heavyRain = Math.Abs(__instance.weatherManager.RainIntensity - 0.25f) < 0.01f && WeatherController.isRaining && WeatherController.IsActive;
//     //         var lightWind = Math.Abs(__instance.weatherManager.windDensity - 0.15f) < 0.01f && WeatherController.IsActive;
//     //         var heavyWind = Math.Abs(__instance.weatherManager.windDensity - 0.25f) < 0.01f && WeatherController.IsActive;
//     //
//     //         if (heavyRain && heavyWind)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Heavy rain and wind!");
//     //         }
//     //         else if (lightRain && lightWind)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Light rain and wind!");
//     //         }
//     //         else if (lightRain && heavyWind)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Light rain and heavy wind!");
//     //         }
//     //         else if (heavyRain && lightWind)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Heavy rain and light wind!");
//     //         }
//     //         else if (heavyRain)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Heavy rain!");
//     //         }
//     //         else if (heavyWind)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Heavy wind!");
//     //         }
//     //         else if (lightRain)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Light rain!");
//     //         }
//     //         else if (lightWind)
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Light wind!");
//     //         }
//     //         else
//     //         {
//     //             NotificationCentre.Instance.PlayGenericNotification("Clear skies!");
//     //         }
//     //     }
//     // }
// }