using System;
using System.Collections;
using System.Text;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
    public delegate void HidingHandler(Mobile m);

    class PersonalHideEntry
    {
        private HidingHandler m_onHide;
        private string m_username;
        public PersonalHideEntry(string username, HidingHandler handler)
        {
            m_onHide = handler;
            m_username = username;
        }

        public string Username { get { return m_username; } }
        public HidingHandler OnHide { get { return m_onHide; } }
    }

    class PersonalHides
    {
        private static Hashtable m_table = new Hashtable();

        public static void Initialize()
        {
            Server.Commands.Register("phide", AccessLevel.GameMaster , new CommandEventHandler(PHide_OnCommand));
        }

        public static void Register(string username, HidingHandler handler)
        {
            m_table.Add(username, new PersonalHideEntry(username, handler));
        }

        public static void PHide_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                Account acc = e.Mobile.Account as Account;
                if (acc != null && acc.Username != null && m_table[acc.Username] != null && m_table[acc.Username] is PersonalHideEntry)
                {
                    PersonalHideEntry phe = m_table[acc.Username] as PersonalHideEntry;
                    phe.OnHide(e.Mobile);
                }
                else
                {
                    e.Mobile.Hidden = !e.Mobile.Hidden;
                }
            }
        }
    }
}