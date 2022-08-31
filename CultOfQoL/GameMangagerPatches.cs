using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CultOfQoL
{
    internal static class GameMangagerPatches
    {
        [HarmonyPatch(typeof(GameManager), "Update")]
        public static class FollowerCommandGroupsOldAgeCommands
        {
            [HarmonyPostfix]
            public static void Postfix(ref int ___CurrentGameSpeed)
            {
                if (!Plugin.EnableGameSpeedManipulation.Value) return;
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
                float newSpeed;
                int num;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    
                    ___CurrentGameSpeed = 1;
                    GameManager.SetTimeScale(1);
                    NotificationCentre.Instance.PlayGenericNotification($"Returned game speed to 1 (default)"
                        );
                    return;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    num = ___CurrentGameSpeed + 1;
                    ___CurrentGameSpeed = num % gameSpeed.Count;
                    newSpeed = gameSpeed[___CurrentGameSpeed];
                    GameManager.SetTimeScale(newSpeed);
                    NotificationCentre.Instance.PlayGenericNotification(newSpeed == 1
                        ? $"Returned game speed to {newSpeed} (default)"
                        : $"Increased game speed to {newSpeed}");
                    return;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    num = ___CurrentGameSpeed - 1;
                    ___CurrentGameSpeed = num % gameSpeed.Count;
                    newSpeed = gameSpeed[___CurrentGameSpeed];
                    GameManager.SetTimeScale(newSpeed);
                    NotificationCentre.Instance.PlayGenericNotification(newSpeed == 1
                        ? $"Returned game speed to {newSpeed} (default)"
                        : $"Decreased game speed to {newSpeed}");
                }
            }
        }
    }
}