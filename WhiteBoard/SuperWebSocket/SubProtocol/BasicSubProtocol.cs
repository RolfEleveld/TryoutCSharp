﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using System.Reflection;
using SuperSocket.Common;
using SuperSocket.SocketBase.Command;

namespace SuperWebSocket.SubProtocol
{
    public class BasicSubProtocol : ISubProtocol
    {
        private Assembly m_CommandAssembly;

        public BasicSubProtocol(Assembly commandAssembly)
            : this()
        {
            m_CommandAssembly = commandAssembly;
        }

        public BasicSubProtocol()
        {
            SubCommandParser = new BasicSubCommandParser();
        }

        #region ISubProtocol Members

        public ISubProtocolCommandParser SubCommandParser { get; private set; }

        public IEnumerable<ISubCommand> GetSubCommands()
        {
            if (m_CommandAssembly == null)
                m_CommandAssembly = Assembly.GetEntryAssembly();

            return m_CommandAssembly.GetImplementedObjectsByInterface<ISubCommand>().Cast<ISubCommand>();
        }

        public bool Initialize(IServerConfig config)
        {
            if (m_CommandAssembly != null)
                return true;

            var commandAssembly = config.Parameters.GetValue("commandAssembly");

            if (string.IsNullOrEmpty(commandAssembly))
                return true;

            try
            {
                m_CommandAssembly = Assembly.Load(commandAssembly);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.LogError(e);
                return false;
            }
        }

        #endregion
    }
}
