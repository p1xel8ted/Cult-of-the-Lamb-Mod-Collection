namespace CultOfQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.2")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
    private const string PluginName = "Cult of QoL Collection";
    private const string PluginVer = "2.1.4";
    private const string GeneralSection = "01. General";
    private const string MenuCleanupSection = "02. Menu Cleanup";
    private const string GameMechanicsSection = "03. Game Mechanics";
    private const string PlayerDamageSection = "05. Player Damage";
    private const string PlayerSpeedSection = "06. Player Speed";
    private const string SavesSection = "07. Saves";
    private const string ScalingSection = "08. Scale";
    private const string WeatherSection = "09. Weather";
    private const string NotificationsSection = "10. Notifications";
    private const string FollowersSection = "11. Followers";
    private const string FarmSection = "12. Farm";
    private const string GameSpeedSection = "13. Game Speed";
    private const string CapacitySection = "14. Capacities";
    private const string AutoInteractSection = "15. Auto-Interact (Chests)";
    private const string PropagandaSection = "16. Propaganda Structure";
    private const string MinesSection = "17. Mines";
    private const string MassSection = "18. Mass Actions";
    private const string StructureSection = "04. Structures";

    internal static ManualLogSource Log = null!;
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

    
        
        //General
        SkipDevIntros = Config.Bind(GeneralSection, "Skip Intros", true, new ConfigDescription("Skip splash screens.", null, new ConfigurationManagerAttributes {Order = 3}));
        SkipCrownVideo = Config.Bind(GeneralSection, "Skip Crown Video", true, new ConfigDescription("Skips the video when the lamb gets given the crown.", null, new ConfigurationManagerAttributes {Order = 2}));
        UnlockTwitchStuff = Config.Bind(GeneralSection, "Unlock Twitch Stuff", true, new ConfigDescription("Unlock pre-order DLC, Twitch plush, and Twitch drops. Paid DLC is excluded on purpose.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Menu Cleanup
        RemoveMenuClutter = Config.Bind(MenuCleanupSection, "Remove Extra Menu Buttons", true, new ConfigDescription("Removes credits/road-map/discord buttons from the menus.", null, new ConfigurationManagerAttributes {Order = 6}));
        RemoveTwitchButton = Config.Bind(MenuCleanupSection, "Remove Twitch Buttons", true, new ConfigDescription("Removes twitch buttons from the menus.", null, new ConfigurationManagerAttributes {Order = 5}));
        DisableAds = Config.Bind(MenuCleanupSection, "Disable Ads", true, new ConfigDescription("Disables the new ad 'feature'.", null, new ConfigurationManagerAttributes {Order = 4}));
        RemoveHelpButtonInPauseMenu = Config.Bind(MenuCleanupSection, "Remove Help Button In Pause Menu", true, new ConfigDescription("Removes the help button in the pause menu.", null, new ConfigurationManagerAttributes {Order = 3}));
        RemoveTwitchButtonInPauseMenu = Config.Bind(MenuCleanupSection, "Remove Twitch Button In Pause Menu", true, new ConfigDescription("Removes the twitch button in the pause menu.", null, new ConfigurationManagerAttributes {Order = 2}));
        RemovePhotoModeButtonInPauseMenu = Config.Bind(MenuCleanupSection, "Remove Photo Mode Button In Pause Menu", true, new ConfigDescription("Removes the photo mode button in the pause menu.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Player Damage
        EnableBaseDamageMultiplier = Config.Bind(PlayerDamageSection, "Enable Base Damage Multiplier", false, new ConfigDescription("Enable/disable the base damage multiplier.", null, new ConfigurationManagerAttributes {Order = 6}));
        BaseDamageMultiplier = Config.Bind(PlayerDamageSection, "Base Damage Multiplier", 1.5f, new ConfigDescription("The base damage multiplier to use.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 5}));
        ReverseGoldenFleeceDamageChange = Config.Bind(GameMechanicsSection, "Reverse Golden Fleece Change", true, new ConfigDescription("Reverts the default damage increase to 10% instead of 5%.", null, new ConfigurationManagerAttributes {Order = 4}));
        IncreaseGoldenFleeceDamageRate = Config.Bind(GameMechanicsSection, "Increase Golden Fleece Rate", true, new ConfigDescription("Doubles the damage increase.", null, new ConfigurationManagerAttributes {Order = 3}));
        UseCustomDamageValue = Config.Bind(GameMechanicsSection, "Use Custom Damage Value", false, new ConfigDescription("Use a custom damage value instead of the default 10%.", null, new ConfigurationManagerAttributes {Order = 2}));
        CustomDamageMulti = Config.Bind(GameMechanicsSection, "Custom Damage Multiplier", 2.0f, new ConfigDescription("The custom damage multiplier to use. Based off the games default 5%.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 1}));
        ;

        //Player Speed
        EnableRunSpeedMulti = Config.Bind(PlayerSpeedSection, "Enable Run Speed Multiplier", true, new ConfigDescription("Enable/disable the run speed multiplier.", null, new ConfigurationManagerAttributes {Order = 6}));
        RunSpeedMulti = Config.Bind(PlayerSpeedSection, "Run Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player runs.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 5}));
        EnableDodgeSpeedMulti = Config.Bind(PlayerSpeedSection, "Enable Dodge Speed Multiplier", true, new ConfigDescription("Enable/disable the dodge speed multiplier.", null, new ConfigurationManagerAttributes {Order = 4}));
        DodgeSpeedMulti = Config.Bind(PlayerSpeedSection, "Dodge Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player dodges.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 3}));
        EnableLungeSpeedMulti = Config.Bind(PlayerSpeedSection, "Enable Lunge Speed Multiplier", true, new ConfigDescription("Enable/disable the lunge speed multiplier.", null, new ConfigurationManagerAttributes {Order = 2}));
        LungeSpeedMulti = Config.Bind(PlayerSpeedSection, "Lunge Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player lunges.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 1}));

        //Save
        SaveOnQuitToDesktop = Config.Bind(SavesSection, "Save On Quit To Desktop", true, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to desktop.", null, new ConfigurationManagerAttributes {Order = 8}));
        SaveOnQuitToMenu = Config.Bind(SavesSection, "Save On Quit To Menu", true, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to menu.", null, new ConfigurationManagerAttributes {Order = 7}));
        HideNewGameButtons = Config.Bind(SavesSection, "Hide New Game Button (s)", true, new ConfigDescription("Hides the new game button if you have at least one save game.", null, new ConfigurationManagerAttributes {Order = 6}));
        EnableQuickSaveShortcut = Config.Bind(SavesSection, "Enable Quick Save Shortcut", true, new ConfigDescription("Enable/disable the quick save keyboard shortcut.", null, new ConfigurationManagerAttributes {Order = 5}));
        SaveKeyboardShortcut = Config.Bind(SavesSection, "Save Keyboard Shortcut", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("The keyboard shortcut to save the game.", null, new ConfigurationManagerAttributes {Order = 4}));
        DirectLoadSave = Config.Bind(SavesSection, "Direct Load Save", false, new ConfigDescription("Directly load the specified save game instead of showing the save menu.", null, new ConfigurationManagerAttributes {Order = 3}));
        DirectLoadSkipKey = Config.Bind(SavesSection, "Direct Load Skip Key", new KeyboardShortcut(KeyCode.LeftShift), new ConfigDescription("The keyboard shortcut to skip the auto-load when loading the game.", null, new ConfigurationManagerAttributes {Order = 2}));
        SaveSlotToLoad = Config.Bind(SavesSection, "Save Slot To Load", 1, new ConfigDescription("The save slot to load.", new AcceptableValueList<int>(1, 2, 3), new ConfigurationManagerAttributes {Order = 1}));
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
        EnableCustomUiScale = Config.Bind(ScalingSection, "Enable Custom UI Scale", false, new ConfigDescription("Enable/disable the custom UI scale.", null, new ConfigurationManagerAttributes {Order = 2}));
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

        CustomUiScale = Config.Bind(ScalingSection, "Custom UI Scale", 50, new ConfigDescription("The custom UI scale to use.", new AcceptableValueRange<int>(1, 101), new ConfigurationManagerAttributes {Order = 1}));
        CustomUiScale.SettingChanged += (_, _) =>
        {
            Scales.UpdateScale();
        };

        //Weather
        ChangeWeatherOnPhaseChange = Config.Bind(WeatherSection, "Change Weather On Phase Change", true, new ConfigDescription("By default, the game changes weather when you exit a structure, or on a new day. Enabling this makes the weather change on each phase i.e. morning, noon, evening, night.", null, new ConfigurationManagerAttributes {Order = 2}));
        RandomWeatherChangeWhenExitingArea = Config.Bind(WeatherSection, "Random Weather Change When Exiting Area", true, new ConfigDescription("When exiting a building/area, the weather will change to a random weather type instead of the previous weather.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Game Mechanics
        EasyFishing = Config.Bind(GameMechanicsSection, "Disable Fishing Mini-Game", true, new ConfigDescription("Fishing mini-game cheese. Just cast and let the mod do the rest.", null, new ConfigurationManagerAttributes {Order = 3}));
        DisableGameOver = Config.Bind(GameMechanicsSection, "No More Game-Over", false, new ConfigDescription("Disables the game over function when you have 0 followers for consecutive days.", null, new ConfigurationManagerAttributes {Order = 2}));
        ThriceMultiplyTarotCardLuck = Config.Bind(GameMechanicsSection, "3x Tarot Luck", true, new ConfigDescription("Luck changes with game difficulty, this will multiply your luck multiplier by 3 for drawing rarer tarot cards.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Mines
        LumberAndMiningStationsDontAge = Config.Bind(MinesSection, "Infinite Lumber & Mining Stations", false, new ConfigDescription("Lumber and mining stations should never run out and collapse. Takes 1st priority.", null, new ConfigurationManagerAttributes {Order = 3}));
        LumberAndMiningStationsDontAge.SettingChanged += (_, _) =>
        {
            if (!LumberAndMiningStationsDontAge.Value) return;
            DoubleLifespanInstead.Value = false;
            FiftyPercentIncreaseToLifespanInstead.Value = false;
        };

        DoubleLifespanInstead = Config.Bind(MinesSection, "Double Life Span Instead", false, new ConfigDescription("Doubles the life span of lumber/mining stations. Takes 2nd priority.", null, new ConfigurationManagerAttributes {Order = 2}));
        DoubleLifespanInstead.SettingChanged += (_, _) =>
        {
            if (!DoubleLifespanInstead.Value) return;
            LumberAndMiningStationsDontAge.Value = false;
            FiftyPercentIncreaseToLifespanInstead.Value = false;
        };

        FiftyPercentIncreaseToLifespanInstead = Config.Bind(MinesSection, "Add 50% to Life Span Instead", true, new ConfigDescription("For when double is too long for your tastes. This will extend their life by 50% instead of 100%. Takes 3rd priority.", null, new ConfigurationManagerAttributes {Order = 1}));
        FiftyPercentIncreaseToLifespanInstead.SettingChanged += (_, _) =>
        {
            if (!FiftyPercentIncreaseToLifespanInstead.Value) return;
            LumberAndMiningStationsDontAge.Value = false;
            DoubleLifespanInstead.Value = false;
        };

        //Structures
        TurnOffSpeakersAtNight = Config.Bind(StructureSection, "Turn Off Speakers At Night", true, new ConfigDescription("Turns the speakers off, and stops fuel consumption at night time.", null, new ConfigurationManagerAttributes {Order = 5}));
        DisablePropagandaSpeakerAudio = Config.Bind(StructureSection, "Disable Propaganda Speaker Audio", true, new ConfigDescription("Disables the audio from propaganda speakers.", null, new ConfigurationManagerAttributes {Order = 4}));
        AddExhaustedToHealingBay = Config.Bind(StructureSection, "Add Exhausted To Healing Bay", true, new ConfigDescription("Allows you to select exhausted followers for rest and relaxation in the healing bays.", null, new ConfigurationManagerAttributes {Order = 3}));
        OnlyShowDissenters = Config.Bind(StructureSection, "Only Show Dissenters In Prison Menu", true, new ConfigDescription("Only show dissenting followers when interacting with the prison.", null, new ConfigurationManagerAttributes {Order = 2}));
        AdjustRefineryRequirements = Config.Bind(StructureSection, "Adjust Refinery Requirements", true, new ConfigDescription("Where possible, halves the materials needed to convert items in the refinery. Rounds up.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Speed
        EnableGameSpeedManipulation = Config.Bind(GameSpeedSection, "Enable Game Speed Manipulation", true, new ConfigDescription("Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.", null, new ConfigurationManagerAttributes {Order = 5}));
        ShortenGameSpeedIncrements = Config.Bind(GameSpeedSection, "Shorten Game Speed Increments", false, new ConfigDescription("Increments in steps of 1, instead of 0.25.", null, new ConfigurationManagerAttributes {Order = 4}));
        FastCollecting = Config.Bind(GameSpeedSection, "Speed Up Collection", true, new ConfigDescription("Increases the rate you can collect from the shrines, and other structures.", null, new ConfigurationManagerAttributes {Order = 3}));
        SlowDownTime = Config.Bind(GameSpeedSection, "Slow Down Time", false, new ConfigDescription("Enables the ability to slow down time. This is different to the increase speed implementation. This will make the days longer, but not slow down animations.", null, new ConfigurationManagerAttributes {Order = 2}));
        SlowDownTimeMultiplier = Config.Bind(GameSpeedSection, "Slow Down Time Multiplier", 2f, new ConfigDescription("The multiplier to use for slow down time. For example, the default value of 2 is making the day twice as long.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 1}));

        //Chest Auto-Interact
        EnableAutoInteract = Config.Bind(AutoInteractSection, "Enable Auto Interact", true, new ConfigDescription("Makes chests automatically send you the resources when you're nearby.", null, new ConfigurationManagerAttributes {Order = 5}));
        TriggerAmount = Config.Bind(AutoInteractSection, "Resource Trigger Amount", 5, new ConfigDescription("How many items you want in the chest before triggering auto collect.", new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes {Order = 4}));
        IncreaseRange = Config.Bind(AutoInteractSection, "Double Activation Range", true, new ConfigDescription("The default range is 5. This will increase it to 10.", null, new ConfigurationManagerAttributes {Order = 3}));
        UseCustomRange = Config.Bind(AutoInteractSection, "Use Custom Range", false, new ConfigDescription("Use a custom range instead of the default or increased range.", null, new ConfigurationManagerAttributes {Order = 2}));
        CustomRangeMulti = Config.Bind(AutoInteractSection, "Custom Range Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for auto-collect range when using custom range.", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes {Order = 1}));

        //Capacity
        UseCustomSiloCapacity = Config.Bind(CapacitySection, "Use Custom Silo Capacity", false, new ConfigDescription("Use a custom silo capacity instead of the default or increased capacity.", null, new ConfigurationManagerAttributes {Order = 5}));
        CustomSiloCapacityMulti = Config.Bind(CapacitySection, "Custom Silo Capacity Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for silo capacity when using custom capacity.", new AcceptableValueRange<float>(0, 1000), null, new ConfigurationManagerAttributes {Order = 4}));
        DoubleSoulCapacity = Config.Bind(CapacitySection, "Double Soul Capacity", true, new ConfigDescription("Doubles the soul capacity of applicable structures.", null, new ConfigurationManagerAttributes {Order = 3}));
        UseCustomSoulCapacity = Config.Bind(CapacitySection, "Use Custom Soul Capacity", false, new ConfigDescription("Use a custom soul capacity instead of the default or doubled capacity.", null, new ConfigurationManagerAttributes {Order = 2}));
        CustomSoulCapacityMulti = Config.Bind(CapacitySection, "Custom Soul Capacity Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for soul capacity when using custom capacity.", new AcceptableValueRange<float>(0, 1000), null, new ConfigurationManagerAttributes {Order = 1}));

        //Notifications
        NotifyOfScarecrowTraps = Config.Bind(NotificationsSection, "Notify of Scarecrow Traps", true, new ConfigDescription("Display a notification when the farm scarecrows have caught a trap!", null, new ConfigurationManagerAttributes {Order = 5}));
        NotifyOfNoFuel = Config.Bind(NotificationsSection, "Notify of No Fuel", true, new ConfigDescription("Display a notification when a structure has run out of fuel.", null, new ConfigurationManagerAttributes {Order = 4}));
        NotifyOfBedCollapse = Config.Bind(NotificationsSection, "Notify of Bed Collapse", true, new ConfigDescription("Display a notification when a bed has collapsed.", null, new ConfigurationManagerAttributes {Order = 3}));
        ShowPhaseNotifications = Config.Bind(NotificationsSection, "Phase Notifications", true, new ConfigDescription("Show a notification when the time of day changes.", null, new ConfigurationManagerAttributes {Order = 2}));
        ShowWeatherChangeNotifications = Config.Bind(NotificationsSection, "Weather Change Notifications", true, new ConfigDescription("Show a notification when the weather changes.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Followers
        GiveFollowersNewNecklaces = Config.Bind(FollowersSection, "Give Followers New Necklaces", true, new ConfigDescription("Followers will be able to receive new necklaces, with the old one being returned to you.", null, new ConfigurationManagerAttributes {Order = 5}));
        CleanseIllnessAndExhaustionOnLevelUp = Config.Bind(FollowersSection, "Cleanse Illness and Exhaustion", true, new ConfigDescription("When a follower 'levels up', if they are sick or exhausted, the status is cleansed.", null, new ConfigurationManagerAttributes {Order = 4}));
        CollectTitheFromOldFollowers = Config.Bind(FollowersSection, "Collect Tithe From Old Followers", true, new ConfigDescription("Enable collecting tithe from the elderly.", null, new ConfigurationManagerAttributes {Order = 3}));
        IntimidateOldFollowers = Config.Bind(FollowersSection, "Intimidate Old Followers", true, new ConfigDescription("Enable intimidating the elderly.", null, new ConfigurationManagerAttributes {Order = 2}));
        RemoveLevelLimit = Config.Bind(FollowersSection, "Remove Level Limit", true, new ConfigDescription("Removes the level limit for followers. They can now level up infinitely.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Mass Section
        MassLevelUp = Config.Bind(MassSection, "Mass Level Up", true, new ConfigDescription("When interacting with a follower than can level, all eligible followers will be leveled up.", null, new ConfigurationManagerAttributes {Order = 14}));
        MassFertilize = Config.Bind(MassSection, "Mass Fertilize", true, new ConfigDescription("When fertilizing a plot, all farm plots are fertilized at once.", null, new ConfigurationManagerAttributes {Order = 13}));
        MassWater = Config.Bind(MassSection, "Mass Water", true, new ConfigDescription("When watering a plot, all farm plots are watered at once.", null, new ConfigurationManagerAttributes {Order = 12}));
        MassBribe = Config.Bind(MassSection, "Mass Bribe", true, new ConfigDescription("When bribing a follower, all followers are bribed at once.", null, new ConfigurationManagerAttributes {Order = 11}));
        MassBless = Config.Bind(MassSection, "Mass Bless", true, new ConfigDescription("When blessing a follower, all followers are blessed at once.", null, new ConfigurationManagerAttributes {Order = 10}));
        MassExtort = Config.Bind(MassSection, "Mass Extort", true, new ConfigDescription("When extorting a follower, all followers are extorted at once.", null, new ConfigurationManagerAttributes {Order = 9}));
        MassPetDog = Config.Bind(MassSection, "Mass Pet Dog", true, new ConfigDescription("When petting a a follower, all followers are petted at once.", null, new ConfigurationManagerAttributes {Order = 8}));
        MassIntimidate = Config.Bind(MassSection, "Mass Intimidate", true, new ConfigDescription("When intimidating a follower, all followers are intimidated at once.", null, new ConfigurationManagerAttributes {Order = 7}));
        MassInspire = Config.Bind(MassSection, "Mass Inspire", true, new ConfigDescription("When inspiring a follower, all followers are inspired at once.", null, new ConfigurationManagerAttributes {Order = 6}));
        MassCollectFromBeds = Config.Bind(MassSection, "Mass Collect From Beds", true, new ConfigDescription("When collecting resources from a bed, all beds are collected from at once.", null, new ConfigurationManagerAttributes {Order = 5}));
        MassCollectFromOuthouses = Config.Bind(MassSection, "Mass Collect From Outhouses", true, new ConfigDescription("When collecting resources from an outhouse, all outhouses are collected from at once.", null, new ConfigurationManagerAttributes {Order = 4}));
        MassCollectFromOfferingShrines = Config.Bind(MassSection, "Mass Collect From Offering Shrines", true, new ConfigDescription("When collecting resources from an offering shrine, all offering shrines are collected from at once.", null, new ConfigurationManagerAttributes {Order = 3}));
        MassCollectFromPassiveShrines = Config.Bind(MassSection, "Mass Collect From Passive Shrines", true, new ConfigDescription("When collecting resources from a passive shrine, all passive shrines are collected from at once.", null, new ConfigurationManagerAttributes {Order = 2}));
        MassCollectFromCompost = Config.Bind(MassSection, "Mass Collect From Compost", true, new ConfigDescription("When collecting resources from a compost, all composts are collected from at once.", null, new ConfigurationManagerAttributes {Order = 1}));
        MassCollectFromHarvestTotems = Config.Bind(MassSection, "Mass Collect From Harvest Totems", true, new ConfigDescription("When collecting resources from a harvest totem, all harvest totems are collected from at once.", null, new ConfigurationManagerAttributes {Order = 0}));
        // if (!SoftDepend.Enabled) return;
        //
        // SoftDepend.AddSettingsMenus();
        // Log.LogInfo("API detected - You can configure mod settings in the settings menu."));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    public static void L(string message)
    {
        Log.LogInfo(message);
    }
}