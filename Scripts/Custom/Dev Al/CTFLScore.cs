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

	public class CTFLScoreBoard : Item
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
			string basePath = Path.Combine("Saves", "CTFScore");
			string idxPath = Path.Combine( basePath, "CTFLScore.idx");
			string binPath = Path.Combine( basePath, "CTFLScore.bin");

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
			string basePath = Path.Combine("Saves", "CTFScore");
			if (!Directory.Exists(basePath))
				Directory.CreateDirectory(basePath);

			string idxPath = Path.Combine( basePath, "CTFLScore.idx");
			string binPath = Path.Combine( basePath, "CTFLScore.bin");

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

		public static void Reset()
		{
			m_Players = new Hashtable();
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
		public CTFLScoreBoard() : base( 0x1e5e )
		{
			this.Movable = false;
			this.Name = "CTF League Score Board";
		}

		public CTFLScoreBoard( Serial serial ) : base(serial)
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
			list.Add( Utility.FixHtml( "CTF League Score Board" ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.CloseGump( typeof( CTFLScoreBoardGump ) );
			from.SendGump( new CTFLScoreBoardGump() );
		}
	}
}

namespace Server.Gumps
{
	public class CTFLScoreBoardGump : Gump
	{
		public CTFLScoreBoardGump() : base( 20, 30 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(-8, 2, 649, 399, 9380);
			this.AddImage(504, 281, 9004);
			this.AddImage(302, 401, 2094);

			ArrayList players = GetCTFLTopList();

			AddHtml( 20, 40, 200, 35, "Rank", false, false );
			AddHtml( 60, 40, 200, 35, "Player", false, false );
			AddHtml( 200, 40, 200, 35, "Guild", false, false );
			AddHtml( 270, 40, 180, 35, "Captures", false, false );
			AddHtml( 320, 40, 180, 35, "Returns", false, false );

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
					AddHtml( 200, 60 + i * 15, 200, 35, guildabb, false, false );
				AddHtml( 270, 60 + i * 15, 180, 35, ((CTFPlayer)players[i]).Captures.ToString(), false, false );
				AddHtml( 320, 60 + i * 15, 180, 35, ((CTFPlayer)players[i]).Returns.ToString(), false, false );
			}
		}

		public static ArrayList GetCTFLTopList()
		{
			ArrayList players = new ArrayList();

			foreach ( CTFPlayer player in CTFLScoreBoard.Players.Values )
				players.Add( player );

			CTFScoreComparer comparer = new CTFScoreComparer();
			players.Sort( comparer );

			return players;
		}
	}
}

namespace Server.Scripts.Commands
{
	public class ResetCTFLeagueStats
	{
		public static void Initialize()
		{
			Server.Commands.Register("ResetCTFLeagueStats", AccessLevel.Seer, new CommandEventHandler(ResetCTFLeagueStats_OnCommand));
		}

		[Usage("ResetCTFLeagueStats")]
		[Description("Resets the Capture The Flag League stats.")]
		public static void ResetCTFLeagueStats_OnCommand(CommandEventArgs e)
		{
			CTFLScoreBoard.Reset();
			e.Mobile.SendMessage("The CTF League stats have been reset.");
		}
	}
}