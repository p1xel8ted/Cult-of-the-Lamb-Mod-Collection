using System.Linq;
using HarmonyLib;
using Lamb.UI.MainMenu;
using Lamb.UI.PauseMenu;
using UnityEngine.UI;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class MenuCleanupPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    public static void MainMenu_Start(ref MainMenu __instance)
    {
        if (!Plugin.RemoveMenuClutter.Value) return;
        __instance._creditsButton.gameObject.SetActive(false);
        __instance._roadmapButton.gameObject.SetActive(false);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.Start))]
    public static void LoadMenu_Start(ref LoadMenu __instance)
    {
        if (!Plugin.RemoveNewGameButton.Value) return;
        var saveCount = __instance._saveSlots.Count(slot => SaveAndLoad.SaveExist(slot._saveIndex));
        if (saveCount > 0)
        {
            __instance._saveSlots[0].gameObject.SetActive(false);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIMainMenuController), nameof(UIMainMenuController.Awake))]
    public static void UIMainMenuController_Awake(ref UIMainMenuController __instance)
    {
        if (!Plugin.RemoveMenuClutter.Value) return;
        __instance._discordButton.gameObject.SetActive(false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UITwitchExtensionButton), nameof(UITwitchExtensionButton.Awake))]
    public static void UITwitchExtensionButton_Awake(ref UITwitchExtensionButton __instance)
    {
        if (!Plugin.RemoveTwitchButton.Value) return;
        __instance.gameObject.SetActive(false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UITwitchButton), nameof(UITwitchButton.Awake))]
    public static void UITwitchButton_Awake(ref UITwitchButton __instance)
    {
        if (!Plugin.RemoveTwitchButton.Value) return;
        __instance.gameObject.SetActive(false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Start))]
    public static void UIPauseMenuController_Start(ref UIPauseMenuController __instance)
    {
        if (!Plugin.RemoveMenuClutter.Value) return;
        __instance._bugReportButton.gameObject.SetActive(false);
        __instance._discordButton.gameObject.SetActive(false);
    }
}