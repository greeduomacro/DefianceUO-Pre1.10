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
using System.Reflection;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Engines.Poker
{
    public class JoinPokerGump : Gump
    {
        private PokerDealer m_Dealer;

        public JoinPokerGump(Mobile mobile, PokerDealer dealer, int balance) : base(10, 10)
        {
            m_Dealer = dealer;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(40, 50, 400, 330, 9270);
            AddBackground(166, 336, 100, 20, 9300);
            AddAlphaRegion(50, 60, 380, 310);
            AddButton(280, 336, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(350, 336, 243, 241, 2, GumpButtonType.Reply, 0);
            AddLabel(200, 72, 1071, "Join Poker Table");
            AddLabel(58, 90, 1071, "============================================================");
            AddLabel(62, 112, 70, "You are asking to join a poker table. If you are unfamiliar");
            AddLabel(62, 128, 70, "with the rules of Texas Hold'em, or are uneasy about");
            AddLabel(62, 144, 70, "losing gold, then you are advised against proceeding. Only");
            AddLabel(62, 160, 70, "specify a buy-in amount that you would be comfortable");
            AddLabel(62, 176, 70, "losing. All bets are real gold and there are no refunds.");
            AddLabel(58, 197, 1071, "============================================================");
            AddLabel(65, 216, 1071, "Small blind:");
            AddLabel(65, 240, 1071, "Big blind:");
            AddLabel(65, 264, 1071, "Min buy-in:");
            AddLabel(65, 288, 1071, "Max buy-in:");
            AddLabel(65, 312, 1071, "Bank balance:");
            AddLabel(65, 336, 1071, "Buy-in amount:");
            AddLabel(168, 216, 50, String.Format("{0:0,0}", (double)m_Dealer.BlindSmall));
            AddLabel(168, 240, 50, String.Format("{0:0,0}", (double)m_Dealer.BlindSmall * 2));
            AddLabel(168, 264, 50, String.Format("{0:0,0}", (double)m_Dealer.BuyinMinimum));
            AddLabel(168, 288, 50, String.Format("{0:0,0}", (double)m_Dealer.BuyinMaximum));
            if(balance == 0)
                AddLabel(168, 312, 33, "0");
            else
                AddLabel(168, 312, balance < m_Dealer.BuyinMinimum ? 33 : 1071, String.Format("{0:0,0}", (double)balance));
            AddTextEntry(168, 336, 120, 20, 50, 1, "");
            if (m_Dealer.DealerMode > DealerSetup.Regular)
            {
                AddBackground(270, 220, 160, 80, 9300);
                AddImage(275, 220, 9008);
                AddLabel(280, 230, 33, "You are about to join");
                AddLabel(280, 245, 33, "a poker tournament.");
                AddLabel(280, 260, 33, "The dealer starts when");
                AddLabel(280, 275, 33, "all seats are taken.");
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            int balance = Banker.GetBalance(from);
            switch (info.ButtonID)
            {
                case 1:
                    {
                        try
                        {
                            TextRelay te = info.GetTextEntry(1);
                            PlayerMobile pm = from as PlayerMobile;
                            if (te != null)
                            {
                                int buyin = Convert.ToInt32(te.Text, 10);
                                if (buyin < m_Dealer.BuyinMinimum || buyin > m_Dealer.BuyinMaximum)
                                {
                                    if (buyin < m_Dealer.BuyinMinimum)
                                        from.SendMessage(33, "The minimum buy-in is {0:0,0}gp!", (double)m_Dealer.BuyinMinimum);
                                    else
                                        from.SendMessage(33, "The maximum buy-in is {0:0,0}gp!", (double)m_Dealer.BuyinMaximum);
                                    from.SendGump(new JoinPokerGump(from, m_Dealer, balance));
                                }
                                else
                                {
                                    if (balance < buyin)
                                    {
                                        from.SendMessage(33, "You cannot afford to join the table with {0:0,0}gp!", (double)buyin);
                                        from.SendGump(new JoinPokerGump(from, m_Dealer, balance));
                                    }
                                    else
                                        m_Dealer.VerifyJoin(from, buyin, true, false);
                                }
                                return;
                            }
                        }
                        catch
                        {
                            from.SendGump(new JoinPokerGump(from, m_Dealer, balance));
                            from.SendMessage(33, "Invalid format.");
                        }
                        break;
                    }
                case 2:
                    {
                        break;
                    }
            }
        }
    }

    public class GetGoldGump : Gump
    {
        PokerDealer m_Dealer;
        PokerPlayer m_Player;

        public GetGoldGump(PokerDealer dealer, PokerPlayer player) : base(150, 150)
        {
            m_Dealer = dealer;
            m_Player = player;

            if (m_Dealer == null || m_Player == null)
                return;

            Closable = true;
            Disposable = false;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 300, 160, 9270);
            AddBackground(113, 122, 100, 20, 9300);
            AddAlphaRegion(10, 10, 280, 140);
            AddLabel(116, 20, 1071, "Need more chips?");
            AddLabel(18, 36, 1071, "===========================================");
            AddLabel(22, 50, 1071, "Your chips:");
            AddLabel(22, 74, 1071, "Max buy-in:");
            AddLabel(22, 98, 1071, "Bank balance:");
            AddLabel(22, 122, 1071, "Buy-in amount:");
            if (m_Player.Bankroll == 0)
                AddLabel(115, 50, 50, "0");
            else
                AddLabel(115, 50, 50, String.Format("{0:0,0}", (double)m_Player.Bankroll));
            AddLabel(115, 74, 50, String.Format("{0:0,0}", (double)m_Dealer.BuyinMaximum));
            int balance = Server.Mobiles.Banker.GetBalance(m_Player.Mobile);
            AddLabel(115, 98, balance < m_Dealer.BuyinMinimum - m_Player.Bankroll ? 33 : balance < m_Dealer.BuyinMinimum * 2 ? 50 : 1071, String.Format("{0:0,0}", (double)balance));
            AddTextEntry(115, 122, 120, 20, 50, 0, String.Format("{0}", m_Dealer.BuyinMinimum - m_Player.Bankroll));
            AddButton(220, 90, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(220, 120, 243, 241, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile m = state.Mobile;

            if (m_Dealer == null || m_Player == null || m == null)
                return;

            switch (info.ButtonID)
            {
                case 1:
                    {
                        try
                        {
                            TextRelay te = info.GetTextEntry(0);
                            if (te != null)
                            {
                                int amount = Convert.ToInt32(te.Text, 10);
                                int realamount = amount + m_Player.Bankroll;
                                if (realamount < m_Dealer.BuyinMinimum || realamount > m_Dealer.BuyinMaximum)
                                {
                                    if (realamount < m_Dealer.BuyinMinimum)
                                        m.SendMessage(33, "The minimum buy-in is {0:0,0}gp!", (double)m_Dealer.BuyinMinimum);
                                    else
                                        m.SendMessage(33, "The maximum buy-in is {0:0,0}gp!", (double)m_Dealer.BuyinMaximum);
                                    m.SendGump(new GetGoldGump(m_Dealer, m_Player));
                                }
                                else if (!Server.Mobiles.Banker.Withdraw(m_Player.Mobile, amount))
                                {
                                    m.SendGump(new GetGoldGump(m_Dealer, m_Player));
                                    m.SendMessage(33, "You cannot afford to join the table with {0:0,0}gp!", (double)amount);
                                }
                                else
                                    m_Dealer.ConfirmedPayment(m_Player, realamount);
                                return;
                            }
                        }
                        catch
                        {
                            m.SendGump(new GetGoldGump(m_Dealer, m_Player));
                            m.SendMessage(33, "Invalid format.");
                        }
                        break;
                    }
                case 2:
                    {
                        m_Dealer.VerifyQuit(m);
                        break;
                    }
            }
        }
    }

    public class LeavePokerGump : Gump
    {
        private PokerDealer m_Dealer;

        public LeavePokerGump(Mobile mobile, PokerDealer dealer) : base(150, 150)
        {
            m_Dealer = dealer;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(25, 25, 315, 185, 9270);
            AddAlphaRegion(35, 35, 295, 165);
            AddLabel(130, 45, 1071, "Stand up and leave?");
            AddLabel(45, 60, 1071, "============================================");
            AddLabel(50, 80, 70, "Are you sure you want to cash-in and");
            AddLabel(50, 100, 70, "leave the poker table? You will play");
            AddLabel(50, 120, 70, "the current hand to completion, any");
            AddLabel(50, 140, 70, "winnings will be deposited in your bank box.");
            AddButton(185, 165, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(255, 165, 242, 241, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (info.ButtonID == 1)
            {
                PokerPlayer player = m_Dealer.GetPlayer(from);
                if (m_Dealer.GameProgress == PokerProgress.None && !m_Dealer.PTActive && player != null)
                    m_Dealer.VerifyQuit(player.Mobile);
                else
                {
                    player.Quitting = true;
                    m_Dealer.RefreshGump(from);
                    if (m_Dealer.ReadyPlayers.Length == 1)
                        m_Dealer.PrepareForRound();
                }
            }
            if (info.ButtonID == 2)
                from.SendMessage("You decide not to leave the poker table.");
        }
    }

    public class BettingGump : Gump
    {
        PokerDealer m_Dealer;
        PokerPlayer m_Player;

        public BettingGump(PokerDealer dealer, PokerPlayer player, bool gump) : base(400, 385)
        {
            m_Dealer = dealer;
            m_Player = player;

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;

            if (m_Dealer == null || m_Player == null)
                return;

            AddPage(0);
            AddBackground(120, 95, 200, 118, 9270);
            AddBackground(228, 175, 75, 20, 9300);
            AddAlphaRegion(125, 100, 190, 108);
            AddButton(225, 145, 247, 248, 4, GumpButtonType.Reply, 0);

            AddRadio(135, 110, 9720, 9723, false, 1);
            if (m_Dealer.HighestBet == player.RoundBet)
                AddLabel(170, 115, 1071, "Check");
            else if (player.Bankroll > m_Dealer.HighestBet - player.RoundBet)
            {
                AddLabel(170, 115, 1071, "Call");
                AddLabel(230, 115, 50, String.Format("{0:0,0}", (double)(m_Dealer.HighestBet - player.RoundBet)));
            }
            else
                AddLabel(170, 115, 50, "All-in");

            AddRadio(135, 140, 9720, 9723, false, 2);
            AddLabel(170, 145, 1071, "Fold");

            if (gump)
            {
                AddRadio(135, 170, 9720, 9723, true, 3);
                AddTextEntry(230, 175, 102, 20, 50, 5, String.Format("{0}", (double)(m_Dealer.BlindSmall * 2)));
            }
            else
                AddButton(135, 170, 9720, 9723, 5, GumpButtonType.Reply, 0);

            if (m_Dealer.HighestBet == 0)
                AddLabel(170, 175, 1071, "Bet");
            else
                AddLabel(170, 175, 1071, "Raise");

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            PokerPlayer player = m_Dealer.GetPlayer(from);
            if (m_Dealer == null || from == null)
                return;

            if (info.ButtonID == 5)
            {
                m_Dealer.SendBettingGump(m_Player, true);
            }

            if (info.ButtonID == 4)
            {
                if (!info.IsSwitched(1) && !info.IsSwitched(2) && !info.IsSwitched(3))
                {
                    from.SendMessage(33, "Please select an action before pressing 'okay'");
                    m_Dealer.SendBettingGump(m_Player, false);
                }
                else if (info.IsSwitched(1))
                {
                    if (m_Dealer.HighestBet == player.RoundBet)
                        m_Dealer.ActionTaken(m_Player, PokerAction.Check);
                    else
                        m_Dealer.ActionTaken(m_Player, PokerAction.Call);
                }
                else if (info.IsSwitched(2))
                    m_Dealer.ActionTaken(m_Player, PokerAction.Fold);
                else if (info.IsSwitched(3))
                {
                    try
                    {
                        TextRelay te = info.GetTextEntry(5);
                        if (te != null)
                        {
                            int betamount = Convert.ToInt32(te.Text, 10);
                            if (betamount <= 0)
                            {
                                from.SendMessage(33, "Please enter a number higher than 0");
                                m_Dealer.SendBettingGump(m_Player, true);
                                return;
                            }
                            else if (betamount < (m_Dealer.BlindSmall * 2))
                            {
                                player.Mobile.SendMessage(33, "You must raise minimum of {0:0,0}gp.", (double)m_Dealer.BlindSmall);
                                m_Dealer.SendBettingGump(player, true);
                                return;
                            }
                            else
                                m_Dealer.ActionTaken(m_Player, PokerAction.Raise, betamount);
                        }
                    }
                    catch
                    {
                        m_Dealer.SendBettingGump(m_Player, true);
                        return;
                    }
                }
            }
        }
    }

    public class GamePokerGump : Gump
    {
        PokerDealer m_Dealer;

        public GamePokerGump(PokerDealer dealer, Mobile mobile) : base(20, 20)
        {
            m_Dealer = dealer;
            PokerPlayer player = m_Dealer.GetPlayer(mobile);
            int i = m_Dealer.GetIndex(player);

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);

            AddBackground(265, 306, 80, 80, 3000);
            AddBackground(295, 306, 80, 80, 3000);

            if (player.Quitting)
                AddImage(358, 305, 56);
            else
                AddButton(358, 305, 55, 55, 1, GumpButtonType.Reply, 0);

            if (m_Dealer.ReadyPlayers.Length > 0)
                DrawPlayers(i);

            if (m_Dealer.Players[i].Status >= Status.Playing && (PokerPlayer)m_Dealer.Players[i] == player && m_Dealer.GameProgress > PokerProgress.None)
                DrawCards(m_Dealer.Players[i].Hand, 265, 306, true);

            if (m_Dealer.GameProgress > PokerProgress.PreFlop && m_Dealer.GameProgress < PokerProgress.End)
            {
                int x = (int)m_Dealer.GameProgress - 2;
                DrawCommunityCards(x);
                AddItem(295, 245, 3823);
                AddLabel(273, 250, 50, String.Format("{0}({1:0,0})", GetSpace(m_Dealer.PotWorth), (double)m_Dealer.PotWorth));
            }
            else if (m_Dealer.GameProgress == PokerProgress.End && m_Dealer.EndWinners.Count > 0)
                m_Dealer.DrawList(this, 225, 95);
        }

        public void DrawPlayers(int index)
        {
            int j = index;
            int i = 0;
            do
            {
                if (m_Dealer.Players[j] != null)
                {
                    PokerPlayer player = m_Dealer.Players[j];
                    Point2D playercoords = GetCoords(i, m_Dealer.SerialPlayers.Length, true);
                    AddBackground(playercoords.X, playercoords.Y, 110, 46, 9200);

                    if (m_Dealer.PlayerTurn == j && m_Dealer.GameProgress > PokerProgress.None && m_Dealer.GameProgress < PokerProgress.End)
                        AddHtml(playercoords.X, playercoords.Y + 5, 110, 25, String.Format("<basefont color=\"#00FF00\"><center>{0}</center></font>", player.Mobile.Name), false, false);
                    else if (player.Status == Status.WaitingForRound || player.Status == Status.GettingGold)
                        AddHtml(playercoords.X, playercoords.Y + 5, 110, 25, String.Format("<center>{0}</center>", player.Mobile.Name), false, false);
                    else
                        AddHtml(playercoords.X, playercoords.Y + 5, 110, 25, String.Format("<basefont color=\"#FFFFFF\"><center>{0}</center></font>", player.Mobile.Name), false, false);

                    if (player.Bankroll == 0 && player.Status > Status.WaitingForRound)
                        AddLabel(playercoords.X + 30, playercoords.Y + 25, 50, String.Format("(All-in)"));
                    else
                        AddLabel(playercoords.X + 10, playercoords.Y + 25, 50, String.Format("{0}({1:0,0})", GetSpace(player.Bankroll), (double)player.Bankroll));

                    if (m_Dealer.ActivePlayers.Length >= 2 && m_Dealer.Dealer == j)
                    {
                        AddBackground(playercoords.X - 13, playercoords.Y - 11, 25, 25, 9568);
                        AddLabel(playercoords.X - 5, playercoords.Y - 7, 0, "D");
                    }
                    if (player.RoundBet != 0)
                    {
                        Point2D goldcoords = GetCoords(i, m_Dealer.SerialPlayers.Length, false);
                        AddItem(goldcoords.X, goldcoords.Y, 3823, 0);
                        AddLabel(goldcoords.X - 20, goldcoords.Y + 5, 50, String.Format("{0}({1:0,0})", GetSpace(player.RoundBet), (double)player.RoundBet));
                    }
                    i++;
                }
                j++;
                if (j > m_Dealer.Players.Length - 1)
                    j -= m_Dealer.Players.Length;
            } while (i < m_Dealer.SerialPlayers.Length);
        }

        public string GetSpace(int amount)
        {
            int br = (10 - amount.ToString().Length) / 2;
            string space = "";
            for (int t = 0; t < br; t++)
                space += " ";
            return space;
        }

        private Point2D GetCoords(int number, int playeramount, bool player)
        {
            if (player)
            {
                int x = (int)(20 + 245 + Math.Cos((-90 + ((-360 / playeramount) * number)) * Math.PI / 180) * 245);
                int y = (int)(30 + 177 - Math.Sin((-90 + ((-360 / playeramount) * number)) * Math.PI / 180) * 177);
                return new Point2D(x, y);
            }
            else
            {
                int x = (int)(128 + 170 + Math.Cos((-90 + ((-360 / playeramount) * number)) * Math.PI / 180) * 170);
                int y = (int)(70 + 145 - Math.Sin((-90 + ((-360 / playeramount) * number)) * Math.PI / 180) * 145);
                return new Point2D(x, y);
            }
        }

        public void DrawCommunityCards(int x)
        {
            DrawCards(m_Dealer.CommunityCards, 245 - 15 * x, 155, false);
        }

        public void DrawCards(CardHand hand, int x, int y, bool player)
        {
            if (hand.Cards != null)
                for (int i = 0; i < hand.Cards.Length; i++)
                    hand.Cards[i].Draw(this, x + 30 * i, y, player);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (info.ButtonID == 1)
            {
                from.CloseGump(typeof(GamePokerGump));
                from.SendGump(new GamePokerGump(m_Dealer, from));
                from.CloseGump(typeof(LeavePokerGump));
                from.SendGump(new LeavePokerGump(from, m_Dealer));
            }
        }
    }

    public class RulesPokerGump : Gump
    {
        private PokerDealer m_Dealer;

        public RulesPokerGump(PokerDealer dealer) : base(150, 150)
        {
            m_Dealer = dealer;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(25, 25, 335, 140, 9270);
            AddAlphaRegion(35, 35, 315, 120);
            AddLabel(47, 45, 1071, "Do you wish to open an url to the poker guide?");
            AddLabel(45, 60, 1071, "=================================================");
            AddLabel(47, 80, 70, "The guide will cover everything you need to know");
            AddLabel(47, 100, 70, "about this poker system, from hand sequences");
            AddLabel(47, 120, 70, "to weekly jackpot payout and more.");
            AddButton(277, 125, 247, 248, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (info.ButtonID == 1)
                from.LaunchBrowser(m_Dealer.URL);
        }
    }

    public class PokerStatGump : Gump
    {
        private PokerDealer m_Dealer;

        public PokerStatGump(PokerDealer dealer) : base(150, 150)
        {
            m_Dealer = dealer;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 400, 230, 5120);
            AddAlphaRegion(10, 10, 380, 210);
            AddImage(300, 10, 4037);
            AddLabel(25, 25, 1071, String.Format("Statistics for poker dealer \"{0}\".", m_Dealer.Name));
            AddLabel(25, 55, 1071, "Gold to jackpot:");
            AddLabel(165, 55, 1071, "absolute total");
            AddLabel(280, 55, 50, String.Format("{0:0,0}gp", (double)m_Dealer.TotalGoldToJackpot));
            AddLabel(165, 80, 1071, "average per round");
            AddLabel(280, 80, 50, String.Format("{0:0,0.##}gp", (double)m_Dealer.TotalGoldToJackpot / m_Dealer.RoundsFinished));
            AddLabel(25, 110, 1071, "Gold drained:");
            AddLabel(165, 110, 1071, "absolute total");
            AddLabel(280, 110, 50, String.Format("{0:0,0}gp", (double)m_Dealer.TotalGoldDrained));
            AddLabel(165, 135, 1071, "average per round");
            AddLabel(280, 135, 50, String.Format("{0:0,0.##}gp", (double)m_Dealer.TotalGoldDrained / m_Dealer.RoundsFinished));
            AddLabel(25, 165, 1071, "Players joined so far:");
            AddLabel(280, 165, 1071, String.Format("{0:0,0}", (double)m_Dealer.PlayerAmount));
            AddLabel(25, 190, 1071, "Table status:");
            AddLabel(280, 190, 1071, String.Format("{0}", m_Dealer.IsHighRoller ? "high-roller" : "low-roller"));
        }
    }
}