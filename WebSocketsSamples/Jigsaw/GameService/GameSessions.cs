// <copyright file="GameSessions.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Collection used to manage WebSocket Game service instances.
    /// </summary>
    internal class GameSessions : IDisposable
    {
        /// <summary>
        /// Collection of GameService instances.
        /// </summary>
        private ServiceCollection<JigsawGameService> innerCache = new ServiceCollection<JigsawGameService>();

        /// <summary>
        /// Reader writer lock to synchronize access to the collection of GameService instances.
        /// </summary>
        private ReaderWriterLockSlim thisLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Sends the message to the destination service.
        /// </summary>
        /// <param name="destinationService">Destination Service.</param>
        /// <param name="value">Message to be sent.</param>
        /// <param name="defunctList">List of defucnt services.</param>
        public static void Send(JigsawGameService destinationService, string value, List<JigsawGameService> defunctList)
        {
            try
            {
                destinationService.SendMessage(value);
            }
            catch
            {
                if (null == defunctList)
                {
                    defunctList = new List<JigsawGameService>();
                }

                defunctList.Add(destinationService);
            }
        }

        /// <summary>
        /// Broadcasts message to all sessions after sending messages to the both associated sessions (Buddy pair).
        /// Then it also broacasts messages to all other session based on the the response type so that they can update
        /// their's buddyList.
        /// </summary>
        /// <param name="responseType">Broadcast message type.</param>
        /// <param name="currentService">Current service instance.</param>
        /// <param name="destinationService">Destination service instance.</param>
        /// <param name="responseToCurrentService">Message to be sent to the currentService.</param>
        /// <param name="responseToDestinationservice">Message to be sent to the destinationService.</param>
        public void BroadcastMessage(BroadcastMessageType responseType, JigsawGameService currentService, JigsawGameService destinationService, string responseToCurrentService, string responseToDestinationservice)
        {
            if (null != currentService)
            {
                this.SendMessage(currentService, responseToCurrentService);
            }

            if (null != destinationService)
            {
                this.SendMessage(destinationService, responseToDestinationservice);
            }

            List<JigsawGameService> defunctServiceList = null;

            // Now also send this to other players who are not playing.
            this.thisLock.EnterReadLock();
            try
            {
                foreach (var service in this.innerCache)
                {
                    // Find out logged in users excluding destination service.
                    if (!string.IsNullOrEmpty(service.Context.LogOnName)
                        && service != currentService && service != destinationService)
                    {
                        switch (responseType)
                        {
                            case BroadcastMessageType.LogOnResponse:
                                if (null == service.Context.PlayerInstance)
                                {
                                    Send(service, "JoinPlayerList:" + currentService.Context.LogOnName, defunctServiceList);
                                }
      
                                break;

                            case BroadcastMessageType.SelectPlayerResponse:
                                if (null == service.Context.PlayerInstance)
                                {
                                    Send(service, "RemoveFromPlayerList:" + currentService.Context.LogOnName + ";" + destinationService.Context.LogOnName, defunctServiceList);
                                }

                                break;
                            case BroadcastMessageType.GameCompleteResponse:
                                if (null == service.Context.PlayerInstance)
                                {
                                    Send(service, "JoinPlayerList:" + currentService.Context.LogOnName + ";" + destinationService.Context.LogOnName, defunctServiceList);
                                }

                                break;

                            case BroadcastMessageType.LogOffResponse:
                                // Now this string has \" char at front and back both.
                                string[] defunctServiceResponse = responseToCurrentService.ToString().Split(new char[] { ':' });

                                // Remove quotes from the end of this string.
                                string defunctServiceName = defunctServiceResponse[1].Substring(0, defunctServiceResponse[1].Length - 1);
                                Send(service, "RemoveFromPlayerList:" + defunctServiceName, defunctServiceList);
                                
                                // Also tell services to join defunct's player in their playerList as it is no more playing with anyone.
                                if ((null != currentService) && !string.IsNullOrEmpty(currentService.Context.LogOnName))
                                {
                                    Send(service, "JoinPlayerList:" + currentService.Context.LogOnName, defunctServiceList); 
                                }
                                
                                break;
                        }
                    }
                }
            }
            finally
            {
                this.thisLock.ExitReadLock();
            }

            // Now look at the defunctService list and take te appropriate action.
            if (null != defunctServiceList)
            {
                // Each pair is made of service name and it's player service instance.
                Dictionary<string, JigsawGameService> defunctPlayerPairs = null;
                
                this.thisLock.EnterWriteLock();
                try
                {
                    foreach (var entry in defunctServiceList)
                    {
                        JigsawGameService playerInstance = entry.Context.PlayerInstance;
                        string defunctName = entry.Context.LogOnName;
                        
                        this.innerCache.Remove(entry);
                        
                        if (null != playerInstance && !string.IsNullOrEmpty(defunctName))
                        {
                            if (null == defunctPlayerPairs)
                            {
                                defunctPlayerPairs = new Dictionary<string, JigsawGameService>(); 
                            }

                            defunctPlayerPairs.Add(defunctName, playerInstance);
                        }
                    }
                }
                finally
                {
                    this.thisLock.ExitWriteLock();
                }

                if (null != defunctPlayerPairs)
                {
                    foreach (KeyValuePair<string, JigsawGameService> playerPair in defunctPlayerPairs)
                    {
                        this.RemovePlayerPair(playerPair.Key, playerPair.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Sends message to destination service.
        /// </summary>
        /// <param name="destinationService">Destination service.</param>
        /// <param name="value">Message to be sent.</param>
        public void SendMessage(JigsawGameService destinationService, string value)
        {
            JigsawGameService defunct = null;
            try
            {
                destinationService.SendMessage(value);
            }
            catch
            {
                if (defunct == null)
                {
                    defunct = destinationService;
                }
            }

            if (defunct != null)
            {
                JigsawGameService buddyInstance = defunct.Context.PlayerInstance;
                string defunctName = defunct.Context.LogOnName;

                this.thisLock.EnterWriteLock();
                try
                {
                    this.innerCache.Remove(defunct);
                }
                finally
                {
                    this.thisLock.ExitWriteLock();
                }

                this.RemovePlayerPair(defunctName, buddyInstance);
            }
        }

        /// <summary>
        /// Updates the playerInstance of died service and broadcasts to all other sessions that 
        /// this service died so that they can take appropriate action.
        /// </summary>
        /// <param name="defunctName">Service name.</param>
        /// <param name="playerInstance">Player Service instance.</param>
        public void RemovePlayerPair(string defunctName, JigsawGameService playerInstance)
        {
            // If there is any playerInstance in the destination service's context 
            // then modify playerInstance's context properly.
            if (null != playerInstance)
            {
                playerInstance.Context.PlayerInstance = null;
            }

            // Only broadcast for logged on users.
            if (!string.IsNullOrEmpty(defunctName))
            {
                this.BroadcastMessage(BroadcastMessageType.LogOffResponse, playerInstance, null, "PlayerDied:" + defunctName, null);
            }
        }

        /// <summary>
        /// Finds session by login name.
        /// </summary>
        /// <param name="loginName">Login Name.</param>
        /// <returns>Service instance.</returns>
        public JigsawGameService FindSession(string loginName)
        {
            JigsawGameService service = null;

            this.thisLock.EnterReadLock();
            try
            {
                service = this.innerCache.FirstOrDefault(s => loginName.Equals(s.Context.LogOnName));
            }
            finally
            {
                this.thisLock.ExitReadLock();
            }

            return service;
        }

        /// <summary>
        /// Get seesion list except the current session.
        /// </summary>
        /// <param name="current">Current session.</param>
        /// <returns>List of other player names.</returns>
        public string GetOtherLoggedInSessionsList(JigsawGameService current)
        {
            string players = null;

            this.thisLock.EnterReadLock();
            try
            {
                foreach (var service in this.innerCache)
                {
                    // Find out logged in users excluding myself.
                    if (!string.IsNullOrEmpty(service.Context.LogOnName) && service != current)
                    {
                        if (null == service.Context.PlayerInstance)
                        {
                            players += string.Format(CultureInfo.InvariantCulture, "{0};", service.Context.LogOnName);
                        }
                    }
                }
            }
            finally
            {
                this.thisLock.ExitReadLock();
            }

            return players;
        }

        /// <summary>
        /// Attempting to add another session to the collection.
        /// </summary>
        /// <param name="entry">Session to add.</param>
        /// <returns>true if session was added; false otherwise.</returns>
        public bool TryAdd(JigsawGameService entry)
        {
            this.thisLock.EnterUpgradeableReadLock();
            try
            {
                if (this.innerCache.Contains(entry))
                {
                   return false;
                }

                this.thisLock.EnterWriteLock();
                try
                {
                    this.innerCache.Add(entry);
                    return true;
                }
                finally
                {
                    this.thisLock.ExitWriteLock();
                }
            }
            finally
            {
                this.thisLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Attempting to remove a session from the collection.
        /// </summary>
        /// <param name="entry">Session to remove.</param>
        public void Remove(JigsawGameService entry)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.RemoveInternal), entry);
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        public void Dispose()
        {
            this.thisLock.Dispose();
        }

        /// <summary>
        /// Removes the session.
        /// </summary>
        /// <param name="state">GameService object.</param>
        private void RemoveInternal(object state)
        {
            var entry = state as JigsawGameService;
            
            if (entry != null)
            {
                JigsawGameService playerInstance = entry.Context.PlayerInstance;
                string defunctName = entry.Context.LogOnName;
                    
                this.thisLock.EnterWriteLock();
                try
                {
                    this.innerCache.Remove(entry);
                }
                finally
                {
                    this.thisLock.ExitWriteLock();
                }

                // if there is any playerInstance in the destination service's context 
                // then modify playerInstance's context properly.
                if (null != playerInstance)
                {
                    playerInstance.Context.PlayerInstance = null;
                }

                // Only broadcast for logged on users.
                if (!string.IsNullOrEmpty(defunctName))
                {
                    this.BroadcastMessage(BroadcastMessageType.LogOffResponse, playerInstance, null, "PlayerDied:" + defunctName, null);
                }
            }
        }
    }
}