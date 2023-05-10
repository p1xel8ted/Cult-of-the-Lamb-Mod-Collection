using UnityEngine;

namespace CultOfQoL;

public static class Helpers
{
    internal static void Callback(Follower follower)
    {
        Plugin.L($"Resetting {follower.name} and sending to next task.");
        follower.Interaction_FollowerInteraction.Close(true,true,false);
        follower.Dropped();
        follower.ResetStateAnimations();
        follower.Brain.ContinueToNextTask();
    }
    
    public static string GetGameObjectPath(GameObject obj)
    {
        var path = obj.name;
        var currentParent = obj.transform.parent;

        while (currentParent != null)
        {
            path = currentParent.name + "/" + path;
            currentParent = currentParent.parent;
        }

        return path;
    }
}