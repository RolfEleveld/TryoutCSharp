// <copyright file="AssignTile.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    using System;

    /// <summary>
    /// Defines the handler for the AssignTile command.
    /// </summary>
    internal class AssignTile : IGameCommand
    {
        /// <summary>
        /// Gets the name of sendDotPoint command.
        /// </summary>
        public string Name
        {
            get { return "AssignTile"; }
        }

        /// <summary>
        /// Implements the execution of the AssignTile command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "AssignTileResponse:Failure;PlayerNotFound.");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    sessions.SendMessage(current, "AssignTileResponse:Failure;WrongFormat.");
                    return;
                }

                string[] selectedTile = message.Split(new char[] { ';' });

                if (selectedTile.Length != 2)
                {
                    sessions.SendMessage(current, "AssignTileResponse:Failure;WrongFormat.");
                    return;
                }

                sessions.SendMessage(current.Context.PlayerInstance, "AssignTileToCell:" + selectedTile[0] + ";" + selectedTile[1]);
                sessions.SendMessage(current, "AssignTileResponse:Successful");
            }
        }
    }
}
