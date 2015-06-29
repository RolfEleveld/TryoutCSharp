using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    public class ClaimsAuthManager:ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            // inspect context and make authorization descision
            var resource = context.Resource.First().Value;
            var acttion = context.Action.First().Value;

            if (acttion.Equals("Show") && resource.Equals("Tower"))
            {
                var hasTower = context.Principal.HasClaim("http://myclaims/tower", true.ToString());
                return hasTower;
            }
            else
            {
                return base.CheckAccess(context);
            }
        }
        //public override void LoadCustomConfiguration(System.Xml.XmlNodeList nodelist)
        //{
        //    // one can configure to load authorizations from an XML document.
        //    base.LoadCustomConfiguration(nodelist);
        //}
    }
}
