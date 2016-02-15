using Server;
using System;
using System.Collections;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using Server.Logging;
using Server.EventPrizeSystem;

namespace Server.Items
{
	public class CTFGame : Item
	{
        private static int m_GameReg = 1;
		private static ArrayList m_Registry = new ArrayList();
		private ArrayList m_Blockers;
		private ArrayList m_Crystals;
		private bool m_IsLeagueGame; //Intentionally not saved, defaults to false.
        private Type m_prize; //Intentionally not saved, defaults to null.
        private static ArrayList ms_allowedPrizes = new ArrayList();
		private bool m_ResPlayers;

		private ArrayList Blockers
		{
			get { if ( m_Blockers == null ) m_Blockers = new ArrayList(); return m_Blockers; }
		}

		//public static ArrayList Registry{ get{ return m_Registry; } }

		public static void Initialize()
		{
			EventSink.PlayerDeath += new PlayerDeathEventHandler( OnPlayerDeath );

			Server.Commands.Register( "endgame", AccessLevel.GameMaster, new CommandEventHandler( EndGame_Command ) );
			Server.Commands.Register( "startgame", AccessLevel.GameMaster, new CommandEventHandler( StartGame_Command ) );
			Server.Commands.Register( "Team", AccessLevel.Player, new CommandEventHandler( TeamMessage_Command ) );
			Server.Commands.Register( "t",  AccessLevel.Player, new CommandEventHandler( TeamMessage_Command ) );

            //Allowed prizes
            ms_allowedPrizes.Add(typeof(BronzePrizeToken));
            ms_allowedPrizes.Add(typeof(SilverPrizeToken));
            //ms_allowedPrizes.Add(typeof(GoldenPrizeToken));
        }

		private static void TeamMessage_Command( CommandEventArgs e )
		{
			string msg = e.ArgString;
			if ( msg == null )
				return;
			msg = msg.Trim();
			if ( msg.Length <= 0 )
				return;

			CTFTeam team = FindTeamFor( e.Mobile );
			if ( team != null )
			{
				msg = String.Format( "Team [{0}]: {1}", e.Mobile.Name, msg );
				for(int m=0;m<team.Members.Count;m++)
					((Mobile)team.Members[m]).SendMessage( msg );
			}
		}

		private static void EndGame_Command( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( EndGame_Target ) );
			e.Mobile.SendMessage( "Target the game control stone to END a game." );
		}

