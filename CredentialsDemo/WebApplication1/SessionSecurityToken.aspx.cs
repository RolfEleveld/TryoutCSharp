using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SessionSecurityToken : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupClaims();
            UseClaims();

        }
        private void UseClaims()
        {
            //var sessionToken = new SessionSecurityToken(Thread.CurrentPrincipal, TimeSpan.FromMinutes(10));
            //FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);
            // need to secure the cookie, can be done if only one machine OOTB, or use SSL or use Guids to hydrate/dehidrate...
        }
        private void SetupClaims()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Me") };
            var p = new ClaimsPrincipal();
            Thread.CurrentPrincipal = p;
        }
    }
}