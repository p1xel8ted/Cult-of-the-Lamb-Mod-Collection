using HarmonyLib;
using Lamb.UI;

namespace Namify;

[HarmonyPatch]
public static class Patches
{
    private static string _pendingName = string.Empty;

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    private static void SaveAndLoad_Save()
    {
        if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO) return;
        Data.SaveData();
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    private static void SaveAndLoad_Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;
        Data.LoadData();
    }

    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController), nameof(UIFollowerIndoctrinationMenuController.Show), typeof(Follower), typeof(OriginalFollowerLookData), typeof
        (bool))]
    [HarmonyPostfix]
    private static void UIFollowerIndoctrinationMenuController_Show(ref UIFollowerIndoctrinationMenuController __instance)
    {
        var instance = __instance;
        __instance._acceptButton.onClick.AddListener(delegate
        {
            var name = instance._targetFollower.Brain.Info.Name;
            if (name != _pendingName) return;
            Plugin.Log.LogInfo($"Follower name {name} confirmed! Removing name from saved name list.");
            Data.Names.RemoveAll(n => n == _pendingName);
        });
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.GenerateName))]
    public static void FollowerInfo_GenerateName(ref string __result)
    {
        if (GameManager.GetInstance() is null) return;
        if (DataManager.Instance is null) return;
        Data.GetNames();
        if (Data.Names.Count <= 0) return;
        if (__result == _pendingName) return;

        _pendingName = Data.Names.RandomElement();

        __result = _pendingName;
    }
}