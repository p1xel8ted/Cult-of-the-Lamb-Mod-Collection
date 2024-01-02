namespace CultOfQoL;

[HarmonyPatch]
public static class Dance
{
    internal static IEnumerator DanceRoutine(Follower follower, interaction_FollowerInteraction instance, FollowerTaskType previousTaskType)
    {
        Plugin.Log.LogInfo($"Follower: {follower.name}, Instance follower: {instance.follower.name}");
        var position = PlayerFarming.Instance.transform.position;
        follower.FacePosition(position);
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.State.CURRENT_STATE = StateMachine.State.Dancing;
        yield return null;
        PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "dance", true);
        follower.SetBodyAnimation("dance", true);
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", position);
        AudioManager.Instance.SetFollowersDance(1f);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.SetFollowersDance(0f);
        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", instance.follower.transform.position);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
        follower.Brain.Stats.Inspired = true;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, delegate
        {
            Plugin.L($"Adding Adoration thoughts to {follower.name}");
            follower.Brain.AddThought(Thought.DancedWithLeader, forced: true);
            instance.eventListener.PlayFollowerVO(instance.bowVO);
            CultFaithManager.AddThought(Thought.Cult_Inspire, follower.Brain.Info.ID, 1f, null);
            if (instance.follower.Brain.Stats.Adoration >= instance.follower.Brain.Stats.MAX_ADORATION)
            {
                instance.follower = follower;
                follower.StartCoroutine(instance.GiveDiscipleRewardRoutine(previousTaskType, () => { Helpers.Callback(follower);}, false));
            }
            Helpers.Callback(follower);
        });
  
    }

}