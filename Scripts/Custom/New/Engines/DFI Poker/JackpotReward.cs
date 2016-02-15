/* This script was made by Massimiliano.
 *
 * Release Date: 13 November 2007.
 *
 * Version: 5.0.1
 *
 *
 * Scripts\Items\Skill Items\Magical\Misc\Moongate.cs
 *
 * add at top of script: using Server.Engines.Poker;
 *
 * and in methode "UseGate( Mobile m )" add:
 *
 * else if (PokerDealer.IsPokerPlayer(m) >= 0)
    {
        m.SendMessage("You cannot travel while playing poker.");
    }
 * This will prevent players from using a gate while they're seated at a poker table.
 *
 *
 * Scripts\Spells\Fourth\Recall.cs & Scripts\Spells\Third\Teleport.cs
 *
 * add at top of script: using Server.Engines.Poker;
 *
 * and in methode "CheckCast()" add:
 *
 *  else if (PokerDealer.IsPokerPlayer(Caster) >= 0)
    {
        Caster.SendMessage("You cannot travel while playing poker.");
        return false;
    }
 * This will prevent players from casting recall or teleport while they're seated at a poker table.
 *
 * Includes the scripts:
 * - Card.cs
 * - CardDeck.cs
 * - CardHand.cs
 * - HandResult.cs
 * - JackpotReward.cs
 * - PokerDealer.cs
 * - PokerGumps.cs
 * - PokerJackpot.cs
 * - PokerPlayer.cs
 * - PokerTablePotGold.cs
 * - PokerTable.cs
 * - PokerTournyTicket.cs
 */

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Poker
{
    public class JackpotReward : Item
    {
        private int m_Worth;
        private string m_Winner;
        public int Worth { get { return m_Worth; } set { m_Worth = value; InvalidateProperties(); } }
        public string Winner { get { return m_Winner; } set { m_Winner = value; InvalidateProperties(); } }

        public JackpotReward(int worth, string winner) : base(0x14EF)
        {
            Weight = 1.0;
            LootType = LootType.Cursed;
            m_Worth = worth;
            m_Winner = winner;
            if (m_Worth < 1000000)
                ItemID = 0x1BEC;
            else if (m_Worth >= 1000000 && m_Worth < 10000000)
                ItemID = 0x1BED;
            else
                ItemID = 0x1BEE;
        }

        public override void OnSingleClick(Mobile from)
        {
            if (m_Worth < 1000000)
                LabelTo(from, "a bar of gold");
            else if (m_Worth >= 1000000 && m_Worth < 10000000)
                LabelTo(from, "a pile of golden bars");
            else
                LabelTo(from, "a large pile of golden bars");
            LabelTo(from, "worth {0:0,0}gp", (double)m_Worth);
            LabelTo(from, "won by {0}", m_Winner);
        }

        public override void OnDoubleClick(Mobile from)
        {
            BankBox bank = from.BankBox;
            Container pack = from.Backpack;
            if (bank != null && (pack != null && Parent == pack))
            {
                from.SendMessage("A total of {0:0,0}gp has been deposited in your bank box", (double)m_Worth);
                while (m_Worth > 1000000)
                {
                    bank.DropItem( new BankCheck( 1000000 ) );
                    m_Worth -= 1000000;
                }
                if ( m_Worth > 5000 )
                    bank.DropItem( new BankCheck( m_Worth ) );
                else if ( m_Worth > 0 )
                    bank.DropItem( new Gold( m_Worth) );
                Delete();
            }
            else
                from.SendLocalizedMessage(1042001);
        }

        public JackpotReward(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(m_Worth);
            writer.Write(m_Winner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            LootType = LootType.Cursed;

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Worth = reader.ReadInt();
                        m_Winner = reader.ReadString();
                        break;
                    }
            }
        }
    }
}