// using HarmonyLib;
// using Lamb.UI.MainMenu;
// using TMPro;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.UI;
//
// namespace CultOfQoL;
//
// [HarmonyPatch]
// internal class Misc
// {
//     private static Button _modsButton;
//     
//     public static void ModButtonClick()
//     {
//         //
//     }
//
//     [HarmonyPatch(typeof(MainMenu), nameof(Lamb.UI.MainMenu.MainMenu.Start))]
//     [HarmonyPrefix]
//     public static void MainMenu_Start(MainMenu __instance)
//     {
//         _modsButton = UnityEngine.Object.Instantiate(__instance._playButton);
//         var myTransform = _modsButton.transform;
//         var copyTransform = __instance._playButton.transform;
//         myTransform.parent = copyTransform.parent;
//
//         // Position
//         myTransform.localRotation = copyTransform.localRotation;
//         myTransform.position = copyTransform.position;
//         myTransform.rotation = copyTransform.rotation;
//         myTransform.localScale = copyTransform.localScale;
//
//
//         _modsButton.onClick.AddListener(new UnityAction(CultOfQoL.Misc.ModButtonClick));
//         foreach (var c in _modsButton.GetComponentsInChildren<Component>())
//         {
//             Plugin.L($"Component: {c.name}, Type: {c.GetType()}"); 
//         }
//         _modsButton.GetComponentInChildren<TextMeshProUGUI>().GetComponentInChildren<TMP_Text>().text = "Mods";
//
//     }
// }