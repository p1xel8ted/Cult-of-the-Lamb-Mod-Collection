using HarmonyLib;

namespace CultOfTheLambMods
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

        [HarmonyPatch(typeof(Interaction_Fishing), "Reel")]
        [HarmonyPatch(typeof(Interaction_Fishing), "Update")]
        public static class InteractionFishingReelPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ref Interaction_Fishing __instance, ref float ___maxReelingSpeed, ref float ___reelingMaxSpeed)
            {
                if (!Plugin.EasyFishing.Value) return;
                if (__instance.state.CURRENT_STATE == StateMachine.State.Reeling)
                {
                    ___maxReelingSpeed = 4f;
                    ___reelingMaxSpeed = 4f;
                }
            }
        }

    }
}