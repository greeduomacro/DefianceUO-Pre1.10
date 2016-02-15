/*
 * SQL QUERRY to create the needed database table:
 * create table bombermanscore (serial int primary key, games int default 0, wins int default 0, kills int default 0, walls int default 0, upgrades int default 0);
 *
 * 2006, Dev Minkio
 */

using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using System.Threading;
using Server.Gumps;
using Server.Misc;
using System.Data;
using MySql.Data.MySqlClient;

namespace Server.Items
{
	public class BMScoreboard : Item
	{
		[Constructable]
		public BMScoreboard() : base(0x1e5e)
		{
			Name = "Bomberman score board";
			Movable = false;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Deleted)
				return;

			SendBMScoreBoardGump(from);
		}

		public static void SendBMScoreBoardGump(Mobile from)
		{
			if (from == null || !(from is PlayerMobile) || from.Deleted )
				return;

			if (BMScore.Scores != null)
			{
				if (BMScore.UpdateAvail)
				{
					BMScore.AddBMScoreCommunicator(new BMScore.BMScorePuller((PlayerMobile)from));
					from.SendMessage("Scores are updating. This can take a second...");
				}

				else if (BMScore.Scores != null && BMScore.Scores.Count != 0)
					from.SendGump(new BMscoreboardgump(BMScore.Scores));
			}
			else
				from.SendMessage("The bomberman scores are not available at the moment.");
		}

		public BMScoreboard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); //version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}

namespace Server.Misc
{
	public class BMPlayerScore
	{
		private PlayerMobile m_Owner;
		private uint m_Kills;
		private uint m_DestroyedWalls;
		private uint m_UpgradesTaken;
		private uint m_GamesPlayed;
		private uint m_Wins;
		private int m_Hue;

		public BMPlayerScore(PlayerMobile from)
		{
			if (from == null)
				return;

			m_Owner = from;
			m_Kills = 0;
			m_DestroyedWalls = 0;
			m_UpgradesTaken = 0;
			m_Hue = 0;
			m_Wins = 0;
			m_GamesPlayed = 0;
		}

		public BMPlayerScore(int serial, uint gamesplayed, uint gameswon, uint kills, uint walls, uint upgrades)
		{
			Mobile from = World.FindMobile(serial) as Mobile;
			if (from == null || !(from is PlayerMobile))
				return;

			m_Owner = (PlayerMobile)from;
			m_Kills = kills;
			m_DestroyedWalls = walls;
			m_UpgradesTaken = upgrades;
			m_GamesPlayed = gamesplayed;
			m_Wins = gameswon;
		}

		public PlayerMobile Owner
		{
			get { return m_Owner; }
		}

		public int Hue
		{
			get { return m_Hue; }
			set { m_Hue = value; }
		}

		public uint Kills
		{
			get { return m_Kills; }
			set { m_Kills = value; }
		}

		public uint DestroyedWalls
		{
			get { return m_DestroyedWalls; }
			set { m_DestroyedWalls = value; }
		}

		public uint GimmicksTaken
		{
			get { return m_UpgradesTaken; }
			set { m_UpgradesTaken = value; }
		}

		public uint Wins
		{
			get { return m_Wins; }
			set { m_Wins = value; }
		}

		public uint GamesPlayed
		{
			get { return m_GamesPlayed; }
			set { m_GamesPlayed = value; }
		}
	}

	public class BMScore
	{
		private static ArrayList m_BMScoreComminicators;
		private static ArrayList m_Scores;
		private static bool m_UpdateAvail;

		public static void AddBMScoreCommunicator(BMScoreCommunicator ec)
		{
			if (m_BMScoreComminicators == null)
				m_BMScoreComminicators = new ArrayList();
			m_BMScoreComminicators.Add(ec);
		}

		public static void RemoveBMScoreCommunicator(BMScoreCommunicator ec)
		{
			m_BMScoreComminicators.Remove(ec);
		}

		public class BMScoreCommunicator
		{
			protected PlayerMobile m_playerMobile;
			protected ArrayList m_playerScores;
			protected Thread m_thread;

			public BMScoreCommunicator()
			{
			}

			public BMScoreCommunicator(ArrayList playerScores)
			{
				if (playerScores != null)
					m_playerScores = playerScores;
			}
		}

		// get the scores on shard bootup
		public static void Initialize()
		{
			AddBMScoreCommunicator(new BMScore.BMScorePuller(null));
		}

		public static ArrayList Scores
		{
			get { return m_Scores; }
		}

		public static bool UpdateAvail
		{
			get { return m_UpdateAvail; }
		}

		public class BMScorePuller : BMScoreCommunicator
		{
			IDbConnection con;
			IDbCommand command;
			IDataReader reader;
			private PlayerMobile m_scoreboardviewer;

			public BMScorePuller(PlayerMobile scoreboardviewer) : base()
			{
				m_scoreboardviewer = scoreboardviewer;
				m_thread = new Thread(new ThreadStart(Pull));
				m_thread.Name = "Server.Misc.BMScorePuller";
				m_thread.Priority = ThreadPriority.BelowNormal;
				m_thread.Start();
			}

			public void Pull()
			{
				if (m_Scores == null)
					m_Scores = new ArrayList();
				else
					m_Scores.Clear();

				m_Scores = ReadScoresFromDB();
				m_UpdateAvail = false;
				BMScore.RemoveBMScoreCommunicator(this);

				if (m_scoreboardviewer != null)
				{
					m_scoreboardviewer.CloseGump(typeof(BMscoreboardgump));
					m_scoreboardviewer.SendGump(new BMscoreboardgump(m_Scores));
				}
			}

