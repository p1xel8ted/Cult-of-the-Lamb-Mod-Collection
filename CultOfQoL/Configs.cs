using BepInEx.Configuration;
using Lamb.UI.MainMenu;

namespace CultOfQoL;

public partial class Plugin
{
    internal static ConfigEntry<bool> EnableQuickSaveShortcut = null!;
    internal static ConfigEntry<KeyboardShortcut> SaveKeyboardShortcut = null!;

    internal static ConfigEntry<bool> DisableAd = null!;
    internal static ConfigEntry<bool> HideNewGameButtons = null!;
    internal static ConfigEntry<bool> EnableCustomUiScale = null!;
    internal static ConfigEntry<int> CustomUiScale = null!;

    internal static ConfigEntry<bool> SkipDevIntros = null!;
    internal static ConfigEntry<bool> SkipCrownVideo = null!;
    internal static ConfigEntry<bool> EasyFishing = null!;
    internal static ConfigEntry<bool> FastCollecting = null!;
    internal static ConfigEntry<bool> RemoveMenuClutter = null!;
    internal static ConfigEntry<bool> RemoveTwitchButton = null!;
    internal static ConfigEntry<bool> BulkFollowerCommands = null!;
    internal static ConfigEntry<bool> ReverseGoldenFleeceDamageChange = null!;
    internal static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate = null!;
    internal static ConfigEntry<bool> AdjustRefineryRequirements = null!;

    internal static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp = null!;
    internal static ConfigEntry<bool> UnlockTwitchStuff = null!;
    internal static ConfigEntry<bool> LumberAndMiningStationsDontAge = null!;
    internal static ConfigEntry<bool> CollectTitheFromOldFollowers = null!;
    internal static ConfigEntry<bool> EnableGameSpeedManipulation = null!;
    internal static ConfigEntry<bool> JustRightSiloCapacity = null!;
    internal static ConfigEntry<bool> DoubleSoulCapacity = null!;
    internal static ConfigEntry<bool> ShortenGameSpeedIncrements = null!;
    internal static ConfigEntry<bool> SlowDownTime = null!;
    internal static ConfigEntry<float> SlowDownTimeMultiplier = null!;
    internal static ConfigEntry<bool> DoubleLifespanInstead = null!;
    internal static ConfigEntry<bool> DisableGameOver = null!;

    internal static ConfigEntry<bool> ModEnabled = null!;
    internal static ConfigEntry<bool> TurnOffSpeakersAtNight = null!;
    internal static ConfigEntry<bool> ThriceMultiplyTarotCardLuck = null!;
    internal static ConfigEntry<bool> FiftyPercentIncreaseToLifespanInstead = null!;

    internal static ConfigEntry<bool> EnableAutoInteract = null!;
    internal static ConfigEntry<int> TriggerAmount = null!;
    internal static ConfigEntry<bool> IncreaseRange = null!;
    internal static ConfigEntry<bool> UseCustomRange = null!;
    internal static ConfigEntry<float> CustomRangeMulti = null!;

    internal static ConfigEntry<bool> AddExhaustedToHealingBay = null!;
    internal static ConfigEntry<bool> NotifyOfScarecrowTraps = null!;
    internal static ConfigEntry<bool> NotifyOfNoFuel = null!;
    internal static ConfigEntry<bool> NotifyOfBedCollapse = null!;

    internal static ConfigEntry<bool> GiveFollowersNewNecklaces = null!;


    public static ConfigEntry<bool> RandomWeatherChangeWhenExitingArea = null!;
    internal static ConfigEntry<bool> ChangeWeatherOnPhaseChange = null!;
    internal static ConfigEntry<bool> ShowPhaseNotifications = null!;
    internal static ConfigEntry<bool> ShowWeatherChangeNotifications = null!;

    internal static ConfigEntry<float> CustomSoulCapacityMulti = null!;
    internal static ConfigEntry<float> CustomSiloCapacityMulti = null!;
    internal static ConfigEntry<bool> UseCustomSoulCapacity = null!;
    internal static ConfigEntry<bool> UseCustomSiloCapacity = null!;

    internal static ConfigEntry<bool> EnableBaseDamageMultiplier = null!;
    internal static ConfigEntry<float> BaseDamageMultiplier = null!;

    public static ConfigEntry<float> CustomDamageMulti = null!;

    
    public static ConfigEntry<bool> RemoveHelpButtonInPauseMenu = null!;
    public static ConfigEntry<bool> RemoveTwitchButtonInPauseMenu  = null!;
    public static ConfigEntry<bool> RemovePhotoModeButtonInPauseMenu = null!;
    
    public static ConfigEntry<bool> EnableRunSpeedMulti = null!;
    public static ConfigEntry<bool> EnableDodgeSpeedMulti = null!;
    public static ConfigEntry<bool> EnableLungeSpeedMulti = null!;
    public static ConfigEntry<float> RunSpeedMulti = null!;
    public static ConfigEntry<float> DodgeSpeedMulti = null!;
    public static ConfigEntry<float> LungeSpeedMulti = null!;
    
    public static ConfigEntry<bool> OnlyShowDissenters = null!;
    
    public static ConfigEntry<bool> DisablePropagandaSpeakerAudio = null!;
    
    public static ConfigEntry<bool> UseCustomDamageValue = null!;
    public static ConfigEntry<bool> MassCollecting = null!;
    public static WriteOnce<float> LumberFastCollect { get; } = new();
    public static WriteOnce<float> OtherFastCollect { get; } = new();

    public static WriteOnce<float> RunSpeed { get; } = new();
    public static WriteOnce<float> DodgeSpeed { get; } = new();
    public static UIMainMenuController UIMainMenuController = null!;
}