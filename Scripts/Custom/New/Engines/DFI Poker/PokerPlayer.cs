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
using Server.Engines.Poker;

namespace Server.Engines.Poker
{
    public enum Status { GettingGold, WaitingForRound, Playing, AllIn }

    public class PokerPlayer
    {
        private Mobile m_Mobile;
        private CardHand m_Hand;
        private HandResult m_Result;
        private Status m_Status;

        private int m_Bankroll;
        private int m_TotalBet;
        private int m_RoundBet;
        private int m_CanWin;
        private int m_IdleRounds;
        private bool m_Quitting;
        private bool m_AutoFold;
        private bool m_AutoCall;
        private ActionTimer m_ActionTimer;
        private GetGoldTimer m_GetGoldTimer;

        public Mobile Mobile { get { return m_Mobile; } }
        public CardHand Hand { get { if (m_Hand == null) m_Hand = new CardHand(); return m_Hand; } }
        public HandResult Result { get { return m_Result; } set { m_Result = value; } }
        public Status Status { get { return m_Status; } set { m_Status = value; } }

        public int Bankroll { get { return m_Bankroll; } set { m_Bankroll = value; } }
        public int TotalBet { get { return m_TotalBet; } set { m_TotalBet = value; } }
        public int RoundBet { get { return m_RoundBet; } set { m_RoundBet = value; } }
        public int CanWin { get { return m_CanWin; } set { m_CanWin = value; } }
        public bool Quitting { get { return m_Quitting; } set { m_Quitting = value; } }
        public bool AutoFold { get { return m_AutoFold; } set { m_AutoFold = value; } }
        public bool AutoCall { get { return m_AutoCall; } set { m_AutoCall = value; } }

        [Constructable]
        public PokerPlayer(Mobile mobile)
        {
            m_Mobile = mobile;
            m_Bankroll = 0;
            m_IdleRounds = 0;
            m_Quitting = false;
            m_AutoFold = false;
            m_AutoCall = false;
            Status = Status.WaitingForRound;
        }

        public void BeginAction(PokerDealer dealer)
        {
            if (m_ActionTimer != null)
            {
                m_ActionTimer.Stop();
                m_ActionTimer = null;
            }
            m_ActionTimer = new ActionTimer(dealer, this);
        }

        void ExpiredAction(PokerDealer dealer)
        {
            if (m_IdleRounds < 1)
            {
                dealer.ActionTaken(this, PokerAction.Fold, true);
                this.Mobile.CloseGump(typeof(BettingGump));
                m_IdleRounds++;
            }
            else
                dealer.VerifyQuit(Mobile);
        }

        public void EndAction()
        {
            m_IdleRounds = 0;
            if (m_ActionTimer != null)
            {
                m_ActionTimer.Stop();
                m_ActionTimer = null;
            }
        }

        public void BeginGetGold(PokerDealer dealer)
        {
            EndGetGold();
            m_GetGoldTimer = new GetGoldTimer(dealer, this);
        }

        void ExpiredGetGold(PokerDealer dealer)
        {
            dealer.VerifyQuit(Mobile);
        }

        public void EndGetGold()
        {
            if (m_GetGoldTimer != null)
            {
                m_GetGoldTimer.Stop();
                m_GetGoldTimer = null;
            }
        }

        class ActionTimer : Timer
        {
            PokerDealer m_Dealer;
            PokerPlayer m_Player;
            int m_TimeLeft;

            public ActionTimer(PokerDealer dealer, PokerPlayer player) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Dealer = dealer;
                m_Player = player;
                m_TimeLeft = m_Dealer.Timer_AutoFold;
                Start();
            }

            protected override void OnTick()
            {
                m_TimeLeft--;
                if (m_TimeLeft > 0)
                {
                    if (m_TimeLeft == 15 || m_TimeLeft == 10 || (m_TimeLeft < 6 && m_TimeLeft > 1))
                        m_Player.Mobile.SendMessage("{0} seconds until forced fold.", m_TimeLeft);
                    if (m_TimeLeft == 1)
                        m_Player.Mobile.SendMessage("1 second until forced fold.");
                }
                else
                {
                    m_Player.ExpiredAction(m_Dealer);
                    Stop();
                }
            }
        }

        class GetGoldTimer : Timer
        {
            PokerDealer m_Dealer;
            PokerPlayer m_Player;
            int m_TimeLeft;

            public GetGoldTimer(PokerDealer dealer, PokerPlayer player) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Dealer = dealer;
                m_Player = player;
                m_TimeLeft = m_Dealer.Timer_GetGold;
                Start();
            }

            protected override void OnTick()
            {
                m_TimeLeft--;
                if (m_TimeLeft > 0)
                {
                    if (m_TimeLeft < 6 && m_TimeLeft > 1)
                        m_Player.Mobile.SendMessage("{0} seconds until you get removed from the poker table.", m_TimeLeft);
                    if (m_TimeLeft == 1)
                        m_Player.Mobile.SendMessage("1 second until you get removed from the poker table.");
                }
                else
                {
                    m_Player.ExpiredGetGold(m_Dealer);
                    m_Dealer.CloseAllGumps(m_Player.Mobile);
                    Stop();
                }
            }
        }
    }
}