using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WindowsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = WindowsIdentity.GetCurrent();
            Console.WriteLine(id.Name);

            var account = new NTAccount(id.Name);
            var sid = account.Translate(typeof(SecurityIdentifier));
            Console.WriteLine(sid.Value);

            foreach (var group in id.Groups.Translate(typeof(NTAccount)))
            {
                Console.WriteLine(group);
            }

            var principal = new WindowsPrincipal(id);

            var localadmins = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            var domainadmins = new SecurityIdentifier(WellKnownSidType.AccountDomainAdminsSid, id.User.AccountDomainSid);
            var user = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            Console.WriteLine(principal.IsInRole(user));
            Console.WriteLine(principal.IsInRole(localadmins));
            Console.WriteLine(principal.IsInRole(domainadmins));
            Console.ReadLine();
        }
    }
}
