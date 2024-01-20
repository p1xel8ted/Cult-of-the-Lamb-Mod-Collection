namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class FollowerPatches
{
    private static IEnumerator ExtortMoneyRoutine(interaction_FollowerInteraction interaction)
    {
        yield return new WaitForEndOfFrame();
        interaction.follower.FacePosition(PlayerFarming.Instance.transform.position);
        yield return new WaitForSeconds(0.25f);
        int num;
        for (var i = 0; i < Random.Range(3, 7); i = num + 1)
        {
            ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, interaction.follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate
            {
                Inventory.AddItem(20, 1);
            });
            yield return new WaitForSeconds(0.1f);
            num = i;
        }
        yield return new WaitForSeconds(0.25f);
    }


    private static IEnumerator RunEnumerator(interaction_FollowerInteraction interaction, bool run, IEnumerator enumerator, Action? onComplete = null)
    {
        if (run) yield break;
        interaction.OnEnable();
        yield return enumerator;
        yield return new WaitForSeconds(0.25f);
        interaction.ShowOtherFollowers();
        interaction.Close(true, true, false);
        onComplete?.Invoke();
    }
    
   

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized), typeof(FollowerCommands[]))]
    public static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
    {
        //todo: fix bribe and re-add it
        if (followerCommands[0] is not (FollowerCommands.ExtortMoney or FollowerCommands.Dance or FollowerCommands.Intimidate or FollowerCommands.Bless))
        {
            return true;
        }

        if (followerCommands[0] == FollowerCommands.ExtortMoney && __instance.follower.Brain.Stats.PaidTithes)
        {
            return true;
        }

        if (followerCommands[0] == FollowerCommands.Dance && __instance.follower.Brain.Stats.Inspired)
        {
            return true;
        }

        if (followerCommands[0] == FollowerCommands.Intimidate && __instance.follower.Brain.Stats.Intimidated)
        {
            return true;
        }

        if (followerCommands[0] == FollowerCommands.Bless && __instance.follower.Brain.Stats.BlessedToday)
        {
            return true;
        }

        if (followerCommands[0] == FollowerCommands.Bribe && __instance.follower.Brain.Stats.Bribed)
        {
            return true;
        }


        //todo: extort works fine
        //todo: dance works fine
        //todo: intimidate works fine
        //todo: bless works fine

        //todo: bribe has menu issues; breaks everything

        foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
        {
            if (followerCommands[0] == FollowerCommands.ExtortMoney)
            {
                interaction.StartCoroutine(RunEnumerator(interaction, interaction.follower.Brain.Stats.PaidTithes, ExtortMoneyRoutine(interaction), delegate
                {
                    Plugin.L($"Extorting money from {interaction.follower.name}");
                    interaction.follower.Brain.Stats.PaidTithes = true;
                }));
            }
            if (followerCommands[0] == FollowerCommands.Dance)
            {
                interaction.StartCoroutine(RunEnumerator(interaction, interaction.follower.Brain.Stats.Inspired, interaction.DanceRoutine(false), delegate
                {
                    Plugin.L($"Dancing with {interaction.follower.name}");
                    interaction.follower.Brain.Stats.Inspired = true;
                }));
            }
            if (followerCommands[0] == FollowerCommands.Intimidate)
            {
                interaction.StartCoroutine(RunEnumerator(interaction, interaction.follower.Brain.Stats.Intimidated, interaction.IntimidateRoutine(false), delegate
                {
                    Plugin.L($"Intimidating {interaction.follower.name}");
                    interaction.follower.Brain.Stats.Intimidated = true;
                }));
            }
            if (followerCommands[0] == FollowerCommands.Bless)
            {
                interaction.StartCoroutine(RunEnumerator(interaction, interaction.follower.Brain.Stats.BlessedToday, interaction.BlessRoutine(false), delegate
                {
                    Plugin.L($"Blessing {interaction.follower.name}");
                    interaction.follower.Brain.Stats.ReceivedBlessing = true;
                }));
            }
            if (followerCommands[0] == FollowerCommands.Bribe)
            {
                interaction.StartCoroutine(RunEnumerator(interaction, interaction.follower.Brain.Stats.Bribed, interaction.BribeRoutine(), delegate
                {
                    Plugin.L($"Bribing {interaction.follower.name}");
                    interaction.follower.Brain.Stats.Bribed = true;
                }));
            }
        }
        foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
        {
            interaction.ShowOtherFollowers();
        }
        
        return false;
    }

    // [HarmonyFinalizer]
    // [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.CacheAndSetFollowerRoutine), MethodType.Enumerator)]
    // private static Exception? Finalizer()
    // {
    //     return null;
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
    public static void interaction_FollowerInteraction_GiveDiscipleRewardRoutine(ref interaction_FollowerInteraction __instance)
    {
        if (!Plugin.CleanseIllnessAndExhaustionOnLevelUp.Value) return;
        if (__instance.follower.Brain.Stats.Exhaustion > 0)
        {
            __instance.follower.Brain._directInfoAccess.Exhaustion = 0f;
            __instance.follower.Brain.Stats.Exhaustion = 0f;
            var onExhaustionStateChanged = FollowerBrainStats.OnExhaustionStateChanged;
            onExhaustionStateChanged?.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
            Plugin.L($"Resetting follower {__instance.follower.name} from exhaustion!");
        }

        if (__instance.follower.Brain.Stats.Illness > 0)
        {
            __instance.follower.Brain._directInfoAccess.Illness = 0f;
            __instance.follower.Brain.Stats.Illness = 0f;
            var onIllnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
            onIllnessStateChanged.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
            Plugin.L($"Resetting follower {__instance.follower.name} from illness!");
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands), typeof(Follower))]
    public static void FollowerCommandGroups_OldAgeCommands(ref List<CommandItem> __result)
    {
        if (!Plugin.CollectTitheFromOldFollowers.Value) return;
        if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
        {
            __result.Add(FollowerCommandItems.Extort());
        }
    }
}