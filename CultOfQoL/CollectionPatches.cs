using HarmonyLib;
using System.Runtime.CompilerServices;

namespace CultOfQoL
{
    public static class CollectingPatches
    {
        [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.OnInteract))]
        public static class OutHouse
        {
            class Patch
            {
                [HarmonyReversePatch]
                [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract))]
                [MethodImpl(MethodImplOptions.NoInlining)]
                public static void InteractPatch(StateMachine state) { }
            }

            [HarmonyPrefix]
            public static void Prefix(StateMachine state, ref bool ___Activating)
            {
                if (!Plugin.FastCollecting.Value) return;
                if (!___Activating)
                {
                    Patch.InteractPatch(state);
                }
            }
        }

        [HarmonyPatch(typeof(BuildingShrine), nameof(Interaction_Outhouse.OnInteract))]
        public static class ShrineBuildings
        {
            [HarmonyPrefix]

            public static void Prefix(ref float ___ReduceDelay)
            {
                if (!Plugin.FastCollecting.Value) return;
                ___ReduceDelay = 0.0f;
            }
        }
    }
}