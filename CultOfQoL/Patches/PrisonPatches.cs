using System.Collections.Generic;
using HarmonyLib;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class PrisonPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Prison), nameof(Prison.ImprisonableFollowers))]
    public static void Prison_ImprisonableFollowers(ref List<Follower> __result)
    {
        if (!Plugin.OnlyShowDissenters.Value) return;
        __result.RemoveAll(follower => follower.Brain.Info.CursedState != Thought.Dissenter);
    }
}