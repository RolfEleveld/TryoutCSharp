// <copyright file="SendJigSawCoordinates.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the handler for the SendJigSawCoordinates command.
    /// </summary>
    internal class SendJigSawCoordinates : IGameCommand
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name
        {
            get { return "SendJigSawCoordinates"; }
        }

        /// <summary>
        /// Implements the execution of SendJigSawCoordinates command.
        /// </summary>
        /// <param name="current">Current service instance.</param>
        /// <param name="sessions">Collection of all sessions.</param>
        /// <param name="message">Command arguments.</param>
        public void Execute(JigsawGameService current, GameSessions sessions, string message)
        {
            if (null == current.Context.PlayerInstance)
            {
                sessions.SendMessage(current, "SendJigSawCoordinatesResponse:Failure;PlayerNotFound.");
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                {
                    sessions.SendMessage(current, "SendJigSawCoordinatesResponse:Failure;WrongFormat.");
                    return;
                }

                string[] location = message.Split(new char[] { ';' });

                if (location.Length != 3)
                {
                    sessions.SendMessage(current, "SendJigSawCoordinatesResponse:Failure;WrongFormat.");
                    return;
                }

                sessions.SendMessage(current.Context.PlayerInstance, "FixJigSawCoordinates:" + location[0] + ";" + location[1] + ";" + location[2]);
                sessions.SendMessage(current, "SendJigSawCoordinatesResponse:Successful");
            }
        }
    }
}
