using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security;

namespace Authorization
{
    class ClaimsTransformer : ClaimsAuthenticationManager
    {
            public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incommingPrincipal)
            {
                // validate incomming claims
                if (string.IsNullOrWhiteSpace(incommingPrincipal.Identity.Name))
                {
                    // something is wrong
                    throw new SecurityException("Name Claim Missing");
                }
                if (incommingPrincipal.Identity.IsAuthenticated)
                {
                    //return base.Authenticate(resourceName, incommingPrincipal);
                    return CreatePrincipal(incommingPrincipal.Identity.Name);
                }
                return incommingPrincipal;
            }
            private ClaimsPrincipal CreatePrincipal(string name)
            {
                
                var tower = false;
                if (name.Contains("\\RolfE"))
                {
                    tower = true;
                }
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name, name),
                    new Claim("http://myclaims/tower", tower.ToString())
                };
                return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
            }

    }
}
