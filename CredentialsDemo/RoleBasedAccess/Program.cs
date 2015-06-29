using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RoleBasedAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentPrincipal = CreateIdentity();
            UsePrincipal();
            Console.ReadLine();
        }

        /// <summary>
        /// Example to show how you could create identities from any data store, based on strings.
        /// </summary>
        /// <returns>GenericPrincipal for bob</returns>
        public static GenericPrincipal CreateIdentity()
        {
            // attached roles
            var roles = new string[] { "Sales", "Marketing" };
            var user = new GenericPrincipal(new GenericIdentity("bob"), roles);
            return user;
        }

        public static void UsePrincipal()
        {
            var p = System.Threading.Thread.CurrentPrincipal;
            Console.WriteLine(p.Identity.Name);
            //Below is commonly practices but there are alternatives
            if (p.IsInRole("Marketing"))
            {
                // do marketing stuff here
            }
            else
            {
                // let the user know he cannot do this
            }

            //Alternatively use PrincipalPermission, which will throw a security exception if not met.
            new PrincipalPermission(null, "Sales").Demand();
            Console.WriteLine("User is in Sales");
            try
            {
                new PrincipalPermission(null, "Development").Demand();
                Console.WriteLine("User is a Developer");
            }
            catch 
            {
                // do the handling here
            }

            // the better way is to decorate the method with required principal permissions
            DoSomeSalesWork();
        }
        //This still requires a CLR compiled hard link to a role name.
        [PrincipalPermission(SecurityAction.Demand, Role="Sales")]
        public static void DoSomeSalesWork()
        {
            Console.WriteLine("You are in Sales now");
        }
    }
}
