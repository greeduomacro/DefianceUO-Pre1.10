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
using Server.Gumps;
using Server.Spells;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.Poker;

namespace Server.Engines.Poker
{
	public enum PokerAction { None, Check, Fold, Raise, Call }

	public enum PokerProgress { None, PreFlop, Flop, Turn, River, End }

	public enum DealerSetup { Regular, SingleLoop, ForwardTable, FinalTable }

	public enum PokerReward { None, PokerLowRollerTicket, PokerHighRollerTicket }

	public enum Award { None, Top1, Top3, Top5, Top10}

	public enum DealerAccept {TournyTicketOnly, GoldOnly, LowRollerTicketsOnly, HighRollerTicketsOnly }

	public class PokerDealer : BaseCreature
	{
		private static ArrayList m_Registry = new ArrayList();

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler(OnLogin);
			Server.Commands.Register("pokerstart", AccessLevel.GameMaster, new CommandEventHandler(StartPoker_Command));
			Server.Commands.Register("pokerkickplayer", AccessLevel.GameMaster, new CommandEventHandler(KickPlayer_Command));
		}

		private static void OnLogin(LoginEventArgs e)
		{
			CheckForGump(e.Mobile);
			FindRefundFor(e.Mobile);
		}

		public static void CheckForGump(Mobile from)
		{
			for (int i = 0; i < m_Registry.Count; i++)
			{
				PokerDealer dealer = (PokerDealer)m_Registry[i];
				if (dealer != null)
					for (int j = 0; j < dealer.Players.Length; j++)
						if (dealer.Players[j] != null && dealer.Players[j].Mobile == from)
						{
							dealer.CloseAllGumps(from);
							PokerPlayer player = dealer.Players[j];
							if (player != null && player.Status == Status.GettingGold)
								from.SendGump(new GetGoldGump(dealer, player));
							else if (dealer.PlayerTurn == j)
								dealer.SendBettingGump(player, false);
							dealer.RefreshGump(from);
							break;
						}
			}
		}

		public static void FindRefundFor(Mobile from)
		{
			for (int i = 0; i < m_Registry.Count; i++)
			{
				PokerDealer dealer = (PokerDealer)m_Registry[i];
				if (dealer != null)
					for (int t = 0; t < dealer.CrashList.Count; t++)
					{
						CrashEntry entry = (CrashEntry)dealer.CrashList[t];
						if (from == entry.m_Mobile)
						{
							from.SendMessage(0x482, "[Poker message] Due to a shard revert,");
							int gold = entry.m_Bankroll;
							if (gold > 0)
							{
								from.SendMessage(0x482, "a total of {0:0,0}gp has been replaced back in your bank.", (double)gold);
								while (gold > 1000000)
								{
									from.BankBox.DropItem(new BankCheck(1000000));
									gold -= 1000000;
								}
								if (gold > 5000)
									from.BankBox.DropItem(new BankCheck(gold));
								else if (gold > 0)
									from.BankBox.DropItem(new Gold(gold));
							}
							if (entry.m_Ticket > 0)
							{
								from.Backpack.DropItem(new PokerTournyTicket(entry.m_Ticket, from));
								from.SendMessage(0x482, "a poker ticket with a total of {0:0,0}gp has been replaced back in your backpack.", (double)entry.m_Ticket);
							}
							if (entry.m_LowRoller)
							{
								from.Backpack.DropItem(new PokerLowRollerTicket());
								from.SendMessage(0x482, "your low-roller poker ticket has been placed back in your backpack.");
							}
							if (entry.m_HighRoller)
							{
								from.Backpack.DropItem(new PokerHighRollerTicket());
								from.SendMessage(0x482, "your high-roller poker ticket has been placed back in your backpack.");
							}
							dealer.CrashList.Remove(entry);
							t--;
						}
					}
			}
		}

		public static int IsPokerPlayer(Mobile from)
		{
			for (int i = 0; i < m_Registry.Count; i++)
			{
				PokerDealer dealer = (PokerDealer)m_Registry[i];
				if (dealer != null)
					for (int j = 0; j < dealer.Players.Length; j++)
						if (dealer.Players[j] != null && dealer.Players[j].Mobile == from)
							return j;
			}
			return -1;
		}

