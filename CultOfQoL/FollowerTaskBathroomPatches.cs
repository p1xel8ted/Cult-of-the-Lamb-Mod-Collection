using HarmonyLib;
using System.Linq;

namespace CultOfQoL
{
    public static class FollowerTaskBathroomPatches
    {

        [HarmonyPatch(typeof(FollowerTask_Bathroom), MethodType.Constructor, typeof(int))]
        public static class FollowerTaskBathroomConstructorPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Structures_Outhouse ____toilet, ref int ____toiletID)
            {
                if (!Plugin.AlwaysGoForOuthouseWithLeastPoop.Value) return;
                var toilets = StructureManager.GetAllStructuresOfType<Structures_Outhouse>().Where(a => !a.ReservedByPlayer || !a.ReservedForTask).ToList();
                if (toilets.Count <= 1) return; //let the game take care of the decision

                var allToiletsFull = toilets.All(t => t.IsFull);

                if (allToiletsFull) return; //let the game take care of the decision

                toilets.Sort((x, y) => x.GetPoopCount().CompareTo(y.GetPoopCount()));

                Structures_Outhouse toilet = null;
                foreach (var t in toilets.Where(t => !t.IsFull && !t.ReservedByPlayer && !t.ReservedForTask))
                {
                    toilet = t;
                }

                if (toilet == null) return; //no suitable toilet found, let the game take care of the decision

                Plugin.Log.LogMessage($"Setting toilet to new toilet. Poop count: {toilet.GetPoopCount()}");
                ____toilet = toilets[0];
                ____toiletID = toilets[0].Data.ID;
            }
        }
    }
}