//using HarmonyLib;

//namespace CultOfTheLambMods
//{
//    public static class BarControllerPatches
//    {

//        [HarmonyPatch(typeof(BarController), nameof(BarController.ShrinkBarToEmpty), typeof(float))]
//        public static class RefineryItemCheckCanAffordPatches
//        {
//            [HarmonyPrefix]
//            public static void Prefix(ref BarController __instance, ref float duration)
//            {
//              //  if (!Plugin..Value) return;
//              if (duration > 0)
//              {
//                  Plugin.Log.LogMessage($"Increasing bar speed for {__instance.name}");
//                  duration = 0.1f;
//              }
//            }
//        }
//    }
//}
