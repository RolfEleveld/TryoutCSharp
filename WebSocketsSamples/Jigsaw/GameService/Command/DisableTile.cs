// <copyright file="DisableTile.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    using System;

    /// <summary>
    /// Defines the handler for the sendDotPoint command.
    /// </summary>
    internal class DisableTile : IGameCommand
    {
        /// <summary>
        /// Gets the name of DisableTile command.
        /// </summary>
        public string Name
        {
            get { return "DisableTile"; }
        }

        /// <summary>
        /// Implements the execution of the DisableTile command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "DisableTileResponse:Failure;PlayerNotFound.");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    sessions.SendMessage(current, "DisableTileResponse:Failure;WrongFormat.");
                    return;
                }

                string[] selectedTile = message.Split(new char[] { ';' });

                if (selectedTile.Length != 1)
                {
                    sessions.SendMessage(current, "DisableTileResponse:Failure;WrongFormat.");
                    return;
                }

                sessions.SendMessage(current.Context.PlayerInstance, "DisableTileFromMove:" + selectedTile[0]);
                sessions.SendMessage(current, "DisableTileResponse:Successful");
            }
        }
    }
}
