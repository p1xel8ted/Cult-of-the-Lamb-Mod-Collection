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
        return false;
    }

    [HarmonyPatch(typeof(LoadMainMenu), nameof(LoadMainMenu.Start))]
    public static class LoadMainMenuStart
    {
        [HarmonyPrefix]
        public static bool Prefix(ref bool __state)
        {
            if (AlreadyRun) return true;
            if (Plugin.SkipDevIntros.Value)
            {
                __state = true;
                return false;
            }

            __state = false;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(ref bool __state)
        {
            if (AlreadyRun) return;
            if (!__state) return;
            if (!Plugin.SkipDevIntros.Value) return;
            AlreadyRun = true;
            AudioManager.Instance.enabled = true;

            MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 0f, "", null);
        }
    }
}