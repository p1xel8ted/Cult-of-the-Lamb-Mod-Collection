using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
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
    
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract))]
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void OnInteract(Interaction instance, StateMachine state)
    {
        Console.WriteLine($"Interaction.OnInteract Test({instance}, {state})");
    }
}