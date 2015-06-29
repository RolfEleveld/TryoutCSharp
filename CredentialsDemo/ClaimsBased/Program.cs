using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimsBased
{
    class Program
    {
        static void Main(string[] args)
        {
            SetUpPrincipal();
            UsePrincipalLegacy();
            UsePrincipalNew();
            Console.ReadLine();
        }

        private static void UsePrincipalLegacy()
        {
            var p = Thread.CurrentPrincipal;
            Console.WriteLine(p.Identity.Name);
            Console.WriteLine(p.IsInRole("Developer"));
            Console.WriteLine(p.IsInRole("Techies Leader"));
        }

        private static void UsePrincipalNew()
        {
            //var p = Thread.CurrentPrincipal;
            //var cp = p as ClaimsPrincipal;
            // below Has same effect
            var cp = ClaimsPrincipal.Current;
            Console.WriteLine(cp.HasClaim(ClaimTypes.Role, "Techies Leader"));
            string email = cp.FindFirst(ClaimTypes.Email).Value;
            Console.WriteLine(email);
            foreach (var id in cp.Identities)
            {
                Console.WriteLine(id.IsAuthenticated);
                Console.WriteLine(id.Name);
            }
        }
        private static void SetUpPrincipal()
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, "Rolf"),
                new Claim(ClaimTypes.Email, "rolf@bajomero.com"),
                new Claim(ClaimTypes.Role, "Techies Leader"),
                new Claim("http://myclaims/location", "Dubai")
            };
            var identity1 = new ClaimsIdentity(claims, "Console App");
            Console.WriteLine(identity1.Name);
            Console.WriteLine(identity1.IsAuthenticated);
            var identity2 = new ClaimsIdentity(claims, "Console App", ClaimTypes.Email, ClaimTypes.Role);
            Console.WriteLine(identity2.Name);
            Console.WriteLine(identity2.IsAuthenticated);
            var identity3 = new ClaimsIdentity(claims);
            Console.WriteLine(identity3.IsAuthenticated);

            var p = new ClaimsPrincipal(identity1);
            Thread.CurrentPrincipal = p;
        }

    }
}
