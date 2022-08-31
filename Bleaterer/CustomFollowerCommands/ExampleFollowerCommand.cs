using COTL_API.CustomFollowerCommand;
using UnityEngine;

namespace Bleaterer.CustomFollowerCommands
{
    internal class ExampleFollowerCommand : CustomFollowerCommand
    {
        public override string InternalName => "Example_Follower_Command";

        public override Sprite CommandIcon => base.CommandIcon;

        public override string GetTitle(Follower follower)
        {
            return "Example Follower Command";
        }

        public override string GetDescription(Follower follower)
        {
            return "This is an example follower command";
        }

        public override void Execute(interaction_FollowerInteraction interaction,
            global::FollowerCommands finalCommand)
        {
            interaction.follower.Brain.MakeOld();

            interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
            {
                interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);
                interaction.follower.Brain.HardSwapToTask(new FollowerTask_Vomit());
            }));
        }
    }
}