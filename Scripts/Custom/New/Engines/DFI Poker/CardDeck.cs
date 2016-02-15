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
using Server.Gumps;
using Server.Items;

namespace Server.Engines.Poker
{
    public class CardDeck : Item
    {
        private Card[] m_Cards;
        private int m_Sequence;

        public CardDeck() : base(4079)
        {
            Name = "a deck of cards";
            Hue = 1150;
            Init();
        }

        public CardDeck(Serial serial) : base(serial)
        {
            Init();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private void Init()
        {
            if (m_Cards == null)
            {
                m_Cards = new Card[52];
                for (int i = 0; i < 52; i++)
                    m_Cards[i] = new Card(i);
            }
            Shuffle();
        }

        public void Shuffle()
        {
            for (int i = 0; i < 52; i++)
            {
                int randomvar = Utility.Random(52);
                Card tempcard = m_Cards[i];
                m_Cards[i] = m_Cards[randomvar];
                m_Cards[randomvar] = tempcard;
            }
            m_Sequence = 0;
        }

        public Card GetCard()
        {
            if (m_Sequence < 52)
            {
                m_Sequence++;
                return m_Cards[m_Sequence - 1];
            }
            else
                return null;
        }
    }
}