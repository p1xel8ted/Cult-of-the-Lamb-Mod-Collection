using System;
using System.Linq;
using COTL_API.CustomSettings;
using HarmonyLib;
using Lamb.UI;

namespace CultOfQoL;

public static class SoftDepend
{
    private static bool? _enabled;

    public static bool Enabled {
        get {
            if (_enabled != null) return (bool) _enabled;
            var plugin = BepInEx.Bootstrap.Chainloader.PluginInfos.FirstOrDefault(a=>a.Value.Metadata.GUID == "io.github.xhayper.COTL_API").Value;
            if(plugin!=null && plugin.Metadata.Version >= new Version(0, 1, 12)) {
                _enabled = true;
            } else {
                _enabled = false;
            }
            return (bool)_enabled;
        }
    }
    
    public static void AddSettingsMenus()
    {
        CustomSettingsManager.AddBepInExConfig("Cult of QoL", Plugin._modEnabled.Definition.Key, Plugin._modEnabled, b =>
        {
            if (!b)
            {
                Plugin.L("Unpatching QoL all patches.");
                Plugin.Harmony.UnpatchSelf();
            }
            else
            {
                Plugin.L("Patching QoL all patches.");
                Plugin.Harmony.PatchAll();
            }
        });
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableBaseDamageMultiplier.Definition.Key, Plugin.EnableBaseDamageMultiplier);
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.BaseDamageMultiplier.Definition.Key, Plugin.BaseDamageMultiplier, 1, MMSlider.ValueDisplayFormat.RawValue);
        
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableRunSpeedMulti.Definition.Key, Plugin.EnableRunSpeedMulti);
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.RunSpeedMulti.Definition.Key, Plugin.RunSpeedMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
        
       
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableDodgeSpeedMulti.Definition.Key, Plugin.EnableDodgeSpeedMulti);
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.DodgeSpeedMulti.Definition.Key, Plugin.DodgeSpeedMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
        
      
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableLungeSpeedMulti.Definition.Key, Plugin.EnableLungeSpeedMulti);
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.LungeSpeedMulti.Definition.Key, Plugin.LungeSpeedMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
   
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Skip Intros", Plugin.SkipDevIntros);

       
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Skip Crown Video", Plugin.SkipCrownVideo);

       
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Extra Menu Buttons", Plugin.RemoveMenuClutter);

     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Twitch Buttons", Plugin.RemoveTwitchButton);

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Unlock Twitch Stuff", Plugin.UnlockTwitchStuff);

   
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "More Dynamic Weather", Plugin.MoreDynamicWeather);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Change Weather During The Day", Plugin.ChangeWeatherOnPhaseChange);
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Light Rain Range", Plugin.RainLowerChance, 1, MMSlider.ValueDisplayFormat.RawValue);

     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Heavy Rain Range", Plugin.RainUpperChance, 1, MMSlider.ValueDisplayFormat.RawValue);

   
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Light Wind Range", Plugin.WindLowerChance, 1, MMSlider.ValueDisplayFormat.RawValue);

       
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Light Wind Range", Plugin.WindUpperChance, 1, MMSlider.ValueDisplayFormat.RawValue);

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Reverse Golden Fleece Change", Plugin.ReverseGoldenFleeceDamageChange);
        
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Increase Golden Fleece Rate", Plugin.IncreaseGoldenFleeceDamageRate);
        
       
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Use Custom Damage Value", Plugin.UseCustomDamageValue);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Custom Damage Multiplier", Plugin.CustomDamageMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
   
        
      
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "Mass Collecting", Plugin.MassCollecting);
        
       
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "Adjust Refinery Requirements", Plugin.AdjustRefineryRequirements);
        
      
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "Disable Fishing Mini-Game", Plugin.EasyFishing);
        
 
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "No More Game-Over", Plugin.DisableGameOver);
        
 
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "3x Tarot Luck", Plugin.ThriceMultiplyTarotCardLuck);
        
  
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Lumber/Mine Mods", "Infinite Lumber & Mining Stations", Plugin.LumberAndMiningStationsDontAge, b =>
         {
             if (!b) return;
             Plugin.DoubleLifespanInstead.Value = false;
             Plugin.FiftyPercentIncreaseToLifespanInstead.Value = false;   
         });
        
   
         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Lumber/Mine Mods", "Double Life Span Instead", Plugin.DoubleLifespanInstead, b =>
         {
             if (!b) return;
             Plugin.LumberAndMiningStationsDontAge.Value = false;
             Plugin.FiftyPercentIncreaseToLifespanInstead.Value = false;
         });
        

         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Lumber/Mine Mods", "Add 50% to Life Span Instead", Plugin.FiftyPercentIncreaseToLifespanInstead, b =>
         {
             if (!b) return;
             Plugin.DoubleLifespanInstead.Value = false;
             Plugin.LumberAndMiningStationsDontAge.Value = false;
         });
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Propaganda", Plugin.TurnOffSpeakersAtNight.Definition.Key, Plugin.TurnOffSpeakersAtNight);
        
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Propaganda", Plugin.DisablePropagandaSpeakerAudio.Definition.Key, Plugin.DisablePropagandaSpeakerAudio);

    
      
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.EnableGameSpeedManipulation.Definition.Key, Plugin.EnableGameSpeedManipulation);
        
  
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.ShortenGameSpeedIncrements.Definition.Key, Plugin.ShortenGameSpeedIncrements);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.FastCollecting.Definition.Key, Plugin.FastCollecting);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.SlowDownTime.Definition.Key, Plugin.SlowDownTime);
        
  
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.SlowDownTimeMultiplier.Definition.Key, Plugin.SlowDownTimeMultiplier, 1, MMSlider.ValueDisplayFormat.RawValue);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.EnableAutoInteract.Definition.Key, Plugin.EnableAutoInteract);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.TriggerAmount.Definition.Key, Plugin.TriggerAmount, 1, MMSlider.ValueDisplayFormat.RawValue);
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.IncreaseRange.Definition.Key, Plugin.IncreaseRange);
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.UseCustomRange.Definition.Key, Plugin.UseCustomRange);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.CustomRangeMulti.Definition.Key, Plugin.CustomRangeMulti, 1, MMSlider.ValueDisplayFormat.RawValue);

    

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.JustRightSiloCapacity.Definition.Key, Plugin.JustRightSiloCapacity);
        
  
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.UseCustomSiloCapacity.Definition.Key, Plugin.UseCustomSiloCapacity);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.CustomSiloCapacityMulti.Definition.Key, Plugin.CustomSiloCapacityMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
        
   
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.DoubleSoulCapacity.Definition.Key, Plugin.DoubleSoulCapacity);
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.UseCustomSoulCapacity.Definition.Key, Plugin.UseCustomSoulCapacity);

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.CustomSoulCapacityMulti.Definition.Key, Plugin.CustomSoulCapacityMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
        
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.NotifyOfScarecrowTraps.Definition.Key, Plugin.NotifyOfScarecrowTraps);
        
  
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.NotifyOfNoFuel.Definition.Key, Plugin.NotifyOfNoFuel);
        
     
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.NotifyOfBedCollapse.Definition.Key, Plugin.NotifyOfBedCollapse);
       
  
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.ShowPhaseNotifications.Definition.Key, Plugin.ShowPhaseNotifications);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.ShowWeatherChangeNotifications.Definition.Key, Plugin.ShowWeatherChangeNotifications);


        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.GiveFollowersNewNecklaces.Definition.Key, Plugin.GiveFollowersNewNecklaces);
        
    
        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.CleanseIllnessAndExhaustionOnLevelUp.Definition.Key, Plugin.CleanseIllnessAndExhaustionOnLevelUp);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.CollectTitheFromOldFollowers.Definition.Key, Plugin.CollectTitheFromOldFollowers);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.AddExhaustedToHealingBay.Definition.Key, Plugin.AddExhaustedToHealingBay);
        

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.BulkFollowerCommands.Definition.Key, Plugin.BulkFollowerCommands);

        CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.OnlyShowDissenters.Definition.Key, Plugin.OnlyShowDissenters);
    }
}