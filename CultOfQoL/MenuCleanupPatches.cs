using System.Collections;
using HarmonyLib;
using Lamb.UI.MainMenu;
using Lamb.UI.PauseMenu;
using MMTools;

namespace CultOfQoL
{
    public static class MenuCleanupPatches
    {

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

        [HarmonyPatch(typeof(UITwitchExtensionButton), "Awake")]
        public static class UiTwitchExtensionButtonAwake
        {
            [HarmonyPostfix]
            public static void Postfix(ref UITwitchExtensionButton __instance)
            {
                if (!Plugin.RemoveTwitchButton.Value) return;
                __instance.gameObject.SetActive(false);

            }
        }

        [HarmonyPatch(typeof(UITwitchButton), "Awake")]
        public static class UiTwitchButtonAwake
        {
            [HarmonyPostfix]
            public static void Postfix(ref UITwitchButton __instance)
            {
                if (!Plugin.RemoveTwitchButton.Value) return;
                __instance.gameObject.SetActive(false);

            }
        }

        [HarmonyPatch(typeof(UIPauseMenuController), "Start")]
        public static class UiPauseMenuControllerStartPatches
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