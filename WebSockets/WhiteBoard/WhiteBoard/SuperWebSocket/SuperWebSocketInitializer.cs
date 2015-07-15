using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperSocket.SocketEngine.Configuration;
using System.Collections.Generic;
using SuperWebSocket;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;

namespace SuperWebSocket
{
    public class SuperWebSocketInitializer : System.Web.HttpApplication
    {

        private List<WebSocketSession> m_Sessions = new List<WebSocketSession>();
        private List<WebSocketSession> m_SecureSessions = new List<WebSocketSession>();
        private object m_SessionSyncRoot = new object();
        private object m_SecureSessionSyncRoot = new object();
        private Timer m_SecureSocketPushTimer;

        [Serializable]
        private class KeyValue
        {
            public string Key
            {
                get;
                set;
            }

            public string Value
            {
                get;
                set;
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            LogUtil.Setup();
            StartSuperWebSocketByConfig();
            //StartSuperWebSocketByProgramming();
        }


        void StartSuperWebSocketByConfig()
        {
            var serverConfig = ConfigurationManager.GetSection("socketServer") as SocketServiceConfig;
            if (!SocketServerManager.Initialize(serverConfig))
                return;

            var socketServer = SocketServerManager.GetServerByName("SuperWebSocket") as WebSocketServer;
            var secureSocketServer = SocketServerManager.GetServerByName("SecureSuperWebSocket") as WebSocketServer;

            HttpContext.Current.Application["WebSocketPort"] = socketServer.Config.Port;
            HttpContext.Current.Application["SecureWebSocketPort"] = secureSocketServer.Config.Port;

            socketServer.CommandHandler += new CommandHandler<WebSocketSession, WebSocketCommandInfo>(socketServer_CommandHandler);
            socketServer.NewSessionConnected += new SessionEventHandler(socketServer_NewSessionConnected);
            socketServer.SessionClosed += new SessionClosedEventHandler(socketServer_SessionClosed);

            secureSocketServer.NewSessionConnected += new SessionEventHandler(secureSocketServer_NewSessionConnected);
            secureSocketServer.SessionClosed += new SessionClosedEventHandler(secureSocketServer_SessionClosed);

            if (!SocketServerManager.Start())
                SocketServerManager.Stop();
        }

        void secureSocketServer_SessionClosed(WebSocketSession session, CloseReason reason)
        {
            lock (m_SecureSessionSyncRoot)
            {
                m_SecureSessions.Remove(session);
            }
        }

        void secureSocketServer_NewSessionConnected(WebSocketSession session)
        {
            lock (m_SecureSessionSyncRoot)
            {
                m_SecureSessions.Add(session);
            }
        }

        void StartSuperWebSocketByProgramming()
        {
            var socketServer = new WebSocketServer();
            socketServer.Setup(new RootConfig(),
                new ServerConfig
                {
                    Ip = "Any",
                    Port = 2011,
                    Mode = SocketMode.Async
                }, SocketServerFactory.Instance);

            socketServer.CommandHandler += new CommandHandler<WebSocketSession, WebSocketCommandInfo>(socketServer_CommandHandler);
            socketServer.NewSessionConnected += new SessionEventHandler(socketServer_NewSessionConnected);
            socketServer.SessionClosed += new SessionClosedEventHandler(socketServer_SessionClosed);

            var secureSocketServer = new WebSocketServer();
            secureSocketServer.Setup(
                new RootConfig(),
                new ServerConfig
                {
                    Ip = "Any",
                    Port = 2012,
                    Mode = SocketMode.Sync,
                    Security = "tls",
                    Certificate = new SuperSocket.SocketBase.Config.CertificateConfig
                    {
                        FilePath = HttpContext.Current.Server.MapPath("~/localhost.pfx"),
                        Password = "supersocket",
                        IsEnabled = true
                    }
                }, SocketServerFactory.Instance);

            secureSocketServer.NewSessionConnected += new SessionEventHandler(secureSocketServer_NewSessionConnected);
            secureSocketServer.SessionClosed += new SessionClosedEventHandler(secureSocketServer_SessionClosed);

            HttpContext.Current.Application["WebSocketPort"] = socketServer.Config.Port;
            HttpContext.Current.Application["SecureWebSocketPort"] = secureSocketServer.Config.Port;

            socketServer.Start();
            secureSocketServer.Start();
        }

        void socketServer_NewSessionConnected(WebSocketSession session)
        {
            lock (m_SessionSyncRoot)
            {
                m_Sessions.Add(session);

                string message = buildJSONMessage("Joined", "System", session);
                SendToAll(message);
            }
        }

        void socketServer_SessionClosed(WebSocketSession session, CloseReason reason)
        {
            lock (m_SessionSyncRoot)
            {
                m_Sessions.Remove(session);
                if (m_Sessions == null || m_Sessions.Count == 0)
                {
                    ApplicationData.Data = null;
                }

                if (reason == CloseReason.ServerShutdown)
                    return;
                if (m_Sessions != null && m_Sessions.Count > 0)
                {
                    string message = buildJSONMessage("Left", "System", session);
                    SendToAll(message);
                }
            }
        }

        private string GetTypeFromMessage(string message)
        {
            Hashtable jsonData = (Hashtable)JsonUtil.JSON.JsonDecode(message);

            return jsonData["Action"].ToString();
        }

        private string GetDataFromMessage(string message)
        {
            Hashtable jsonData = (Hashtable)JsonUtil.JSON.JsonDecode(message);

            return jsonData["Message"].ToString();
        }

        void socketServer_CommandHandler(WebSocketSession session, WebSocketCommandInfo commandInfo)
        {
            lock (m_SessionSyncRoot)
            {
                string messageType = GetTypeFromMessage(commandInfo.Data);
                string message = buildJSONMessage(GetDataFromMessage(commandInfo.Data), messageType, session);

                List<string> messages = ApplicationData.Data as List<string>;
                if (messages == null)
                {
                    messages = new List<string>();
                    ApplicationData.Data = messages;
                }
                messages.Add(message);
                ApplicationData.Data = messages;
                
                SendToAll(message);
            }
        }

        string buildJSONMessage(string message, string type, WebSocketSession session)
        {
            StringBuilder JSONMessage = new StringBuilder();

            JSONMessage = JSONMessage.AppendFormat("{{\"user\":\"{0}\",\"type\":\"{1}\",\"data\":\"{2}\"}}", session.Cookies["name"], type, message);

            return JSONMessage.ToString();
        }

        void SendToAll(string message)
        {
            if (m_Sessions != null)
            {
                foreach (var s in m_Sessions)
                {
                    s.SendResponse(message);
                }
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            SocketServerManager.Stop();
            ApplicationData.Data = null;
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }
    }
}