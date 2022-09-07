using System.IO;
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using UnityEngine;


namespace Rebirth
{
    internal class RebirthSubCommand : CustomFollowerCommand
    {
        private const InventoryItem.ITEM_TYPE Item = InventoryItem.ITEM_TYPE.BONE;
        private const int ItemQty = 25;
        public override string InternalName => "REBIRTH_SUB_COMMAND";

        public override Sprite CommandIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));

        public override string GetTitle(Follower follower)
        {
            return $"Rebirth for {ItemQty} " + "<sprite name=\"icon_bones\">";
        }

        public RebirthSubCommand()
        {
            SubCommands = FollowerCommandGroups.AreYouSureCommands();
        }

        public override string GetDescription(Follower follower)
        {
            return "Doesn't matter whose...";
        }

        public override string GetLockedDescription(Follower follower)
        {
            return Inventory.GetItemQuantity(Item) < ItemQty  ? $"You don't have enough bones to perform this action. Come back when you have {ItemQty} bones!" : string.Empty;
        }

        public override bool ShouldAppearFor(Follower follower)
        {
            var bornAgainFollower = SaveData.GetBornAgainFollowerData(follower.Brain._directInfoAccess);
            return bornAgainFollower is {BornAgain: true};
        }

        public override bool IsAvailable(Follower follower)
        {
            return Inventory.GetItemQuantity(Item) >= ItemQty;
        }


        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            if (finalCommand != FollowerCommands.AreYouSureYes) return;
            RebirthFollowerCommand.SpawnRecruit(interaction.follower,true);
            Inventory.ChangeItemQuantity(Item,-ItemQty);
        }
    }
}