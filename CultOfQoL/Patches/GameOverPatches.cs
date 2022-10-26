using HarmonyLib;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class GameOverPatches
{
    [HarmonyPatch(typeof(PlayerController), "Update")]
    public static class PlayerControllerUpdatePatch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (!Plugin.DisableGameOver.Value) return;
            DataManager.Instance.GameOverEnabled = false;
            DataManager.Instance.GameOver = false;
            DataManager.Instance.InGameOver = false;
            DataManager.Instance.DisplayGameOverWarning = false;
        }
    }

    [HarmonyPatch(typeof(TimeManager), "StartNewPhase", typeof(DayPhase))]
    public static class TimeManagerStartNewPhasePatch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (!Plugin.DisableGameOver.Value) return;
            DataManager.Instance.GameOverEnabled = false;
            DataManager.Instance.GameOver = false;
            DataManager.Instance.InGameOver = false;
            DataManager.Instance.DisplayGameOverWarning = false;
        }
    }
}