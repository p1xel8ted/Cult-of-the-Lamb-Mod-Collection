using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace CultOfQoL;

[HarmonyPatch]
public static class Intimidate
{
    public static IEnumerator IntimidateRoutine(Follower follower,
        interaction_FollowerInteraction instance,
        FollowerTaskType previousTaskType)
    {
        Plugin.Log.LogInfo($"Follower: {follower.name}, Instance follower: {instance.follower.name}");
        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.Spine.CustomMaterialOverride.Clear();
        follower.Spine.CustomMaterialOverride.Add(follower.NormalMaterial, follower.BW_Material);
        PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
        PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial,
            PlayerFarming.Instance.BW_Material);
        HUD_Manager.Instance.ShowBW(0.33f, 0f, 1f);
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.State.CURRENT_STATE = StateMachine.State.Dancing;
        yield return null;
        PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "intimidate", false);
        PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0f);
        float num = follower.SetBodyAnimation("Reactions/react-intimidate", false);
        follower.AddBodyAnimation("idle", true, 0f);
        AudioManager.Instance.PlayOneShot("event:/player/intimidate_follower", PlayerFarming.Instance.gameObject);
        yield return new WaitForSeconds(num - 2.25f);
        PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
        follower.Spine.CustomMaterialOverride.Clear();
        HUD_Manager.Instance.ShowBW(0.33f, 1f, 0f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", follower.transform.position);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
        follower.Brain.Stats.Intimidated = true;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Intimidate, delegate
        {
            Plugin.L($"Adding Adoration thoughts to {follower.name}");
            follower.Brain.AddThought(Thought.Intimidated);
            if (follower.Brain.Stats.Adoration >= follower.Brain.Stats.MAX_ADORATION)
            {
                instance.follower = follower;
                follower.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, () => { Helpers.Callback(follower); }, false));
            }

            Helpers.Callback(follower);
        });
    }
}