using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CultOfQoL
{
    internal class SiloCopacityPatch
    {
        [HarmonyPatch(typeof(Structures_SiloFertiliser))]
        public static class SiloFertilizer
        {
            [HarmonyPrefix]

            public static void Prefix(ref float ___Capacity)
            {
                if (!Plugin.JustRightSiloCopacity.Value) return;
                ___Capacity = 32f;
            }
        }

        [HarmonyPatch(typeof(Structures_SiloSeed))]
        public static class SiloSeed
        {
            [HarmonyPrefix]

            public static void Prefix(ref float ___Capacity)
            {
                if (!Plugin.JustRightSiloCopacity.Value) return;
                ___Capacity = 32f;
            }
        }

    }
}
