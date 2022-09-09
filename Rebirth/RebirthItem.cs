using System.IO;
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;

namespace Rebirth;

public class RebirthItem: CustomInventoryItem
{
    public override Sprite InventoryIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));
  
    public override string LocalizedDescription()
    {
        return "A something something used for Rebirth.";
    }

    public override string InternalName => "REBIRTH_ITEM";

    public override string LocalizedName() => "Rebirth Talisman";

    public override string LocalizedLore() => "Said to be dropped by Death herself.";
    
    //Assets/Prefabs/Structures/Other/Meal Good Fish.prefab

    //public override GameObject GameObject { get; } = Plugin.Assets.LoadAsset<GameObject>("Rebirth");

    public override InventoryItem.ITEM_CATEGORIES ItemCategory => InventoryItem.ITEM_CATEGORIES.COINS;
}