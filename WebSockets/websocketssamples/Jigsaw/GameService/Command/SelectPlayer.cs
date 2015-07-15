// <copyright file="SelectPlayer.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    /// <summary>
    /// Defines the handler for the selectPlayer command. 
    /// </summary>
    internal class SelectPlayer : IGameCommand
    {
        /// <summary>
        /// selectPlayer object lock.
        /// </summary>
        private static object selectPlayerLock = new object();

        /// <summary>
        /// Gets the name of SelectPlayer command.
        /// </summary>
        public string Name
        {
            get { return "SelectPlayer"; }
        }

        /// <summary>
        /// Implements the execution of selectPlayer command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null != current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "SelectPlayerResponse:Failure;PlayerExist.");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    sessions.SendMessage(current, "SelectPlayerResponse:Failure;WrongRequest.");
                    return;
                }
                
                var playerInstance = sessions.FindSession(message);
                
                if (null == playerInstance)
                {
                    sessions.SendMessage(current, "SelectPlayerResponse:Failure;PlayerDoesNotExist" + message);
                    return;
                }
                else
                {
                    // Associate both players and then send all responses in the same atomic operation.
                    lock (selectPlayerLock)
                    {
                        if (null == playerInstance.Context.PlayerInstance && null == current.Context.PlayerInstance)
                        {
                            current.Context.PlayerInstance = playerInstance;
                            playerInstance.Context.PlayerInstance = current;

                            // Need to send selectPlayer response to the current service, fixedPlayerresponse to PlayerInstance
                            // and also we need to braodcast selectPlayerResposne to other sessions.
                            sessions.BroadcastMessage(BroadcastMessageType.SelectPlayerResponse, current, playerInstance, "SelectPlayerResponse:Successful", "FixedPlayerResponse:" + current.Context.LogOnName);
                            return;
                        }  
                    }
                    
                    // Selected player is already a Player.
                    sessions.SendMessage(current, "SelectPlayerResponse:Failure;BusyPlayer;" + message);
                }
            }
        }
    }
}
