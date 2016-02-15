using Server.Gumps;
using Server;
using System.IO;
using System;
using System.Collections;
using Server.Mobiles;
using Server.Factions;
using Server.Network;
using Server.Items;
using Server.Regions;

namespace Server.Items
{
	public class CTFScoreBoard : Item
	{
		static Hashtable m_Players;

		public static Hashtable Players
		{
			get
			{
				if ( m_Players == null )
					m_Players = new Hashtable();
				return m_Players;
			}
		}

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );
			Load();
		}

		private static void EventSink_WorldSave( WorldSaveEventArgs e )
		{
			Save();
		}

		public static void Load()
		{
			string idxPath = Path.Combine( "Saves/CTFScore", "CTFScore.idx" );
			string binPath = Path.Combine( "Saves/CTFScore", "CTFScore.bin" );

			if (File.Exists(idxPath) && File.Exists(binPath))
			{
				// Declare and initialize reader objects.
				FileStream idx = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream bin = new FileStream(binPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryReader idxReader = new BinaryReader(idx);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				// Start by reading the number of duels from an index file
				int playerCount = idxReader.ReadInt32();

				for (int i = 0; i < playerCount; ++i)
				{
					CTFPlayer player = new CTFPlayer();
					// Read start-position and length of current order from index file
					long startPos = idxReader.ReadInt64();
					int length = idxReader.ReadInt32();
					// Point the reading stream to the proper position
					binReader.Seek(startPos, SeekOrigin.Begin);

					try
					{

						player.Deserialize( binReader );

						if (binReader.Position != (startPos + length))
							throw new Exception(String.Format("***** Bad serialize on CTFScore[{0}] *****", i));
					}
					catch
					{
						//handle
					}
					if ( player != null && player.Mobile != null )
						Players.Add( player.Mobile, player );
				}
				// Remember to close the streams
				idxReader.Close();
				binReader.Close();
			}

		}

		public static void Save()
		{
			if (!Directory.Exists("Saves/CTFScore/"))
				Directory.CreateDirectory("Saves/CTFScore/");

			string idxPath = Path.Combine( "Saves/CTFScore", "CTFScore.idx" );
			string binPath = Path.Combine( "Saves/CTFScore", "CTFScore.bin" );

			GenericWriter idx = new BinaryFileWriter(idxPath, false);
			GenericWriter bin = new BinaryFileWriter(binPath, true);

			idx.Write( (int)Players.Values.Count );
			foreach ( CTFPlayer player in Players.Values )
			{
				long startPos = bin.Position;
				player.Serialize( bin );
				idx.Write( (long)startPos );
				idx.Write( (int)(bin.Position - startPos) );
			}
			idx.Close();
			bin.Close();
		}

		public static void Captured( Mobile m )
		{
			CTFPlayer player = Players[m] as CTFPlayer;
			if ( player == null )
			{
				player = new CTFPlayer( m );
				Players.Add( m, player );
			}
			player.Captures++;
		}

		public static void Returned( Mobile m )
		{
			CTFPlayer player = Players[m] as CTFPlayer;
			if ( player == null )
			{
				player = new CTFPlayer( m );
				Players.Add( m, player );
			}
			player.Returns++;
		}

		[Constructable]
		public CTFScoreBoard() : base( 0x1e5e )
		{
			this.Movable = false;
			this.Name = "CTF Score Board";
		}

		public CTFScoreBoard( Serial serial ) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					break;
				}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			list.Add( Utility.FixHtml( "CTF Score Board" ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.CloseGump( typeof( CTFScoreBoardGump ) );
			from.SendGump( new CTFScoreBoardGump() );
		}
	}
}

namespace Server.Gumps
{
	public class CTFPlayer
	{
		Mobile m_Mobile;
		int m_Captures;
		int m_Returns;

		public Mobile Mobile { get { return m_Mobile; } }

		public int Captures
		{
			get
			{
				return m_Captures;
			}
			set
			{
				m_Captures = value;
			}
		}

		public int Returns
		{
			get
			{
				return m_Returns;
			}
			set
			{
				m_Returns = value;
			}
		}

		public CTFPlayer()
		{
		}

		public CTFPlayer( Mobile m )
		{
			m_Mobile = m;
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int)0 );
			writer.Write( m_Mobile );
			writer.Write( m_Captures );
			writer.Write( m_Returns );
		}

		public void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();
			m_Mobile = reader.ReadMobile();
			m_Captures = reader.ReadInt();
			m_Returns = reader.ReadInt();

		}
	}

	public class CTFScoreComparer : IComparer
	{
		public int Compare( object a, object b )
		{
			if ( !( a is CTFPlayer ) || !( b is CTFPlayer ) )
				return 0;
			CTFPlayer ctfpa = (CTFPlayer)a;
			CTFPlayer ctfpb = (CTFPlayer)b;
			if ( ctfpa.Captures + ctfpa.Returns > ctfpb.Captures + ctfpb.Returns )
				return -1;
			else if ( ctfpa.Captures + ctfpa.Returns < ctfpb.Captures + ctfpb.Returns )
				return 1;
			else
				return 0;
		}
	}

	public class CTFScoreBoardGump : Gump
	{
		public CTFScoreBoardGump() : base( 20, 30 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(-8, 2, 649, 399, 9380);
			this.AddImage(504, 281, 9004);
			this.AddImage(302, 401, 2094);

			ArrayList players = GetCTFTopList();

			AddHtml( 20, 40, 200, 35, "Rank", false, false );
			AddHtml( 60, 40, 200, 35, "Player", false, false );
			AddHtml( 160, 40, 200, 35, "Guild", false, false );
			AddHtml( 230, 40, 180, 35, "Captures", false, false );
			AddHtml( 280, 40, 180, 35, "Returns", false, false );

			for ( int i = 0; i < players.Count; i++ )
			{
				if ( i >= 20 )
					break;

				Mobile m = ((CTFPlayer)players[i]).Mobile;
				string guildabb = null;

				if ( m.Guild != null )
				{
					guildabb = String.Format( "[{0}]", m.Guild.Abbreviation );
				}

				AddHtml( 20, 60 + i * 15, 200, 35, (i + 1).ToString(), false, false );
				AddHtml( 60, 60 + i * 15, 200, 35, ((CTFPlayer)players[i]).Mobile.Name, false, false );
				if ( guildabb != null )
					AddHtml( 160, 60 + i * 15, 200, 35, guildabb, false, false );
				AddHtml( 230, 60 + i * 15, 180, 35, ((CTFPlayer)players[i]).Captures.ToString(), false, false );
				AddHtml( 280, 60 + i * 15, 180, 35, ((CTFPlayer)players[i]).Returns.ToString(), false, false );
			}
		}

		public static ArrayList GetCTFTopList()
		{
			ArrayList players = new ArrayList();

			foreach ( CTFPlayer player in CTFScoreBoard.Players.Values )
				players.Add( player );

			CTFScoreComparer comparer = new CTFScoreComparer();
			players.Sort( comparer );

			return players;
		}
	}
}