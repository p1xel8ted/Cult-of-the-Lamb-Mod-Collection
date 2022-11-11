using COTL_API.CustomMission;

namespace Rebirth;

public class MissionItem3: CustomMission
{
    public override string InternalName => "REBIRTH_MISSION_3";

    public override InventoryItem.ITEM_TYPE RewardType => InventoryItem.ITEM_TYPE.DOCTRINE_STONE;

    public override int BaseChance => 75;

    public override IntRange RewardRange => new(2, 5);
}