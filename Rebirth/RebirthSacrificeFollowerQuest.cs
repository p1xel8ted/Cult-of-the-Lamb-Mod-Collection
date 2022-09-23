using COTL_API.CustomObjectives;

namespace Rebirth;

public class RebirthSacrificeFollowerQuest : CustomObjective
{
    public override string InternalName => "REBIRTH_SACRIFICE_QUEST";

    public override string InitialQuestText => "Leader, I dislike the look of that follower, ";

    public override ObjectivesData ObjectiveData => CustomObjectiveManager.Objective.PerformRitual(ObjectiveKey, UpgradeSystem.Type.Ritual_Sacrifice);
}