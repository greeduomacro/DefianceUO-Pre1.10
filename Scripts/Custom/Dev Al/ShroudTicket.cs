using System;
using Server.Gumps;
using Server.Items;

namespace Server.Items
{
    public class ShroudTicket : Item
    {
        [Constructable]
        public ShroudTicket() : base(0x14F0)
        {
            Hue = 0x58c;
            Name = "Ticket for a colored hooded shroud";
            Weight = 11.0;
        }

        public ShroudTicket(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack == null || !IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.CloseGump(typeof(ShroudTicketGump));
                from.SendGump(new ShroudTicketGump(from, this));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

}

namespace Server.Gumps
{
    public class ShroudTicketGump : Gump
    {
        private static readonly int ms_entryCount = 7;
        private ShroudTicket m_deed;

        public ShroudTicketGump(Mobile owner, ShroudTicket deed) : base(200, 100)
        {
            m_deed = deed;

            int lineCount = ms_entryCount + 2;
            int i=0;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(15, 15, 298 - 50, 30 + 50 * lineCount, 9250);
            AddAlphaRegion(25, 25, 276 - 50, 30 + 50 * lineCount - 22);

            AddLabel(40, 30, 1149, @"Please select hue of your shroud:");

            AddButton(80, 60 + 50 * i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x58c);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x4E2);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x4d3);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x653);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x66c);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x842);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 50 + 50 * i, 0x2684, 0x479);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(120, 60 + 50 * i, 1149, @"CANCEL");

            AddLabel(40, 50 + 50 * ++i, 38, @"This selection cannot be undone!");
        }

        public override void OnResponse(Server.Network.NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            int index = info.ButtonID; //note: 0==exit
            if (from == null || index < 1 || index > ms_entryCount) return; //Avoid crashing the server with faked buttonIDs
            if (m_deed == null || m_deed.Deleted || from.Backpack == null || !m_deed.IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            else
            {
                int hue = 0;
                switch (index)
                {
                    case 1: hue = 0x58c; break;
                    case 2: hue = 0x4e2; break;
                    case 3: hue = 0x4d3; break;
                    case 4: hue = 0x653; break;
                    case 5: hue = 0x66c; break;
                    case 6: hue = 0x842; break;
                    case 7: hue = 0x479; break;
                }
                if (hue != 0) //Check if valid hue has been set
                {
                    HoodedShroudOfShadows shroud = new HoodedShroudOfShadows();
                    shroud.Hue = hue;
                    if (from.AddToBackpack(shroud))
                    {
                        from.SendMessage("The shroud has been placed in your backpack.");
                        m_deed.Delete();
                    }
                    else
                    {
                        shroud.Delete();
                        from.SendMessage("An error occured while creating the shroud.");
                    }
                }
            }
        }
    }
}