using HarmonyLib;
using MMTools;

namespace CultOfQoL;

public static class SkipIntros
{
    private static bool AlreadyRun { get; set; }

    [HarmonyPatch]
    public static class SkipIntrosPatch
    {
        [HarmonyPatch(typeof(LoadMainMenu), "Start")]
        public static class LoadMainMenuStart
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __state)
            {
                if (AlreadyRun) return true;
                if (Plugin.SkipIntros.Value)
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
                if (!Plugin.SkipIntros.Value) return;
                AlreadyRun = true;
                AudioManager.Instance.enabled = true;

                MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 0f, "", null);
            }
        }
    }
}