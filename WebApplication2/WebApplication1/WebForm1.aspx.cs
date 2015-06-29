using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using WebApplication1.jdcmobile;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //host entry: 10.11.31.179 jdcmobile
           jdcmobile.PageStoreService dfs = new PageStoreService();
          // dfs.Credentials = new NetworkCredential("mttwsuser", "mttwsuser1234");
           //dfs.PreAuthenticate = true;

            PageReference pss = new PageReference();
            pss.ID = 1100;
            pss.RemoteSite = null;
            pss.WorkID = 0;
            LanguageSelector lngsel = new LanguageSelector();
            RawPage rp = dfs.GetPage(pss, lngsel, AccessLevel.Read);

            string strProps = string.Empty;

            for (int i = 0; i < rp.Property.Count(); i++)
            {
                if (string.IsNullOrEmpty(strProps))
                {
                    strProps = i.ToString() + ") " + rp.Property[i].Name.ToString() + " : " + rp.Property[i].Value.ToString() + "<br/>";
                }
                else
                {
                    strProps += i.ToString() + ") " + rp.Property[i].Name.ToString() + " : " + rp.Property[i].Value.ToString() + "<br/>";
                }
            }
           
        }
    }

        }
   
