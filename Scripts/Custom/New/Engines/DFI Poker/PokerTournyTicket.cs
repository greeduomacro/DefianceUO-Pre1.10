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
    public class PokerTournyTicket : Item
    {
        private int m_Worth;
        private Mobile m_Owner;
        public int Worth { get { return m_Worth; } set { m_Worth = value; InvalidateProperties(); } }
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }

        public PokerTournyTicket(int worth, Mobile winner) : base(5360)
        {
            Weight = 1.0;
            Movable = false;
            Hue = 1171;
            m_Worth = worth;
            m_Owner = winner;
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "a poker tournament ticket");
            LabelTo(from, "worth {0:0,0}gp", (double)m_Worth);
            LabelTo(from, "only usable by {0}", m_Owner.Name);
        }

        public PokerTournyTicket(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(m_Worth);
            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Worth = reader.ReadInt();
                        m_Owner = reader.ReadMobile();
                        break;
                    }
            }
        }
    }
}