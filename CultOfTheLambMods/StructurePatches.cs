using HarmonyLib;

namespace CultOfTheLambMods
{
    internal static class StructurePatches
    {
        [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.IncreaseAge))]
        public static class StructuresLumberjackStationAgePatches
        {
            [HarmonyPrefix]
            public static void Prefix(ref Structures_LumberjackStation __instance)
            {
                if (!Plugin.LumberAndMiningStationsDontAge.Value) return;
                __instance.Data.Age = 0;
                Plugin.Log.LogWarning($"Resetting age of lumber/mining station to 0!");

            }
        }
    }
}
