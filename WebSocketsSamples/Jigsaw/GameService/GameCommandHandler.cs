// <copyright file="GameCommandHandler.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ServiceModel.WebSockets.Samples.Command;

    /// <summary>
    /// Maps commands to their handlers.
    /// </summary>
    public sealed class GameCommandHandler
    {
        /// <summary>
        /// Dictionary maps the command names to the corresponding command handler.
        /// </summary>
        private static readonly Dictionary<string, IGameCommand> commands = InitializeList();

        /// <summary>
        ///  Prevents a default instance of the GameCommandHandler class from being created.
        /// </summary>
        private GameCommandHandler()
        {
        }

        /// <summary>
        /// Entry point for all commands execution.
        /// </summary>
        /// <param name="current"> Current session.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Message sent by chat cient.</param>
        internal static void HandleGameMessage(JigsawGameService current, GameSessions sessions, string message)
        {
            string command = string.Empty;
            string commandArgs = string.Empty;

            var index = message.IndexOf(":", StringComparison.OrdinalIgnoreCase);
            if (index > 0)
            {
                command = message.Substring(0, index);
                commandArgs = message.Substring(index + 1, message.Length - index - 1);
            }
            else
            {
                command = message;
            }

            Logger.LogMessage(command);
            Logger.LogMessage(commandArgs);
        
            IGameCommand handler = null;
            if (commands.TryGetValue(command, out handler))
            {
                handler.Execute(current, sessions, commandArgs);
            }
        }

        /// <summary>
        /// Initializes the dictionary object.
        /// </summary>
        /// <returns>Commands dictionary.</returns>
        private static Dictionary<string, IGameCommand> InitializeList()
        {
            Dictionary<string, IGameCommand> list = new Dictionary<string, IGameCommand>();

            IGameCommand loginCommand = new Login();
            list.Add(loginCommand.Name, loginCommand);

            IGameCommand getPlayerCommand = new GetPlayersList();
            list.Add(getPlayerCommand.Name, getPlayerCommand);

            IGameCommand selectPlayerCommand = new SelectPlayer();
            list.Add(selectPlayerCommand.Name, selectPlayerCommand);

            IGameCommand uiLoadCompleteCommand = new UILoadComplete();
            list.Add(uiLoadCompleteCommand.Name, uiLoadCompleteCommand);

            IGameCommand updateScoreCommand = new UpdateScore();
            list.Add(updateScoreCommand.Name, updateScoreCommand);

            IGameCommand gameCompleteCommand = new GameComplete();
            list.Add(gameCompleteCommand.Name, gameCompleteCommand);

            IGameCommand sendJigSawCoordinates = new SendJigSawCoordinates();
            list.Add(sendJigSawCoordinates.Name, sendJigSawCoordinates);

            IGameCommand selectTile = new SelectTile();
            list.Add(selectTile.Name, selectTile);

            IGameCommand disableTile = new DisableTile();
            list.Add(disableTile.Name, disableTile);

            IGameCommand assignTile = new AssignTile();
            list.Add(assignTile.Name, assignTile);

            IGameCommand resetTile = new ResetTile();
            list.Add(resetTile.Name, resetTile);

            return list;
        }
    }
}
