using System;
using UnityEngine;

namespace CultOfQoL;

public partial class Plugin
{
    private void Update()
    {
        if (Plugin.EnableQuickSaveShortcut.Value && Plugin.SaveKeyboardShortcut.Value.IsUp())
        {
            SaveAndLoad.Save();
            NotificationCentre.Instance.PlayGenericNotification("Game Saved!");
        }
        
        if (Plugin.DisableAd.Value && UIMainMenuController!=null)
        {
            foreach (var comp in UIMainMenuController.ad.GetComponents<UnityEngine.Component>())
            {
                comp.gameObject.SetActive(false);
            }
            UIMainMenuController.ad.gameObject.SetActive(false);
        }
    }
}