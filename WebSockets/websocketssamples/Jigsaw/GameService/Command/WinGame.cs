// <copyright file="WinGame.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    /// <summary>
    /// Defines handler for WinGame command.
    /// </summary>
    internal class WinGame : IGameCommand
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name
        {
            get { return "WinGame"; }
        }

        /// <summary>
        /// Implements execution of this command.
        /// </summary>
        /// <param name="current">Current gameservice instance.</param>
        /// <param name="sessions">Collection of all sessons.</param>
        /// <param name="message">Command parameters.</param>
        public void Execute(GameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "WinGameResponse:Failure;PlayerNotFound.");
                return;
            }
            else
            {
                sessions.SendMessage(current.Context.PlayerInstance, "WinGame");
                sessions.SendMessage(current, "WinGameResponse:Successful");
            }
        }
    }
}
