using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SuperWebSocketWeb
{
    public partial class WhiteBoard : System.Web.UI.Page
    {
        protected object WebSocketPort
        {
            get
            {
                var extPort = ConfigurationManager.AppSettings["extPort"];

                if (string.IsNullOrEmpty(extPort))
                    return Application["WebSocketPort"];
                else
                    return extPort;
            }
        }

     
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}