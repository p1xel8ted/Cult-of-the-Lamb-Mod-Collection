using System.ComponentModel;
using HarmonyLib;
using Lamb.UI.MainMenu;
using Lamb.UI.PauseMenu;
using UnityEngine;

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
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIMainMenuController), nameof(UIMainMenuController.Awake))]
    public static void UIMainMenuController_Awake(ref UIMainMenuController __instance)
    {
        Plugin.UIMainMenuController = __instance;
        if (Plugin.RemoveMenuClutter.Value)
        {
            __instance._discordButton.gameObject.SetActive(false);
            var stuff = GameObject.Find("Main Menu Controller/Main Menu/MainMenuContainer/Right/Transform/");
            if (stuff != null)
            {
                stuff.gameObject.SetActive(false);
            }
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Awake))]
    public static void UIPauseMenuController_Awake(ref UIPauseMenuController __instance)
    {
        if (Plugin.RemoveHelpButtonInPauseMenu.Value)
        {
            __instance._helpButton.gameObject.SetActive(false);
        }
        if (Plugin.RemovePhotoModeButtonInPauseMenu.Value)
        {
            __instance._photoModeButton.gameObject.SetActive(false);
        }
        if (Plugin.RemoveTwitchButtonInPauseMenu.Value)
        {
            __instance._twitchSettingsButton.gameObject.SetActive(false);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MenuAdController), nameof(MenuAdController.Start))]
    //[HarmonyPatch(typeof(MenuAdController), nameof(MenuAdController.Update))]
    public static void MenuAdController_Start(ref MenuAdController __instance)
    {
        var one = GameObject.Find("Main Menu Controller/Main Menu/MainMenuContainer/Right/Ad/Outline");
        var two = GameObject.Find("Main Menu Controller/Main Menu/MainMenuContainer/Right/Ad/Transform");
        one.SetActive(false);
        two.SetActive(false);
        // __instance.ads.Clear();
        // __instance.nonRemoteAds.Clear();
        // __instance._bannerObj = null;
        __instance.gameObject.SetActive(false);
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