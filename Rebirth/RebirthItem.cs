using System.IO;
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;

namespace Rebirth;

public sealed class RebirthItem: CustomInventoryItem
{
    public override Sprite InventoryIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_item.png"));
    
    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate => InventoryItem.ITEM_TYPE.BLACK_GOLD;
    public override CustomItemManager.ItemRarity Rarity => CustomItemManager.ItemRarity.COMMON;

    public override string LocalizedDescription()
    {
        return "A special token obtained while on crusades that are used as currency to Rebirth followers.";
    }

    public override string InternalName => "REBIRTH_ITEM";

    public override bool AddItemToOfferingShrine => true;
    
    public override string LocalizedName() => "Rebirth Token";

    public override string LocalizedLore() => "Said to be dropped by Death herself.";
    
    //assets must be stored in a bundle, can't be loose prefabs
    public override GameObject GameObject { get; } = Plugin.Assets.LoadAsset<GameObject>("Rebirth");
    public override Vector3 LocalScale { get; } = new(0.6f, 0.6f, 0.6f);

    public override InventoryItem.ITEM_CATEGORIES ItemCategory => InventoryItem.ITEM_CATEGORIES.COINS;
    public override bool IsCurrency  => true;
}