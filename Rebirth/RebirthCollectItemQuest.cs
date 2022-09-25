using System;
using System.Collections.Generic;
using COTL_API.CustomObjectives;
using Random = UnityEngine.Random;

namespace Rebirth;

public class RebirthCollectItemQuest : CustomObjective
{
   // public ObjectivesData QuestData;

    public override string InternalName => "REBIRTH_COLLECT_QUEST";

    public override string InitialQuestText => "Please Leader, please! I'm weary of this existence and seek to be reborn! I will do anything for you! Can you please help me?";

    public override ObjectivesData ObjectiveData => CustomObjectiveManager.Objective.CollectItem(ObjectiveKey, Plugin.RebirthItem, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
        // QuestData = 
        //QuestData;
}