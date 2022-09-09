using System.IO;
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using UnityEngine;


namespace Rebirth
{
    internal class RebirthSubCommand : CustomFollowerCommand
    {
        private static readonly InventoryItem.ITEM_TYPE Item = Plugin.RebirthItem;
        private const int ItemQty = 25;
        public override string InternalName => "REBIRTH_SUB_COMMAND";

        public override Sprite CommandIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));

        public override string GetTitle(Follower follower)
        {
            return $"Rebirth for {ItemQty} talismans.";
        }

        public RebirthSubCommand()
        {
            SubCommands = FollowerCommandGroups.AreYouSureCommands();
        }

        public override string GetDescription(Follower follower)
        {
            return "Perform a Rebirth using special talismans ripped from the corpses of your enemies.";
        }

        public override string GetLockedDescription(Follower follower)
        {
            return "Requires 25 Rebirth talismans to perform.";
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
            RebirthFollowerCommand.SpawnRecruit(interaction.follower);
            Inventory.ChangeItemQuantity(Item,-ItemQty);
        }
    }
}