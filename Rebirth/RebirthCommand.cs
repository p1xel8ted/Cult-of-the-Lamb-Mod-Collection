using System;
using System.IO;
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using Lamb.UI;
using UnityEngine;

namespace Rebirth
{
    internal class RebirthFollowerCommand : CustomFollowerCommand
    {
        public override string InternalName => "REBIRTH_COMMAND";

        public override Sprite CommandIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));

        public override string GetTitle(Follower follower)
        {
            return "Rebirth";
        }

        public override string GetDescription(Follower follower)
        {
            return "Tired of looking at this sheep? Order a rebirth!";
        }

        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            var rebirthMenu = MonoSingleton<UIManager>.Instance.ShowIndoctrinationMenu(interaction.follower);

            rebirthMenu.OnIndoctrinationCompleted = (Action) Delegate.Combine(rebirthMenu.OnIndoctrinationCompleted, new Action(delegate
            {
                GameManager.GetInstance().OnConversationEnd();
                interaction.enabled = true;
                interaction.follower.SimpleAnimator.ResetAnimationsToDefaults();
                interaction.follower.Resume();
                interaction.follower.Brain.CompleteCurrentTask();
            }));
        }
    }
}