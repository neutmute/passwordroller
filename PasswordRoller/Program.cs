using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace PasswordRoller
{
    class Program
    {
        static void Main(string[] args)
        {
            // whatever value the "Enforce Password History" setting is in group security policy
            const int PasswordHistoryMemory = 12;

            string currentPassword = args[0];
            string originalPassword = args[0];
            string targetPassword = args[1];
            
            using (var context = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, Environment.UserName))
            {
                for (int rollCount = 0; rollCount < PasswordHistoryMemory; rollCount++)
                {
                    Console.WriteLine("Changing to [Origina]+" + rollCount);
                    user.ChangePassword(currentPassword, originalPassword + rollCount);
                    currentPassword = originalPassword + rollCount;
                    //Console.WriteLine("Changed to " + currentPassword);
                }

                Console.WriteLine("Changing Password to final");
                user.ChangePassword(currentPassword, targetPassword);
            }

        }
    }
}
