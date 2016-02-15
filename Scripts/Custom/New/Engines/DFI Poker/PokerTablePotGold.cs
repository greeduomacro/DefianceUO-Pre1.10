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
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Poker;

namespace Server.Engines.Poker
{
    public class PokerTablePotGold : Item
    {
        private PokerDealer m_Dealer;

        [CommandProperty(AccessLevel.Seer)]
        public PokerDealer Dealer { get { return m_Dealer; } set { m_Dealer = value;} }

        [Constructable]
        public PokerTablePotGold() : base(3820)
        {
            Movable = false;
        }

        public PokerTablePotGold(Serial serial) : base(serial)
        {
        }

        public override void OnSingleClick(Mobile from)
        {
            if (m_Dealer != null)
            {
                if (m_Dealer.PotWorth == 0)
                    LabelTo(from, "tablepot worth: 0gp");
                else
                    LabelTo(from, "tablepot worth: {0:0,0}gp", (double)m_Dealer.PotWorth);
            }
            else
                LabelTo(from, "34,897gp");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(m_Dealer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Dealer = reader.ReadMobile() as PokerDealer;
                        break;
                    }
            }
        }
    }
}