using System;
using System.Collections.Generic;
using COTL_API.CustomInventory;
using HarmonyLib;
using Lamb.UI;
using Lamb.UI.KitchenMenu;
using src.Extensions;
using src.UI;
using src.UI.InfoCards;
using src.UI.Menus;
using UnityEngine;
using UnityEngine.ProBuilder;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Rebirth;

[HarmonyPatch]
public static class CookingMenu
{
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetReward))]
    // public static bool GetReward(ref InventoryItem.ITEM_TYPE type, ref float chance, ref int followerID, ref InventoryItem[] __result)
    // {
    //     if (type != Plugin.RebirthItem) return true;
    //     var num = Random.Range(0f, 1f);
    //     foreach (var objective in DataManager.Instance.CompletedObjectives)
    //     {
    //         if (objective.Follower != followerID) continue;
    //         chance = float.MaxValue;
    //         break;
    //     }
    //
    //     if (chance > num)
    //     {
    //         __result = new[] {new InventoryItem(Plugin.RebirthItem, TokenRange.Random())};
    //     }
    //
    //     return false;
    // }
    //
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetRewardRange))]
    // public static void MissionaryManager_GetRewardRange(ref IntRange __result, InventoryItem.ITEM_TYPE type)
    // {
    //     if (type == Plugin.RebirthItem)
    //     {
    //         __result = TokenRange;
    //     }
    // }

    // private static readonly Dictionary<int, RecipeItem> RecipeItems = new();
    //
    //
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(RecipeInfoCard), nameof(RecipeInfoCard.Configure))]
    // public static void RecipeInfoCard_Configure(ref RecipeInfoCard __instance, InventoryItem.ITEM_TYPE config)
    // {
    //     if (RecipeItems.ContainsKey(__instance.GetInstanceID()))
    //     {
    //         RecipeItems[__instance.GetInstanceID()].Configure(config);
    //         RecipeItems[__instance.GetInstanceID()].Start();
    //         return;
    //     }
    //
    //     var mission = __instance._missionButtons.RandomElement();
    //
    //     var newMission = Object.Instantiate(mission, __instance._missionButtons[0].transform.parent);
    //     __instance._missionButtons.Add(newMission);
    //
    //     newMission.name = $"Rebirth Mission";
    //     newMission._type = Plugin.RebirthItem;
    //     newMission.Configure(config);
    //     newMission.Start();
    //     var card = __instance;
    //     newMission.OnMissionSelected += delegate(InventoryItem.ITEM_TYPE itemType)
    //     {
    //         var onMissionSelected = card.OnMissionSelected;
    //         if (onMissionSelected == null)
    //         {
    //             return;
    //         }
    //
    //         onMissionSelected(itemType);
    //     };
    //
    //     MissionButtons.Add(__instance.GetInstanceID(), newMission);
    // }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetRecipe))]
    public static void CookingData_GetRecipe(InventoryItem.ITEM_TYPE mealType, ref List<List<InventoryItem>> __result)
    {
        if (mealType == Plugin.RebirthFoodItem)
        {
            __result = new List<List<InventoryItem>>
            {
                new()
                {
                    new InventoryItem(InventoryItem.ITEM_TYPE.BONE, 4)
                }
            };
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllMeals))]
    public static void CookingData_GetAllMeals_Prefix(ref InventoryItem.ITEM_TYPE[] __result)
    {
        if (!DataManager.Instance.RecipesDiscovered.Contains(Plugin.RebirthFoodItem))
        {
            DataManager.Instance.RecipesDiscovered.Add(Plugin.RebirthFoodItem);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllFoods))]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllMeals))]
    public static void CookingData_GetAllMeals_Postfix(ref InventoryItem.ITEM_TYPE[] __result)
    {
        if (!__result.Contains(Plugin.RebirthFoodItem))
        {
            __result.Add(Plugin.RebirthFoodItem);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetTummyRating))]
    public static void CookingData_GetTummyRating_Postfix(ref InventoryItem.ITEM_TYPE meal, ref float __result)
    {
        if (meal == Plugin.RebirthFoodItem)
        {
            __result = 1f;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetSatationAmount))]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetSatationLevel))]
    public static void CookingData_GetSatationAmount_Postfix(ref InventoryItem.ITEM_TYPE meal, ref int __result)
    {
        if (meal == Plugin.RebirthFoodItem)
        {
            __result = 1;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.HasRecipeDiscovered))]
    public static void HasRecipeDiscovered(InventoryItem.ITEM_TYPE meal, ref bool __result)
    {
        if (meal == Plugin.RebirthFoodItem)
        {
            __result = true;
        }
    }
    //public static float GetMealCookDuration(InventoryItem.ITEM_TYPE mealType)

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealCookDuration), typeof(InventoryItem.ITEM_TYPE))]
    public static void CookingData_GetMealCookDuration_Postfix(ref InventoryItem.ITEM_TYPE mealType, ref float __result)
    {
        if (mealType == Plugin.RebirthFoodItem)
        {
            __result = 180f;
        }
    }

    //public static StructureBrain.TYPES GetStructureFromMealType(InventoryItem.ITEM_TYPE mealType)

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetCookedMeal), typeof(InventoryItem.ITEM_TYPE))]
    public static void CookingData_GetCookedMeal_Postfix(ref InventoryItem.ITEM_TYPE mealType, ref int __result)
    {
        if (mealType == Plugin.RebirthFoodItem)
        {
            __result = DataManager.Instance.FISH_HIGH_MEALS_COOKED;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetStructureFromMealType), typeof(InventoryItem.ITEM_TYPE))]
    public static void CookingData_GetStructureFromMealType_Postfix(ref InventoryItem.ITEM_TYPE mealType, ref StructureBrain.TYPES __result)
    {
        if (mealType == Plugin.RebirthFoodItem)
        {
            __result = StructureBrain.TYPES.MEAL_GREAT_FISH;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetIcon), typeof(InventoryItem.ITEM_TYPE))]
    public static void CookingData_GetIcon_Postfix(ref InventoryItem.ITEM_TYPE mealType, ref Sprite __result)
    {
        if (mealType == Plugin.RebirthFoodItem)
        {
            __result = Plugin.RebirthFoodItemInstance.Sprite;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIMenuBase), nameof(UIMenuBase.Show), typeof(bool))]
    public static void UIRefineryMenuController_Show(ref UIMenuBase __instance)
    {
        if (__instance is not UICookingFireMenuController menu) return;

        recipeItem = menu._recipesMenu._recipeItemTemplate.Instantiate(menu._recipesMenu._contentContainer, true);
        recipeItem.Configure(Plugin.RebirthFoodItem, true, false);

        var item = new InventoryItem();
        item.Init((int) Plugin.RebirthFoodItem, 1);
        recipeItem._item = item;
        recipeItem.OnRecipeChosen += menu._recipesMenu.OnRecipeClicked;
        menu._recipesMenu._items.Add(recipeItem);
    }

    private static RecipeItem recipeItem;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_Kitchen), nameof(Interaction_Kitchen.MealFinishedCooking))]
    public static void Interaction_Kitchen_MealFinishedCooking(ref Interaction_Kitchen __instance)
    {
        Plugin.Log.LogWarning($"MealFinishedCooking: Queue: {__instance.structure.Structure_Info.QueuedMeals[0].MealType}");
    }
}