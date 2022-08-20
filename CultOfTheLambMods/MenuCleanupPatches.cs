using HarmonyLib;
using Lamb.UI.MainMenu;
using MMTools;

namespace CultOfTheLambMods
{
    public static class MenuCleanupPatches
    {
        [HarmonyPatch(typeof(LoadMainMenu), "Start")]
        public static class LoadMainMenuStartPatches
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (!Plugin.SkipIntros.Value) return;
                MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade,
                    "Main Menu", 1f, "", null);
            }
        }

        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        public static class MainMenuStartPatches
        {
            [HarmonyPostfix]
            public static void Postfix(ref UnityEngine.UI.Button ____creditsButton, ref UnityEngine.UI.Button ____roadmapButton)
            {
                if (!Plugin.RemoveMenuClutter.Value) return;
                ____creditsButton.gameObject.SetActive(false);
                ____roadmapButton.gameObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(UIMainMenuController), nameof(UIMainMenuController.Awake))]
        public static class UiMainMenuControllerAwakePatches
        {
            [HarmonyPostfix]
            public static void Postfix(ref MMButton ____bugReportButton, ref MMButton ____discordButton)
            {
                if (!Plugin.RemoveMenuClutter.Value) return;
                ____bugReportButton.gameObject.SetActive(false);
                ____discordButton.gameObject.SetActive(false);
            }
        }
    }
}