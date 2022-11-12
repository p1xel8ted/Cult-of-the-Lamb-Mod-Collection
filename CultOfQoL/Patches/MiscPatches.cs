using System;
using DG.Tweening.Core;
using HarmonyLib;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public class MiscPatches
{
    //for some reason, args is being passed null, which eventually makes its way to _extraText and causes a null reference exception
    //having this here stops the null
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationFaith), nameof(NotificationFaith.Localize))]
    public static void NotificationFaith_Localize(ref NotificationFaith __instance)
    {
        if (__instance._extraText is {Length: > 0})
        {
            foreach (var s in __instance._extraText)
            {
                Plugin.L($"ExtraText: {s}");
            }
        }
        else
        {
            __instance._extraText ??= Array.Empty<string>();
        }
    }

}