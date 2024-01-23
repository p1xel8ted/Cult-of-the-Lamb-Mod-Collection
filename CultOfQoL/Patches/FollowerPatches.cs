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

    private static IEnumerator RunEnumerator(bool run, IEnumerator enumerator, Action? onComplete = null)
    {
        if (run) yield break;
        yield return enumerator;
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();
    }

    private static bool ShouldMassBribe(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBribe.Value) return false;
        if (followerCommands != FollowerCommands.Bribe) return false;
        var notBribedCount = Follower.Followers.Count(follower => !follower.Brain.Stats.Bribed);
        return notBribedCount > 1;
    }

    private static bool ShouldMassPetDog(FollowerCommands followerCommands)
    {
        if (!Plugin.MassPetDog.Value) return false;
        if (followerCommands != FollowerCommands.PetDog) return false;
        var notPettedCount = Follower.Followers.Count(f => !f.Brain.Stats.PetDog);
        return notPettedCount > 1;
    }

    private static bool ShouldMassExtort(FollowerCommands followerCommands)
    {
        if (!Plugin.MassExtort.Value) return false;
        if (followerCommands != FollowerCommands.ExtortMoney) return false;
        var notPaidTithesCount = Follower.Followers.Count(follower => !follower.Brain.Stats.PaidTithes);
        return notPaidTithesCount > 1;
    }

    private static bool ShouldMassInspire(FollowerCommands followerCommands)
    {
        if (!Plugin.MassInspire.Value) return false;
        if (followerCommands != FollowerCommands.Dance) return false;
        var notInspiredCount = Follower.Followers.Count(follower => !follower.Brain.Stats.Inspired);
        return notInspiredCount > 1;
    }

    private static bool ShouldMassIntimidate(FollowerCommands followerCommands)
    {
        if (!Plugin.MassIntimidate.Value) return false;
        if (followerCommands != FollowerCommands.Intimidate) return false;
        var notIntimidatedCount = Follower.Followers.Count(follower => !follower.Brain.Stats.Intimidated);
        return notIntimidatedCount > 1;
    }

    private static bool ShouldMassBless(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBless.Value) return false;
        if (followerCommands != FollowerCommands.Bless) return false;
        var notBlessedCount = Follower.Followers.Count(follower => !follower.Brain.Stats.BlessedToday);
        return notBlessedCount > 1;
    }


    private static bool RunForThisFollower(FollowerBrain brain)
    {
        if (brain.CurrentTaskType is not (FollowerTaskType.Sleep or FollowerTaskType.SleepBedRest or FollowerTaskType.Dissent or FollowerTaskType.Imprisoned or FollowerTaskType.Mating)) return true;
        Plugin.L($"Skipping {brain.Info.Name} because they are busy with task: {brain.CurrentTaskType.ToString()}");
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized), typeof(FollowerCommands[]))]
    public static void interaction_FollowerInteraction_OnFollowerCommandFinalized(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
    {
        if (followerCommands[0] is not (FollowerCommands.Bribe or FollowerCommands.PetDog or FollowerCommands.ExtortMoney or FollowerCommands.Dance or FollowerCommands.Intimidate or FollowerCommands.Bless))
        {
            return;
        }

        if (ShouldMassPetDog(followerCommands[0]))
        {
            foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
            {
                if (!RunForThisFollower(interaction.follower.Brain)) continue;
                var isDog = interaction.follower.Brain._directInfoAccess.SkinName.Equals("Dog", StringComparison.OrdinalIgnoreCase);
                if (!isDog)
                {
                    Plugin.L($"Skipping {interaction.follower.name} because they are a not a dog!");
                    continue;
                }
                interaction.StartCoroutine(RunEnumerator(interaction.follower.Brain.Stats.PetDog, interaction.PetDogRoutine(), delegate
                {
                    Plugin.L($"Petted {interaction.follower.name}!");
                    interaction.follower.Brain.Stats.PetDog = true;
                }));
            }
        }

        if (ShouldMassExtort(followerCommands[0]))
        {
            foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
            {
                if (!RunForThisFollower(interaction.follower.Brain)) continue;
                interaction.StartCoroutine(RunEnumerator(interaction.follower.Brain.Stats.PaidTithes, ExtortMoneyRoutine(interaction), delegate
                {
                    Plugin.L($"Extorted {interaction.follower.name}!");
                    interaction.follower.Brain.Stats.PaidTithes = true;
                }));
            }
        }

        if (ShouldMassInspire(followerCommands[0]))
        {
            foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
            {
                if (!RunForThisFollower(interaction.follower.Brain)) continue;
                interaction.StartCoroutine(RunEnumerator(interaction.follower.Brain.Stats.Inspired, interaction.DanceRoutine(false), delegate
                {
                    Plugin.L($"Inspired {interaction.follower.name}!");
                    interaction.follower.Brain.Stats.Inspired = true;
                }));
            }
        }

        if (ShouldMassIntimidate(followerCommands[0]))
        {
            foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
            {
                if (!RunForThisFollower(interaction.follower.Brain)) continue;
                interaction.StartCoroutine(RunEnumerator(interaction.follower.Brain.Stats.Intimidated, interaction.IntimidateRoutine(false), delegate
                {
                    Plugin.L($"Intimidated {interaction.follower.name}!");
                    interaction.follower.Brain.Stats.Intimidated = true;
                }));
            }
        }


        if (ShouldMassBless(followerCommands[0]))
        {
            foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
            {
                if (!RunForThisFollower(interaction.follower.Brain)) continue;
                interaction.StartCoroutine(RunEnumerator(interaction.follower.Brain.Stats.BlessedToday, interaction.BlessRoutine(false), delegate
                {
                    Plugin.L($"Blessed {interaction.follower.name}!");
                    interaction.follower.Brain.Stats.ReceivedBlessing = true;
                    interaction.follower.Brain.Stats.LastBlessing = DataManager.Instance.CurrentDayIndex;
                }));
            }
        }


        if (ShouldMassBribe(followerCommands[0]))
        {
            foreach (var interaction in Follower.Followers.Select(follower => follower.gameObject.GetComponent<interaction_FollowerInteraction>()))
            {
                if (!RunForThisFollower(interaction.follower.Brain)) continue;
                interaction.StartCoroutine(RunEnumerator(interaction.follower.Brain.Stats.Bribed, interaction.BribeRoutine(), delegate
                {
                    Plugin.L($"Bribed {interaction.follower.name}!");
                    interaction.follower.Brain.Stats.Bribed = true;
                }));
            }
        }
    }


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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.Close), typeof(bool), typeof(bool), typeof(bool))]
    public static void interaction_FollowerInteraction_Close(ref bool DoResetFollower, ref bool unpause, ref bool reshowMenu)
    {
        DoResetFollower = true;
        unpause = true;
        reshowMenu = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
    public static void interaction_FollowerInteraction_LevelUpRoutine(ref Action? Callback, ref bool GoToAndStop, ref bool onFinishClose)
    {
        Callback = null;
        GoToAndStop = false;
        onFinishClose = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands), typeof(Follower))]
    public static void FollowerCommandGroups_OldAgeCommands(ref List<CommandItem> __result)
    {
        if (Plugin.CollectTitheFromOldFollowers.Value)
        {
            if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
            {
                __result.Add(FollowerCommandItems.Extort());
            }
        }
        if (Plugin.IntimidateOldFollowers.Value)
        {
            if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
            {
                __result.Add(FollowerCommandItems.Intimidate());
            }
        }
    }
}