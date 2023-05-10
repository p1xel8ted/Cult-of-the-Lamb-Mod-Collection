using System;
using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

[HarmonyPatch]
public static class Bless
{
    public static IEnumerator BlessRoutine(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {
        var position = PlayerFarming.Instance.transform.position;
        follower.FacePosition(position);
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.State.CURRENT_STATE = StateMachine.State.Dancing;
        yield return null;
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", position);
        AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", position);
        follower.SetBodyAnimation("devotion/devotion-start", false);
        follower.AddBodyAnimation("devotion/devotion-waiting", true, 0f);
        yield return PlayerFarming.Instance.Spine.YieldForAnimation("bless");
        PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true, 0f);
        follower.SetBodyAnimation("idle", true);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
        instance.eventListener.PlayFollowerVO(instance.generalAcknowledgeVO);
        follower.Brain.Stats.ReceivedBlessing = true;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, delegate
        {
            Plugin.L($"Adding Adoration thoughts to {follower.name}");
            CultFaithManager.AddThought(Thought.Cult_Bless, -1, 1f, Array.Empty<string>());
            if (follower.Brain.Stats.Adoration >= follower.Brain.Stats.MAX_ADORATION)
            {
                instance.follower = follower;
                follower.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, () => { Helpers.Callback(follower);}, false));
            }

            Helpers.Callback(follower);
        });
    }

}