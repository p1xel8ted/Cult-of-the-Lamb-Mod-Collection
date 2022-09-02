// using System;
// using System.Collections;
//
// namespace CultOfQoL;
//
// public class Test
// {
//     public static void Test2()
//     {
//         GameManager.GetInstance().StartCoroutine(GiveItem(follower, ItemToGive, interaction));
//     }
//     
//     
//     public static IEnumerator GiveItem(Follower follower, InventoryItem.ITEM_TYPE ItemToGive, interaction_FollowerInteraction interaction)
//     {
//         int waiting = 0;
//         AudioManager.Instance.PlayOneShot("event:/followers/pop_in", follower.transform.position);
//         ResourceCustomTarget.Create(follower.gameObject, PlayerFarming.Instance.CameraBone.transform.position, ItemToGive, delegate()
//         {
//             AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);
//             FollowerBrain brain = follower.Brain;
//             FollowerBrain.AdorationActions adorationActions = FollowerBrain.AdorationActions.Necklace;
//
//             void Action3()
//             {
//                 int num5 = waiting + 1;
//                 waiting = num5;
//             }
//
//             brain.AddAdoration(adorationActions, Action3);
//             Action<Follower, InventoryItem.ITEM_TYPE, Action> action4 = InventoryItem.GiveToFollowerCallbacks(ItemToGive);
//             if (action4 != null)
//             {
//                 Follower follower3 = follower;
//                 InventoryItem.ITEM_TYPE itemToGive2 = ItemToGive;
//
//                 void Action5()
//                 {
//                     int num5 = waiting + 1;
//                     waiting = num5;
//                 }
//
//                 action4(follower3, itemToGive2, Action5);
//             }
//
//             Inventory.ChangeItemQuantity((int) ItemToGive, -1, 0);
//         }, false);
//         while (waiting < 2)
//         {
//             yield return null;
//         }
//         ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveGift, -1);
//         if (follower.Brain.Stats.Adoration >= follower.Brain.Stats.MAX_ADORATION)
//         {
//             interaction.StartCoroutine(interaction.GiveDiscipleRewardRoutine(interaction.previousTaskType, interaction.Close));
//         }
//         else
//         {
//             interaction.Close();
//         }
//         yield break;
//     }
// }