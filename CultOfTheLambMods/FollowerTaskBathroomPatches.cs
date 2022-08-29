using HarmonyLib;
using System.Linq;

namespace CultOfTheLambMods
{
    public static class FollowerTaskBathroomPatches
    {

        [HarmonyPatch(typeof(FollowerTask_Bathroom), MethodType.Constructor, typeof(int))]
        public static class UiFishingIsNeedleWithinSectionPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Structures_Outhouse ____toilet, ref int ____toiletID)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                var toilets = StructureManager.GetAllStructuresOfType<Structures_Outhouse>().Where(a => !a.ReservedByPlayer || !a.ReservedForTask).ToList();

                if (toilets.Count <= 1) return;
                toilets.Sort((x, y) => x.GetPoopCount().CompareTo(y.GetPoopCount()));

                var toilet = toilets[0];

                Plugin.Log.LogMessage($"Setting toilet to new toilet. Poop count: {toilet.GetPoopCount()}");
                ____toilet = toilets[0];
                ____toiletID = toilets[0].Data.ID;
            }
        }
    }
}