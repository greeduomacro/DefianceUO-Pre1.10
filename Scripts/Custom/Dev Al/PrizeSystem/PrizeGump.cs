using System;
using System.Collections;
using Server;
using Server.Gumps;

namespace Server.EventPrizeSystem
{
    //Todo: Multipage like in DonationSkillBall
    public class EventPrizeGump : Gump
    {
        private ArrayList m_prizeDefs;
        public EventPrizeGump(Mobile owner) : base(200, 100)
        {
            m_prizeDefs = PrizeSystem.PrizeDefinitions;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(15, 15, 298+100, 60 + 20 * m_prizeDefs.Count, 9250);//421
            AddAlphaRegion(25, 25, 276+100, 60 + 20 * m_prizeDefs.Count - 22);//399

            AddLabel(110+50, 30, 1149, @"Event Reward System");
            AddItem(110 + 50 - 50, 30, 0xEF3, 0x8A5);
            AddItem(110 + 50 - 50 + 190, 30, 0xEF3, 0x8A5);

            for (int i = 0; i < m_prizeDefs.Count; i++)
            {
                PrizeDefinition pd = m_prizeDefs[i] as PrizeDefinition;
                AddButton(40, 60 + 20 * i , 4011, 4012, i+1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
                AddLabel(80, 60 + 20 * i, 1149, String.Format("{0} ({1})",pd.Name,pd.PriceString));
            }
        }

        public override void OnResponse(Server.Network.NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            int index = info.ButtonID - 1; //i+1 because 0 = exit
            if (from == null || index < 0 || index > m_prizeDefs.Count - 1) return; //Avoid crashing the server with faked buttonIDs

            PrizeDefinition prizeDef = m_prizeDefs[index] as PrizeDefinition;
            //if (prizeDef != null) prizeDef.TryGiveOut(from);
            if (prizeDef != null) from.SendGump(new EventPrizeConfirmGump(from, prizeDef));
        }

    }

    class EventPrizeConfirmGump : Gump
    {
        private PrizeDefinition m_prizeDefinition;
        private static readonly String ms_confirmationString =
                "Are you sure you want to buy '{0}' for {1}. This cannot be undone.";

        public EventPrizeConfirmGump(Mobile mobile, PrizeDefinition prizeDefinition) : base(110, 100)
        {
            m_prizeDefinition = prizeDefinition;

            mobile.CloseGump(typeof(EventPrizeConfirmGump));
            mobile.CloseGump(typeof(EventPrizeGump));

            Closable = true;

            AddPage(0);

            AddBackground(0, 0, 420, 280, 5054);

            AddImageTiled(10, 10, 400, 20, 2624);
            AddAlphaRegion(10, 10, 400, 20);

            AddHtml(10, 10, 400, 20, "<BASEFONT COLOR=#FFFF00><CENTER>Event Prize Confirmation</CENTER></BASEFONT>", false, false);

            AddImageTiled(10, 40, 400, 200, 2624);
            AddAlphaRegion(10, 40, 400, 200);

            AddHtml(10, 40, 400, 200, String.Format(ms_confirmationString,m_prizeDefinition.Name, m_prizeDefinition.PriceString), false, true);

            AddImageTiled(10, 250, 400, 20, 2624);
            AddAlphaRegion(10, 250, 400, 20);

            AddButton(10, 250, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(40, 250, 170, 20, 1011036, 32767, false, false); // OKAY

            AddButton(210, 250, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(240, 250, 170, 20, 1011012, 32767, false, false); // CANCEL
        }

        public override void OnResponse(Server.Network.NetState state, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                m_prizeDefinition.TryGiveOut(state.Mobile);
            }
        }
    }


}