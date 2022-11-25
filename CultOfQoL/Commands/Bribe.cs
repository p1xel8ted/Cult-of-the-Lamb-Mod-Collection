using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

[HarmonyPatch]
public static class Bribe
{
    public static IEnumerator BribeRoutine(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        PlayerFarming.Instance.simpleSpineAnimator.Animate("give-item/generic", 0, false);
        PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0f);
        follower.AddThought(Thought.Bribed);
        var i = -1;
        for (;;)
        {
            var num = i + 1;
            i = num;
            if (num > 2)
            {
                break;
            }

            if (i < 2)
            {
                AudioManager.Instance.PlayOneShot("event:/followers/pop_in", follower.transform.position);
                ResourceCustomTarget.Create(follower.gameObject, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, null, false);
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                AudioManager.Instance.PlayOneShot("event:/followers/pop_in", follower.transform.position);
                ResourceCustomTarget.Create(follower.gameObject, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate
                {
                    follower.SetBodyAnimation("Reactions/react-love2", false);
                    follower.AddBodyAnimation("idle", true, 0f);
                    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);
                    follower.Brain.Stats.Bribed = true;
                    instance.eventListener.PlayFollowerVO(instance.generalAcknowledgeVO);
                    Inventory.ChangeItemQuantity(20, -3);
                    follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bribe, delegate
                    {
                        Plugin.L($"Adding Adoration thoughts to {follower.name}");
                        follower.Brain.AddThought(Thought.Bribed);
                        if (instance.follower.Brain.Stats.Adoration >= instance.follower.Brain.Stats.MAX_ADORATION)
                        {
                            instance.follower = follower;
                            follower.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, null, false));
                        }
                    });
                }, false);
            }
        }

        Plugin.L($"Resetting {follower.name} and sending to next task.");
        follower.Dropped();
        follower.ResetStateAnimations();
        follower.Brain.ContinueToNextTask();
    }
}