// <copyright file="UILoadComplete.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    /// <summary>
    /// Defines the handler of a UILoadComplete command.
    /// </summary>
    internal class UILoadComplete : IGameCommand
    {
        /// <summary>
        /// Gets the name of UILoadComplete command.
        /// </summary>
        public string Name
        {
            get { return "UILoadComplete"; }
        }

        /// <summary>
        /// Implements the execution of the UILoadComplete command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "UILoadCompleteResponse:Failure;PlayerNotFound.");
                return;
            }
            else
            {
                sessions.SendMessage(current.Context.PlayerInstance, "CompleteUILoad:" + current.Context.LogOnName);
                sessions.SendMessage(current, "UILoadCompleteResponse:Successful");
            }
        }
    }
}
