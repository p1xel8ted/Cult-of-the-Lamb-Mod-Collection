using System.IO;
using System.Linq;
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using UnityEngine;


namespace Rebirth
{
    internal class RebirthSubCommand : CustomFollowerCommand
    {
        private const int ItemQty = 25;
        public override string InternalName => "REBIRTH_SUB_COMMAND";

        public override Sprite CommandIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));

        public override string GetTitle(Follower follower)
        {
            return $"Rebirth for {ItemQty} tokens.";
        }

        public RebirthSubCommand()
        {
            SubCommands = FollowerCommandGroups.AreYouSureCommands();
        }

        public override string GetDescription(Follower follower)
        {
            return "Perform a Rebirth using special tokens obtained while on crusades.";
        }

        public override string GetLockedDescription(Follower follower)
        {
            return "Requires 25 Rebirth tokens to perform.";
        }

        public override bool ShouldAppearFor(Follower follower)
        {
            var bornAgainFollower = SaveData.GetBornAgainFollowerData(follower.Brain._directInfoAccess);
            return bornAgainFollower is {BornAgain: true};
        }

        public override bool IsAvailable(Follower follower)
        {
            return Inventory.GetItemQuantity((int)Plugin.RebirthItem) >= ItemQty;
        }


        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            if (finalCommand != FollowerCommands.AreYouSureYes) return;
            RebirthFollowerCommand.SpawnRecruit(interaction.follower);
            Inventory.ChangeItemQuantity(Plugin.RebirthItem,-ItemQty);
        }
    }
}