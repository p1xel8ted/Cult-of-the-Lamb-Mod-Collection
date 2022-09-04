// using HarmonyLib;
// using UnityEngine.ProBuilder.MeshOperations;
//
// namespace Rebirth;
//
// [HarmonyPatch]
// public static class Helper
// {
//     public static bool TooOld { get; private set; }
//     public static bool BornAgain { get; private set; }
//     public static bool Available { get; private set; }
//     private static SaveData.BornAgainFollowerData BornAgainFollower { get; set; }
//     private static bool IsOld(Follower follower)
//     {
//         return follower.Outfit.CurrentOutfit == FollowerOutfitType.Old && (follower.Brain.Info.OldAge || follower.Brain.HasThought(Thought.OldAge));
//     }
//     
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract))]
//     public static class IsAvailablePatch
//     {
//         [HarmonyPrefix]
//         public static void Prefix(interaction_FollowerInteraction __instance)
//         {
//             BornAgainFollower = SaveData.GetBornAgainFollowerData(__instance.follower.Brain._directInfoAccess);
//
//             TooOld = IsOld(__instance.follower);
//             BornAgain = BornAgainFollower is {BornAgain: true};
//             
//             if (TooOld)
//             {
//                 Available = false;
//                 return;
//             }
//
//             if (BornAgain)
//             {
//                 Available = false;
//                 return;
//             }
//
//             Available = true;
//
//         }
//     }
// }