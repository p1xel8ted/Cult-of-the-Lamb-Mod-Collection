using HarmonyLib;

namespace CultOfQoL
{

    public static class FishingPatches
    {
        [HarmonyPatch(typeof(UIFishing), nameof(UIFishing.IsNeedleWithinSection))]
        public static class UiFishingIsNeedleWithinSectionPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                if (!Plugin.EasyFishing.Value) return;
                __result = true;
            }
        }
        
        [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.IsNeedleWithinSection))]
        public static class UiFishingOverlayControllerIsNeedleWithinSectionPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                if (!Plugin.EasyFishing.Value) return;
                __result = true;
            }
        }
    }
}