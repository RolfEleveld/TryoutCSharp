using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inheiritance
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = new CorpIdentity("bob", "john", "B3A123");
        }
    }
    class CorpIdentity : ClaimsIdentity
    {
        public CorpIdentity(string name, string reportsTo, string office)
        {
            AddClaim(new Claim(ClaimTypes.Name, name));
            AddClaim(new Claim("reportsTo", reportsTo));
            AddClaim(new Claim("office", office));
        }

        public string Office { get { return FindFirst("office").Value; } }

        public string ReportsTo { get { return FindFirst("reporrtsTo").Value; } }
    }
}
