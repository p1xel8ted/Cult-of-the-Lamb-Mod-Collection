using COTL_API.CustomMission;

namespace Rebirth;

public class MissionItem2: CustomMission
{
    public override string InternalName => "REBIRTH_MISSION_2";

    public override InventoryItem.ITEM_TYPE RewardType => InventoryItem.ITEM_TYPE.VINES;

    public override int BaseChance => 75;

    public override IntRange RewardRange => new(5, 15);
}