		private static void EndGame_Target( Mobile from, object o )
		{
			if ( o is CTFGame )
			{
				CTFGame game = (CTFGame)o;
				game.EndGame();
				from.SendMessage( "The game has been ended." );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( EndGame_Target ) );
				from.SendMessage( "Target the game stone." );
			}
		}

		private static void StartGame_Command( CommandEventArgs e )
		{
			if ( e.Arguments.Length < 1 )
			{
				e.Mobile.SendMessage( "Usage: startgame <ResetTeams>" );
				e.Mobile.SendMessage( "So, if you want to start the game and force everyone to choose a new team, do [startgame true" );
			}

			string str = e.GetString( 0 ).ToUpper().Trim();
			bool reset;
			if ( str == "YES" || str == "TRUE" || str == "Y" || str == "T" || str == "1" )
				reset = true;
			else
				reset = false;

			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( StartGame_Target ), reset );
			e.Mobile.SendMessage( "Target the game control stone to START a game." );
		}

		private static void StartGame_Target( Mobile from, object o, object state )
		{
			bool reset = state is bool ? (bool)state : false;

			if ( o is CTFGame )
			{
				CTFGame game = (CTFGame)o;
				game.StartGame( reset );
				from.SendMessage( "The game has been started." );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( StartGame_Target ), reset );
				from.SendMessage( "Target the game stone." );
			}
		}

		private static void OnPlayerDeath( PlayerDeathEventArgs e )
		{
			CTFTeam teamvictim = FindTeamFor( e.Mobile );
            CTFTeam teamkiller = null;
            if (e.Mobile.LastKiller != null)
                teamkiller = FindTeamFor(e.Mobile.LastKiller);
            if ( teamvictim != null && teamvictim.Game != null && teamvictim.Game.ResPlayers )
				new DeathTimer( e.Mobile, teamvictim, teamkiller ).Start();
		}

		public static CTFTeam FindTeamFor( Mobile m )
		{
			for(int i=0;i<m_Registry.Count;i++)
			{
				CTFGame game = (CTFGame)m_Registry[i];
				CTFTeam team = game.GetTeam( m );
				if ( team != null )
					return team;
			}

			return null;
		}

		private ArrayList m_Teams;
		private int m_Game;

		private bool m_DeathPoint;
		private bool m_GiveRobe;
		private bool m_Open;
		private bool m_Running;
		private bool m_MsgStaff;
		private int m_MaxScore;
		private TimeSpan m_Length;

		private DateTime m_StartTime;

		private Timer m_GameTimer, m_ScoreTimer;

		[Constructable]
		public CTFGame( int numTeams ) : base( 0xEDC )
		{
			m_ResPlayers = true;
			m_Game = m_GameReg++;
			m_Teams = new ArrayList( numTeams );
			m_Length = TimeSpan.FromHours( 1.0 );
			m_IsLeagueGame = false;

			for(int i=0;i<numTeams;i++)
				m_Teams.Add( new CTFTeam( this, i ) );

			Movable = false;
			Name = "Game Control Stone";

			m_Registry.Add( this );

			m_Running = m_Open = false;
		}

		public CTFGame( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)4 );//version
			RemoveInvalidBlockers();
			RemoveInvalidCrystals();

			writer.Write( (bool)m_ResPlayers );

			if (m_Crystals != null)
			{
				writer.Write(m_Crystals.Count);
				for (int i = 0; i < m_Crystals.Count; i++)
					writer.Write((Item)m_Crystals[i]);
			}
			else
			{
				writer.Write((int)0);
			}

			writer.Write( Blockers.Count );
			for ( int i = 0; i < Blockers.Count; i++ )
				writer.Write( (Item)Blockers[i] );

			writer.Write( m_MaxScore );

			writer.Write( m_Game );
			writer.Write( m_DeathPoint );
			writer.Write( m_GiveRobe );
			writer.Write( m_Open );
			writer.Write( m_MsgStaff );
			writer.Write( m_Length );

			writer.Write( (int)m_Teams.Count );
			for (int i=0;i<m_Teams.Count;i++)
				((CTFTeam)m_Teams[i]).Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			m_IsLeagueGame = false;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 4:
				{
					m_ResPlayers = reader.ReadBool();
					goto case 3;
				}
				case 3:
				{
					int c = reader.ReadInt();
					if (c > 0)
					{
						m_Crystals=new ArrayList();
						for (int i = 0; i < c; i++)
							m_Crystals.Add(reader.ReadItem());
					}
					goto case 2;
				}
				case 2:
				{
					int c = reader.ReadInt();
					for ( int i = 0; i < c; i++ )
						AddBlocker( reader.ReadItem() );
					goto case 1;
				}
				case 1:
				{
					m_MaxScore = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_Game = reader.ReadInt();
					m_DeathPoint = reader.ReadBool();
					m_GiveRobe = reader.ReadBool();
					m_Open = reader.ReadBool();
					m_MsgStaff = reader.ReadBool();
					m_Length = reader.ReadTimeSpan();

					int len = reader.ReadInt();
					m_Teams = new ArrayList( len );
					for (int i=0;i<len;i++)
						m_Teams.Add( new CTFTeam( reader ) );
					break;
				}
			}

			EventSink.PlayerDeath += new PlayerDeathEventHandler( OnPlayerDeath );
			m_Registry.Add( this );

			m_Running = m_Open = false;
			//Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ResetGame ) );
		}

		[CommandProperty( AccessLevel.Counselor )]
		public bool Running { get { return m_Running; } }

		public override void OnAfterDelete()
		{
			for(int t=0;t<m_Teams.Count;t++)
			{
				CTFTeam team = (CTFTeam)m_Teams[t];
				while ( team.Members.Count > 0 )
					LeaveGame( (Mobile)team.Members[0] );
			}

			m_Registry.Remove( this );
			m_Teams.Clear();

			base.OnAfterDelete();
		}

		public void LeaveGame( Mobile m )
		{
			CTFTeam t = GetTeam( m );
			if ( t != null )
				t.RemoveMember( m );

			if ( m.AccessLevel == AccessLevel.Player && !m.Blessed )
			{
				if ( m.Alive )
				{
					m.Kill();
					if ( m.Corpse != null && !m.Corpse.Deleted )
						m.Corpse.Delete();
				}
				m.Resurrect();
			}

			Item robe = m.FindItemOnLayer( Layer.OuterTorso );
			if ( robe is CTFRobe )
				robe.Delete();
			if ( m.Backpack != null )
			{
				Item[] robes = m.Backpack.FindItemsByType( typeof( CTFRobe ) );
				for(int i=0;i<robes.Length;i++)
					robes[i].Delete();
			}

			m.Delta( MobileDelta.Noto );
		}

		public void PlayerMessage( string message, params object[] args )
		{
			PlayerMessage( String.Format( message, args ) );
		}

		public void PlayerMessage( string message )
		{
			message = "Game: " + message;
			for (int i=0;i<m_Teams.Count;i++)
			{
				CTFTeam team = (CTFTeam)m_Teams[i];
				for (int j=0;j<team.Members.Count;j++)
					((Mobile)team.Members[j]).SendMessage( 0x489, message );
			}

			if (m_Crystals != null && m_Crystals.Count > 0)
				for (int i = 0; i < m_Crystals.Count; i++)
					if (m_Crystals[i] != null && m_Crystals[i] is CTFCrystal)
						((CTFCrystal)m_Crystals[i]).Announce(message);

			if ( m_MsgStaff )
				Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Counselor, 0x489, message );
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_Open )
				LabelTo( from, "Time left: {0:00}:{1:00}:{2:00}", (int)(TimeLeft.TotalSeconds/60/60), (int)(TimeLeft.TotalSeconds/60)%60, (int)(TimeLeft.TotalSeconds)%60 );
			base.OnSingleClick( from );
		}

		public int GameNumber{ get{ return m_Game; } }
		public ArrayList Teams{ get{ return m_Teams; } }
		public TimeSpan TimeLeft{ get{ return m_Length - (DateTime.Now-m_StartTime); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ResPlayers{ get{ return m_ResPlayers; } set{ m_ResPlayers = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DeathPoint{ get{ return m_DeathPoint; } set { m_DeathPoint = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool GiveRobe{ get{ return m_GiveRobe; } set{ m_GiveRobe = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool MessageStaff{ get{ return m_MsgStaff; } set{ m_MsgStaff = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Length{ get{ return m_Length; } set{ m_Length = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxScore
		{
			get
			{
				if ( m_MaxScore <= 0 )
					return int.MaxValue;
				else
					return m_MaxScore;
			}
			set{ m_MaxScore = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TeamCount
		{
			get { return m_Teams.Count; }
			set
			{
				if ( value > m_Teams.Count )
				{
					for (int i=m_Teams.Count;i<value;i++)
						m_Teams.Add( new CTFTeam( this, i ) );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool OpenJoin
		{
			get { return m_Open; }
			set { m_Open = value; }
		}

		[CommandProperty(AccessLevel.Seer)]//Avoid GMs messing up with CTF scores.
		public bool IsLeagueGame
		{
			get { return m_IsLeagueGame; }
			set { m_IsLeagueGame = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Type Prize
        {
            get
            {
                return m_prize;
            }
            set
            {
                if (ms_allowedPrizes.Contains(value))
                    m_prize = value;
                else
                    m_prize = null;
            }
        }

		public bool IsInGame( CTFTeam team )
		{
			return m_Teams != null ? m_Teams.Contains( team ) : false;
		}

		public bool IsInGame( Mobile m )
		{
			return GetTeam( m ) != null;
		}

		public CTFTeam GetTeam( int uid )
		{
			if ( m_Teams != null )
			{
				for (int i=0;i<m_Teams.Count;i++)
				{
					CTFTeam team = (CTFTeam)m_Teams[i];
					if ( team.UId == uid )
						return team;
				}
			}
			return null;
		}

		public CTFTeam GetTeam( Mobile m )
		{
			if ( m_Teams != null )
			{
				for (int i=0;i<m_Teams.Count;i++)
				{
					CTFTeam team = (CTFTeam)m_Teams[i];
					if ( team.Members.Contains( m ) )
						return team;
				}
			}
			return null;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
				from.SendGump( new PropertiesGump( from, this ) );
		}

		public void SwitchTeams( Mobile from, CTFTeam team )
		{
			CTFTeam old = GetTeam( from );
			if ( old == team )
				return;
			else if ( old != null )
				old.RemoveMember( from );

			team.AddMember( from );
			from.Delta( MobileDelta.Noto );

			Item robe = from.FindItemOnLayer( Layer.OuterTorso );
			if ( robe is CTFRobe )
			{
				robe.Name = team.Name + " Game Robe";
				robe.Hue = team.Hue;
			}
			else
			{
				if ( robe != null )
				{
					if ( robe.Movable )
						from.AddToBackpack( robe );
					else
						robe.Delete();
				}

				robe = new CTFRobe( team );
				from.EquipItem( robe );
			}
		}

		public int TeamSize
		{
			get
			{
				int count = 0;
				for (int i=0;i<this.Teams.Count;i++)
					count += ((CTFTeam)m_Teams[i]).ActiveMemberCount;

				count /= Teams.Count;
				return count + 5;
			}
		}

        public void GivePrize(PlayerMobile pm)
        {
            if (m_prize == null || pm == null || pm.Deleted || pm.Account == null || !(pm.Account is Server.Accounting.Account)) return;
            try
            {
                object o = Activator.CreateInstance(m_prize);
                Item item = o as Item;
                if (item != null && pm.BankBox != null)
                {
                    pm.BankBox.TryDropItem(pm, item, false);
                    pm.SendMessage("A prize has been added to your bankbox.");
                    GeneralLogging.WriteLine("CTF", "Prize of type {0} created for playermobile '{1}', account '{2}'.", m_prize.Name, pm.Name, ((Server.Accounting.Account)pm.Account).Username);
                }
                else
                {
                    GeneralLogging.WriteLine("CTF", "Error creating prize of type {0}.", m_prize.Name);
                }
            }
            catch
            {
                GeneralLogging.WriteLine("CTF", "Error creating prize of type {0}.", m_prize.Name);
            }
        }

        public void GivePrize(CTFTeam team)
        {
            if (team == null || team.Members.Count == 0) return;
            foreach (PlayerMobile pm in team.Members) GivePrize(pm);
        }



		public void EndGame()
		{
			if ( !m_Running )
				return;

			if ( m_GameTimer != null )
			{
				m_GameTimer.Stop();
				m_GameTimer = null;
			}

			if ( m_ScoreTimer != null )
			{
				m_ScoreTimer.Stop();
				m_ScoreTimer = null;
			}

            ArrayList winners = new ArrayList();
            for (int i=0;i<m_Teams.Count;i++)
			{
				CTFTeam team = (CTFTeam)m_Teams[i];

				for (int j=0; j<team.Members.Count; j++ )
				{
					((Mobile)team.Members[j]).MoveToWorld( team.Home, team.Map );
				}

				if ( team.Flag != null )
					team.Flag.ReturnToHome();

                if (team.Points > 0)
                {
                    if (winners.Count == 0 || team.Points == ((CTFTeam)winners[0]).Points)
                    {
                        winners.Add(team);
                    }
                    else if (team.Points > ((CTFTeam)winners[0]).Points)
                    {
                        winners.Clear();
                        winners.Add(team);
                    }
                }
			}

			if ( winners.Count==1 )
			{
                PlayerMessage("The {2}game is over. The winner is {0} with {1} points!", ((CTFTeam)winners[0]).Name, ((CTFTeam)winners[0]).Points, IsLeagueGame ? "LEAGUE " : "");
                if (m_prize != null) GivePrize((CTFTeam)winners[0]);
			}
            else if (winners.Count > 1)
            {
                string winnerstr = "";
                foreach (CTFTeam t in winners)
                {
                    if (winnerstr != "") winnerstr += " and ";
                    winnerstr += t.Name;
                    if (m_prize != null) GivePrize(t);
                }
                PlayerMessage("The {2}game is over. The winners are {0} with {1} points!", winnerstr, ((CTFTeam)winners[0]).Points, IsLeagueGame ? "LEAGUE " : "");
            }
            else
            {
                PlayerMessage("The {0}game is over.", IsLeagueGame ? "LEAGUE " : "");
            }

			m_Running = false;
			m_IsLeagueGame = false;
            m_prize = null;

			PlaceBlockers();
		}

		public void StartGame( bool resetTeams )
		{
			m_Running = false;

			if ( m_GameTimer != null )
				m_GameTimer.Stop();

			if ( m_ScoreTimer != null )
				m_ScoreTimer.Stop();

			/*if ( resetTeams )
			{
				PlayerMessage( "The game will start in 30 seconds, please select your team." );

				int teamSize = this.TeamSize;
				ArrayList players = new ArrayList();
				for(int i=0;i<m_Teams.Count;i++)
				{
					CTFTeam t = (CTFTeam)m_Teams[i];
					for (int j=0;j<t.Members.Count;j++)
					{
						Mobile m = (Mobile)t.Members[i];
						m.Frozen = true;
						m.SendGump( new GameTeamSelector( this, teamSize ) );
						players.Add( m );
					}

					t.Members.Clear();
				}

				new StartTimer( this, players ).Start();
			}
			else*/
			{
				ResetGame();
			}

			RemoveBlockers();
		}

		private class StartTimer : Timer
		{
			public static TimeSpan StartDelay = TimeSpan.FromMinutes( 0.5 );
			private CTFGame m_Game;
			private ArrayList m_List;

			public StartTimer( CTFGame game, ArrayList list ) : base( StartDelay )
			{
				m_Game = game;
				m_List = list;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				int sm = -1, ns = -1;
				int[] amc = new int[m_Game.m_Teams.Count];
				for(int i=0;i<m_Game.m_Teams.Count;i++)
				{
					amc[i] = ((CTFTeam)m_Game.m_Teams[i]).ActiveMemberCount;
					if ( sm == -1 || amc[i] < amc[sm] )
					{
						ns = sm;
						sm = i;
					}
					else if ( amc[i] < amc[ns] )
					{
						ns = i;
					}
				}

				for (int i=0;i<m_List.Count;i++)
				{
					Mobile m = (Mobile)m_List[i];

					m.Frozen = false;
					m.CloseGump( typeof( GameTeamSelector ) );

					if ( m_Game.GetTeam( m ) == null )
					{
						int t;
						if ( m.NetState == null )
						{
							t = Utility.Random( amc.Length );
						}
						else
						{
							if ( amc[sm] >= amc[ns] )
								t = Utility.Random( amc.Length );
							else
								t = sm;
							amc[t]++;
						}

						CTFTeam team = (CTFTeam)m_Game.m_Teams[t];

						m_Game.SwitchTeams( m, team );
						m.SendMessage( "You have joined team {0}!", team.Name );
					}
				}
				m_Game.ResetGame();
			}
		}

		public void ResetGame()
		{
			PlayerMessage( "The {0}game has started.", IsLeagueGame ? "LEAGUE " : "" );

			m_StartTime = DateTime.Now;

			m_GameTimer = Timer.DelayCall( Length, new TimerCallback( EndGame ) );
			m_Running = true;

			if ( m_ScoreTimer == null )
				m_ScoreTimer = new ScoreTimer( this );
			m_ScoreTimer.Start();

			for (int i=0;i<m_Teams.Count;i++)
			{
				CTFTeam team = (CTFTeam)m_Teams[i];

				team.Points = 0;
				if ( team.Flag != null )
					team.Flag.ReturnToHome();

				for(int j=0;j<team.Members.Count;j++)
				{
					Mobile m = (Mobile)team.Members[j];

					/*m.Kill();
					if ( m.Corpse != null && !m.Corpse.Deleted )
						m.Corpse.Delete();*/

					m.LogoutLocation = team.Home;
					m.Location = team.Home;
					m.Location = team.Home;
					m.Map = team.Map;

					m.Resurrect();

					m.Hits = m.HitsMax;
					m.Mana = m.ManaMax;
					m.Stam = m.StamMax;
				}
			}
		}

		private void RemoveInvalidBlockers()
		{
			for ( int i = 0; i < Blockers.Count; i++ )
			{
				Item blocker = (Item)Blockers[i];
				if ( blocker == null || blocker.Deleted )
				{
					Blockers.Remove( blocker );
					i--;
				}
			}
		}

		private void RemoveInvalidCrystals()
		{
			if (m_Crystals == null) return;
			for (int i = 0; i < m_Crystals.Count; i++)
			{
				Item crystal = (Item)m_Crystals[i];
				if (crystal == null || crystal.Deleted)
				{
					m_Crystals.Remove(crystal);
					i--;
				}
			}
			if (m_Crystals.Count == 0) m_Crystals = null;
		}

		public void PlaceBlockers()
		{
			RemoveInvalidBlockers();
			for ( int i = 0; i < Blockers.Count; i++ )
			{
				//Assumption: Blockers are on the same map as the game stone
				((Item)Blockers[i]).Map = this.Map;
			}
		}

		public void RemoveBlockers()
		{
			RemoveInvalidBlockers();
			for ( int i = 0; i < Blockers.Count; i++ )
			{
				((Item)Blockers[i]).Map = Map.Internal;
			}
		}

		public void AddBlocker( Item blocker )
		{
			Blockers.Add( blocker );
		}

		public void AddCrystal(Item crystal)
		{
			if (!(crystal is CTFCrystal)) return;
			if (m_Crystals == null) m_Crystals = new ArrayList();
			m_Crystals.Add(crystal);
		}

		public CTFTeam RandomTeam()
		{
			CTFTeam lowest_count = null;
			CTFTeam test_team = null;
			if ( m_Teams != null )
			{
				for (int i=0;i<m_Teams.Count;i++)
				{
					test_team = (CTFTeam)m_Teams[i];
					if ( lowest_count == null || test_team.ActiveMemberCount <= lowest_count.ActiveMemberCount )
						lowest_count = test_team;
				}

				CTFTeam randomteam = (CTFTeam)m_Teams[Utility.Random(0,m_Teams.Count-1)];
				if ( randomteam != null && randomteam.ActiveMemberCount <= lowest_count.ActiveMemberCount )
					lowest_count = randomteam;

				return lowest_count;
			}
			return null;
		}

		private class DeathTimer : Timer
		{
			public static TimeSpan DeathDelay =  TimeSpan.FromSeconds( 10 );

			private CTFTeam m_Team;
            private CTFTeam m_KillerTeam;
            private Mobile m_Mob;


			public DeathTimer( Mobile m, CTFTeam t, CTFTeam tkiller ) : base( DeathDelay )
			{
				m_Mob = m;
				m_Team = t;
                m_KillerTeam = tkiller;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Mob.Corpse != null && !m_Mob.Corpse.Deleted )
					m_Mob.Corpse.Delete();

				//Al: For some reason the OnDeath event is triggered twice...
                if ( !m_Mob.Alive )
				{
					m_Mob.Location = m_Team.Home;
					m_Mob.Map = m_Team.Map;
					m_Mob.Resurrect();
					m_Mob.Hits = m_Mob.HitsMax/2;
					m_Mob.Mana = m_Mob.ManaMax;
					m_Mob.Stam = m_Mob.StamMax;

                    if (m_Team.Game.DeathPoint)
                    {
                        if (m_Team.Points > 0)
                            --m_Team.Points;
                        if ((m_KillerTeam != null)&&(m_KillerTeam != m_Team))
                            m_KillerTeam.Points += 5;
                    }
                }
			}
		}

		private class ScoreTimer : Timer
		{
			private CTFGame m_Game;

			public ScoreTimer( CTFGame g ) : base( TimeSpan.FromMinutes( 5.0 ), TimeSpan.FromMinutes( 5.0 ) )
			{
				m_Game = g;
			}

			protected override void OnTick()
			{
				m_Game.PlayerMessage( "Time left: {0:0}:{1:00}:{2:00}  <>  Scores:", (int)(m_Game.TimeLeft.TotalSeconds/60/60), (int)(m_Game.TimeLeft.TotalSeconds/60)%60, (int)(m_Game.TimeLeft.TotalSeconds)%60 );
				for (int i=0;i<m_Game.Teams.Count;i++)
				{
					CTFTeam team = (CTFTeam)m_Game.Teams[i];
					m_Game.PlayerMessage( "Team {0}: {1} points", team.Name, team.Points );
				}
			}
		}
	}
}