﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SuperWebSocketWeb
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
            var nameCookie = Request.Cookies.Get("name");

            if (nameCookie == null)
                Response.Redirect("~/Default.aspx");
        }
    }
}