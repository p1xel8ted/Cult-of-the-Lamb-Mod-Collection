using HarmonyLib;
using Map;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class Weather
{
    private static WeatherSystemController? WeatherSystemController { get; set; }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(TimeManager __instance, ref DayPhase phase)
    {
        if (Plugin.ChangeWeatherOnPhaseChange.Value)
        {
            if (WeatherSystemController != null)
            {
                var weather = WeatherSystemController.weatherData.RandomElement();
                WeatherSystemController.SetWeather(weather.WeatherType, weather.WeatherStrength, 3f);
            }

            Plugin.L($"New phase: {phase.ToString()}, changing weather!");
        }


        if (Plugin.ShowPhaseNotifications.Value && phase is not DayPhase.Count or DayPhase.None)
        {
            NotificationCentre.Instance.PlayGenericNotification($"{phase.ToString()} has started!");
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.Awake))]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.Start))]
    public static void WeatherSystemController_Assign(ref WeatherSystemController? __instance)
    {
        WeatherSystemController = __instance;  
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.SetWeather))]
    public static void WeatherSystemController_SetWeather_Prefix(ref WeatherSystemController __instance, ref WeatherSystemController.WeatherType weatherType,
        ref WeatherSystemController.WeatherStrength weatherStrength)
    {
        if (Plugin.RandomWeatherChangeWhenExitingArea.Value)
        {
            if (WeatherSystemController != null)
            {
                var weather = WeatherSystemController.weatherData.Random();
                weatherType = weather.WeatherType;
                weatherStrength = weather.WeatherStrength;
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.SetWeather))]
    public static void WeatherSystemController_SetWeather_Postfix(ref WeatherSystemController.WeatherType weatherType,
        WeatherSystemController.WeatherStrength weatherStrength, float transitionDuration)
    {
        if (!Plugin.ShowWeatherChangeNotifications.Value) return;
        
        var ws = string.Empty;
        switch (weatherType)
        {
            case WeatherSystemController.WeatherType.None:
                break;
            case WeatherSystemController.WeatherType.Raining:
                ws = "Rain";
                break;
            case WeatherSystemController.WeatherType.Windy:
                ws = "Wind";
                break;
            case WeatherSystemController.WeatherType.Snowing:
                ws = "Snow";
                break;
            default:
                return;
        }

        NotificationCentre.Instance.PlayGenericNotification(weatherType == WeatherSystemController.WeatherType.None
            ? "Weather cleared!"
            : $"Weather changed to {weatherStrength.ToString()} {ws}!");
    }
}