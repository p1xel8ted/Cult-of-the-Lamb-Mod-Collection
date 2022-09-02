﻿using HarmonyLib;
using System.Collections.Generic;
using MMTools;
using UnityEngine;

namespace CultOfQoL
{
    public static class GameSpeedManipulationPatches
    {
        private static float _newGameSpeed;
        private static int _newSpeed;
        
        [HarmonyPatch(typeof(GameManager), "Update")]
        public static class GameManagerUpdatePatches
        {
            [HarmonyPostfix]
            public static void Postfix(ref int ___CurrentGameSpeed)
            {
                if (!Plugin.EnableGameSpeedManipulation.Value) return;
                var gameSpeedShort = new List<float>
                {
                    1,
                    2,
                    3,
                    4,
                    5

                };
                var gameSpeed = new List<float>
                {
                    1,
                    1.25f,
                    1.5f,
                    1.75f,
                    2,
                    2.25f,
                    2.5f,
                    2.75f,
                    3,
                    3.25f,
                    3.5f,
                    3.75f,
                    4,
                    4.25f,
                    4.5f,
                    4.75f,
                    5

                };
             
               
               
                int num;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    
                    ___CurrentGameSpeed = 1;
                    _newGameSpeed = 1;
                    GameManager.SetTimeScale(1);
                    NotificationCentre.Instance.PlayGenericNotification($"Returned game speed to 1 (default)"
                        );
                    return;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    num = ___CurrentGameSpeed + 1;
                    if (Plugin.ShortenGameSpeedIncrements.Value)
                    {
                        ___CurrentGameSpeed = num % gameSpeedShort.Count;
                        _newSpeed  = num % gameSpeedShort.Count;
                        _newGameSpeed = gameSpeedShort[___CurrentGameSpeed];   
                    }
                    else
                    {
                        ___CurrentGameSpeed = num % gameSpeed.Count;
                        _newSpeed  = num % gameSpeed.Count;
                        _newGameSpeed = gameSpeed[___CurrentGameSpeed];
                    }

                    GameManager.SetTimeScale(_newGameSpeed);
                    NotificationCentre.Instance.PlayGenericNotification(_newGameSpeed == 1
                        ? $"Returned game speed to {_newGameSpeed} (default)"
                        : $"Increased game speed to {_newGameSpeed}");
                    return;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    num = ___CurrentGameSpeed - 1;
                    if (Plugin.ShortenGameSpeedIncrements.Value)
                    {
                        ___CurrentGameSpeed = num % gameSpeedShort.Count;
                        _newSpeed  = num % gameSpeedShort.Count;
                        _newGameSpeed = gameSpeedShort[___CurrentGameSpeed];   
                    }
                    else
                    {
                        ___CurrentGameSpeed = num % gameSpeed.Count;
                        _newSpeed  = num % gameSpeed.Count;
                        _newGameSpeed = gameSpeed[___CurrentGameSpeed];
                    }

                    GameManager.SetTimeScale(_newGameSpeed);
                    NotificationCentre.Instance.PlayGenericNotification(_newGameSpeed == 1
                        ? $"Returned game speed to {_newGameSpeed} (default)"
                        : $"Decreased game speed to {_newGameSpeed}");
                }
                
                if(_newGameSpeed <= 0 || _newSpeed <= 0) return;
                
                GameManager.SetTimeScale(_newGameSpeed);
                ___CurrentGameSpeed = _newSpeed;
            }
        }
    }
}