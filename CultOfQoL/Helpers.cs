// using BepInEx;
// using HarmonyLib;
//
// namespace CultOfQoL;
//
// public static class Helpers
// {
//     [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.CheckForSpeakers), typeof(Structure))]
//     public static class FollowerBrainCheckForSpeakersPatch
//     {
//         [HarmonyPostfix]
//         public static void Postfix(FollowerBrain __instance, ref bool __result)
//         {
//             //need to check if the follower has the necklace item, otherwise every follower with just be near a speaker
//             //also, not sure if that's the correct check, but you get the idea
//             Plugin.Log.LogWarning($"Follower: {__instance.Info.Name}, NecklaceType: {__instance.Info.Necklace}");
//             if (__instance.Info.Necklace is InventoryItem.ITEM_TYPE.Necklace_1)
//             {
//                 __result = true;
//             }
//             
//         }
//     }
// }