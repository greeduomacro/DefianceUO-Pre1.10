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
    public enum CardCombo { None, Pair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush, }

	public class HandResult
	{
        private Card[] m_Cards;
        public Card[] Cards { get { return m_Cards; } }
		private CardHand m_Hand;
		private int m_HandID;
		public CardHand Hand { get { return m_Hand; } }
		public int HandID { get { return m_HandID; } }
		public CardCombo BestCombo { get { return (CardCombo)(m_HandID >> 20); } }

		public string ComboName
		{
			get
			{
				switch ( BestCombo )
                {
                    case CardCombo.None:
                        return String.Format("highcard {0}", m_Hand.Cards[0].GetValueString(true));
                    case CardCombo.Pair:
                        return String.Format("a pair of {0}{1}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[0].Value == 6 ? "es" : "s");
                    case CardCombo.TwoPair:
                        return String.Format("two pairs, {0}{1} and {2}{3}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[0].Value == 6 ? "es" : "s", m_Hand.Cards[2].GetValueString(true), m_Hand.Cards[2].Value == 6 ? "es" : "s");
                    case CardCombo.ThreeOfAKind:
                        return String.Format("three of a kind, {0}{1}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[0].Value == 6 ? "es" : "s");
                    case CardCombo.Straight:
                        return String.Format("a straight, {0} to {1}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[4].GetValueString(true));
                    case CardCombo.Flush:
                        return String.Format("a flush, {0} high", m_Hand.Cards[0].GetValueString(true));
                    case CardCombo.FullHouse:
                        return String.Format("a full house, {0}{1} over {2}{3}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[0].Value == 6 ? "es" : "s", m_Hand.Cards[4].GetValueString(true), m_Hand.Cards[4].Value == 6 ? "es" : "s");
                    case CardCombo.FourOfAKind:
                        return String.Format("four of a kind, {0}{1}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[0].Value == 6 ? "es" : "s");
                    case CardCombo.StraightFlush:
                        return String.Format("a straight flush, {0} to {1}", m_Hand.Cards[0].GetValueString(true), m_Hand.Cards[4].GetValueString(true));
                    case CardCombo.RoyalFlush:
                        return String.Format("a royal flush");
                }
                return "invalid hand. call a GM";
            }
		}

		public HandResult( CardHand hand, int handid )
		{
			m_Hand = new CardHand();
			foreach ( Card card in hand.Cards )
				m_Hand.AddCard( card );
            m_HandID = handid;
		}
	}
}