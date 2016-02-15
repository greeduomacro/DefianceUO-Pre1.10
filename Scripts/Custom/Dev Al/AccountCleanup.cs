//Al@05-25-2006
using System;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Logging;
using System.Collections;

namespace Server.Accounting
{
    class AccountCleanup
    {
        public const bool Enabled = false;       //Cleanup enabled
        public const bool Delete = true;        //If set to false no accounts will be deleted, only logged

        public static void Initialize()
        {
            int deletes = 0;
            ArrayList accountsToDelete = new ArrayList();

            if (!Enabled) return;
            if (Delete)
            {
                Console.Write("Cleaning up accounts...");
                GeneralLogging.WriteLine("AccountCleanup", "Account cleanup started.");
            }
            else
            {
                Console.Write("Cleaning up accounts (deletion disabled)...");
                GeneralLogging.WriteLine("AccountCleanup", "Account cleanup started (deletion disabled).");
            }

            foreach (Account acc in Accounts.Table.Values)
            {
                //Count characters
                int chars = 0;
                int maxskillsum = 0;
				TimeSpan gametime = TimeSpan.Zero;
                for (int i = 0; i < acc.Length; i++)
                {
                    if (acc[i] != null)
                    {
                        chars++;
                        if (acc[i].SkillsTotal >= maxskillsum)
							maxskillsum = acc[i].SkillsTotal;
						if ( ((PlayerMobile)acc[i]).GameTime > gametime )
							gametime = ((PlayerMobile)acc[i]).GameTime;
                    }
                }
                Mobile firstchar = acc[0] as Mobile;

                //Check conditions
                if (
					acc.TotalGameTime < TimeSpan.FromHours( 5.0 ) &&
					/*( ( chars == 0 ) || (
                    (maxskillsum <= 3000) &&                                  //A maximum skillsum of 300.0 on best character
					gametime < TimeSpan.FromHours( 5.0 ) ) ) &&					//Played less than 5 hours
                    */(DateTime.Now - acc.LastLogin > TimeSpan.FromDays(14)) &&  //Account has not logged in within 14 days
                    (!DonationSystem.HasDonated(acc))                         //Account is no donator
                )
                {
                    GeneralLogging.WriteLine("AccountCleanup", "Inactive account: {0}", acc.Username);
                    deletes++;
                    accountsToDelete.Add(acc);
                }
            }
            if (Delete)
            {
                foreach (Account acc in accountsToDelete) acc.Delete();
                Console.WriteLine("done. {0} accounts have been deleted.", deletes);
                GeneralLogging.WriteLine("AccountCleanup", "Cleanup done. {0} accounts have been deleted.", deletes);
            }
            else
            {
                Console.WriteLine("done. {0} accounts would have been affected.", deletes);
                GeneralLogging.WriteLine("AccountCleanup", "Cleanup done. {0} accounts would have been affected.", deletes);
            }
        }
    }
}