			// returnes an arraylist of BMPlayerScore
			public ArrayList ReadScoresFromDB()
			{
				ArrayList PlayerScores = new ArrayList();

				try
				{
					con = new MySqlConnection(Server.StaticConfiguration.ModsDatabaseConnectString);
					con.Open();
					command = con.CreateCommand();
					command.CommandText = "SELECT serial, games, wins, kills, walls, upgrades " +
										  "FROM bombermanscore ORDER BY wins DESC, kills DESC, games LIMIT 20";

					reader = command.ExecuteReader();

					while (reader.Read())
					{
						PlayerScores.Add(new BMPlayerScore(reader.GetInt32(0), (uint)reader.GetInt32(1), (uint)reader.GetInt32(2), (uint)reader.GetInt32(3), (uint)reader.GetInt32(4), (uint)reader.GetInt32(5)));
					}
					return PlayerScores;
				}
				catch
				{
					return null;
				}
				finally
				{
					if (con != null)
						con.Close();
				}
			}
		}

		public class BMScorePusher : BMScoreCommunicator
		{
			IDbConnection m_con;
			IDbCommand m_command;
			IDataReader m_reader;

			public BMScorePusher(ArrayList playerScores) : base(playerScores)
			{
				if (playerScores == null)
					return;

				m_thread = new Thread(new ThreadStart(Push));
				m_thread.Name = "Server.Misc.BMScorePusher";
				m_thread.Priority = ThreadPriority.BelowNormal;
				m_thread.Start();
			}

			public void Push()
			{
				if (m_playerScores == null)
					return;

				try
				{
					m_con = new MySqlConnection(Server.StaticConfiguration.ModsDatabaseConnectString);
					m_con.Open();

					foreach (BMPlayerScore ps in m_playerScores)
					{
						if (ps == null || ps.Owner == null || ps.Owner.Deleted)
							continue;

						if (HasScores(ps.Owner))
						{
							m_command = m_con.CreateCommand();
							m_command.CommandText = "UPDATE bombermanscore" +
												  " SET games=games+1," +
												  " wins=wins+?Wins," +
												  " kills=kills+?Kills," +
												  " walls=walls+?Walls," +
												  " upgrades=upgrades+?Upgrades" +
												  " WHERE serial=?Serial";
						}
						else
						{
							m_command = m_con.CreateCommand();
							m_command.CommandText = "INSERT INTO bombermanscore" +
												  " SET serial=?Serial," +
												  " games=1," +
												  " wins=?Wins," +
												  " kills=?Kills," +
												  " walls=?Walls," +
												  " upgrades=?Upgrades";
						}

						IDataParameter serial = (IDataParameter)m_command.CreateParameter();
						serial.ParameterName = "?Serial";
						serial.DbType = DbType.Int32;
						serial.Value = ps.Owner.Serial.Value;
						m_command.Parameters.Add(serial);

						IDataParameter wins = (IDataParameter)m_command.CreateParameter();
						wins.ParameterName = "?Wins";
						wins.DbType = DbType.Int32;
						wins.Value = ps.Wins;
						m_command.Parameters.Add(wins);

						IDataParameter kills = (IDataParameter)m_command.CreateParameter();
						kills.ParameterName = "?Kills";
						kills.DbType = DbType.Int32;
						kills.Value = ps.Kills;
						m_command.Parameters.Add(kills);

						IDataParameter walls = (IDataParameter)m_command.CreateParameter();
						walls.ParameterName = "?Walls";
						walls.DbType = DbType.Int32;
						walls.Value = ps.DestroyedWalls;
						m_command.Parameters.Add(walls);

						IDataParameter upgrades = (IDataParameter)m_command.CreateParameter();
						upgrades.ParameterName = "?Upgrades";
						upgrades.DbType = DbType.Int32;
						upgrades.Value = ps.GimmicksTaken;
						m_command.Parameters.Add(upgrades);

						int RowsAffected = m_command.ExecuteNonQuery();
					}
				}
				catch
				{
				}
				finally
				{
					m_UpdateAvail = true;

					if (m_con != null)
						m_con.Close();
					if (m_command != null)
						m_command.Dispose();

					BMScore.RemoveBMScoreCommunicator(this);
				}
			}

			public bool HasScores(PlayerMobile player)
			{
				try
				{
					m_command = m_con.CreateCommand();
					m_command.CommandText = "SELECT serial FROM bombermanscore WHERE serial=?Serial";

					IDataParameter serial = (IDataParameter)m_command.CreateParameter();
					serial.ParameterName = "?Serial";
					serial.DbType = DbType.Int32;
					serial.Value = player.Serial.Value;
					m_command.Parameters.Add(serial);

					m_reader = m_command.ExecuteReader();
					while (m_reader.Read())
						if (m_reader.GetInt32(0) == player.Serial.Value)
							return true;

				}
				catch
				{
					return false;
				}
				finally
				{
					if (m_reader != null)
						m_reader.Close();
					if (m_command != null)
						m_command.Dispose();
				}
				return false;
			}
		}
	}
}

namespace Server.Scripts.Commands
{
	public class ReloadBomberManScoresCommand
	{
		public static void Initialize()
		{
			Server.Commands.Register("ReloadBomberManScores", AccessLevel.Administrator, new CommandEventHandler(rbms_OnCommand));
			Server.Commands.Register("rbms", AccessLevel.Administrator, new CommandEventHandler(rbms_OnCommand));
		}

		[Usage("ReloadBomberManScores")]
		[Aliases("rbms")]
		[Description("Reloads the bomber man scores from the database.")]
		private static void rbms_OnCommand(CommandEventArgs e)
		{
			if (e.Mobile != null && !e.Mobile.Deleted)
				e.Mobile.SendMessage("Updating bomberman scores...");

			BMScore.AddBMScoreCommunicator(new BMScore.BMScorePuller(null));
		}
	}
}