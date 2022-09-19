using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace CultOfQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
    private const string PluginName = "Cult of QoL Collection";
    private const string PluginVer = "1.9.0";

    internal static ManualLogSource Log;
    private static readonly Harmony Harmony = new(PluginGuid);

    internal static ConfigEntry<bool> SkipIntros;
    internal static ConfigEntry<bool> EasyFishing;
    internal static ConfigEntry<bool> FastCollecting;
    internal static ConfigEntry<bool> RemoveMenuClutter;
    internal static ConfigEntry<bool> RemoveTwitchButton;
    internal static ConfigEntry<bool> BulkInspireAndExtort;
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

    private static ConfigEntry<bool> _modEnabled;
    internal static ConfigEntry<bool> TurnOffSpeakersAtNight;
    internal static ConfigEntry<bool> ThriceMultiplyTarotCardLuck;
    internal static ConfigEntry<bool> FiftyPercentIncreaseToLifespanInstead;

    internal static ConfigEntry<bool> EnableAutoInteract;
    internal static ConfigEntry<int> TriggerAmount;
    internal static ConfigEntry<bool> IncreaseRange;

    internal static ConfigEntry<bool> AddExhaustedToHealingBay;
    internal static ConfigEntry<bool> NotifyOfScarecrowTraps;
    internal static ConfigEntry<bool> NotifyOfNoFuel;

    internal static ConfigEntry<bool> GiveFollowersNewNecklaces;

    private void Awake()
    {
        Log = new ManualLogSource("Cult-of-QoL-Collection");
        BepInEx.Logging.Logger.Sources.Add(Log);

        _modEnabled = Config.Bind("General", "Mod Enabled", true, "Enable/disable this mod.");

        //General
        SkipIntros = Config.Bind("General", "Skip Intros", true, "Skip splash screens.");
        RemoveMenuClutter = Config.Bind("General", "Remove Extra Menu Buttons", true, "Removes credits/road-map/discord buttons from the menus.");
        RemoveTwitchButton = Config.Bind("General", "Remove Twitch Buttons", true, "Removes twitch buttons from the menus.");
        UnlockTwitchStuff = Config.Bind("General", "Unlock Twitch Stuff", true, "Unlock pre-order DLC, Twitch plush, and three drops.");

        //Game Mechanics
        ReverseGoldenFleeceDamageChange = Config.Bind("Game Mechanics", "Reverse Golden Fleece Change", true, "Reverts the default damage increase to 10% instead of 5%.");
        IncreaseGoldenFleeceDamageRate = Config.Bind("Game Mechanics", "Increase Golden Fleece Rate", true, "Doubles the damage increase.");
        AdjustRefineryRequirements = Config.Bind("Game Mechanics", "Adjust Refinery Requirements", true, "Where possible, halves the materials needed to convert items in the refinery. Rounds up.");
        EasyFishing = Config.Bind("Game Mechanics", "Cheese Fishing Mini-Game", true, "Fishing mini-game cheese. Just cast and let the mod do the rest.");
        DisableGameOver = Config.Bind("Game Mechanics", "No More Game-Over", false, "Disables the game over function when you have 0 followers for consecutive days.");
        ThriceMultiplyTarotCardLuck = Config.Bind("Game Mechanics", "3x Tarot Luck", true, "Luck changes with game difficulty, this will multiply your luck multiplier by 3 for drawing rarer tarot cards.");

        //Lumber/mining
        LumberAndMiningStationsDontAge = Config.Bind("Lumber/Mine Mods", "Infinite Lumber & Mining Stations", false, "Lumber and mining stations should never run out and collapse. Takes 1st priority.");
        DoubleLifespanInstead = Config.Bind("Lumber/Mine Mods", "Double Life Span Instead", false, "Doubles the life span of lumber/mining stations. Takes 2nd priority.");
        FiftyPercentIncreaseToLifespanInstead = Config.Bind("Lumber/Mine Mods", "Add 50% to Life Span Instead", true, "For when double is too long for your tastes. This will extend their life by 50% instead of 100%. Takes 3rd priority.");

        //Propaganda
        TurnOffSpeakersAtNight = Config.Bind("Propaganda Mods", "Turn Off Speakers At Night", true, "Turns the speakers off, and stops fuel consumption at night time.");

        //Speed
        EnableGameSpeedManipulation = Config.Bind("Speed", "Enable Game Speed Manipulation", true, "Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.");
        ShortenGameSpeedIncrements = Config.Bind("Speed", "Shorten Game Speed Increments", false, "Increments in steps of 1, instead of 0.25.");
        FastCollecting = Config.Bind("Speed", "Speed Up Collection", true, "Increases the rate you can collect from the shrines, and other structures.");
        SlowDownTime = Config.Bind("Speed", "Slow Down Time", false, "Enables the ability to slow down time. This is different to the increase speed implementation. This will make the days longer, but not slow down animations.");
        SlowDownTimeMultiplier = Config.Bind("Speed", "Slow Down Time Multiplier", 2f, "The multiplier to use for slow down time. For example, the default value of 2 is making the day twice as long.");

        //Chest Auto-Interact
        EnableAutoInteract = Config.Bind("Chest Auto-Interact", "Enable Auto Interact", true, "Makes chests automatically send you the resources when you're nearby.");
        TriggerAmount = Config.Bind("Chest Auto-Interact", "Resource Trigger Amount", 5, "How many items you want in the chest before triggering auto collect.");
        IncreaseRange = Config.Bind("Chest Auto-Interact", "Activation Range", true, "The default range is 5. This will increase it to 10.");

        //Capacity
        JustRightSiloCapacity = Config.Bind("Capacity", "Set Silo Capacity to 32", true, "Set silo capacity for seed and fertilizer at 32.");
        DoubleSoulCapacity = Config.Bind("Capacity", "Double Soul Capacity", true, "Doubles the soul capacity of applicable structures.");

        //Notifications
        NotifyOfScarecrowTraps = Config.Bind("Notifications", "Notify of Scarecrow Traps", true, "Display a notification when the farm scarecrows have caught a trap!");
        NotifyOfNoFuel = Config.Bind("Notifications", "Notify of No Fuel", true, "Display a notification when a structure has run out of fuel.");

        //Followers
        GiveFollowersNewNecklaces = Config.Bind("Followers", "Give Followers New Necklaces", true, "Followers will be able to receive new necklaces, with the old one being returned to you.");
        CleanseIllnessAndExhaustionOnLevelUp = Config.Bind("Followers", "Cleanse Illness and Exhaustion", true, "When a follower 'levels up', if they are sick or exhausted, the status is cleansed.");
        CollectTitheFromOldFollowers = Config.Bind("Followers", "Collect Tithe From Old Followers", true, "Re-enable collecting tithe from the elderly. Brutal.");
        AddExhaustedToHealingBay = Config.Bind("Followers", "Add Exhausted To Healing Bay", true, "Allows you to select exhausted followers for rest and relaxation in the healing bays.");
        BulkInspireAndExtort = Config.Bind("Followers", "Bulk Inspire/Extort", true, "When collecting tithes, or inspiring, all followers are done at once.");
    }

    private void OnEnable()
    {
        if (_modEnabled.Value)
        {
            Harmony.PatchAll();
            Log.LogInfo($"Loaded {PluginName}!");
        }
        else
        {
            Log.LogInfo($"{PluginName} is disabled in config!");
        }
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
        L($"Unloaded {PluginName}!");
    }

    public static void L(string message)
    {
        Log.LogWarning(message);
    }
}