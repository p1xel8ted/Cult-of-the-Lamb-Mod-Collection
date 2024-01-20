using System.Diagnostics.CodeAnalysis;

namespace CultOfQoL.Patches;

[Harmony]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class InteractionPatches
{
    private static GameManager GI => GameManager.GetInstance();

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract), typeof(StateMachine))]
    public static void Interaction_Follower_OnInteract(ref interaction_FollowerInteraction __instance)
    {
        if (!Plugin.MassLevelUp.Value) return;
        if (!__instance.follower.Brain.CanLevelUp()) return;
        GI.StartCoroutine(LevelUpAllFollowers());
    }

    private static IEnumerator LevelUpAllFollowers()
    {
        yield return new WaitForEndOfFrame();
        foreach (var follower in Follower.Followers.Where(follower => follower.Brain.CanLevelUp()))
        {
            yield return new WaitForSeconds(0.15f);
            var interaction = follower.gameObject.GetComponent<interaction_FollowerInteraction>();
            GI.StartCoroutine(interaction.LevelUpRoutine(follower.Brain.CurrentTaskType, null, false));
        }
    }


    private static IEnumerator WaterAllPlants()
    {
        yield return new WaitForEndOfFrame();
        var plots = Resources.FindObjectsOfTypeAll<FarmPlot>();
        foreach (var plot in plots.Where(a => a.Structure != null && a.Structure.Brain?.Data?.GrowthStage > 0))
        {
            Plugin.Log.LogInfo($"Watering {plot.name}");
            yield return new WaitForSeconds(0.10f);
            plot.StructureInfo.Watered = true;
            plot.StructureInfo.WateredCount = 0;
            plot.WateringTime = 0.95f;
            plot.UpdateWatered();
            plot.UpdateCropImage();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.OnInteract), typeof(StateMachine))]
    public static void Interaction_OfferingShrine_OnInteract(ref FarmPlot __instance, ref StateMachine state)
    {
        GI.StartCoroutine(WaterAllPlants());
    }
}