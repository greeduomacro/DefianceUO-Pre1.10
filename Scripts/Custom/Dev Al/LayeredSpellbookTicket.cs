using System;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
    public class LayeredSpellbookTicket : Item
    {
        [Constructable]
        public LayeredSpellbookTicket() : base(0x14F0)
        {
            Hue = 0x555;
            Name = "Ticket for a layered spellbook";
            Weight = 11.0;
        }

        public LayeredSpellbookTicket(Serial serial) : base(serial)
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
                from.CloseGump(typeof(LayeredSpellbookTicketHueGump));
                from.CloseGump(typeof(LayeredSpellbookTicketNameGump));
                from.SendGump(new LayeredSpellbookTicketHueGump(from, this));
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
    public class LayeredSpellbookTicketHueGump : Gump
    {
        private static readonly int ms_entryCount = 5;
        private LayeredSpellbookTicket m_deed;

        public LayeredSpellbookTicketHueGump(Mobile owner, LayeredSpellbookTicket deed)
            : base(200, 100)
        {
            m_deed = deed;

            int lineCount = ms_entryCount + 1;
            int i = 0;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(15, 15, 298 - 25, 30 + 50 * lineCount, 9250);
            AddAlphaRegion(25, 25, 276 - 25, 30 + 50 * lineCount - 22);

            AddLabel(40, 30, 1149, @"Please select hue of your spellbook:");

            AddButton(80, 60 + 50 * i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 60 + 50 * i, 0xEFA, 0x479);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 60 + 50 * i, 0xEFA, 0x555);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 60 + 50 * i, 0xEFA, 0x455);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 60 + 50 * i, 0xEFA, 0x4E2);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0); //i+1 because 0 = exit
            AddItem(120, 60 + 50 * i, 0xEFA, 0x54E);

            AddButton(80, 60 + 50 * ++i, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(120, 60 + 50 * i, 1149, @"CANCEL");
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
                    case 1: hue = 0x479; break;
                    case 2: hue = 0x555; break;
                    case 3: hue = 0x455; break;
                    case 4: hue = 0x4E2; break;
                    case 5: hue = 0x54E; break;
                }
                if (hue != 0) //Check if valid hue has been set
                    from.SendGump(new LayeredSpellbookTicketNameGump(from, m_deed, hue));
            }
        }
    }
    public class LayeredSpellbookTicketNameGump : Gump
    {
        private static readonly int ms_entryCount = 17;
        private LayeredSpellbookTicket m_deed;
        private int m_selectedHue;

        public LayeredSpellbookTicketNameGump(Mobile owner, LayeredSpellbookTicket deed, int selectedHue)
            : base(200, 100)
        {
            m_deed = deed;
            m_selectedHue = selectedHue;

            int lineCount = ms_entryCount + 3;
            int i = 0;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(15, 15, 298 - 15, 40 + 20 * lineCount, 9250);
            AddAlphaRegion(25, 25, 276 - 15, 40 + 20 * lineCount - 22);

            AddLabel(40, 30, 1149, @"Please select name of your spellbook:");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Britain's Book of Compassion");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Buccaneer's Den's Book of Chaos");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Cove's Book of Love");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Jhelom's Book of Valor");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Magincia's Book of Pride");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Minoc's Book of Sacrifice");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Moonglow's Book of Honesty");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Nujel'm's Book of Pleasure");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Occlo's Book of the Mountain");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Serpents Hold's Book of Order");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Skara Brae's Book of Spirituality");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Trinsic's Book of Honor");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Vesper's Book of Industry");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Wind's Book of Magicka");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Yew's Book of Justice");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Delucia's Book of the Lost Lands");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"Papua's Book of the Swamp");

            AddButton(40, 40 + 20 * ++i, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0); //0 = exit
            AddLabel(80, 40 + 20 * i, 1149, @"CANCEL");

            AddLabel(40, 40 + 20 * ++i, 38, @"This selection cannot be undone!");
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
                string name = "";
                switch (index)
                {
                    case 1: name = "Britain's Book of Compassion"; break;
                    case 2: name = "Buccaneer's Den's Book of Chaos"; break;
                    case 3: name = "Cove's Book of Love"; break;
                    case 4: name = "Jhelom's Book of Valor"; break;
                    case 5: name = "Magincia's Book of Pride"; break;
                    case 6: name = "Minoc's Book of Sacrifice"; break;
                    case 7: name = "Moonglow's Book of Honesty"; break;
                    case 8: name = "Nujel'm's Book of Pleasure"; break;
                    case 9: name = "Occlo's Book of the Mountain"; break;
                    case 10: name = "Serpents Hold's Book of Order"; break;
                    case 11: name = "Skara Brae's Book of Spirituality"; break;
                    case 12: name = "Trinsic's Book of Honor"; break;
                    case 13: name = "Vesper's Book of Industry"; break;
                    case 14: name = "Wind's Book of Magicka"; break;
                    case 15: name = "Yew's Book of Justice"; break;
                    case 16: name = "Delucia's Book of the Lost Lands"; break;
                    case 17: name = "Papua's Book of the Swamp"; break;
                }
                if (name != "") //Check if valid name has been set
                {
                    Spellbook book = new Spellbook();
                    book.Content = ulong.MaxValue; //Fill with all spells
					book.Layer = Layer.Neck; //Layer
                    book.Name = String.Format("{0} (layered)", name);
                    book.Hue = m_selectedHue;

                    if (from.AddToBackpack(book))
                    {
                        from.SendMessage("The book has been placed in your backpack.");
                        m_deed.Delete();
                    }
                    else
                    {
                        book.Delete();
                        from.SendMessage("An error occured while creating the book.");
                    }
                }
            }
        }
    }
}