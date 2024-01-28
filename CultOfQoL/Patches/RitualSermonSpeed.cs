﻿namespace CultOfQoL.Patches;

[Harmony]
[HarmonyWrapSafe]
public static class RitualSermonSpeed
{
    internal static bool RitualRunning { get; set; }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoRitual))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoSermon))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoDoctrine))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoPlayerUpgrade))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoCultUpgrade))]
    private static void Interaction_TempleAltar_Do()
    {
        if (Plugin.FastRitualSermons.Value)
        {
            RitualRunning = true;
        }
    }

   
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Follower), nameof(Follower.UseUnscaledTime), MethodType.Getter)]
    private static void Follower_UseUnscaledTime_Get(ref bool __result)
    {
        if (!RitualRunning) return;
        __result = false;
    }

   
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Follower), nameof(Follower.UseUnscaledTime), MethodType.Setter)]
    private static void Follower_UseUnscaledTime_Set(ref bool value)
    {
        if (!RitualRunning) return;
        value = false;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.RitualOnEnd))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoCancel))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.Close))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.CloseAndSpeak))]
    private static void Interaction_TempleAltar_End()
    {
        if (Plugin.FastRitualSermons.Value)
        {
            RitualRunning = false;
            GameManager.SetTimeScale(1);
        }
    }

    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    private static void GameManager_Update()
    {
        if (!RitualRunning || !Plugin.FastRitualSermons.Value) return;

        GameManager.SetTimeScale(10); //set this too fast and stuff starts to break...
    }
}