using System;
using System.Collections;
using System.IO;
using Server.Accounting;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.FSPvpPointSystem
{
	public class FSPvpSystem
	{
		public static void Initialize()
		{
			new DecayTimer( DateTime.Now + TimeSpan.FromDays( 14.0 ) ).Start();
			new PvpCleanUpTimer( DateTime.Now + TimeSpan.FromDays( 1.0 ) ).Start();

			EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );
			Load();
		}

		public static ArrayList Winners;
		public static ArrayList Losers;
		public static ArrayList ResKillers;
		public static ArrayList ResKillies;

		public static void CheckTopWinners()
		{
			PvpStats curTop = null;

			if ( Winners == null )
				Winners = new ArrayList();

			if ( Winners.Count >= 20 )
				Winners = new ArrayList();

			if ( PlayerStats.Count < 20 )
				return;

			foreach ( PvpStats ps in PlayerStats.Values )
			{
				if ( curTop == null )
					curTop = ps;

				if ( ps.Wins >= curTop.Wins && !Winners.Contains( ps ) && ps.Owner.AccessLevel == AccessLevel.Player )
					curTop = ps;
			}

			if ( curTop != null )
				Winners.Add( curTop );

			if ( Winners.Count < 20 )
				CheckTopWinners();
		}

		public static void CheckTopLosers()
		{
			PvpStats curTop = null;

			if ( Losers == null )
				Losers = new ArrayList();

			if ( Losers.Count >= 20 )
				Losers = new ArrayList();

			if ( PlayerStats.Count < 20 )
				return;

			foreach ( PvpStats ps in PlayerStats.Values )
			{
				if ( curTop == null )
					curTop = ps;

				if ( ps.Loses >= curTop.Loses && !Losers.Contains( ps ) && ps.Owner.AccessLevel == AccessLevel.Player )
					curTop = ps;
			}

			if ( curTop != null )
				Losers.Add( curTop );

			if ( Losers.Count < 20 )
				CheckTopLosers();
		}

		public static void CheckTopResKillers()
		{
			PvpStats curTop = null;

			if ( ResKillers == null )
				ResKillers = new ArrayList();

			if ( ResKillers.Count >= 20 )
				ResKillers = new ArrayList();

			if ( PlayerStats.Count < 20 )
				return;

			foreach ( PvpStats ps in PlayerStats.Values )
			{
				if ( curTop == null )
					curTop = ps;

				if ( ps.ResKills >= curTop.ResKills && !ResKillers.Contains( ps ) && ps.Owner.AccessLevel == AccessLevel.Player )
					curTop = ps;
			}

			if ( curTop != null )
				ResKillers.Add( curTop );

			if ( ResKillers.Count < 20 )
				CheckTopResKillers();
		}

		public static void CheckTopResKillies()
		{
			PvpStats curTop = null;

			if ( ResKillies == null )
				ResKillies = new ArrayList();

			if ( ResKillies.Count >= 20 )
				ResKillies = new ArrayList();

			if ( PlayerStats.Count < 20 )
				return;

			foreach ( PvpStats ps in PlayerStats.Values )
			{
				if ( curTop == null )
					curTop = ps;

				if ( ps.ResKills >= curTop.ResKills && !ResKillies.Contains( ps ) && ps.Owner.AccessLevel == AccessLevel.Player )
					curTop = ps;
			}

			if ( curTop != null )
				ResKillies.Add( curTop );

			if ( ResKillies.Count < 20 )
				CheckTopResKillies();
		}

		public static Type[] PvpArtifacts = new Type[]
		{
			typeof( Katana ),
			typeof( WarAxe )
		};

		public class PvpCleanUpTimer : Timer
		{
			public PvpCleanUpTimer( DateTime end ) : base( end - DateTime.Now )
			{
			}

			protected override void OnTick()
			{
				DoPvpCleanUp();
				Stop();
			}
		}

		public class DecayTimer : Timer
		{
			public DecayTimer( DateTime end ) : base( end - DateTime.Now )
			{
			}

			protected override void OnTick()
			{
				DoDecayCheck();
				Stop();
			}
		}

		public class ResKillTimer : Timer
		{
			private PvpStats m_PS;

			public ResKillTimer( DateTime end, PvpStats ps ) : base( end - DateTime.Now )
			{
				m_PS = ps;
			}

			protected override void OnTick()
			{
				ToggleNoResKill( m_PS.Owner, false );
				Stop();
			}
		}

		private static Hashtable m_PlayerStats;
		private static Hashtable PlayerStats
		{
			get
			{
				if ( m_PlayerStats == null )
					m_PlayerStats = new Hashtable();

				return m_PlayerStats;
			}
		}

		private static TimeSpan CleanUpTime = TimeSpan.FromDays( 30 );

		public static void DoPvpCleanUp()
		{
			ArrayList toCheck = new ArrayList();
			int inactiveCount = 0;
			int invalidCount = 0;

			foreach ( PvpStats ps in PlayerStats.Values )
				toCheck.Add( ps );

			foreach ( PvpStats ps in toCheck )
			{
				Account acct = (Account)ps.Owner.Account;

				if ( ps.Owner == null )
				{
					PlayerStats.Remove( ps );
					invalidCount += 1;
				}

				if ( acct != null && acct.LastLogin < DateTime.Now - CleanUpTime )
				{
					PlayerStats.Remove( ps.Owner );
					PlayerStats.Remove( ps );
					inactiveCount += 1;
				}
			}

			if ( inactiveCount != 0 )
				Console.WriteLine( "FS-Pvp System: {0} Inactive pvp stats have been deleted.", inactiveCount );

			if ( invalidCount != 0 )
				Console.WriteLine( "FS-Pvp System: {0} Invalid pvp stats have been deleted.", invalidCount );
		}

		public static void DoDecayCheck()
		{
			ArrayList toCheck = new ArrayList( PlayerStats.Values );

			foreach ( PvpStats check in toCheck )
			{
				if ( check.LastKill == DateTime.MinValue )
					check.LastKill = DateTime.Now;
				else if ( check.LastKill == DateTime.MaxValue )
					check.LastKill = DateTime.Now;
				else if ( check.LastKill <= DateTime.Now - TimeSpan.FromDays( 14.0 ) )
				{
					int pointlose = check.Points / 50;
					//int oldpoints = check.Points;
					check.Points -= pointlose;
					//check.DoRankCheck( oldpoints );
					check.Owner.SendMessage( "Due to your lack of kills you have taken a 2% lose in your total pvp points." );
				}
			}
		}

		public class PvpStats
		{
			private Mobile m_Owner;
			private int m_Points;
			private int m_Wins;
			private int m_Loses;
			private int m_ResKills;
			private int m_ResKilled;
			private bool m_ShowPvpTitle;
			private int m_RankType;
			private bool m_NoResKill;
			private DateTime m_LastKill;
			private PvPKillsCollection m_Killed;

			public Mobile Owner{ get{ return m_Owner; } }

			[CommandProperty( AccessLevel.Seer )]
			public int Points{ get{ return m_Points; } set{ m_Points = value; DoRankCheck(); } }
			[CommandProperty( AccessLevel.Seer )]
			public int Wins{ get{ return m_Wins; } set{ m_Wins = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public int Loses{ get{ return m_Loses; } set{ m_Loses = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public int ResKills{ get{ return m_ResKills; } set{ m_ResKills = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public int ResKilled{ get{ return m_ResKilled; } set{ m_ResKilled = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public bool ShowPvpTitle{ get{ return m_ShowPvpTitle; } set{ m_ShowPvpTitle = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public int RankType{ get{ return m_RankType; } set{ m_RankType = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public bool NoResKill{ get{ return m_NoResKill; } set{ m_NoResKill = value; } }
			[CommandProperty( AccessLevel.Seer )]
			public DateTime LastKill{ get{ return m_LastKill; } set{ m_LastKill = value; } }
			public PvPKillsCollection Killed{ get{ return m_Killed; } }

			public PvpStats() : this( null )
			{
			}

			public PvpStats( Mobile mob )
			{
				m_Owner = mob;
				m_RankType = 1;
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int) 2 );

				// version 2
				writer.WriteDeltaTime( m_LastKill );

				// version 1
				writer.Write( (bool) m_NoResKill );

				// version 0
				writer.Write( (Mobile) m_Owner );
				writer.Write( (int) m_Points );
				writer.Write( (int) m_Wins );
				writer.Write( (int) m_Loses );
				writer.Write( (int) m_ResKills );
				writer.Write( (int) m_ResKilled );
				writer.Write( (bool) m_ShowPvpTitle );
				writer.WriteEncodedInt( (int) m_RankType );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 2:
					{
						m_LastKill = reader.ReadDeltaTime();
						goto case 1;
					}
					case 1:
					{
						m_NoResKill = reader.ReadBool();
						goto case 0;
					}
					case 0:
					{
						m_Owner = reader.ReadMobile();
						m_Points = reader.ReadInt();
						m_Wins = reader.ReadInt();
						m_Loses = reader.ReadInt();
						m_ResKills = reader.ReadInt();
						m_ResKilled = reader.ReadInt();
						m_ShowPvpTitle = reader.ReadBool();
						m_RankType = reader.ReadEncodedInt();
						break;
					}
				}

				if ( m_NoResKill == true )
					m_NoResKill = false;
			}

			public void DoRankCheck()
			{
				int rank = m_RankType;
				PvpRankInfo[] ranks = PvpRankInfo.Table;

				for ( int i = 1; i < ranks.Length; i++ )
				{
					if ( m_Points < ranks[i].Required )
					{
						m_RankType = ranks[i].Rank;
						break;
					}
				}

				if ( rank != m_RankType )
					m_Owner.SendGump( new RankPromotion( m_Owner, ranks[m_RankType].Title, m_RankType < rank ) );
			}

			public void AddPoints( Mobile from, int amount )
			{
				Points += amount;

				if ( from != null )
					from.SendMessage( "You have gained {0} points.", amount );
			}

			public void RemovePoints( Mobile from, int amount )
			{
				Points -= amount;

				if ( from != null )
					from.SendMessage( "You have lost {0} points.", amount );

				if ( m_Points < 0 )
					m_Points = 0;
			}

			public void AddWins( int amount )
			{
				m_Wins += amount;
			}

			public void AddLoses( int amount )
			{
				m_Loses += amount;
			}

			public void AddResKills( int amount )
			{
				m_ResKills += amount;
			}

			public void AddResKilled( int amount )
			{
				m_ResKilled += amount;
			}

			public void ToggleTitle( bool toggle )
			{
				m_ShowPvpTitle = toggle;
			}

			public void ToggleNoResKill( bool toggle )
			{
				m_NoResKill = toggle;
			}

			public void OnKilled( Mobile victim )
			{
				if ( m_Killed == null )
					m_Killed = new PvPKillsCollection();

				m_Killed.Add( new PvPKillEntry( victim ) );
			}

			public bool CanGainKill( Mobile mob )
			{
				if ( m_Killed == null )
					return true;

				for ( int i = 0; i < m_Killed.Count; ++i )
				{
					PvPKillEntry entry = m_Killed[i];

					if ( entry.IsExpired )
						m_Killed.RemoveAt( i-- );
					else if ( entry.Killed == mob )
						return false;
				}

				return true;
			}
		}

		private static void EventSink_WorldSave( WorldSaveEventArgs e )
		{
			Save();
		}

		public static void Load()
		{
			string idxPath = Path.Combine( "Saves/FS Systems/FSPvp", "PlayerStats.idx" );
			string binPath = Path.Combine( "Saves/FS Systems/FSPvp", "PlayerStats.bin" );

			if (File.Exists(idxPath) && File.Exists(binPath))
			{
				// Declare and initialize reader objects.
				FileStream idx = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream bin = new FileStream(binPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryReader idxReader = new BinaryReader(idx);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				// Start by reading the number of duels from an index file
				int orderCount = idxReader.ReadInt32();

				for (int i = 0; i < orderCount; ++i)
				{

					PvpStats ps = new PvpStats();
					// Read start-position and length of current order from index file
					long startPos = idxReader.ReadInt64();
					int length = idxReader.ReadInt32();
					// Point the reading stream to the proper position
					binReader.Seek(startPos, SeekOrigin.Begin);

					try
					{
						ps.Deserialize(binReader);

						if (binReader.Position != (startPos + length))
							throw new Exception(String.Format("***** Bad serialize on PvpStats[{0}] *****", i));
					}
					catch
					{
						//handle
					}

					if ( ps != null && ps.Owner != null )
						PlayerStats.Add( ps.Owner, ps );
				}

				// Remember to close the streams
				idxReader.Close();
				binReader.Close();
			}

		}


		public static void Save()
		{
			if (!Directory.Exists("Saves/FS Systems/FSPvp"))
				Directory.CreateDirectory("Saves/FS Systems/FSPvp");

			string idxPath = Path.Combine( "Saves/FS Systems/FSPvp", "PlayerStats.idx" );
			string binPath = Path.Combine( "Saves/FS Systems/FSPvp", "PlayerStats.bin" );


			GenericWriter idx = new BinaryFileWriter(idxPath, false);
			GenericWriter bin = new BinaryFileWriter(binPath, true);

			idx.Write( (int)PlayerStats.Values.Count );
			foreach ( PvpStats ps in PlayerStats.Values )
			{
				long startPos = bin.Position;
				ps.Serialize( bin );
				idx.Write( (long)startPos );
				idx.Write( (int)(bin.Position - startPos) );
			}

			idx.Close();
			bin.Close();
		}

		public static PvpStats GetPvpStats( Mobile m )
		{
			if ( m == null )
				return null;

			PvpStats ps = PlayerStats[m] as PvpStats;
			if ( ps == null )
			{
				ps = new PvpStats( m );
				PlayerStats.Add( m, ps );
			}

			return ps;
		}

		public static void AddPoints( Mobile from, int amount )
		{
			if ( from != null && amount > 0 )
			{
				PvpStats ps = GetPvpStats( from );
				ps.AddPoints( from, amount );
			}
		}

		public static void RemovePoints( Mobile from, int amount )
		{
			if ( from != null && amount > 0 )
			{
				PvpStats ps = GetPvpStats( from );
				ps.RemovePoints( from, amount );
			}
		}

		public static void AddWins( Mobile from, int amount )
		{
			if ( from != null && amount > 0 )
			{
				PvpStats ps = GetPvpStats( from );
				ps.AddWins( amount );
			}
		}

		public static void AddLoses( Mobile from, int amount )
		{
			if ( from != null && amount > 0 )
			{
				PvpStats ps = GetPvpStats( from );
				ps.AddLoses( amount );
			}
		}

		public static void AddResKills( Mobile from, int amount )
		{
			if ( from != null && amount > 0 )
			{
				PvpStats ps = GetPvpStats( from );
				ps.AddResKills( amount );
			}
		}

		public static void AddResKilled( Mobile from, int amount )
		{
			if ( from != null && amount > 0 )
			{
				PvpStats ps = GetPvpStats( from );
				ps.AddResKilled( amount );
			}
		}

		public static void ToggleTitle( Mobile from, bool toggle )
		{
			if ( from != null )
			{
				PvpStats ps = GetPvpStats( from );
				ps.ToggleTitle( toggle );
			}
		}

		public static void ToggleNoResKill( Mobile from, bool toggle )
		{
			if ( from != null )
			{
				PvpStats ps = GetPvpStats( from );
				ps.ToggleNoResKill( toggle );
			}
		}

		public static void GiveReward( Mobile winner )
		{
			int chance = Utility.Random( 10000 );

			if ( chance <= 25 )
			{
				Item item = (Item)Activator.CreateInstance( PvpArtifacts[Utility.Random(PvpArtifacts.Length)] );

				if ( winner.AddToBackpack( item ) )
					winner.SendMessage( "You have been rewarded for slaying your foe." );
				else
					item.Delete();
			}
		}

		public static void PvpDeathCheck( Mobile loser )
		{
			if ( loser is PlayerMobile )
			{
				PvpStats ps = GetPvpStats( loser );

				ArrayList toGive = new ArrayList();
				ArrayList rights = loser.DamageEntries;
				bool isPlayer = false;

				foreach ( DamageEntry entry in rights )
				{
					if ( entry.Damager is PlayerMobile )
					{
						toGive.Add( entry.Damager );
						entry.LastDamage -= TimeSpan.FromMinutes( 2.0 );
						isPlayer = true;
					}
				}

				if ( toGive.Count == 0 )
					return;

				if ( isPlayer )
					AddLoses( loser, 1 );

				if ( toGive.Contains( loser ) )
					toGive.Remove( loser );

				foreach ( Mobile m in toGive )
				{
					PvpStats winPS = GetPvpStats( m );
					//int oldPoints = winPS.Points;

					if ( m is PlayerMobile && m != loser )
					{
						if ( ps.NoResKill )
						{
							RemovePoints( m, Utility.RandomMinMax( 5, 10 ) );
							AddResKills( m, 1 );
							winPS.OnKilled( loser );
							m.SendMessage( "You have lost points due to your res kill of {0}.", loser.Name );
						}
						else if ( !winPS.CanGainKill( loser ) )
							m.SendMessage( "You cannot gain anything from the recently killed {0}.", loser.Name );
						else
						{
							int points = 10 / toGive.Count;

							if ( points < 1 )
								points = 1;

							if ( PvpRankInfo.GetInfo( ps.RankType ).Rank < PvpRankInfo.GetInfo( winPS.RankType ).Rank - 5 )
								m.SendMessage( "You cannot gain points of someone five or more ranks below you." );
							else
							{
								AddPoints( m, points );
								AddWins( m, 1 );
								GiveReward( m );
							}
							winPS.OnKilled( loser );
						}
					}
				}

				if ( ps.NoResKill )
					AddResKilled( loser, 1 );
				else
				{
					TimeSpan reskilltime = TimeSpan.FromMinutes( 2.0 );
					DateTime newTime = DateTime.Now + reskilltime;
					Timer resTimer = null;
					resTimer = new ResKillTimer( newTime, ps );
					resTimer.Start();

					ToggleNoResKill( loser, true );
				}
			}
		}
	}
}