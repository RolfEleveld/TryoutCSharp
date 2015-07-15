// <copyright file="GetPlayersList.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    /// <summary>
    /// Handles the GetPlayersList Command.
    /// </summary>
    internal class GetPlayersList : IGameCommand
    {
        /// <summary>
        /// Gets the command string..
        /// </summary>
        public string Name
        {
            get { return "GetPlayersList"; }
        }

        /// <summary>
        /// Implements the execution of GetPlayersList command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            var players = string.Empty;
            players = sessions.GetOtherLoggedInSessionsList(current);
            
            // Removes last semicolon from players string.
            if (!string.IsNullOrEmpty(players))
            {
                players = players.Substring(0, players.Length - 1);
            }

            sessions.SendMessage(current, "GetPlayersListResponse:" + players);
        }
    }
}
