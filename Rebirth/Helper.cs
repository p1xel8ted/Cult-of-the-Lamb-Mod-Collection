using UnityEngine;

namespace Rebirth;

public static class Helper
{
    public static bool TooOld { get; set; }
    
    public static bool IsOld(Follower follower)
    {
        return follower.Outfit.CurrentOutfit == FollowerOutfitType.Old && (follower.Brain.Info.OldAge || follower.Brain.HasThought(Thought.OldAge));
    }

    public static bool DoHalfStats()
    {
        return Random.Range(0f, 1f) <= 0.2f;
    }
}