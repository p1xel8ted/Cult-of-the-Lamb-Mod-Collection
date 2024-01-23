namespace CultOfQoL.Routines;

[Harmony]
public static class RoutinesTranspilers
{

    private readonly static List<string> AlreadyLogged = [];

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract))]
    private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_OnInteract(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var declaringType = original.DeclaringType!.ToString();
        if (!AlreadyLogged.Contains(declaringType))
        {
            AlreadyLogged.Add(declaringType);
            Plugin.L($"[Mass Actions] -> Patching {declaringType}:{original.Name}");
        }
        var BiomeConstantsInstance = AccessTools.Field(typeof(BiomeConstants), nameof(BiomeConstants.Instance));
        var DepthOfFieldTween = AccessTools.Method(typeof(BiomeConstants), nameof(BiomeConstants.DepthOfFieldTween));
        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].LoadsField(BiomeConstantsInstance) && codes[i + 6].Calls(DepthOfFieldTween))
            {
                Plugin.L($"[Mass Actions] -> Patching {declaringType}:{original.Name} -> Skipping DepthOfFieldTween");
                for (var j = 0; j < 7; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                }
            }
        }
        return codes.AsEnumerable();
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.IntimidateRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BlessRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.DanceRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.PetDogRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var declaringType = original.DeclaringType!.ToString();
        if (!AlreadyLogged.Contains(declaringType))
        {
            AlreadyLogged.Add(declaringType);
            Plugin.L($"[Mass Actions] -> Patching {declaringType}:{original.Name}");
        }

        var GetInstance = AccessTools.Method(typeof(GameManager), nameof(GameManager.GetInstance));
        var OnConversationNext = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNext));
        var OnConversationNew = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNew), [typeof(bool), typeof(bool)]);
        var AddPlayerToCamera = AccessTools.Method(typeof(GameManager), nameof(GameManager.AddPlayerToCamera));
        var CameraSetOffset = AccessTools.Method(typeof(GameManager), nameof(GameManager.CameraSetOffset));
        var HudManagerHide = AccessTools.Method(typeof(HUD_Manager), nameof(HUD_Manager.Hide));
        var HudManagerInstance = AccessTools.Field(typeof(HUD_Manager), nameof(HUD_Manager.Instance));
        var PlayerInstance = AccessTools.Field(typeof(PlayerFarming), nameof(PlayerFarming.Instance));
        var setStateMachine = AccessTools.Property(typeof(StateMachine), nameof(StateMachine.CURRENT_STATE)).SetMethod;
        var simpleAnimatorAnimate = AccessTools.Method(typeof(SimpleSpineAnimator), nameof(SimpleSpineAnimator.Animate), [typeof(string), typeof(int), typeof(bool)]);
        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].LoadsField(PlayerInstance) && codes[i + 3].Calls(setStateMachine))
            {
                Plugin.Log.LogInfo($"[Mass Actions] -> Patching {declaringType}:{original.Name} -> Skipping SermonLevelUpAnimation Part 1");
                for (var j = 0; j < 4; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                }
            }

            if (codes[i].LoadsField(PlayerInstance) && codes[i + 5].Calls(simpleAnimatorAnimate))
            {
                Plugin.Log.LogInfo($"[Mass Actions] -> Patching {declaringType}:{original.Name} -> Skipping SermonLevelUpAnimation Part 2");
                for (var j = 0; j < 14; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                }
            }

            if (codes[i].LoadsField(HudManagerInstance) && codes[i + 4].Calls(HudManagerHide))
            {
                Plugin.Log.LogInfo($"[Mass Actions] -> Patching {declaringType}:{original.Name} -> Skipping Hiding HUD");
                for (var j = 0; j < 5; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                }
            }

            if (codes[i].Calls(GetInstance) && codes[i + 3].Calls(OnConversationNew) && codes[i + 4].Calls(GetInstance) && codes[i + 8].Calls(OnConversationNext))
            {
                Plugin.Log.LogInfo($"[Mass Actions] -> Patching {declaringType}:{original.Name} -> Skipping Conversation Start");
                for (var j = 0; j < 9; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                }
            }

            if (codes[i].Calls(GetInstance) && codes[i + 4].Calls(OnConversationNext) && codes[i + 6].Calls(AddPlayerToCamera) && codes[i + 9].Calls(CameraSetOffset))
            {
                Plugin.Log.LogInfo($"[Mass Actions] -> Patching {declaringType}:{original.Name} -> Skipping Camera Move");
                for (var j = 0; j < 10; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                }
            }
        }
        return codes.AsEnumerable();
    }
}