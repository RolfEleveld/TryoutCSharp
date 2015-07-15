// <copyright file="GameComplete.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    /// <summary>
    /// Defines handler for GameComplete command.
    /// </summary>
    internal class GameComplete : IGameCommand
    {
        /// <summary>
        /// Lock object for this class.
        /// </summary>
        private static object gameCompleteLock = new object();

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name
        {
            get { return "GameComplete"; }
        }

        /// <summary>
        /// Implements execution of this command.
        /// </summary>
        /// <param name="current">Current gameservice instance.</param>
        /// <param name="sessions">Collection of all sessons.</param>
        /// <param name="message">Command parameters.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "GameCompleteResponse:Failure;Player does not exist.");
                return;
            }
            else
            {
                JigsawGameService playerInstance = current.Context.PlayerInstance;
                lock (gameCompleteLock)
                {
                    if (null != playerInstance)
                    {
                        playerInstance.Context.PlayerInstance = null;
                    }

                    if (null != current)
                    {
                        current.Context.PlayerInstance = null;
                    }

                    // Need to send gameComplete response to the current service, gameComplete to playerInstance
                    // and also we need to braodcast gameCompleteResponse to all other sessions.
                    sessions.BroadcastMessage(BroadcastMessageType.GameCompleteResponse, current, playerInstance, "GameCompleteResponse:Successful", "GameComplete");
                    return;
                }
            }
        }
    }
}
