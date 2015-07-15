// <copyright file="Program.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples
{
    using System;
    using System.Configuration;
    using System.Windows.Forms;

    internal partial class GameWindow : Form
    {
        private WebSocketsHost<JigsawGameService> sh;

        public GameWindow()
        {
            this.InitializeComponent();
        }

        private void GameService_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void GameWindow_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
            }
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
            string appName = ConfigurationManager.AppSettings["AppName"];
            string portNumber = ConfigurationManager.AppSettings["PortNumber"];
            string machineName = ConfigurationManager.AppSettings["MachineName"];
            string uriString = machineName + ":" + portNumber + "/" + appName;

            this.sh = new WebSocketsHost<JigsawGameService>(new Uri("ws://" + uriString));
            this.sh.AddWebSocketsEndpoint();
            this.sh.Open();
        }
    }
}
