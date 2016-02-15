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
using Server.Gumps;
using Server.Engines.Poker;

namespace Server.Engines.Poker
{
    public class CardHand
    {
        private Card[] m_Cards;

        public Card[] Cards { get { return m_Cards; } }

        public CardHand()
        {
        }

        public void Clear()
        {
            m_Cards = null;
        }

        public void AddCard(Card card)
        {
            Card[] tempcards = null;
            if (m_Cards != null)
            {
                tempcards = m_Cards;
                m_Cards = new Card[m_Cards.Length + 1];
                for (int i = 0; i < tempcards.Length; i++)
                    m_Cards[i] = tempcards[i];
            }
            else
                m_Cards = new Card[1];
            m_Cards[m_Cards.Length - 1] = card;
        }

        public void SortByValue(bool acehigh)
        {
            Array.Sort(m_Cards, new CardValueComparer(acehigh));
        }

        public void SortByValue(int nums)
        {
            CardHand temphand = new CardHand();
            for (int i = 0; i < nums; i++)
            {
                temphand.AddCard(m_Cards[m_Cards.Length - nums + i]);
            }
            Array.Sort(temphand.Cards, new CardValueComparer(true));
            for (int i = 0; i < nums; i++)
            {
                m_Cards[m_Cards.Length - nums + i] = temphand.Cards[i];
            }
        }

        public void FlipCards(int index1, int index2)
        {
            Card tempcard = m_Cards[index1];
            m_Cards[index1] = m_Cards[index2];
            m_Cards[index2] = tempcard;
        }

        private class CardValueComparer : IComparer
        {
            private bool m_AceHigh;
            public CardValueComparer(bool acehigh)
            {
                m_AceHigh = acehigh;
            }

            public int Compare(object a, object b)
            {
                if (!(a is Card) || !(b is Card))
                    return 0;
                Card carda = (Card)a;
                Card cardb = (Card)b;
                int vala = carda.Value;
                int valb = cardb.Value;
                if (m_AceHigh)
                {
                    if (vala == 1)
                        vala = 14;
                    if (valb == 1)
                        valb = 14;
                }
                if (vala > valb)
                    return -1;
                else if (vala < valb)
                    return 1;
                else
                    return 0;
            }
        }
    }
}