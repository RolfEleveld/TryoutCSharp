using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupPrincipal();
            UsePrincipal();
            Console.ReadLine();
        }

        [ClaimsPrincipalPermission(System.Security.Permissions.SecurityAction.Demand, Operation="Show", Resource="Tower")]
        private static void ShowTower()
        {
            // old style: if the user is allowed to show the tower
            Console.WriteLine("Tower");

        }
        private static void UsePrincipal()
        {
            var authZ = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager;
            //if (authZ.CheckAccess(new System.Security.Claims.AuthorizationContext(Thread.CurrentPrincipal, "Tower", "Show")))
            //{
            //    // only do stuff when you have access!
            //    Console.WriteLine("Show Tower Access");
            //}

            ShowTower();
        }

        
        [ClaimsPrincipalPermission(System.Security.Permissions.SecurityAction.Demand, Operation = "Get", Resource = "Customer")]
        private Customer GetCustomer(string reference)
        {
            return new Customer();
        }
        private void Print(Customer customer)
        {
            //if (ClaimsPrincipalPermission.CheckAccess("Customer", "Print"))
            //{
            //}
        }
        private static void SetupPrincipal()
        {
            var p = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            System.Threading.Thread.CurrentPrincipal = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager.Authenticate("none", p); // source should be service or site or context;
        }

    }
    class Customer
    {
        public string Reference { get; set; }
    }
}
