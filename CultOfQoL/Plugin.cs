namespace CultOfQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API", BepInDependency.DependencyFlags.SoftDependency)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
    private const string PluginName = "Cult of QoL Collection";
    private const string PluginVer = "2.1.4";

    internal static ManualLogSource Log = null!;
    private readonly static Harmony Harmony = new(PluginGuid);

    internal static CanvasScaler? GameCanvasScaler { get; set; }
    internal static CanvasScaler? DungeonCanvasScaler { get; set; }

    private void Awake()
    {
        Log = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(Log);

        SceneManager.sceneLoaded += (_, _) =>
        {
            var buttons = Resources.FindObjectsOfTypeAll<MMButton>();
            foreach (var button in buttons)
            {
                button.Selectable.navigation = button.Selectable.navigation with {mode = Navigation.Mode.Automatic};
            }
        };
        
        ModEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");

        //Player
        EnableBaseDamageMultiplier = Config.Bind("Player", "Enable Base Damage Multiplier", false, "Enable/disable the base damage multiplier.");
        BaseDamageMultiplier = Config.Bind("Player", "Base Damage Multiplier", 1.5f, new ConfigDescription("The base damage multiplier.", new AcceptableValueRange<float>(0, 100)));
        EnableRunSpeedMulti = Config.Bind("Player", "Enable Run Speed Multiplier", true, "Enable/disable the run speed multiplier.");
        RunSpeedMulti = Config.Bind("Player", "Run Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player runs.", new AcceptableValueRange<float>(0, 100)));
        EnableDodgeSpeedMulti = Config.Bind("Player", "Enable Dodge Speed Multiplier", true, "Enable/disable the dodge speed multiplier.");
        DodgeSpeedMulti = Config.Bind("Player", "Dodge Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player dodges.", new AcceptableValueRange<float>(0, 100)));
        EnableLungeSpeedMulti = Config.Bind("Player", "Enable Lunge Speed Multiplier", true, "Enable/disable the lunge speed multiplier.");
        LungeSpeedMulti = Config.Bind("Player", "Lunge Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player lunges.", new AcceptableValueRange<float>(0, 100)));

        //General
        SkipDevIntros = Config.Bind("General", "Skip Intros", true, "Skip splash screens.");
        SkipCrownVideo = Config.Bind("General", "Skip Crown Video", true, "Skips the video when the lamb gets given the crown.");
        RemoveMenuClutter = Config.Bind("General", "Remove Extra Menu Buttons", true, "Removes credits/road-map/discord buttons from the menus.");
        RemoveTwitchButton = Config.Bind("General", "Remove Twitch Buttons", true, "Removes twitch buttons from the menus.");
        UnlockTwitchStuff = Config.Bind("General", "Unlock Twitch Stuff", true, "Unlock pre-order DLC, Twitch plush, and three drops.");
        DisableAd = Config.Bind("General", "Disable Ads", true, "Disables the new ad 'feature'.");
        RemoveHelpButtonInPauseMenu = Config.Bind("General", "Remove Help Button In Pause Menu", true, "Removes the help button in the pause menu.");
        RemoveTwitchButtonInPauseMenu = Config.Bind("General", "Remove Twitch Button In Pause Menu", true, "Removes the twitch button in the pause menu.");
        RemovePhotoModeButtonInPauseMenu = Config.Bind("General", "Remove Photo Mode Button In Pause Menu", true, "Removes the photo mode button in the pause menu.");

        //Save
        SaveOnQuitToDesktop = Config.Bind("Save", "Save On Quit To Desktop", true, "Modify the confirmation dialog to save the game when you quit to desktop.");
        SaveOnQuitToMenu = Config.Bind("Save", "Save On Quit To Menu", true, "Modify the confirmation dialog to save the game when you quit to menu.");
        HideNewGameButtons = Config.Bind("Save", "Hide New Game Button (s)", true, "Hides the new game button if you have at least one save game.");
        EnableQuickSaveShortcut = Config.Bind("Save", "Enable Quick Save Shortcut", true, "Enable/disable the quick save keyboard shortcut.");
        SaveKeyboardShortcut = Config.Bind("Save", "Save Keyboard Shortcut", new KeyboardShortcut(KeyCode.F5), "The keyboard shortcut to save the game.");
        DirectLoadSave = Config.Bind("Save", "Direct Load Save", false, "Directly load the specified save game instead of showing the save menu.");
        DirectLoadSkipKey = Config.Bind("Save", "Direct Load Skip Key", new KeyboardShortcut(KeyCode.LeftShift), "The keyboard shortcut to skip the auto-load when loading the game.");
        SaveSlotToLoad = Config.Bind("Save", "Save Slot To Load", 1, new ConfigDescription("The save slot to load.", new AcceptableValueList<int>(1, 2, 3)));
        SaveSlotToLoad.SettingChanged += (_, _) =>
        {
            if (!SaveAndLoad.SaveExist(SaveSlotToLoad.Value))
            {
                L($"The slot you have select doesn't contain a save game.");
                return;
            }
            L($"Save slot to load changed to {SaveSlotToLoad.Value}");
        };
        //Scale
        EnableCustomUiScale = Config.Bind("Scale", "Enable Custom UI Scale", false, "Enable/disable the custom UI scale.");
        EnableCustomUiScale.SettingChanged += (_, _) =>
        {
            if (EnableCustomUiScale.Value)
            {
                Scales.UpdateScale();
            }
            else
            {
                Scales.RestoreScale();
            }
        };

        CustomUiScale = Config.Bind("Scale", "Custom UI Scale", 50, new ConfigDescription("The custom UI scale to use.", new AcceptableValueRange<int>(1, 101)));
        CustomUiScale.SettingChanged += (_, _) => { Scales.UpdateScale(); };

        //Weather
        ChangeWeatherOnPhaseChange = Config.Bind("Weather", "Change Weather On Phase Change", true, "By default, the game changes weather when you exit a structure, or on a new day. Enabling this makes the weather change on each phase i.e. morning, noon, evening, night.");
        RandomWeatherChangeWhenExitingArea = Config.Bind("Weather", "Random Weather Change When Exiting Area", true, "When exiting a building/area, the weather will change to a random weather type instead of the previous weather.");

        //Game Mechanics
        ReverseGoldenFleeceDamageChange = Config.Bind("Game Mechanics", "Reverse Golden Fleece Change", true, "Reverts the default damage increase to 10% instead of 5%.");
        IncreaseGoldenFleeceDamageRate = Config.Bind("Game Mechanics", "Increase Golden Fleece Rate", true, "Doubles the damage increase.");
        UseCustomDamageValue = Config.Bind("Game Mechanics", "Use Custom Damage Value", false, "Use a custom damage value instead of the default 10%.");
        CustomDamageMulti = Config.Bind("Game Mechanics", "Custom Damage Multiplier", 2.0f, new ConfigDescription("The custom damage multiplier to use. Based off the games default 5%.", new AcceptableValueRange<float>(0, 100)));
        MassCollecting = Config.Bind("Game Mechanics", "Mass Collecting", true, "Allows you to collect resources from multiple structures at once. Currently, only the bed is working.");
        AdjustRefineryRequirements = Config.Bind("Game Mechanics", "Adjust Refinery Requirements", true, "Where possible, halves the materials needed to convert items in the refinery. Rounds up.");
        EasyFishing = Config.Bind("Game Mechanics", "Disable Fishing Mini-Game", true, "Fishing mini-game cheese. Just cast and let the mod do the rest.");
        DisableGameOver = Config.Bind("Game Mechanics", "No More Game-Over", false, "Disables the game over function when you have 0 followers for consecutive days.");
        ThriceMultiplyTarotCardLuck = Config.Bind("Game Mechanics", "3x Tarot Luck", true, "Luck changes with game difficulty, this will multiply your luck multiplier by 3 for drawing rarer tarot cards.");

        //Lumber/mining
        LumberAndMiningStationsDontAge = Config.Bind("Lumber/Mine Mods", "Infinite Lumber & Mining Stations", false, "Lumber and mining stations should never run out and collapse. Takes 1st priority.");
        LumberAndMiningStationsDontAge.SettingChanged += (_, _) =>
        {
            if (!LumberAndMiningStationsDontAge.Value) return;
            DoubleLifespanInstead.Value = false;
            FiftyPercentIncreaseToLifespanInstead.Value = false;
        };

        DoubleLifespanInstead = Config.Bind("Lumber/Mine Mods", "Double Life Span Instead", false, "Doubles the life span of lumber/mining stations. Takes 2nd priority.");
        DoubleLifespanInstead.SettingChanged += (_, _) =>
        {
            if (!DoubleLifespanInstead.Value) return;
            LumberAndMiningStationsDontAge.Value = false;
            FiftyPercentIncreaseToLifespanInstead.Value = false;
        };

        FiftyPercentIncreaseToLifespanInstead = Config.Bind("Lumber/Mine Mods", "Add 50% to Life Span Instead", true, "For when double is too long for your tastes. This will extend their life by 50% instead of 100%. Takes 3rd priority.");
        FiftyPercentIncreaseToLifespanInstead.SettingChanged += (_, _) =>
        {
            if (!FiftyPercentIncreaseToLifespanInstead.Value) return;
            LumberAndMiningStationsDontAge.Value = false;
            DoubleLifespanInstead.Value = false;
        };


        //Propaganda
        TurnOffSpeakersAtNight = Config.Bind("Propaganda Mods", "Turn Off Speakers At Night", true, "Turns the speakers off, and stops fuel consumption at night time.");
        DisablePropagandaSpeakerAudio = Config.Bind("Propaganda Mods", "Disable Propaganda Speaker Audio", true, "Disables the audio from propaganda speakers.");

        //Speed
        EnableGameSpeedManipulation = Config.Bind("Speed", "Enable Game Speed Manipulation", true, "Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.");
        ShortenGameSpeedIncrements = Config.Bind("Speed", "Shorten Game Speed Increments", false, "Increments in steps of 1, instead of 0.25.");
        FastCollecting = Config.Bind("Speed", "Speed Up Collection", true, "Increases the rate you can collect from the shrines, and other structures.");
        SlowDownTime = Config.Bind("Speed", "Slow Down Time", false, "Enables the ability to slow down time. This is different to the increase speed implementation. This will make the days longer, but not slow down animations.");
        SlowDownTimeMultiplier = Config.Bind("Speed", "Slow Down Time Multiplier", 2f, new ConfigDescription("The multiplier to use for slow down time. For example, the default value of 2 is making the day twice as long.", new AcceptableValueRange<float>(0, 100)));

        //Chest Auto-Interact
        EnableAutoInteract = Config.Bind("Chest Auto-Interact", "Enable Auto Interact", true, "Makes chests automatically send you the resources when you're nearby.");
        TriggerAmount = Config.Bind("Chest Auto-Interact", "Resource Trigger Amount", 5, new ConfigDescription("How many items you want in the chest before triggering auto collect.", new AcceptableValueRange<int>(0, 1000)));
        IncreaseRange = Config.Bind("Chest Auto-Interact", "Double Activation Range", true, "The default range is 5. This will increase it to 10.");
        UseCustomRange = Config.Bind("Chest Auto-Interact", "Use Custom Range", false, "Use a custom range instead of the default or increased range.");
        CustomRangeMulti = Config.Bind("Chest Auto-Interact", "Custom Range Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for auto-collect range when using custom range.", new AcceptableValueRange<float>(0, 100)));

        //Capacity
        //JustRightSiloCapacity = Config.Bind("Capacity", "Set Silo Capacity to 32", true, "Set silo capacity for seed and fertilizer at 32.");
        UseCustomSiloCapacity = Config.Bind("Capacity", "Use Custom Silo Capacity", false, "Use a custom silo capacity instead of the default or increased capacity.");
        CustomSiloCapacityMulti = Config.Bind("Capacity", "Custom Silo Capacity Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for silo capacity when using custom capacity.", new AcceptableValueRange<float>(0, 1000)));
        DoubleSoulCapacity = Config.Bind("Capacity", "Double Soul Capacity", true, "Doubles the soul capacity of applicable structures.");
        UseCustomSoulCapacity = Config.Bind("Capacity", "Use Custom Soul Capacity", false, "Use a custom soul capacity instead of the default or doubled capacity.");
        CustomSoulCapacityMulti = Config.Bind("Capacity", "Custom Soul Capacity Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for soul capacity when using custom capacity.", new AcceptableValueRange<float>(0, 1000)));

        //Notifications
        NotifyOfScarecrowTraps = Config.Bind("Notifications", "Notify of Scarecrow Traps", true, "Display a notification when the farm scarecrows have caught a trap!");
        NotifyOfNoFuel = Config.Bind("Notifications", "Notify of No Fuel", true, "Display a notification when a structure has run out of fuel.");
        NotifyOfBedCollapse = Config.Bind("Notifications", "Notify of Bed Collapse", true, "Display a notification when a bed has collapsed.");
        ShowPhaseNotifications = Config.Bind("Notifications", "Phase Notifications", true, "Show a notification when the time of day changes.");
        ShowWeatherChangeNotifications = Config.Bind("Notifications", "Weather Change Notifications", true, "Show a notification when the weather changes.");

        //Followers
        GiveFollowersNewNecklaces = Config.Bind("Followers", "Give Followers New Necklaces", true, "Followers will be able to receive new necklaces, with the old one being returned to you.");
        CleanseIllnessAndExhaustionOnLevelUp = Config.Bind("Followers", "Cleanse Illness and Exhaustion", true, "When a follower 'levels up', if they are sick or exhausted, the status is cleansed.");
        CollectTitheFromOldFollowers = Config.Bind("Followers", "Collect Tithe From Old Followers", true, "Re-enable collecting tithe from the elderly. Brutal.");
        AddExhaustedToHealingBay = Config.Bind("Followers", "Add Exhausted To Healing Bay", true, "Allows you to select exhausted followers for rest and relaxation in the healing bays.");
        BulkFollowerCommands = Config.Bind("Followers", "Bulk Inspire/Extort", true, "When collecting tithes, or inspiring, all followers are done at once.");
        OnlyShowDissenters = Config.Bind("Followers", "Only Show Dissenters In Prison Menu", true, "Only show dissenting followers when interacting with the prison.");
        MassLevelUp = Config.Bind("Followers", "Mass Level Up", true, "When interacting with a follower than can level, all eligible followers will be leveled up.");
        RemoveLevelLimit = Config.Bind("Followers", "Remove Level Limit", true, "Removes the level limit for followers. They can now level up infinitely.");
        // if (!SoftDepend.Enabled) return;
        //
        // SoftDepend.AddSettingsMenus();
        // Log.LogInfo("API detected - You can configure mod settings in the settings menu.");
    }
    private void OnEnable()
    {
        if (ModEnabled.Value)
        {
            Harmony.PatchAll();
            L($"Loaded {PluginName}!");
        }
        else
        {
            L($"{PluginName} is disabled in config!");
        }
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
    }

    public static void L(string message)
    {
        Log.LogInfo(message);
    }
}