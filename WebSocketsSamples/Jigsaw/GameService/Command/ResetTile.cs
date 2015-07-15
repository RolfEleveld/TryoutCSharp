// <copyright file="ResetTile.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    using System;

    /// <summary>
    /// Defines the handler for the ResetTile command.
    /// </summary>
    internal class ResetTile : IGameCommand
    {
        /// <summary>
        /// Gets the name of sendDotPoint command.
        /// </summary>
        public string Name
        {
            get { return "ResetTile"; }
        }

        /// <summary>
        /// Implements the execution of the ResetTile command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "ResetTileResponse:Failure;PlayerNotFound.");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    sessions.SendMessage(current, "ResetTileResponse:Failure;WrongFormat.");
                    return;
                }

                string[] selectedTile = message.Split(new char[] { ';' });

                if (selectedTile.Length != 1)
                {
                    sessions.SendMessage(current, "ResetTileResponse:Failure;WrongFormat.");
                    return;
                }

                sessions.SendMessage(current.Context.PlayerInstance, "ResetTileBack:" + selectedTile[0]);
                sessions.SendMessage(current, "ResetTileResponse:Successful");
            }
        }
    }
}
