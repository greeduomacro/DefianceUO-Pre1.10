using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Items
{
	public class BomberManGame : Item
	{
		private RegionControl m_RegionControl;
		private ArrayList m_Participants = new ArrayList();
		private ArrayList m_StartLocations = new ArrayList();
		private ArrayList m_WallCoordinates = new ArrayList();
		private ArrayList m_Walls = new ArrayList();
		private TimeSpan m_GameTime;
		private bool m_Running;
		private BMGameTimer m_GameTimer;
		private bool m_OpenJoin;
		private Point3D m_ExitLosers;
		private Point3D m_ExitWinner;
		private Point3D m_ExitOnDraw;
		private bool m_LinkBombs;
		private int m_ChanceForUpgrade;
		private bool m_DanageWhileFS;
		private bool m_WriteScoreboard;
		private ArrayList m_GameResults = new ArrayList();
		private bool m_KeepOpenJoin;
		private bool m_AutoStart;

		public BomberManGame(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)4); //version

			writer.Write(m_WallCoordinates.Count);
			for (int i = 0; i < m_WallCoordinates.Count; i++)
				writer.Write((Point3D)m_WallCoordinates[i]);

			writer.Write(m_StartLocations.Count);
			for (int i = 0; i < m_StartLocations.Count; i++)
				writer.Write((Point3D)m_StartLocations[i]);

			writer.Write(m_GameTime);
			writer.Write((Item)m_RegionControl);
			writer.Write(m_ExitLosers);
			writer.Write(m_LinkBombs);
			writer.Write(m_ChanceForUpgrade);
			writer.Write(m_DanageWhileFS);
			writer.Write(m_WriteScoreboard);

			writer.Write(m_ExitWinner);
			writer.Write(m_ExitOnDraw);
			writer.Write(m_AutoStart);
			writer.Write(m_KeepOpenJoin);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			int count;
			count = reader.ReadInt();
			for (int i = 0; i < count; i++)
				m_WallCoordinates.Add(reader.ReadPoint3D());

			count = reader.ReadInt();
			for (int i = 0; i < count; i++)
				m_StartLocations.Add(reader.ReadPoint3D());

			m_GameTime = reader.ReadTimeSpan();
			m_RegionControl = reader.ReadItem() as BMregion;
			m_ExitLosers = reader.ReadPoint3D();

			if (version >= 1)
			{
				m_LinkBombs = reader.ReadBool();
				m_ChanceForUpgrade = reader.ReadInt();

				if (version >= 2)
				{
					m_DanageWhileFS = reader.ReadBool();

					if (version >= 3)
						m_WriteScoreboard = reader.ReadBool();
					{
						if (version >= 4)
						{
							m_ExitWinner = reader.ReadPoint3D();
							m_ExitOnDraw = reader.ReadPoint3D();
							m_AutoStart = reader.ReadBool();
							m_KeepOpenJoin = reader.ReadBool();
						}
					}
				}
			}

			m_Running = false;
			m_OpenJoin = false;
		}

		[Constructable]
		public BomberManGame() : base(0xEDC)
		{
			Name = "Bomber Man Game Stone";
			Movable = false;
			Visible = false;
			m_OpenJoin = false;
			m_Running = false;
			m_LinkBombs = true;
			m_ChanceForUpgrade = 33;
			m_DanageWhileFS = false;
			m_WriteScoreboard = false;
			m_GameTime = TimeSpan.FromMinutes(5.0);
			m_KeepOpenJoin = false;
			m_AutoStart = false;
		}

		public ArrayList Participants
		{
			get { return m_Participants; }
		}

		public ArrayList Walls
		{
			get { return m_Walls; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool DamageWhileFS
		{
			get { return m_DanageWhileFS; }
			set { m_DanageWhileFS = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool OpenJoin
		{
			get { return m_OpenJoin; }
			set { m_OpenJoin = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int ChanceForUpgrade
		{
			get { return m_ChanceForUpgrade; }
			set { m_ChanceForUpgrade = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool LinkBombdetonations
		{
			get { return m_LinkBombs; }
			set { m_LinkBombs = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool WriteScoreboard
		{
			get { return m_WriteScoreboard; }
			set { m_WriteScoreboard = value; }
		}

		public ArrayList WallCoordinates
		{
			get { return m_WallCoordinates; }
			set { m_WallCoordinates = value; }
		}

		public ArrayList StartLocations
		{
			get { return m_StartLocations; }
			set { m_StartLocations = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan RoundTime
		{
			get { return m_GameTime; }
			set { m_GameTime = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D ExitLosers
		{
			get { return m_ExitLosers; }
			set { m_ExitLosers = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D ExitWinner
		{
			get { return m_ExitWinner; }
			set { m_ExitWinner = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D ExitOnDraw
		{
			get { return m_ExitOnDraw; }
			set { m_ExitOnDraw = value; }
		}

		/*
		[CommandProperty(AccessLevel.GameMaster)]
		public uint Rounds
		{
			get { return m_GameRounds; }
			set { m_GameRounds = value; }
		}
		*/

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AutoStart
		{
			get { return m_AutoStart; }
			set { m_AutoStart = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool KeepOpenJoin
		{
			get { return m_KeepOpenJoin; }
			set { m_KeepOpenJoin = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public RegionControl RegionControler
		{
			get { return m_RegionControl; }
			set { m_RegionControl = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Running
		{
			get { return m_Running; }
		}

		public ArrayList LastGameResults
		{
			get { return m_GameResults; }
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Deleted || !(from is PlayerMobile) ||
				from.AccessLevel < AccessLevel.GameMaster )
				return;

			if (m_RegionControl == null || m_RegionControl.MyRegion == null)
			{
				from.SendMessage("Please add a BMregion and define the region first. Then link it via this stones [props.");
				return;
			}

			from.CloseGump(typeof(BMMapSetupGump));
			from.SendGump(new BMStoneGump((PlayerMobile)from, this));
		}

		public uint FreePlayerSlots
		{
			get
			{
				int freeslots = m_StartLocations.Count - m_Participants.Count;
				return ((freeslots >= 0) ? (uint)freeslots : (uint)0);
			}
		}

		public void AddParticipant (PlayerMobile participant)
		{
			if (participant != null && !m_Participants.Contains(participant))
			{
				m_Participants.Add(participant);
				int startloc = Utility.RandomMinMax(0, m_StartLocations.Count-1);
				participant.MoveToWorld((Point3D)m_StartLocations[startloc], this.Map);
				participant.SendMessage("You joined the Bomber Man Game.");

				if (m_AutoStart && FreePlayerSlots == 0)
					this.StartGame(null);

			}
		}

		public bool RemoveParticipant(PlayerMobile participant)
		{
			if (participant != null && m_Participants.Contains(participant))
			{
				m_Participants.Remove(participant);
				return true;
			}
			return false;
		}

		public void DestroyWall(BMwall wall)
		{
			if (wall != null)
			{
				Effects.SendLocationParticles((IEntity)wall, 0x36BD, 20, 10, 5044); // explosion
				Effects.PlaySound(wall.Location, wall.Map, 0x307); // explosion
				m_Walls.Remove(wall);
				wall.Delete();

				if (m_Walls.Count == 0)
					Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(SpawnGimmickCallback));
			}
		}

		public void SpawnGimmick(Point3D loc)
		{
			if (loc == Point3D.Zero)
				return;

			BMBasegimmick gimmick = null;

			int i = Utility.RandomMinMax(1, 2);

			switch (i)
			{
				case 1:
					gimmick = new BMbombbonus(this);
					break;
				case 2:
					gimmick = new BMrangebonus(this);
					break;
			}

			/* Give out all gimmicks
			int i = Utility.RandomMinMax(1, 4);
			switch (i)
			{
				case 1:
					gimmick = new BMfullhits(this);
					break;
				case 2:
					gimmick = new BMrangebonus(this);
					break;
				case 3:
					gimmick = new BMdamagebonus(this);
					break;
				case 4:
					gimmick = new BMbombbonus(this);
					break;
			}
			*/

			if (gimmick != null)
			{
				gimmick.MoveToWorld(loc, this.Map);
			}
		}

		public void SpawnGimmickCallback()
		{
			if (!this.Running)
				return;

			int i = new Random().Next(0, m_WallCoordinates.Count - 1);
			SpawnGimmick((Point3D)m_WallCoordinates[i]);

			Timer.DelayCall(TimeSpan.FromSeconds(3), new TimerCallback(SpawnGimmickCallback));
		}

		public void StartGame(Mobile from)
		{
			if (m_Participants.Count < 2)
			{
				if (from != null)
					from.SendMessage("Minimum of two participants needed to start the game.");
				return;
			}

			if (m_Participants.Count > m_StartLocations.Count)
			{
				if (from != null)
					from.SendMessage("More participants than start locations existing.");
				return;
			}

			if (m_ExitLosers == Point3D.Zero || m_ExitWinner == Point3D.Zero ||m_ExitOnDraw == Point3D.Zero)
			{
				if (from != null)
					from.SendMessage("You have to define all exit locations.");
				return;
			}

			PlaceWalls();
			PreparePlayers();
			m_GameTimer = new BMGameTimer(this);
			m_GameTimer.Start();
			m_Running = true;
			GameBroadcast(34, "The game has started.");
		}

		public void EndGame(PlayerMobile winner)
		{
			if (m_GameTimer != null)
			{
				m_GameTimer.Stop();
				m_GameTimer = null;
			}

			if (!m_KeepOpenJoin)
				m_OpenJoin = false;

			if (m_Running)
			{
				m_GameResults = CreatePlayerStats(winner == null ? true : false);
				ArrayList players = new ArrayList();
				players = m_Participants.Clone() as ArrayList;
				Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerStateCallback(PublishGameResultsCallback), new Object[] { m_GameResults, players });
			}

			ExitPlayers(winner);
			RemovePlacedBombs();
			RemoveWalls();

			m_Running = false;
		}

		public void PublishGameResultsCallback(object o)
		{
			object[] stats = (object[])o;
			ArrayList playerstats = (ArrayList)stats[0] as ArrayList;
			ArrayList players = (ArrayList)stats[1] as ArrayList;

			if (players == null || playerstats == null)
				return;

			foreach (PlayerMobile player in players)
			{
				if (player != null)
					player.SendGump(new BMResultGump(playerstats, false));
			}

			if (m_WriteScoreboard)
			{
				BMScore.AddBMScoreCommunicator(new BMScore.BMScorePusher(playerstats));
			}
		}

		public ArrayList CreatePlayerStats(bool draw)
		{
			ArrayList playerStats = new ArrayList();
			foreach (PlayerMobile player in m_Participants)
			{
				if (player is PlayerMobile && player.Backpack != null )
				{
					Item bompplacer = player.Backpack.FindItemByType(typeof(BMbombplacer));

					if (bompplacer == null)
						continue;

					((BMbombplacer)bompplacer).PlayerStats.Wins = player.Alive && !draw ? (uint)1 : (uint)0;
					((BMbombplacer)bompplacer).PlayerStats.Hue = bompplacer.Hue;
					playerStats.Add(((BMbombplacer)bompplacer).PlayerStats);
				}
			}
			return playerStats;
		}

		public void RemovePlacedBombs()
		{
			if (m_RegionControl == null || m_RegionControl.MyRegion == null ||
				m_RegionControl.MyRegion.Coords.Count == 0)
				return;

			IPooledEnumerable eable = null;
			ArrayList toDelete = new ArrayList();
			foreach (Rectangle2D rec in m_RegionControl.MyRegion.Coords)
				eable = m_RegionControl.MyRegion.Map.GetItemsInBounds(rec);

				foreach (object obj in eable)
				{
					if (obj is BMPlacedBomb)
						//((BMPlacedBomb)obj).Delete();
						toDelete.Add((BMPlacedBomb)obj);
				}
			eable.Free();

			foreach (BMPlacedBomb bomb in toDelete)
				bomb.Delete();
		}

		public void MaySpawnGimmick(Point3D loc)
		{
			if (m_ChanceForUpgrade > Utility.RandomMinMax(1, 100))
				SpawnGimmick(loc);
		}

		public void PlaceWalls()
		{
			RemoveWalls();

			foreach (Point3D loc in m_WallCoordinates)
			{
				BMwall wall = new BMwall();
				m_Walls.Add(wall);
				wall.MoveToWorld(loc, m_RegionControl.MyRegion.Map);
			}
		}

		public void PreparePlayers()
		{
			for (int cnt = 0; cnt <= m_Participants.Count - 1; cnt++)
			{
				PlayerMobile pm = m_Participants[cnt] as PlayerMobile;
				if (pm != null && m_StartLocations[cnt] is Point3D && pm.Region == m_RegionControl.MyRegion)
				{
					pm.MoveToWorld((Point3D)m_StartLocations[cnt], m_RegionControl.MyRegion.Map);
					if (pm.Backpack != null)
					{
						pm.CloseAllGumps();
						BMbombplacer bomb = new BMbombplacer(this, pm);
						pm.AddToBackpack(bomb);
						bomb.Hue = 6 + (cnt * 10);
						pm.HueMod = bomb.Hue;
						pm.BodyMod = 17;
						pm.Frozen = false;
					}
					else
					{
						RemoveParticipant(pm);
					}
				}

			}
		}

		public void ExitPlayers(PlayerMobile winner)
		{
			foreach (PlayerMobile player in m_Participants)
			{
				if (player != null && !player.Deleted)
				{
					if (player.Backpack != null)
						foreach (Item item in player.Backpack.FindItemsByType(typeof(BMbombplacer)))
							item.Delete();

					player.Kill();

					if (player.Region == m_RegionControl.MyRegion)
					{
						if (winner == null)
							player.MoveToWorld(m_ExitOnDraw, m_RegionControl.MyRegion.Map);
						else if (player == winner)
							player.MoveToWorld(m_ExitWinner, m_RegionControl.MyRegion.Map);
						else
							player.MoveToWorld(m_ExitLosers, m_RegionControl.MyRegion.Map);
					}

					player.Resurrect();
					player.Hits = player.HitsMax;
					player.Stam = player.StamMax;
					player.Mana = player.ManaMax;
				}
			}
			m_Participants.Clear();
		}

		public void RemoveWalls()
		{
			foreach (BMwall wall in m_Walls)
			{
				if (wall == null || !(wall is BMwall))
					continue;

				wall.Delete();
			}
			m_Walls.Clear();
		}

		public void WhipeWalls()
		{
			RemoveWalls();
			if (m_WallCoordinates != null)
				m_WallCoordinates.Clear();
		}

		public void GameBroadcast(int hue, string message)
		{
			if (message == null)
				return;

			foreach (PlayerMobile pm in m_Participants)
			{
				if (pm == null)
					return;
				pm.SendMessage(hue, message);
			}
		}

		public void GameBroadcast(string message)
		{
			GameBroadcast(1150, message);
		}

		private class BMGameTimer : Timer
		{
			private static TimeSpan StartDelay = TimeSpan.FromSeconds( 1 );
			private static TimeSpan TimerInterval = TimeSpan.FromSeconds(5);
			private BomberManGame m_Game;
			private DateTime StartTime = new DateTime();

			public BMGameTimer( BomberManGame game) : base( StartDelay, TimerInterval )
			{
				m_Game = game;
				Priority = TimerPriority.TwoFiftyMS;
				StartTime = DateTime.Now;
			}

			protected override void OnTick()
			{
				PlayerMobile winner = null;
				uint alive = 0;

				//time up
				if (m_Game.RoundTime != TimeSpan.Zero && StartTime + m_Game.RoundTime < DateTime.Now)
				{
					m_Game.EndGame(null);
				}

				foreach (PlayerMobile pm in m_Game.Participants)
				{
					if (pm.Region != m_Game.RegionControler.MyRegion)
						pm.Kill();

					if (pm.Alive)
					{
						alive++;
						winner = pm;
					}
				}

				if (alive <= 1)
					m_Game.EndGame(winner);
			}
		}
	}
}