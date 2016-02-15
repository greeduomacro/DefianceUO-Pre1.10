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
    public enum CardColor { Hearts, Diamonds, Clubs, Spades }

    public class Card
    {
        public int m_Value;
        public CardColor m_Color;

        public int Value { get { return m_Value; } }

        public CardColor Color { get { return m_Color; } }

        public Card(int val)
        {
            m_Value = (val % 13) + 1;
            m_Color = (CardColor)(val / 13);
        }

        public void Draw(Gump g, int x, int y, bool player)
        {
            if (!player)
                g.AddBackground(x, y, 90, 115, 3000);
            g.AddHtml(x, y + 5, 32, 25, String.Format("<BODY TEXT='{0}'><center>{1}</center></BODY>", GetHue(), GetValueString(false)), false, false);
            g.AddHtml(x, y + 25, 32, 25, String.Format("<BODY TEXT='{0}'><center>{1}</center></BODY>", GetHue(), String.Format("{0}", (char)GetSymbol())), false, false);
        }

        public string GetHue()
        {
            if ((int)m_Color < 2)
                return "red";
            else
                return "black";
        }

        public string GetName()
        {
            if (m_Color == CardColor.Hearts)
                return "hearts";
            else if (m_Color == CardColor.Diamonds)
                return "diamonds";
            else if (m_Color == CardColor.Clubs)
                return "clubs";
            else
                return "spades";
        }

        public int GetSymbol()
        {
            switch (m_Color)
            {
                case CardColor.Hearts:
                    return 9829;
                case CardColor.Diamonds:
                    return 9670;
                case CardColor.Clubs:
                    return 9827;
                case CardColor.Spades:
                    return 9824;
            }
            return 0;
        }

        public string GetValueString(bool dealer)
        {
            if (dealer)
                switch (m_Value)
                {
                    case 1:
                        return "ace";
                    case 2:
                        return "two";
                    case 3:
                        return "three";
                    case 4:
                        return "four";
                    case 5:
                        return "five";
                    case 6:
                        return "six";
                    case 7:
                        return "seven";
                    case 8:
                        return "eight";
                    case 9:
                        return "nine";
                    case 10:
                        return "ten";
                    case 11:
                        return "jack";
                    case 12:
                        return "queen";
                    case 13:
                        return "king";
                    case 14:
                        return "ace";
                }
            else
                switch (m_Value)
                {
                    case 1:
                        return "A";
                    case 11:
                        return "J";
                    case 12:
                        return "Q";
                    case 13:
                        return "K";
                    case 14:
                        return "A";
                    default:
                        return m_Value.ToString();
                }
            return null;
        }

        public override string ToString()
        {
            return String.Format("{0} of {1}", GetValueString(true), GetName());
        }
    }
}