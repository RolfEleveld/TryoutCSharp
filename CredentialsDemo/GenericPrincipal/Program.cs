using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GenericPrincipalDemo
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
            Console.WriteLine(p.IsInRole("Development"));
            Console.WriteLine(p.IsInRole("Sales"));
        }
    }
}
