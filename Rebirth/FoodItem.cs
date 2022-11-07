using System.IO;
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;

namespace Rebirth;

public sealed class FoodItem : CustomInventoryItem
{
    public override Sprite InventoryIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_item.png"));
    public override Sprite Sprite { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_item.png"));


    public override string LocalizedDescription()
    {
        return "A special token obtained while on crusades that are used as currency to Rebirth followers.";
    }

    public override string InternalName => "REBIRTH_FOOD_ITEM";

    public override bool AddItemToDungeonChests => false;
    public override int DungeonChestSpawnChance => 15;
    public override int DungeonChestMinAmount => 4;
    public override int DungeonChestMaxAmount => 7;
    public override Vector3 LocalScale { get; } = new(0.7f, 0.7f, 1f);
    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate => InventoryItem.ITEM_TYPE.MEAL_GREAT;
    public override CustomItemManager.ItemRarity Rarity => CustomItemManager.ItemRarity.RARE;
    public override bool AddItemToOfferingShrine => false;

    public override bool CanBeRefined => false;
    public override InventoryItem.ITEM_TYPE RefineryInput => InventoryItem.ITEM_TYPE.BONE;
    public override int RefineryInputQty => 15;

    public override float CustomRefineryDuration => 256f;

    public override string LocalizedName() => "Rebirth Food";

    public override string LocalizedLore() => "Said to be dropped by Death herself.";
    public override bool IsFood => true;

    public override InventoryItem.ITEM_CATEGORIES ItemCategory => InventoryItem.ITEM_CATEGORIES.MEALS;
    public override bool IsCurrency => false;
}