		private static void StartPoker_Command(CommandEventArgs e)
		{
			e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(StartPoker_Target));
			e.Mobile.SendMessage("Target the poker dealer to start the game. NOTE: Only use in tournament environment!");
		}

		private static void StartPoker_Target(Mobile from, object o)
		{
			if (o is PokerDealer)
			{
				PokerDealer dealer = (PokerDealer)o;
				if (dealer.ReadyPlayers.Length > 0 && dealer.GameProgress == PokerProgress.None)
				{
					dealer.PTActive = true;
					dealer.PrepareForRound();
					from.SendMessage("The game has been started.");
				}
				else if (dealer.GameProgress > PokerProgress.None)
					from.SendMessage("The game is already running.");
			}
			else
			{
				from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(StartPoker_Target));
				from.SendMessage("Target a poker dealer.");
			}
		}

		private static void KickPlayer_Command(CommandEventArgs e)
		{
			e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(KickPlayer_Target));
			e.Mobile.SendMessage("CAUTION! The targetted poker player will be removed at the end of the round, use this command with care, no revert possible!");
		}

		private static void KickPlayer_Target(Mobile from, object o)
		{
			if (o is PlayerMobile)
			{
				Mobile mobile = o as PlayerMobile;
				int nr = IsPokerPlayer(mobile);
				if (nr == -1)
					from.SendMessage("That is not a poker player!");
				else
					for (int i = 0; i < m_Registry.Count; i++)
					{
						PokerDealer dealer = (PokerDealer)m_Registry[i];
						if (dealer.Players[nr] != null && dealer.Players[nr].Mobile == mobile)
						{
							dealer.Players[nr].Quitting = true;
							from.SendMessage("The player will be removed from the table.");
							return;
						}
					}
			}
			else
			{
				from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(StartPoker_Target));
				from.SendMessage("Target a player.");
			}
		}

		private int m_Dealer;
		private int m_PotWorth;
		private int m_HighestBet;
		private int m_PlayerTurn;
		private int m_MovesSinceDeal;
		private Timer m_ActionTimer;
		private Timer m_PrepareTimer;
		private Timer m_GumpTimer;
		private CardDeck m_Deck;
		private PokerPlayer[] m_Players;
		private CardHand m_CommunityCards;
		private PokerProgress m_GameProgress;

		public ArrayList AwardList = new ArrayList();
		public ArrayList EndWinners = new ArrayList();
		public int Dealer { get { return m_Dealer; } }
		public int PotWorth { get { return m_PotWorth; } }
		public int HighestBet { get { return m_HighestBet; } }
		public int PlayerTurn { get { return m_PlayerTurn; } }
		public CardDeck Deck { get { if (m_Deck == null) m_Deck = new CardDeck(); return m_Deck; } }
		public PokerPlayer[] Players { get { if (m_Players == null) m_Players = new PokerPlayer[MaxPlayers]; return m_Players; } set { m_Players = value; } }
		public CardHand CommunityCards { get { if (m_CommunityCards == null) m_CommunityCards = new CardHand(); return m_CommunityCards; } }
		public PokerProgress GameProgress { get { return m_GameProgress; } }

		public PokerPlayer[] SerialPlayers
		{
			get
			{
				ArrayList list = new ArrayList();
				for (int i = 0; i < Players.Length; i++)
					if (Players[i] != null && Players[i].Status >= Status.GettingGold)
						list.Add(Players[i]);
				PokerPlayer[] players = new PokerPlayer[list.Count];
				for (int i = 0; i < list.Count; i++)
					players[i] = (PokerPlayer)list[i];
				return players;
			}
		}

		public PokerPlayer[] ReadyPlayers
		{
			get
			{
				ArrayList list = new ArrayList();
				for (int i = 0; i < Players.Length; i++)
					if (Players[i] != null && Players[i].Status > Status.GettingGold)
						list.Add(Players[i]);
				PokerPlayer[] players = new PokerPlayer[list.Count];
				for (int i = 0; i < list.Count; i++)
					players[i] = (PokerPlayer)list[i];
				return players;
			}
		}

		public PokerPlayer[] ActivePlayers
		{
			get
			{
				ArrayList list = new ArrayList();
				for (int i = 0; i < Players.Length; i++)
				{
					if (Players[i] != null && Players[i].Status > Status.WaitingForRound)
						list.Add(Players[i]);
				}
				PokerPlayer[] players = new PokerPlayer[list.Count];
				for (int i = 0; i < list.Count; i++)
					players[i] = (PokerPlayer)list[i];
				return players;
			}
		}

		public PokerPlayer[] PlayersWithStatus(Status status)
		{
			ArrayList list = new ArrayList();
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i] != null && Players[i].Status == status)
					list.Add(Players[i]);
			}
			PokerPlayer[] players = new PokerPlayer[list.Count];
			for (int i = 0; i < list.Count; i++)
				players[i] = (PokerPlayer)list[i];
			return players;
		}

		#region Specials
		//Xmas cards deco
		#endregion

		#region PokerTournament Settings
		private bool m_Active;
		private Item m_TournyReward;
		private bool m_msg;
		private PokerReward m_PokerReward;
		private PokerDealer m_PTNextDealer;
		private Award m_Award;
		private Point3D m_WinnerPoint;
		private int m_maxforward;
		private int m_memblind;
		private int m_RaiseSmall;
		private int m_RoundRaise;

		[CommandProperty(AccessLevel.Seer)]
		public bool PTBroadcast { get { return m_msg; } set { if (m_DealerSetup == DealerSetup.SingleLoop || m_DealerSetup == DealerSetup.FinalTable) m_msg = value; else m_msg = false; } }
		[CommandProperty(AccessLevel.Seer)]
		public bool PTActive { get { return m_Active; } set { if (m_DealerSetup > DealerSetup.Regular)m_Active = value; else m_Active = false; } }
		[CommandProperty(AccessLevel.Seer)]
		public Item PTcustomReward { get { return m_TournyReward; } set { if (m_DealerSetup == DealerSetup.FinalTable) m_TournyReward = value; else m_TournyReward = null; } }
		[CommandProperty(AccessLevel.Seer)]
		public PokerDealer PTNextDealer { get { return m_PTNextDealer; } set { if (m_DealerSetup == DealerSetup.ForwardTable) m_PTNextDealer = value; else m_PTNextDealer = null; } }
		[CommandProperty(AccessLevel.Seer)]
		public Point3D Point_Winners { get { if (m_WinnerPoint == Point3D.Zero) return m_WinnerPoint = new Point3D(this.X, this.Y + 1, this.Z); else return m_WinnerPoint; } set { m_WinnerPoint = value; } }
		[CommandProperty(AccessLevel.Seer)]
		public int PTMaxForward { get { if (m_maxforward < 0 || m_maxforward > MaxPlayers)return 1; else return m_maxforward; } set { if (m_DealerSetup == DealerSetup.ForwardTable) m_maxforward = value; else m_maxforward = 0; } }
		[CommandProperty(AccessLevel.Seer)]
		public int PTRaiseSmall { get { if (m_RaiseSmall < 0)return 0; else return m_RaiseSmall; } set { if (m_DealerSetup > DealerSetup.Regular) m_RaiseSmall = value; else m_RaiseSmall = 0; } }
		[CommandProperty(AccessLevel.Seer)]
		public PokerReward PTticketReward { get { return m_PokerReward; } set { if (m_DealerSetup == DealerSetup.SingleLoop || m_DealerSetup == DealerSetup.FinalTable) m_PokerReward = value; else m_PokerReward = PokerReward.None; } }

		[CommandProperty(AccessLevel.Seer)]
		public Award PTAward
		{
			get { return m_Award; }
			set
			{
				if (m_DealerSetup == DealerSetup.SingleLoop || m_DealerSetup == DealerSetup.FinalTable)
					m_Award = value;
				else
					m_Award = Award.None;
				switch (m_Award)
				{
					case Award.None:
						break;
					case Award.Top1:
						break;
					case Award.Top3:
						if (MaxPlayers < 3)
							PTAward = Award.Top1;
						break;
					case Award.Top5:
						if (MaxPlayers < 5)
							PTAward = Award.Top3;
						break;
					case Award.Top10:
						if (MaxPlayers < 10)
							PTAward = Award.Top5;
						break;
				}
			}
		}
		#endregion

		#region PokerDealer Settings
		public ArrayList CrashList = new ArrayList();
		private class CrashEntry
		{
			public Mobile m_Mobile;
			public int m_Bankroll;
			public int m_Ticket;
			public bool m_LowRoller;
			public bool m_HighRoller;
			public CrashEntry(Mobile mobile, int bankroll, int ticket, bool lowroller, bool highroller)
			{
				m_Mobile = mobile;
				m_Bankroll = bankroll;
				m_Ticket = ticket;
				m_LowRoller = lowroller;
				m_HighRoller = highroller;
			}
		}

		private void AddCrashEntry(Mobile mobile, int bankroll, int ticket, bool lowroller, bool highroller)
		{
			foreach (CrashEntry entry in CrashList)
				if (entry.m_Mobile == mobile)
				{
					entry.m_Bankroll = bankroll;
					entry.m_Ticket = ticket;
					entry.m_LowRoller = lowroller;
					entry.m_HighRoller = lowroller;
					return;
				}
			CrashList.Add(new CrashEntry(mobile, bankroll, ticket, lowroller, highroller));
		}

		private Point3D[] m_Seat = new Point3D[10];
		private PokerJackpot m_Stone;
		private DealerSetup m_DealerSetup;
		private DealerAccept m_DealerAccept;

		private bool m_Open;
		private bool m_AllowSameIP;
		private bool m_IsHighRoller;

		private int m_MinBuyIn;
		private int m_MaxBuyIn;
		private int m_BlindSmall;
		private int m_DrainToJackpot;
		private int m_DrainFromPot;
		private int m_DrainFromBuyIn;
		private int m_GetGoldTimer;
		private int m_AutoFoldTimer;
		private int m_TotalGoldDrained;
		private int m_TotalGoldToJackpot;
		private int m_RoundsFinished;
		private int m_PlayerAmount;
		private int m_MaxPlayers;
		private int m_TotalMoneyAtStart;

		private Point3D m_ExitPoint;
		private string m_URL;

		[CommandProperty(AccessLevel.Seer)]
		public DealerSetup DealerMode
		{
			get { return m_DealerSetup; }
			set
			{
				if (SerialPlayers.Length == 0)
				{
					m_DealerSetup = value;
					switch (m_DealerSetup)
					{
						case DealerSetup.Regular:
							DealerOpen = false;
							PTActive = false;
							PTAward = Award.None;
							PTBroadcast = false;
							PTcustomReward = null;
							PTMaxForward = 0;
							PTNextDealer = null;
							PTRaiseSmall = 0;
							PTticketReward = PokerReward.None;
							DrainFromBuyIn = 0;
							DealerAccepts = DealerAccept.GoldOnly;
							break;
						case DealerSetup.SingleLoop:
							DealerOpen = false;
							PTActive = false;
							PTcustomReward = null;
							PTMaxForward = 0;
							PTNextDealer = null;
							DrainToJackpot = 0;
							DrainFromPot = 0;
							Pokerstone = null;
							DealerAccepts = DealerAccept.GoldOnly;
							break;
						case DealerSetup.ForwardTable:
							DealerOpen = false;
							PTActive = false;
							PTAward = Award.None;
							PTBroadcast = false;
							PTcustomReward = null;
							DrainToJackpot = 0;
							DrainFromPot = 0;
							Pokerstone = null;
							PTticketReward = PokerReward.None;
							DealerAccepts = DealerAccept.GoldOnly;
							break;
						case DealerSetup.FinalTable:
							DealerOpen = false;
							PTMaxForward = 0;
							PTNextDealer = null;
							DrainToJackpot = 0;
							DrainFromPot = 0;
							DrainFromBuyIn = 0;
							Pokerstone = null;
							DealerAccepts = DealerAccept.TournyTicketOnly;
							PTMaxForward = 0;
							break;
					}
				}
			}
		}

		[CommandProperty(AccessLevel.Seer)]
		public DealerAccept DealerAccepts
		{
			get { return m_DealerAccept; }
			set
			{
				if (m_DealerSetup == DealerSetup.FinalTable)
					m_DealerAccept = DealerAccept.GoldOnly;
				else if (m_DealerSetup == DealerSetup.FinalTable)
					m_DealerAccept = DealerAccept.TournyTicketOnly;
				else
					m_DealerAccept = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int MaxPlayers
		{
			get { return m_MaxPlayers; }
			set
			{
				if (SerialPlayers.Length == 0)
				{
					m_MaxPlayers = value;
					if (m_MaxPlayers < 2 || m_MaxPlayers > 10)
						m_MaxPlayers = 10;
					Players = new PokerPlayer[m_MaxPlayers];
					PTAward = m_Award;
					PTMaxForward = m_maxforward;
				}
			}
		}

		[CommandProperty(AccessLevel.Seer)]
		public string URL { get { return m_URL; } set { m_URL = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public int Timer_AutoFold { get { if (m_AutoFoldTimer <= 0)return 30; else return m_AutoFoldTimer; } set { m_AutoFoldTimer = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public int Timer_GetGold { get { if (m_GetGoldTimer <= 0)return 30; else return m_GetGoldTimer; } set { m_GetGoldTimer = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public bool IsHighRoller { get { return m_IsHighRoller; } set { m_IsHighRoller = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public PokerJackpot Pokerstone { get { return m_Stone; } set { if (m_DealerSetup == DealerSetup.Regular)m_Stone = value; else m_Stone = null; } }

		[CommandProperty(AccessLevel.Seer)]
		public bool AllowSameIP { get { return m_AllowSameIP; } set { m_AllowSameIP = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition0 { get { if (m_Seat[0] == Point3D.Zero) return m_Seat[0] = new Point3D(this.X - 2, this.Y - 1, this.Z); else return m_Seat[0]; } set { m_Seat[0] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition1 { get { if (m_Seat[1] == Point3D.Zero) return m_Seat[1] = new Point3D(this.X - 2, this.Y - 2, this.Z); else return m_Seat[1]; } set { m_Seat[1] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition2 { get { if (m_Seat[2] == Point3D.Zero) return m_Seat[2] = new Point3D(this.X - 2, this.Y - 3, this.Z); else return m_Seat[2]; } set { m_Seat[2] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition3 { get { if (m_Seat[3] == Point3D.Zero) return m_Seat[3] = new Point3D(this.X - 2, this.Y - 4, this.Z); else return m_Seat[3]; } set { m_Seat[3] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition4 { get { if (m_Seat[4] == Point3D.Zero) return m_Seat[4] = new Point3D(this.X - 2, this.Y - 5, this.Z); else return m_Seat[4]; } set { m_Seat[4] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition5 { get { if (m_Seat[5] == Point3D.Zero) return m_Seat[5] = new Point3D(this.X + 2, this.Y - 5, this.Z); else return m_Seat[5]; } set { m_Seat[5] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition6 { get { if (m_Seat[6] == Point3D.Zero) return m_Seat[6] = new Point3D(this.X + 2, this.Y - 4, this.Z); else return m_Seat[6]; } set { m_Seat[6] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition7 { get { if (m_Seat[7] == Point3D.Zero) return m_Seat[7] = new Point3D(this.X + 2, this.Y - 3, this.Z); else return m_Seat[7]; } set { m_Seat[7] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition8 { get { if (m_Seat[8] == Point3D.Zero) return m_Seat[8] = new Point3D(this.X + 2, this.Y - 2, this.Z); else return m_Seat[8]; } set { m_Seat[8] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SeatPosition9 { get { if (m_Seat[9] == Point3D.Zero) return m_Seat[9] = new Point3D(this.X + 2, this.Y - 1, this.Z); else return m_Seat[9]; } set { m_Seat[9] = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public bool DealerOpen { get { return m_Open; } set { m_Open = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public int BlindSmall { get { if (m_BlindSmall <= 0)return 500; else return m_BlindSmall; } set { m_BlindSmall = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public int BuyinMinimum { get { if (m_MinBuyIn <= 0)return (BlindSmall * 10); else return m_MinBuyIn; } set { m_MinBuyIn = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public int BuyinMaximum { get { if (m_MaxBuyIn <= 0)return (BuyinMinimum * 10); else return m_MaxBuyIn; } set { m_MaxBuyIn = value; } }

		[CommandProperty(AccessLevel.Seer)]
		public int DrainToJackpot { get { if (m_DrainToJackpot < 0)return 0; else if (m_DrainToJackpot > 3)return 3; else return m_DrainToJackpot; } set { if (m_DealerSetup == DealerSetup.Regular)m_DrainToJackpot = value; else m_DrainToJackpot = 0; } }

		[CommandProperty(AccessLevel.Seer)]
		public int DrainFromPot { get { if (m_DrainFromPot < 0 )return 0; else if( m_DrainFromPot > 3)return 3; else return m_DrainFromPot; } set { if (m_DealerSetup == DealerSetup.Regular)m_DrainFromPot = value; else m_DrainFromPot = 0; } }

		[CommandProperty(AccessLevel.Seer)]
		public int DrainFromBuyIn { get { if (m_DrainFromBuyIn < 0)return 0; else if (m_DrainFromBuyIn > 20)return 20; else return m_DrainFromBuyIn; } set { if (m_DealerSetup > DealerSetup.Regular) m_DrainFromBuyIn = value; else m_DrainFromBuyIn = 0; } }

		public int TotalGoldToJackpot { get { return m_TotalGoldToJackpot; } }
		public int TotalGoldDrained { get { return m_TotalGoldDrained; } }
		public int RoundsFinished { get { return m_RoundsFinished; } }
		public int PlayerAmount { get { return m_PlayerAmount; } }

		[CommandProperty(AccessLevel.Seer)]
		public Point3D Point_Exit { get { if (m_ExitPoint == Point3D.Zero) return m_ExitPoint = new Point3D(this.X, this.Y + 1, this.Z); else return m_ExitPoint; } set { m_ExitPoint = value; } }
		#endregion

		[Constructable]
		public PokerDealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.8, 3.0)
		{
			m_Registry.Add(this);
			m_MaxPlayers = 10;
			DealerMode = DealerSetup.Regular;
			m_TotalGoldToJackpot = 0;
			m_TotalGoldDrained = 0;
			m_RoundsFinished = 0;
			m_PlayerAmount = 0;
			m_memblind = 0;

			#region NPC Properties
			SetStr(55, 100);
			SetDex(55, 100);
			SetInt(55, 100);
			Fame = 50;
			Karma = 50;
			SpeechHue = Utility.RandomDyedHue();
			Title = "the pokerdealer";
			Hue = Utility.RandomSkinHue();
			NameHue = 0x35;
			Blessed = true;
			if (this.Female = Utility.RandomBool())
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName("female");
				Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2045, 0x204A, 0x2046, 0x2049));
				hair.Hue = Utility.RandomHairHue();
				hair.Layer = Layer.Hair;
				hair.Movable = false;
				AddItem(hair);
				Item hat = new Bonnet();
				AddItem(hat);
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName("male");
				Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2048));
				hair.Hue = Utility.RandomHairHue();
				hair.Layer = Layer.Hair;
				hair.Movable = false;
				AddItem(hair);
				Item beard = new Item(Utility.RandomList(0x0000, 0x2040, 0x2041, 0x204D));
				beard.Hue = hair.Hue;
				beard.Layer = Layer.FacialHair;
				beard.Movable = false;
				AddItem(beard);
				Item hat = new StrawHat();
				AddItem(hat);
			}
			AddItem(new HalfApron());
			AddItem(new Skirt());
			AddItem(new Sandals());
			AddItem(new Doublet());
			Container pack = new Backpack();
			pack.DropItem(new Gold(5, 500));
			pack.Movable = false;
			pack.Visible = false;
			AddItem(pack);
			#endregion
		}

		public PokerDealer(Serial serial) : base(serial)
		{
		}

		public override void OnAfterDelete()
		{
			m_Registry.Remove(this);
			base.OnAfterDelete();
		}

		public override bool OnGoldGiven(Mobile from, Gold gold)
		{
			if (gold.Amount <= 500)
			{
				switch (Utility.Random(3))
				{
					default:
					case 0: SayTo(from, "Thou art giving me gold?"); break;
					case 1: SayTo(from, "Money is always welcome."); break;
					case 2: SayTo(from, "Art thou trying to bribe me?"); break;
				}
				return true;
			}
			else if (gold.Amount == 1337)
			{
				SayTo(from, "Thou'rt a truely 1337 person {0}!", from.Name);
				return true;
			}
			else
			{
				SayTo(from, "Art thou trying to bribe me?");
				return false;
			}
		}

		public override void OnThink()
		{
			if (Hidden)
				Hidden = false;
			base.OnThink();
		}

		public override bool CanBeDamaged() { return false; }

		public override bool DisallowAllMoves { get { return true; } }

		public override bool ClickTitle { get { return false; } }

		public override bool HandlesOnSpeech(Mobile from)
		{
			if (from.InRange(this.Location, 6))
				return true;
			return base.HandlesOnSpeech(from);
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;
			if (from.InRange(this, 6))
			{
				string text = e.Speech.ToLower();
				if (text == "jackpot")
					if(m_Stone == null)
						SayTo(from, "I am a private poker dealer");
					else
						SayTo(from, "Current jackpot: {0:0,0}gp", (double)m_Stone.Jackpot);
				else if (text == "highest hand")
					if (m_Stone == null)
						SayTo(from, "I am a private poker dealer");
					else
					{
						int cnt = m_Stone.JackpotWinner.Count;
						if (cnt == 0)
							SayTo(from, "Leading hand: currently no leading hand.");
						else
							if (cnt == 1)
							{
								Mobile player = (Mobile)m_Stone.JackpotWinner[0];
								SayTo(from, "Leading hand: {0} with {1}. {2}", player.Name, m_Stone.HandName, m_Stone.IsHighRoller ? "(high-roller)" : "(low-roller)");
							}
							else
							{
								string message = "";
								for (int i = 0; i < cnt; ++i)
								{
									Mobile player = (Mobile)m_Stone.JackpotWinner[i];
									if (i == 0)
										message += String.Format("Leading hand: tied between {0}", player.Name);
									else if (i < cnt - 1)
										message += String.Format(", {0}", player.Name);
									else
										message += String.Format(" and {0} with {1}. {2}", player.Name, m_Stone.HandName, m_Stone.IsHighRoller ? "(high-roller)" : "(low-roller)");
								}
								SayTo(from, message);
							}
					}
				else if (text == "table status")
					SayTo(from, m_IsHighRoller ? "high-roller table" : "low-roller table");
				else if (text == "rules" || text == "guide")
				{
					if (m_URL == null)
						SayTo(from, "You can find a poker guide in the DefianceUO forum guide section");
					else
					{
						from.CloseGump(typeof(RulesPokerGump));
						from.SendGump(new RulesPokerGump(this));
					}
				}
				else if (text == "credits")
					SayTo(from, "© Massimiliano 2007");
			}
			base.OnSpeech(e);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel >= AccessLevel.Seer)
			{
				from.CloseGump(typeof(PokerStatGump));
				from.SendGump(new PokerStatGump(this));
			}
			else if (from.InRange(this, 2))
			{
				PokerTournyTicket ticket = from.Backpack.FindItemByType(typeof(PokerTournyTicket)) as PokerTournyTicket;
				PokerLowRollerTicket lowticket = from.Backpack.FindItemByType(typeof(PokerLowRollerTicket)) as PokerLowRollerTicket;
				PokerHighRollerTicket highticket = from.Backpack.FindItemByType(typeof(PokerHighRollerTicket)) as PokerHighRollerTicket;

				if (!m_Open)
					SayTo(from, "I am out of service.");
				else if (from.AccessLevel == AccessLevel.Counselor || from.AccessLevel == AccessLevel.GameMaster)
					SayTo(from, "It might not be wise for staff to be playing...");
				else if (GetPlayer(from) != null)
					SayTo(from, "You already have a seat at this poker table!");
				else if (IsPokerPlayer(from) >= 0)
					SayTo(from, "You are already seated at a poker table!");
				else if (SerialPlayers.Length == MaxPlayers)
					SayTo(from, "The poker table is full, try again later.");
				else if (m_DealerSetup > DealerSetup.Regular && ticket != null)
					if (ticket.Owner == from)
					{
						VerifyJoin(from, ticket.Worth, false, true);
						ticket.Delete();
					}
					else
						SayTo(from, "That is not your ticket...");
				else if (m_DealerSetup == DealerSetup.FinalTable)
					SayTo(from, "You cannot join this table without a tournament ticket.");
				else if (m_DealerAccept == DealerAccept.LowRollerTicketsOnly && m_DealerSetup > DealerSetup.Regular)
				{
					if (lowticket == null)
						SayTo(from, "I only accept players with a low-roller poker ticket!");
					else if (!PTActive)
					{
						VerifyJoin(from, BuyinMaximum, false, true);
						lowticket.Delete();
					}
					else
						SayTo(from, "You cannot join while the tournament table is active");
				}
				else if (m_DealerAccept == DealerAccept.HighRollerTicketsOnly && m_DealerSetup > DealerSetup.Regular)
				{
					if (highticket == null)
						SayTo(from, "I only accept players with a high-roller poker ticket!");
					else if (!PTActive)
					{
						VerifyJoin(from, BuyinMaximum, false, true);
						highticket.Delete();
					}
					else
						SayTo(from, "You cannot join while the tournament table is active");
				}
				else if (m_DealerAccept == DealerAccept.GoldOnly)
				{
					if (CanJoin(from))
					{
						from.CloseGump(typeof(JoinPokerGump));
						from.SendGump(new JoinPokerGump(this, this, Banker.GetBalance(from)));
					}
				}
			}
			else
				from.SendMessage("That is too far away.");
		}

		public void saymsg(string message, Mobile mobile, bool dealer)
		{
			if (dealer)
			{
				RevealingAction();
				Say(message);
			}
			else
			{
				mobile.RevealingAction();
				IMount mount = mobile.Mount;
				if (mount != null)
					mount.Rider = null;
				mobile.Say(message);
			}
			foreach (PokerPlayer player in Players)
				if (player != null)
					player.Mobile.SendMessage(0x482, "[{0}] {1}", dealer ? "Dealer" : mobile.Name, message);
		}

		private ArrayList GetAddresses()
		{
			ArrayList adrlist = new ArrayList();
			foreach (PokerPlayer player in ReadyPlayers)
				if (player != null)
					if (player.Mobile.NetState != null)
						adrlist.Add(player.Mobile.NetState.Address);
			return adrlist;
		}

		public bool CanJoin(Mobile from)
		{
			if (from == null || from.Deleted)
				return false;

			if (from.Criminal)
			{
				SayTo(from, "Thou'rt a criminal and cannot join the poker table.");
				return false;
			}
			else if (!from.InRange(this, 2))
			{
				from.SendMessage("Thou need to step closer to the poker table to sit down.");
				return false;
			}
			else if (SpellHelper.CheckCombat(from) || from.Combatant != null)
			{
				SayTo(from, "Wouldst thou flee during the heat of battle?");
				return false;
			}
			else if (from.Mounted)
			{
				SayTo(from, "Please dismount when you wish to join the poker table.");
				return false;
			}
			else if (!from.Alive)
			{
				SayTo(from, "Thou cannot join while being dead.");
				return false;
			}
			else if (from.Poisoned)
			{
				SayTo(from, "Thou cannot join the poker table while poisoned.");
				return false;
			}
			else if (from.Hits < ((100 - from.Str) / 2) + from.Str)
			{
				SayTo(from, "Thou must have full health to join the poker table.");
				return false;
			}
			else if (Factions.Sigil.ExistsOn(from))
			{
				SayTo(from, "Thou cannot join the poker table while carrying a sigil!");
				return false;
			}
			else if (PTActive)
			{
				SayTo(from, "Thou may not join the poker table during a tournament.");
				return false;
			}
			else if (GetPlayer(from) != null)
			{
				SayTo(from, "You already have a seat at this poker table!");
				return false;
			}
			else if (IsPokerPlayer(from) >= 0)
			{
				SayTo(from, "You are already seated at a poker table!");
				return false;
			}
			else if (SerialPlayers.Length == MaxPlayers)
			{
				SayTo(from, "The poker table is full, try again later.");
				return false;
			}
			else
			{
				if (!m_AllowSameIP)
				{
					ArrayList adr = GetAddresses();
					for (int i = 0; i < adr.Count; ++i)
						if (adr[i].ToString() == from.NetState.Address.ToString())
						{
							from.SendMessage(33, "There is already someone at the poker table with your ip-address.");
							return false;
						}
				}
			}
			return true;
		}

		public void VerifyJoin(Mobile from, int buyin, bool check, bool ticket)
		{
			if (check)
				if (!CanJoin(from))
					return;

			int index = GetSitLoc();
			if (index == -1)
				from.SendMessage(33, "There are no open seats. Try to join the poker table later.");
			else
			{
				if (Players[index] == null)
					Players[index] = new PokerPlayer(from);
				else
					return;
				if (m_DealerAccept == DealerAccept.GoldOnly && !ticket)
				{
					Banker.Withdraw(from, buyin);
					m_TotalGoldDrained += (buyin * DrainFromBuyIn) / 100;
				}
				Players[index].Bankroll = buyin - ((buyin * DrainFromBuyIn) / 100);
				from.SendMessage("Thou'rt taking a place at the poker table with {0:0,0}gp.", (double)buyin);
				IMount mount = from.Mount;
				if (mount != null)
					mount.Rider = null;
				if (from.Spell != null)
					from.Spell = null;
				from.RevealingAction();
				from.MoveToWorld(m_Seat[index], Map);
				from.CantWalk = true;
				Players[index].Status = Status.WaitingForRound;
				m_PlayerAmount++;
				RefreshGump();
				if (m_DealerSetup == DealerSetup.Regular && ReadyPlayers.Length >= 2 && m_GameProgress == PokerProgress.None)
					PrepareForRound();
				else if (m_DealerSetup > DealerSetup.Regular && ReadyPlayers.Length == MaxPlayers && m_GameProgress == PokerProgress.None)
				{
					PTActive = true;
					m_memblind = BlindSmall;
					m_RoundRaise = 0;

					AwardList.Clear();
					m_TotalMoneyAtStart = 0;
					for (int i = 0; i < Players.Length; ++i)
						if (Players[i] != null)
							m_TotalMoneyAtStart += Players[i].Bankroll;

					Say("Let's play poker!");
					if (m_DealerSetup == DealerSetup.FinalTable)
						Say("The total amount of chips at the final table is {0:0,0}gp", (double)m_TotalMoneyAtStart);
					PlaySound(491);
					PrepareForRound();
				}
			}
		}

		public int GetSitLoc()
		{
			ArrayList openseats = GetOpenSeat();
			if (openseats.Count == 0)
				return -1;
			else
			{
				int cnt = Utility.Random(openseats.Count);
				for (int i = 0; i < Players.Length; ++i)
					if (i.ToString() == openseats[cnt].ToString())
						return i;
				return -1;
			}
		}

		private ArrayList GetOpenSeat()
		{
			ArrayList nrlist = new ArrayList();
			for (int i = 0; i < Players.Length; i++)
				if (Players[i] == null)
					nrlist.Add(i);
			return nrlist;
		}

		public void VerifyQuit( Mobile from )
		{
			PokerPlayer player = GetPlayer(from);
			if ( player != null )
			{
				player.EndAction();
				KickPlayer(player);
				RefreshGump();
			}
			from.CantWalk = false;
		}

		public int GetIndex(PokerPlayer player)
		{
			for (int i = 0; i < Players.Length; i++)
				if (player == (PokerPlayer)Players[i])
					return i;
			return -1;
		}

		public void KickPlayer(Mobile player)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if ( Players[i] != null && player == ((PokerPlayer)Players[i]).Mobile )
				{
						KickPlayer( (PokerPlayer)Players[i] );
				}
			}
		}

		public void KickPlayer(PokerPlayer player)
		{
			for (int i = 0; i < Players.Length; i++)
				if (player == (PokerPlayer)Players[i])
				{
					if (player.Status >= Status.Playing && m_GameProgress > PokerProgress.None && m_GameProgress < PokerProgress.End)
						ActionTaken(player, PokerAction.Fold);

					if (player.Status == Status.GettingGold)
						player.EndGetGold();

					if (player.Bankroll == 0)
						player.Mobile.SendMessage("Thou'rt leaving the poker table broke.");
					else if (PTActive || m_DealerSetup == DealerSetup.FinalTable)
					{
						player.Mobile.SendMessage("No winnings have been deposited as you left during a tournament.");
						player.Bankroll = 0;
					}
					else
					{
						switch (m_DealerAccept)
						{
							case DealerAccept.GoldOnly:
								Payout(player, player.Bankroll);
								player.Bankroll = 0;
								player.Mobile.PlaySound(0x32);
								player.Mobile.SendMessage("Your winnings have been deposited in your bank box.");
								break;
							case DealerAccept.LowRollerTicketsOnly:
								player.Mobile.SendMessage("Your low-roller poker ticket has been placed in your backpack.");
								player.Mobile.Backpack.DropItem(new PokerLowRollerTicket());
								break;
							case DealerAccept.HighRollerTicketsOnly:
								player.Mobile.SendMessage("Your high-roller poker ticket has been placed in your backpack.");
								player.Mobile.Backpack.DropItem(new PokerHighRollerTicket());
								break;
						}
					}
					player.Mobile.MoveToWorld(m_ExitPoint, Map);
					CloseAllGumps(player.Mobile);
					Players[i] = null;
					return;
				}
		}

		public void GiveAward(int top)
		{
			int x = 0;
			AwardList.Reverse();
			for (int i = 0; i < top; i++)
			{
				Mobile mobile = (Mobile)AwardList[i];
				if (mobile != null)
				{
					int payout = GetPayOutAmount(x);
					x++;
					foreach (NetState state in NetState.Instances)
					{
						Mobile from = state.Mobile;
						if (from != null && m_msg && payout > 0)
							from.SendMessage(0x482, String.Format("{0} has won {1:0,0}gp by finishing {2}{3}.", mobile.Name, (double)payout, x, x > 3 ? "th" : x == 1 ? "st" : x == 2 ? "nd" : "rd"));
					}
					while (payout > 1000000)
					{
						mobile.BankBox.DropItem(new BankCheck(1000000));
						payout -= 1000000;
					}
					if (payout > 5000)
						mobile.BankBox.DropItem(new BankCheck(payout));
					else if (payout > 0)
						mobile.BankBox.DropItem(new Gold(payout));
				}
			}
		}

		public int GetPayOutAmount(int number)
		{
			switch (PTAward)
			{
				case Award.None:
					return 0;
				case Award.Top1:
					return m_TotalMoneyAtStart * 100 /100;
				case Award.Top3:
					switch (number)
					{
						case 0:
							return m_TotalMoneyAtStart * 60 / 100;
						case 1:
							return m_TotalMoneyAtStart * 25 / 100;
						case 2:
							return m_TotalMoneyAtStart * 15 / 100;
					}
					return 0;
				case Award.Top5:
					switch (number)
					{
						case 0:
							return m_TotalMoneyAtStart * 50 / 100;
						case 1:
							return m_TotalMoneyAtStart * 25 / 100;
						case 2:
							return m_TotalMoneyAtStart * 15 / 100;
						case 3:
							return m_TotalMoneyAtStart * 7 / 100;
						case 4:
							return m_TotalMoneyAtStart * 3 / 100;
					}
					return 0;
				case Award.Top10:
					switch (number)
					{
						case 0:
							return m_TotalMoneyAtStart * 40 / 100;
						case 1:
							return m_TotalMoneyAtStart * 20 / 100;
						case 2:
							return m_TotalMoneyAtStart * 12 / 100;
						case 3:
							return m_TotalMoneyAtStart * 7 / 100;
						case 4:
							return m_TotalMoneyAtStart * 6 / 100;
						case 5:
							return m_TotalMoneyAtStart * 5 / 100;
						case 6:
							return m_TotalMoneyAtStart * 4 / 100;
						case 7:
							return m_TotalMoneyAtStart * 3 / 100;
						case 8:
							return m_TotalMoneyAtStart * 2 / 100;
						case 9:
							return m_TotalMoneyAtStart * 1 / 100;
					}
					return 0;
			}
			return 0;
		}

		public void Payout(PokerPlayer player, int bankroll)
		{
			if (player.Mobile == null)
				return;

			while (bankroll > 1000000)
			{
				player.Mobile.BankBox.DropItem(new BankCheck(1000000));
				bankroll -= 1000000;
			}
			if (bankroll > 5000)
				player.Mobile.BankBox.DropItem(new BankCheck(bankroll));
			else if (bankroll > 0)
				player.Mobile.BankBox.DropItem(new Gold(bankroll));
		}

		public void ConfirmedPayment(PokerPlayer player, int gold)
		{
			if (player != null)
			{
				player.EndGetGold();
				player.Bankroll = gold;
				player.Status = Status.WaitingForRound;
			}
			RefreshGump();
			if (ReadyPlayers.Length >= 2 && m_GameProgress == PokerProgress.None)
				PrepareForRound();
		}

		public void CloseAllGumps(Mobile from)
		{
			from.CloseGump(typeof(JoinPokerGump));
			from.CloseGump(typeof(GetGoldGump));
			from.CloseGump(typeof(LeavePokerGump));
			from.CloseGump(typeof(BettingGump));
			from.CloseGump(typeof(GamePokerGump));
		}

		public void StartGettingGold(PokerPlayer player)
		{
		}

		public void RefreshGump()
		{
			RefreshGump(null);
		}

		public void RefreshGump(Mobile from)
		{
			if (from == null)
			{
				foreach (PokerPlayer player in Players)
					if (player != null)
						RefreshGump(player.Mobile);
			}
			else
				RefreshMainGump(from);
		}

		public void RefreshMainGump(Mobile from)
		{
			NetState ns = from.NetState;
			if (ns == null)
			{
				from.CloseGump(typeof(GamePokerGump));
				from.SendGump(new GamePokerGump(this, from));
			}
			else
			{
				GumpCollection gumps = ns.Gumps;
				for (int i = 0; i < gumps.Count; ++i)
					if (gumps[i] is GamePokerGump)
						from.CloseGump(typeof(GamePokerGump));
				from.CloseGump(typeof(GamePokerGump));
				from.SendGump(new GamePokerGump(this, from));
			}
		}

		public PokerPlayer GetPlayer(Mobile m)
		{
			foreach (PokerPlayer player in Players)
				if (player != null && player.Mobile == m)
					return player;
			return null;
		}

		#region Mainloop
		public void PrepareForRound()
		{
			m_GameProgress = PokerProgress.None;

			if (m_PrepareTimer != null)
			{
				m_PrepareTimer.Stop();
				m_PrepareTimer = null;
			}

			Deck.Shuffle();
			CommunityCards.Clear();
			m_PotWorth = 0;
			m_MovesSinceDeal = 0;
			m_HighestBet = 0;

			int count = 0;
			foreach (PokerPlayer player in PlayersWithStatus(Status.AllIn))
			{
				if (player != null && player.Bankroll == 0)
					count++;
			}

			while (m_Award > Award.None && PTActive && count > 0 && ReadyPlayers.Length > 0)
			{
				int lowestallin = 0;
				PokerPlayer allinplayer = null;
				foreach (PokerPlayer player in Players)
				{
					if (player != null)
						if (player.Bankroll == 0 && player.Status == Status.AllIn)
							if (lowestallin == 0)
							{
								allinplayer = player;
								lowestallin = player.TotalBet;
							}
							else if (player.TotalBet < lowestallin)
							{
								allinplayer = player;
								lowestallin = player.TotalBet;
							}
				}
				if (allinplayer != null)
				{
					AwardList.Add(allinplayer.Mobile);
					allinplayer.Status = Status.WaitingForRound;
					count--;
				}
			}

			foreach (PokerPlayer player in Players)
			{
				if (player != null)
				{
					player.RoundBet = 0;
					player.TotalBet = 0;
					player.Result = null;
					player.AutoFold = false;
					player.AutoCall = false;
					player.Hand.Clear();
					player.Mobile.CloseGump(typeof(BettingGump));
				}
			}

			for (int i = 0; i < ReadyPlayers.Length; i++)
			{
				PokerPlayer player = (PokerPlayer)ReadyPlayers[i];
				if (player.Bankroll <= 0)
				{
					if (PTActive)
						VerifyQuit(player.Mobile);
					else
					{
						player.Status = Status.GettingGold;
						player.BeginGetGold(this);
						player.Mobile.SendGump(new GetGoldGump(this, player));
					}
					i--;
				}
				else if (player.Quitting)
				{
					VerifyQuit(player.Mobile);
					i--;
				}
			}

			if (m_DealerSetup > DealerSetup.Regular && ReadyPlayers.Length > 0 && PTActive)
			{
				while (m_DealerSetup == DealerSetup.ForwardTable && ReadyPlayers.Length > 0 && ReadyPlayers.Length <= PTMaxForward)
				{
					for (int i = 0; i < Players.Length; i++)
					{
						PokerPlayer player = (PokerPlayer)Players[i];
						if (player != null)
						{
							int bankroll = player.Bankroll;
							Mobile mobile = player.Mobile;
							CloseAllGumps(mobile);
							Players[i] = null;
							if (m_PTNextDealer == null)
							{
								mobile.CantWalk = false;
								mobile.Backpack.DropItem(new PokerTournyTicket(bankroll, mobile));
								player.Mobile.MoveToWorld(m_WinnerPoint, Map);
								player.Mobile.SendMessage("Thou'rt being transferred to the winners area, wait for further instructions.");
							}
							else
							{
								m_PTNextDealer.VerifyJoin(mobile, bankroll, false, false);
								player.Mobile.SendMessage("Thou have been transferred to the next table, the game will start when the table is full.");
							}
							break;
						}
					}
				}

				if ((m_DealerSetup == DealerSetup.FinalTable || m_DealerSetup == DealerSetup.SingleLoop) && ReadyPlayers.Length == 1)
				{
					PokerPlayer player = (PokerPlayer)ReadyPlayers[0];
					AwardList.Add(player.Mobile);
					foreach (NetState state in NetState.Instances)
					{
						Mobile from = state.Mobile;
						if (from != null && m_msg)
						{
							from.SendMessage(0x482, String.Format("Poker tournament announcement!"));
							from.SendMessage(0x482, String.Format("{0} has won the poker tournament,", player.Mobile.Name));
							if (m_TournyReward != null)
								from.SendMessage(0x482, String.Format(" with \"{0}\" as reward", m_TournyReward.Name));
						}
					}

					if (m_TournyReward != null)
					{
						player.Mobile.SendMessage("You recieved a poker tournament reward in your bank box.");
						Item item = m_TournyReward;
						player.Mobile.BankBox.DropItem(item);
						item.Movable = true;
						m_TournyReward = null;
					}

					switch (PTticketReward)
					{
						case PokerReward.None:
							break;
						case PokerReward.PokerLowRollerTicket:
							player.Mobile.SendMessage("You recieved a low-roller poker entry ticket your bank box.");
							player.Mobile.BankBox.DropItem(new PokerLowRollerTicket());
							break;
						case PokerReward.PokerHighRollerTicket:
							player.Mobile.SendMessage("You recieved a high-roller poker entry ticket your bank box.");
							player.Mobile.BankBox.DropItem(new PokerHighRollerTicket());
							break;
					}

					if(AwardList.Count > 0)
						GiveAward(PTAward == Award.None ? 0 : PTAward == Award.Top1 ? 1 : PTAward == Award.Top3 ? 3 : PTAward == Award.Top5 ? 5 : 10);

					player.Mobile.MoveToWorld(m_WinnerPoint, Map);
					CloseAllGumps(player.Mobile);
					player.Mobile.CantWalk = false;
					int i = GetIndex(player);
					if (i >= 0)
						Players[i] = null;
				}
			}

			if (DealerMode == DealerSetup.SingleLoop && ReadyPlayers.Length == 0)
			{
				m_RoundRaise = 0;
				BlindSmall = m_memblind;
				PTActive = false;
			}

			if (PTActive)
			{
				m_RoundRaise++;
				if (m_RoundRaise > ReadyPlayers.Length)
				{
					m_RoundRaise = 0;
					BlindSmall += PTRaiseSmall;
				}
			}

			RefreshGump();

			if (ReadyPlayers.Length >= 2 && m_PrepareTimer == null)
				m_PrepareTimer = Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(StartRound));
		}

		public void StartRound()
		{
			m_GameProgress = PokerProgress.PreFlop;

			if (m_PrepareTimer != null)
			{
				m_PrepareTimer.Stop();
				m_PrepareTimer = null;
			}

			if (ReadyPlayers.Length < 2)
				return;

			foreach (PokerPlayer player in ReadyPlayers)
			{
				player.Status = Status.Playing;
				player.Hand.AddCard(Deck.GetCard());
				player.Hand.AddCard(Deck.GetCard());
			}

			m_Dealer = NextValidIndex(m_Dealer, Status.Playing);
			m_PlayerTurn = NextValidIndex(m_Dealer, Status.Playing);
			Raise(Players[m_PlayerTurn], BlindSmall, true);
			m_PlayerTurn = NextValidIndex(m_PlayerTurn, Status.Playing);
			Raise(Players[m_PlayerTurn], BlindSmall, true);
			CalculateAction();
		}

		public void ProgressGame()
		{
			if (m_PrepareTimer != null)
			{
				m_PrepareTimer.Stop();
				m_PrepareTimer = null;
			}

			foreach (PokerPlayer player in ReadyPlayers)
				player.RoundBet = 0;

			m_GameProgress++;
			m_MovesSinceDeal = 0;
			m_HighestBet = 0;

			switch (GameProgress)
			{
				//case PokerProgress.PreFlop:
				//	{
				//		break;
				//	}
				case PokerProgress.Flop:
					{
						string msg = "";
						for (int i = 0; i < 3; i++)
						{
							Card card = Deck.GetCard();
							CommunityCards.AddCard(card);
							msg += card;
							if (i == 0)
								msg += ", ";
							if (i == 1)
								msg += " and ";
						}
						if (ActivePlayers.Length > 1)
							saymsg(String.Format("The flop shows: {0}", msg), this, true);
						break;
					}
				case PokerProgress.Turn:
					{
						Card card = Deck.GetCard();
						CommunityCards.AddCard(card);
						if (ActivePlayers.Length > 1)
							saymsg(String.Format("On the turn: {0}", card), this, true);
						break;
					}
				case PokerProgress.River:
					{
						Card card = Deck.GetCard();
						CommunityCards.AddCard(card);
						if (ActivePlayers.Length > 1)
							saymsg(String.Format("And the river: {0}", card), this, true);
						break;
					}
				case PokerProgress.End:
					{
						EndWinners.Clear();
						break;
					}
			}
			m_PlayerTurn = m_Dealer;
			CalculateAction();
		}

		public void CalculateWinner()
		{
			m_RoundsFinished++;
			if (m_Stone != null)
			{
				int tojackpot = (m_PotWorth * DrainToJackpot) / 100;
				m_TotalGoldToJackpot += tojackpot;
				m_Stone.Jackpot += tojackpot;
				m_PotWorth -= tojackpot;
			}
			int todrain = (m_PotWorth * DrainFromPot) / 100;
			m_TotalGoldDrained += todrain;
			m_PotWorth -= todrain;

			int MaxTotalBet = 0;
			foreach (PokerPlayer player in Players)
			{
				if (player != null)
					if (player.TotalBet > MaxTotalBet)
						MaxTotalBet = player.TotalBet;
			}

			foreach (PokerPlayer player in Players)
			{
				if (player != null)
				{
					if (player.TotalBet == MaxTotalBet)
						player.CanWin = m_PotWorth;
					else
						player.CanWin = player.TotalBet * ActivePlayers.Length;
				}
			}

			if (ActivePlayers.Length == 1)
			{
				PokerPlayer player = ActivePlayers[0];
				player.Bankroll += m_PotWorth;
				saymsg(String.Format("{0} won {1:0,0}gp.", player.Mobile.Name, (double)m_PotWorth), this, true);
				if (m_Stone != null)
				{
					player.Result = ScoreHand(player.Hand, CommunityCards);
					m_Stone.CompareHand(player.Mobile, player.Result.HandID, IsHighRoller, player.Result.ComboName);
				}
			}
			else if (ActivePlayers.Length > 1)
			{
				PokerPlayer[] completers = ActivePlayers;
				foreach (PokerPlayer player in completers)
					if (player != null)
					{
						player.Result = ScoreHand(player.Hand, CommunityCards);
						saymsg(String.Format("I have {0}", player.Result.ComboName), player.Mobile, false);
						if (m_Stone != null)
							m_Stone.CompareHand(player.Mobile, player.Result.HandID, IsHighRoller, player.Result.ComboName);
					}

				Array.Sort(completers, new PokerWinnerComparer());

				int w = m_PotWorth;
				int count1 = 0;
				do
				{
					int count2 = 0;
					ArrayList paylist = new ArrayList();
					foreach (PokerPlayer player in completers)
					{
						if ((paylist.Count == 0 || player.Result.HandID == ((PokerPlayer)paylist[0]).Result.HandID) && count2 == count1)
						{
							paylist.Add(player);
							count1++;
						}
						count2++;
					}
					w = DistributeOver(paylist, w, EndWinners);
				} while (w > 0);

				foreach (WinnerEntry entry in EndWinners)
				{
					saymsg(String.Format("{0} won {1:0,0}gp with {2}.", entry.m_Player.Mobile.Name, (double)entry.m_Amount, entry.m_Player.Result.ComboName), this, true);
				}
			}
			RefreshGump();
			m_PrepareTimer = Timer.DelayCall(TimeSpan.FromSeconds(7.5), new TimerCallback(PrepareForRound));
		}
		#endregion

		#region Betting
		public void SendBettingGump(PokerPlayer player, bool gump)
		{
			if (GameProgress > PokerProgress.None && GameProgress < PokerProgress.End)
				if (player.AutoFold)
					ActionTaken(player, PokerAction.Fold);
				else
					player.Mobile.SendGump(new BettingGump(this, player, gump));
		}

		public void ActionTaken(PokerPlayer player, PokerAction action)
		{
			ActionTaken(player, action, 0);
		}

		public void ActionTaken(PokerPlayer player, PokerAction action, bool forced)
		{
			ActionTaken(player, action, 0, forced);
		}

		public void ActionTaken(PokerPlayer player, PokerAction action, int val)
		{
			ActionTaken(player, action, val, false);
		}

		public void ActionTaken(PokerPlayer player, PokerAction action, int val, bool forced)
		{
			if (!forced)
				player.EndAction();

			if (GameProgress == PokerProgress.None || GameProgress == PokerProgress.End || Players[m_PlayerTurn] != player && !forced)
				return;

			bool done = false;
			switch (action)
			{
				case PokerAction.Check:
					{
						done = Check(player);
						break;
					}
				case PokerAction.Fold:
					{
						done = Fold(player);
						break;
					}
				case PokerAction.Raise:
					{
						done = Raise(player, val);
						break;
					}
				case PokerAction.Call:
					{
						done = Call(player);
						break;
					}
				default:
					{
						player.Mobile.SendMessage(33, "Invalid action.");
						SendBettingGump(Players[m_PlayerTurn], false);
						return;
					}
			}
			if (done)
				CalculateAction();
		}

		private bool AllBetsEqual()
		{
			foreach (PokerPlayer player in ActivePlayers)
				if (player.Status == Status.Playing)
					if (player.RoundBet != m_HighestBet)
						return false;
			return true;
		}

		public bool Check(PokerPlayer player)
		{
			m_MovesSinceDeal++;
			saymsg(String.Format("Check."), player.Mobile, false);
			return true;
		}

		public bool Call(PokerPlayer player)
		{
			int amount = m_HighestBet - player.RoundBet;
			if (player.Bankroll <= amount)
				AllIn(player, true, false);
			else
			{
				m_MovesSinceDeal++;
				saymsg(String.Format("I call."), player.Mobile, false);
				Bet(player, amount);
			}
			return true;
		}

		public bool Fold(PokerPlayer player)
		{
			saymsg(String.Format("I fold."), player.Mobile, false);
			player.Status = Status.WaitingForRound;
			player.RoundBet = 0;
			return true;
		}

		public bool Raise(PokerPlayer player, int amount)
		{
			return Raise(player, amount, false);
		}

		public bool Raise(PokerPlayer player, int amount, bool isblind)
		{
			int realamount = m_HighestBet - player.RoundBet + amount;
			if (player.Bankroll <= realamount)
				AllIn(player, false, isblind);
			else
			{
				m_MovesSinceDeal++;
				if (isblind)
					saymsg(String.Format("I post a blind of {0:0,0}.", (double)realamount), player.Mobile, false);
				else
					if (HighestBet == 0)
						saymsg(String.Format("I bet {0:0,0}.", (double)amount), player.Mobile, false);
					else
						saymsg(String.Format("I raise by {0:0,0}.", (double)amount), player.Mobile, false);
				Bet(player, realamount);
			}
			return true;
		}

		public void AllIn(PokerPlayer player, bool call, bool isblind)
		{
			player.Status = Status.AllIn;
			if (isblind)
				if (m_HighestBet == 0)
					m_HighestBet = BlindSmall;
				else
					m_HighestBet += BlindSmall;

			if (call)
				saymsg(String.Format("I call all-in!"), player.Mobile, false);
			else
				saymsg(String.Format("All-in for {0:0,0}!", (double)player.Bankroll), player.Mobile, false);
			Bet(player, player.Bankroll);
		}

		public void Bet(PokerPlayer player, int amount)
		{
			player.RoundBet += amount;
			player.TotalBet += amount;
			player.Bankroll -= amount;
			if (player.RoundBet > m_HighestBet)
				m_HighestBet = player.RoundBet;
			m_PotWorth += amount;
		}
		#endregion

		#region Loop sub scripts
		private int NextValidIndex(int index, Status comparestatus)
		{
			for (int i = index + 1; i < Players.Length; i++)
				if (Players[i] != null && Players[i].Status == comparestatus)
					return i;
			for (int i = 0; i < index; i++)
				if (Players[i] != null && Players[i].Status == comparestatus)
					return i;
			return index;
		}

		private void CalculateAction()
		{
			m_PlayerTurn = NextValidIndex(m_PlayerTurn, Status.Playing);
			RefreshGump();
			if (m_GameProgress < PokerProgress.End)
			{
				if (ActivePlayers.Length == 0)
					PrepareForRound();
				else if (ActivePlayers.Length == 1)
					ProgressGame();
				else if (PlayersWithStatus(Status.Playing).Length == 0)
					m_PrepareTimer = Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(ProgressGame));
				else if (PlayersWithStatus(Status.Playing).Length == 1 && AllBetsEqual())
					m_PrepareTimer = Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(ProgressGame));
				else if (m_GameProgress == PokerProgress.PreFlop && m_MovesSinceDeal >= PlayersWithStatus(Status.Playing).Length + 2 && AllBetsEqual())
					ProgressGame();
				else if (m_GameProgress > PokerProgress.PreFlop && m_MovesSinceDeal >= PlayersWithStatus(Status.Playing).Length && AllBetsEqual())
					ProgressGame();
				else
				{
					Players[m_PlayerTurn].BeginAction(this);
					SendBettingGump(Players[m_PlayerTurn], false);
				}
			}
			else
				CalculateWinner();
		}

		private class PokerWinnerComparer : IComparer
		{
			public int Compare(object a, object b)
			{
				if (!(a is PokerPlayer) || !(b is PokerPlayer))
					return 0;
				PokerPlayer pokerplayera = (PokerPlayer)a;
				PokerPlayer pokerplayerb = (PokerPlayer)b;
				if (pokerplayera.Result.HandID > pokerplayerb.Result.HandID)
					return -1;
				else if (pokerplayera.Result.HandID < pokerplayerb.Result.HandID)
					return 1;
				else
					return 0;
			}
		}

		public static HandResult ScoreHand(CardHand hand, CardHand communitycards)
		{
			CardHand totalcards = new CardHand();
			CardHand handcheck = new CardHand();
			HandResult besthand = null;

			foreach (Card card in hand.Cards)
				totalcards.AddCard(card);
			foreach (Card card in communitycards.Cards)
				totalcards.AddCard(card);

			for (int i = 0; i < totalcards.Cards.Length; i++)
				for (int j = 0; j < totalcards.Cards.Length; j++)
				{
					handcheck.Clear();
					int number = 0;
					for (int k = 0; k < totalcards.Cards.Length; k++)
						if (k != i && k != j && i != j)
						{
							handcheck.AddCard(totalcards.Cards[k]);
							number++;
						}
					if (number > 4)
					{
						int handid = 0;
						int x = 0;
						int count = 0;
						int[] match = new int[5] { 0, 0, 0, 0, 0 };
						int pairs = 0;
						int[] aP = new int[2] { -1, -1 };
						bool flush = false;
						bool acehigh = false;
						bool straight = false;

						handcheck.SortByValue(false);
						if (handcheck.Cards[0].Value - 1 == handcheck.Cards[1].Value && handcheck.Cards[1].Value - 1 == handcheck.Cards[2].Value && handcheck.Cards[2].Value - 1 == handcheck.Cards[3].Value && handcheck.Cards[3].Value - 1 == handcheck.Cards[4].Value)
							straight = true;

						if (handcheck.Cards[0].Color == handcheck.Cards[1].Color && handcheck.Cards[0].Color == handcheck.Cards[2].Color && handcheck.Cards[0].Color == handcheck.Cards[3].Color && handcheck.Cards[0].Color == handcheck.Cards[4].Color)
							flush = true;

						handcheck.SortByValue(true);
						if ((handcheck.Cards[0].Value == 1 ? 14 : handcheck.Cards[0].Value) - 1 == handcheck.Cards[1].Value && handcheck.Cards[1].Value - 1 == handcheck.Cards[2].Value && handcheck.Cards[2].Value - 1 == handcheck.Cards[3].Value && handcheck.Cards[3].Value - 1 == handcheck.Cards[4].Value)
							acehigh = true;

						for (int m = 0; m < 5; m++)
						{
							for (int k = 0; k < 5; k++)
							{
								int temp = handcheck.Cards[k].Value;
								if (handcheck.Cards[m].Value == temp && k != m)
									match[m]++;
							}
							count += match[m];
						}

						for (int k = 0; k < 4; k++)
						{
							if (handcheck.Cards[k].Value == handcheck.Cards[k + 1].Value && (count == 2 || count == 4))
							{
								aP[pairs++] = k;
								k++;
							}
						}

						if (flush && (acehigh || straight))
						{//9 royal, 8 straight flush
							if (acehigh)
								x = handcheck.Cards[4].Value == 10 ? 9 : 8;
							else
							{
								x = 8;
								handcheck.SortByValue(false);
							}
						}
						else if (count == 12)
						{//four of a kind
							x = 7;
							if (handcheck.Cards[0].Value != handcheck.Cards[1].Value)
								handcheck.FlipCards(0, 4);
						}
						else if (count == 8)
						{//full house
							x = 6;
							if (handcheck.Cards[0].Value != handcheck.Cards[2].Value)
							{
								handcheck.FlipCards(0, 3);
								handcheck.FlipCards(1, 4);
							}
						}
						else if (flush)
							x = 5;
						else if (straight || acehigh)
						{
							x = 4;
							if (straight)
								handcheck.SortByValue(false);
						}
						else if (count == 6)
						{//three of a kind
							x = 3;
							if (handcheck.Cards[2].Value == handcheck.Cards[3].Value && handcheck.Cards[3].Value != handcheck.Cards[4].Value)
								handcheck.FlipCards(0, 3);
							if (handcheck.Cards[0].Value != handcheck.Cards[2].Value)
							{
								handcheck.FlipCards(0, 3);
								handcheck.FlipCards(1, 4);
							}
						}
						else if (count == 4)
						{//two pair
							x = 2;
							handcheck.FlipCards(0, aP[0]);
							handcheck.FlipCards(1, aP[0] + 1);
							handcheck.FlipCards(2, aP[1]);
							handcheck.FlipCards(3, aP[1] + 1);
						}
						else if (count == 2)
						{//pair
							x = 1;
							handcheck.FlipCards(0, aP[0]);
							handcheck.FlipCards(1, aP[0] + 1);
							handcheck.SortByValue(3);
						}
						else//high card
							x = 0;
						handid |= x << (4 * 5);
						handid |= (handcheck.Cards[0].Value == 1 ? 14 : handcheck.Cards[0].Value) << (4 * 4);
						handid |= (handcheck.Cards[1].Value == 1 ? 14 : handcheck.Cards[1].Value) << (4 * 3);
						handid |= (handcheck.Cards[2].Value == 1 ? 14 : handcheck.Cards[2].Value) << (4 * 2);
						handid |= (handcheck.Cards[3].Value == 1 ? 14 : handcheck.Cards[3].Value) << (4 * 1);
						handid |= (handcheck.Cards[4].Value == 1 ? 14 : handcheck.Cards[4].Value) << (4 * 0);

						HandResult result = new HandResult(handcheck, handid);
						if (besthand == null || result.HandID > besthand.HandID)
							besthand = result;
					}
				}
			return besthand;
		}

		public void DrawList(Gump g, int x, int y)
		{
			int b = 0;
			g.AddBackground(x, y - 5, 190, 35 + (35 * EndWinners.Count), 83);
			g.AddHtml(x, y + 3, 190, 25, String.Format("<BODY TEXT=\"#330000\"><center>{0}</center></BODY>", EndWinners.Count == 1 ? "Round Winner" : "Round Winners"), false, false);
			foreach (WinnerEntry entry in EndWinners)
			{
				g.AddHtml(x + 10, y + 17 + (35 * b), 140, 25, String.Format("<BODY TEXT='white'><left>{0}</left></BODY>", entry.m_Player.Mobile.Name), false, false);
				g.AddHtml(x + 10, y + 32 + (35 * b), 140, 25, String.Format("<BODY TEXT=\"#FFCC00\"><left>{0:0,0}gp</left></BODY>", (double)entry.m_Amount), false, false);
				if (EndWinners.Count - 1 != b)
					g.AddHtml(x + 9, y + 45 + (35 * b), 173, 25, String.Format("<BODY TEXT='black'><center>-----------------------------------</center></BODY>"), false, false);
				if (entry.m_Player.Result.Hand.Cards != null)
					for (int i = 0; i < entry.m_Player.Result.Hand.Cards.Length; i++)
						entry.m_Player.Result.Hand.Cards[i].Draw(g, x + 100 + (15 * i), y + 12 + (35 * b), true);
				b++;
			}
		}

		private int DistributeOver(ArrayList list, int amount, ArrayList winnerlist)
		{
			if (list.Count == 0 || amount == 0)
			{
				m_TotalGoldDrained += amount;
				return 0;
			}
			else if (list.Count == 1)
			{
				PokerPlayer player = (PokerPlayer)list[0];
				int canwin = player.CanWin;
				if (canwin > amount)
					canwin = amount;
				AddWinnerAmount(winnerlist, player, canwin);
				player.Bankroll += canwin;
				player.CanWin = 0;
				amount -= canwin;
			}
			else
			{
				int MaxWin = amount / list.Count;
				for (int i = 0; i < list.Count; i++)
				{
					PokerPlayer player = (PokerPlayer)list[i];
					if (player != null)
						if (player.Status == Status.AllIn)
						{
							int canwin = player.CanWin;
							if (canwin > MaxWin)
								canwin = MaxWin;
							player.Bankroll += canwin;
							player.CanWin = 0;
							amount -= canwin;
							AddWinnerAmount(winnerlist, player, canwin);
							list.Remove(player);
							i--;
						}
				}

				if (list.Count == 0)
					return amount;

				if (amount > 0)
				{
					MaxWin = amount / list.Count;
					foreach (PokerPlayer player in list)
					{
						int canwin = player.CanWin;
						if (canwin > MaxWin)
							canwin = MaxWin;
						player.Bankroll += canwin;
						player.CanWin = 0;
						amount -= canwin;
						AddWinnerAmount(winnerlist, player, canwin);
					}
					if (amount != 0)
					{
						m_TotalGoldDrained += amount;
						amount = 0;
					}
				}
			}
			return amount;
		}

		private class WinnerEntry
		{
			public PokerPlayer m_Player;
			public int m_Amount;
			public WinnerEntry(PokerPlayer player, int amount)
			{
				m_Player = player;
				m_Amount = amount;
			}
		}

		private void AddWinnerAmount(ArrayList list, PokerPlayer player, int amount)
		{
			foreach (WinnerEntry entry in list)
				if (entry.m_Player == player)
				{
					entry.m_Amount += amount;
					return;
				}
			list.Add(new WinnerEntry(player, amount));
		}
		#endregion

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

			writer.Write(CrashList.Count);
			foreach (CrashEntry entry in CrashList)
			{
				writer.Write(entry.m_Mobile);
				writer.Write(entry.m_Bankroll);
				writer.Write(entry.m_Ticket);
				writer.Write(entry.m_LowRoller);
				writer.Write(entry.m_HighRoller);
			}
			writer.Write(m_AllowSameIP);
			writer.Write(m_BlindSmall);
			writer.Write(m_MaxBuyIn);
			writer.Write(m_MinBuyIn);
			writer.Write((int)m_DealerAccept);
			writer.Write((int)m_DealerSetup);
			writer.Write(m_Open);
			writer.Write(m_DrainFromBuyIn);
			writer.Write(m_DrainFromPot);
			writer.Write(m_DrainToJackpot);
			writer.Write(m_IsHighRoller);
			writer.Write(m_MaxPlayers);
			writer.Write(m_ExitPoint);
			writer.Write(m_WinnerPoint);
			writer.Write(m_Stone);
			writer.Write(m_Active);
			writer.Write((int)m_Award);
			writer.Write(m_msg);
			writer.Write(m_TournyReward);
			writer.Write(m_maxforward);
			writer.Write(m_PTNextDealer);
			writer.Write(m_RaiseSmall);
			writer.Write((int)m_PokerReward);
			for (int i = 0; i < 10; ++i)
				writer.Write(m_Seat[i]);
			writer.Write(m_AutoFoldTimer);
			writer.Write(m_GetGoldTimer);
			writer.Write(m_URL);
			writer.Write(SerialPlayers.Length);
			foreach (PokerPlayer player in SerialPlayers)
			{
				writer.Write(player.Mobile);
				writer.Write(player.Bankroll + player.TotalBet);
			}
			writer.Write(m_memblind);
			writer.Write(m_TotalGoldToJackpot);
			writer.Write(m_TotalGoldDrained);
			writer.Write(m_RoundsFinished);
			writer.Write(m_PlayerAmount);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			Blessed = true;

			switch (version)
			{
				case 0:
					{
						int listlength = reader.ReadInt();
						for (int i = 0; i < listlength; i++)
						{
							Mobile mobile = reader.ReadMobile() as PlayerMobile;
							int bankroll = reader.ReadInt();
							int ticket = reader.ReadInt();
							bool lowroller = reader.ReadBool();
							bool highroller = reader.ReadBool();
							mobile.CantWalk = false;
							CrashList.Add(new CrashEntry(mobile, bankroll, ticket, lowroller, highroller));
						}
						m_AllowSameIP = reader.ReadBool();
						m_BlindSmall = reader.ReadInt();
						m_MaxBuyIn = reader.ReadInt();
						m_MinBuyIn = reader.ReadInt();
						m_DealerAccept = (DealerAccept)reader.ReadInt();
						m_DealerSetup = (DealerSetup)reader.ReadInt();
						m_Open = reader.ReadBool();
						m_DrainFromBuyIn = reader.ReadInt();
						m_DrainFromPot = reader.ReadInt();
						m_DrainToJackpot = reader.ReadInt();
						m_IsHighRoller = reader.ReadBool();
						m_MaxPlayers = reader.ReadInt();
						m_ExitPoint = reader.ReadPoint3D();
						m_WinnerPoint = reader.ReadPoint3D();
						m_Stone = reader.ReadItem() as PokerJackpot;
						m_Active = reader.ReadBool();
						m_Award = (Award)reader.ReadInt();
						m_msg = reader.ReadBool();
						m_TournyReward = reader.ReadItem();
						m_maxforward = reader.ReadInt();
						m_PTNextDealer = reader.ReadMobile() as PokerDealer;
						m_RaiseSmall = reader.ReadInt();
						m_PokerReward = (PokerReward)reader.ReadInt();
						for (int i = 0; i < 10; ++i)
							m_Seat[i] = reader.ReadPoint3D();
						m_AutoFoldTimer = reader.ReadInt();
						m_GetGoldTimer = reader.ReadInt();
						m_URL = reader.ReadString();
						int playerlength = reader.ReadInt();
						for (int i = 0; i < playerlength; i++)
						{
							Mobile mobile = reader.ReadMobile();
							int bankroll = reader.ReadInt();
							mobile.LogoutLocation = m_ExitPoint;
							mobile.CantWalk = false;
							int ticket = bankroll;
							bool lowroller = false;
							bool highroller = false;
							switch (m_DealerAccept)
							{
								case DealerAccept.GoldOnly:
									if (m_Active)
										bankroll = 0;
									else
										ticket = 0;
									break;
								case DealerAccept.LowRollerTicketsOnly:
									if (m_Active)
										bankroll = 0;
									else
									{
										bankroll = 0;
										ticket = 0;
										lowroller = true;
									}
									break;
								case DealerAccept.HighRollerTicketsOnly:
									if (m_Active)
										bankroll = 0;
									else
									{
										bankroll = 0;
										ticket = 0;
										highroller = true;
									}
									break;
							}
							AddCrashEntry(mobile, bankroll, ticket, lowroller, highroller);
						}
						m_memblind = reader.ReadInt();
						m_TotalGoldToJackpot = reader.ReadInt();
						m_TotalGoldDrained = reader.ReadInt();
						m_RoundsFinished = reader.ReadInt();
						m_PlayerAmount = reader.ReadInt();
						break;
					}
			}
			m_Registry.Add(this);

			if (DealerMode == DealerSetup.SingleLoop)
			{
				m_Active = false;
				if (m_memblind > 0)
					BlindSmall = m_memblind;
			}

			//Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( FixCrashed ) );
		}

		public void FixCrashed()
		{
			foreach ( CrashEntry entry in CrashList )
			{
				FindRefundFor( entry.m_Mobile );
				KickPlayer( entry.m_Mobile );
				entry.m_Mobile.CantWalk = false;
			}
		}
	}
}