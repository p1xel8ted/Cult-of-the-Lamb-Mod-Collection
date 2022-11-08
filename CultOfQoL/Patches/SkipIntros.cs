using HarmonyLib;
using MMTools;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class SkipIntros
{
    private static bool AlreadyRun { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(IntroDeathSceneManager), nameof(IntroDeathSceneManager.GiveCrown))]
    public static bool IntroDeathSceneManager_GiveCrownOne(ref IntroDeathSceneManager __instance)
    {
        if (!Plugin.SkipCrownVideo.Value) return true;
        __instance.VideoComplete();
        DataManager.Instance.HadInitialDeathCatConversation = true;
        return false;
    }

    [HarmonyPatch(typeof(LoadMainMenu), nameof(LoadMainMenu.Start))]
    [HarmonyPrefix]
    public static bool Prefix()
    {
        if (AlreadyRun) return true;
        if (!Plugin.SkipDevIntros.Value) return true;
        AlreadyRun = true;
        AudioManager.Instance.enabled = true;
        MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 0f, "", null);
        return false;
    }
}