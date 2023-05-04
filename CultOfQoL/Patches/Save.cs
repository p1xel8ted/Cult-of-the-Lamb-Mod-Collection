using System.Linq;
using HarmonyLib;
using Lamb.UI.MainMenu;
using src.UI;
using UnityEngine;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class Save
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.Start))]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.OnShowStarted))]
    public static void LoadMenu_Show(ref LoadMenu __instance)
    {
        if(!Plugin.HideNewGameButtons.Value) return;
        
        var count = __instance._saveSlots.Count(a => a.Occupied);
        if (count == 0) return;

        foreach (var slot in __instance._saveSlots.Where(a => !a.Occupied))
        {
            slot.gameObject.SetActive(false);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIMenuConfirmationWindow), nameof(UIMenuConfirmationWindow.OnShowStarted))]
    public static void UIMenuConfirmationWindow_Show(ref UIMenuConfirmationWindow __instance)
    {
        if (PhotoModeManager.PhotoModeActive) return;
        if (!Plugin.SaveOnQuit.Value) return;
        if (!SaveAndLoad.Loaded) return;
        __instance._headerText.text = "Save & Quit";
        __instance._bodyText.text = "Are you sure you want to quit? Your progress will be saved.";
        __instance.OnConfirm = delegate
        {
            SaveAndLoad.Save();
            Application.Quit();
        };
    }
}