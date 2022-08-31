using HarmonyLib;
using System.Runtime.CompilerServices;

namespace CultOfQoL
{
    public static class BuildingShrinePatches
    {
        [HarmonyPatch(typeof(BuildingShrine), nameof(Interaction_Outhouse.OnInteract))]
        public static class BuildingShrineInteractionOuthouseOnInteract
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