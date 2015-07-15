using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnJoinDiscussion_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = new HttpCookie("name", txtName.Text.Trim());
        Response.AppendCookie(cookie);
        Response.Redirect("~/WhiteBoard.aspx");
    }
}