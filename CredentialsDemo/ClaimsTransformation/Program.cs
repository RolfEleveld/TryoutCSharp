using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading;
using System.Security.Principal;
using System.IdentityModel.Services;


namespace ClaimsTransformation
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupPrincipal();
            UsePrincipal();
            Console.ReadLine();
        }

        private static void UsePrincipal()
        {
        }
        private static void SetupPrincipal()
        {
            var p = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            System.Threading.Thread.CurrentPrincipal = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager.Authenticate("none", p); // source should be service or site or context;
        }
    }
}
