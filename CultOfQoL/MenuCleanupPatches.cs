using HarmonyLib;
using Lamb.UI.MainMenu;
using Lamb.UI.PauseMenu;

namespace CultOfQoL;

[HarmonyPatch]
public static class MenuCleanupPatches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    [HarmonyPostfix]
    public static void MainMenu_Start(ref MainMenu __instance)
    {
        if (!Plugin.RemoveMenuClutter.Value) return;
        __instance._creditsButton.gameObject.SetActive(false);
        __instance._roadmapButton.gameObject.SetActive(false);
    }


    [HarmonyPatch(typeof(UIMainMenuController), nameof(UIMainMenuController.Awake))]
    [HarmonyPostfix]
    public static void UIMainMenuController_Awake(ref UIMainMenuController __instance)
    {
        if (!Plugin.RemoveMenuClutter.Value) return;
        __instance._discordButton.gameObject.SetActive(false);
    }


    [HarmonyPatch(typeof(UITwitchExtensionButton), nameof(UITwitchExtensionButton.Awake))]
    [HarmonyPostfix]
    public static void UITwitchExtensionButton_Awake(ref UITwitchExtensionButton __instance)
    {
        if (!Plugin.RemoveTwitchButton.Value) return;
        __instance.gameObject.SetActive(false);
    }


    [HarmonyPatch(typeof(UITwitchButton), nameof(UITwitchButton.Awake))]
    [HarmonyPostfix]
    public static void UITwitchButton_Awake(ref UITwitchButton __instance)
    {
        if (!Plugin.RemoveTwitchButton.Value) return;
        __instance.gameObject.SetActive(false);
    }


    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Start))]
    [HarmonyPostfix]
    public static void UIPauseMenuController_Start(ref UIPauseMenuController __instance)
    {
        if (!Plugin.RemoveMenuClutter.Value) return;
        __instance._bugReportButton.gameObject.SetActive(false);
        __instance._discordButton.gameObject.SetActive(false);
    }
}