using BepInEx.Configuration;

namespace CultOfQoL;

public partial class Plugin
{
    internal static ConfigEntry<bool> SkipDevIntros;
    internal static ConfigEntry<bool> SkipCrownVideo;
    internal static ConfigEntry<bool> EasyFishing;
    internal static ConfigEntry<bool> FastCollecting;
    internal static ConfigEntry<bool> RemoveMenuClutter;
    internal static ConfigEntry<bool> RemoveNewGameButton;
    internal static ConfigEntry<bool> RemoveTwitchButton;
    internal static ConfigEntry<bool> BulkFollowerCommands;
    internal static ConfigEntry<bool> ReverseGoldenFleeceDamageChange;
    internal static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate;
    internal static ConfigEntry<bool> AdjustRefineryRequirements;

    internal static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp;
    internal static ConfigEntry<bool> UnlockTwitchStuff;
    internal static ConfigEntry<bool> LumberAndMiningStationsDontAge;
    internal static ConfigEntry<bool> CollectTitheFromOldFollowers;
    internal static ConfigEntry<bool> EnableGameSpeedManipulation;
    internal static ConfigEntry<bool> JustRightSiloCapacity;
    internal static ConfigEntry<bool> DoubleSoulCapacity;
    internal static ConfigEntry<bool> ShortenGameSpeedIncrements;
    internal static ConfigEntry<bool> SlowDownTime;
    internal static ConfigEntry<float> SlowDownTimeMultiplier;
    internal static ConfigEntry<bool> DoubleLifespanInstead;
    internal static ConfigEntry<bool> DisableGameOver;

    internal static ConfigEntry<bool> ModEnabled;
    internal static ConfigEntry<bool> TurnOffSpeakersAtNight;
    internal static ConfigEntry<bool> ThriceMultiplyTarotCardLuck;
    internal static ConfigEntry<bool> FiftyPercentIncreaseToLifespanInstead;

    internal static ConfigEntry<bool> EnableAutoInteract;
    internal static ConfigEntry<int> TriggerAmount;
    internal static ConfigEntry<bool> IncreaseRange;
    internal static ConfigEntry<bool> UseCustomRange;
    internal static ConfigEntry<float> CustomRangeMulti;

    internal static ConfigEntry<bool> AddExhaustedToHealingBay;
    internal static ConfigEntry<bool> NotifyOfScarecrowTraps;
    internal static ConfigEntry<bool> NotifyOfNoFuel;
    internal static ConfigEntry<bool> NotifyOfBedCollapse;

    internal static ConfigEntry<bool> GiveFollowersNewNecklaces;

    internal static ConfigEntry<bool> MoreDynamicWeather;
    internal static ConfigEntry<int> RainLowerChance;
    internal static ConfigEntry<int> RainUpperChance;
    internal static ConfigEntry<int> WindLowerChance;
    internal static ConfigEntry<int> WindUpperChance;
    internal static ConfigEntry<bool> ChangeWeatherOnPhaseChange;
    internal static ConfigEntry<bool> ShowPhaseNotifications;
    internal static ConfigEntry<bool> ShowWeatherChangeNotifications;

    internal static ConfigEntry<float> CustomSoulCapacityMulti;
    internal static ConfigEntry<float> CustomSiloCapacityMulti;
    internal static ConfigEntry<bool> UseCustomSoulCapacity;
    internal static ConfigEntry<bool> UseCustomSiloCapacity;

    internal static ConfigEntry<bool> EnableBaseDamageMultiplier;
    internal static ConfigEntry<float> BaseDamageMultiplier;

    public static ConfigEntry<float> CustomDamageMulti;

    public static ConfigEntry<bool> EnableRunSpeedMulti;
    public static ConfigEntry<bool> EnableDodgeSpeedMulti;
    public static ConfigEntry<bool> EnableLungeSpeedMulti;
    public static ConfigEntry<float> RunSpeedMulti;
    public static ConfigEntry<float> DodgeSpeedMulti;
    public static ConfigEntry<float> LungeSpeedMulti;
    
    public static ConfigEntry<bool> DisablePropagandaSpeakerAudio;
    
    public static ConfigEntry<bool> UseCustomDamageValue;
    public static WriteOnce<float> LumberFastCollect { get; } = new();
    public static WriteOnce<float> OtherFastCollect { get; } = new();

    public static WriteOnce<float> RunSpeed { get; } = new();
    public static WriteOnce<float> DodgeSpeed { get; } = new();
}