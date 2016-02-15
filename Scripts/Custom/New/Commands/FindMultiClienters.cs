using System;
using System.Collections;

namespace Server.Scripts.Commands
{
    public class FindMultiClienters
    {
        public static void Initialize()
        {
            Server.Commands.Register("FindMultiClienters", AccessLevel.GameMaster, new CommandEventHandler(FindMultiClienters_OnCommand));
        }

        [Usage("FindMultiClienters")]
        [Description("Displays a list of players with identical IP addresses in the current region.")]
        private static void FindMultiClienters_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.Region==null||e.Mobile.Region.Mobiles==null) return;
            bool flag = false;
            Hashtable mobiles = new Hashtable();

            foreach (Mobile m in e.Mobile.Region.Mobiles)
                if (m.Player && m.AccessLevel==AccessLevel.Player && m.NetState != null && m.NetState.Address != null)
                {
                    if (mobiles[m.NetState.Address] == null)
                        mobiles[m.NetState.Address] = new ArrayList();
                    else
                        flag = true;
                    ((ArrayList)mobiles[m.NetState.Address]).Add(m);
                }

            if (flag)
            {
                e.Mobile.SendMessage("Possible multi clienters found:");
                foreach (ArrayList a in mobiles.Values)
                    if (a.Count > 1)
                    {
                        foreach (Mobile m in a)
                            if (m.Account!=null)
                                e.Mobile.SendMessage("{0} (acc: {1})", m.Name, m.Account.ToString());
                        e.Mobile.SendMessage("");
                    }
            }
            else
                e.Mobile.SendMessage("No possible multi clienters found.");
        }
    }
}