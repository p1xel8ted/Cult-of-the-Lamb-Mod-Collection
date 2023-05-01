using System;

namespace CultOfQoL;

public partial class Plugin
{
    private void Update()
    {
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