using System;
using System.Collections;
using System.Text;
using Server.Mobiles;
using Server.Items;
using Server.Misc;

namespace Server.Items
{
	public class BMbombplacer : Item
	{
		private ArrayList m_PlacedBombs = new ArrayList();
		private int m_DetonationDamage;
		private int m_DetonationRange;
		private TimeSpan m_PlacementDelay = new TimeSpan();
		private int m_DetonationDelay;
		private DateTime m_LastBombPlacement = new DateTime();
		private BomberManGame m_Game;
		private int m_MaxNumberOfBombs;
		private int m_CurrentNumberOfBombs;
		private BMPlayerScore m_PlayerStats;

		public BMbombplacer(BomberManGame game, PlayerMobile owner) : base(7194)
		{
			Name = "Bombermans bomb";
			Movable = false;
			m_DetonationDamage = 200; // =kill
			m_DetonationRange = 2;
			m_PlacementDelay = TimeSpan.FromSeconds(4);
			m_LastBombPlacement = DateTime.MinValue;
			m_DetonationDelay = 2;
			m_Game = game;
			Weight = 12;
			m_MaxNumberOfBombs = 1;
			m_CurrentNumberOfBombs = 0;
			m_PlayerStats = new BMPlayerScore(owner);
		}

		public BomberManGame Game
		{
			get { return m_Game; }
		}

		public BMPlayerScore PlayerStats
		{
			get { return m_PlayerStats; }
			set { m_PlayerStats = value; }
		}

		public int MaxNumberOfBombs
		{
			get { return m_MaxNumberOfBombs; }
			set { m_MaxNumberOfBombs = value; }
		}

		public int CurrentPlacedBombs
		{
			get { return m_CurrentNumberOfBombs; }
			set { m_CurrentNumberOfBombs = value; }
		}

		public int DetonationDelay
		{
			get { return m_DetonationDelay; }
			set { m_DetonationDelay = value; }
		}

		public int DetonationDamage
		{
			get { return m_DetonationDamage; }
			set { m_DetonationDamage = value; }
		}

		public int DetonationRange
		{
			get { return m_DetonationRange; }
			set { m_DetonationRange = value; }
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Deleted || !(from is PlayerMobile))
				return;

			// just in case a bombplacer escapes the game arena...
			if (m_Game == null || m_Game.RegionControler == null ||
				m_Game.RegionControler.MyRegion != from.Region)
			{
				this.Delete();
				return;
			}

			if (!m_Game.Running)
			{
				from.SendMessage("The game has not started yet.");
				return;
			}

			if (m_LastBombPlacement + m_PlacementDelay < DateTime.Now ||
				m_CurrentNumberOfBombs < m_MaxNumberOfBombs)
			{
				BMPlacedBomb bomb = new BMPlacedBomb((PlayerMobile)from, this);
				bomb.MoveToWorld(from.Location, from.Map);
				m_LastBombPlacement = DateTime.Now;
				m_CurrentNumberOfBombs++;
				from.SendMessage("You placed a bomb.");
			}
			else
			{
				from.SendMessage("The bomb isnt ready yet.");
			}
		}

		public BMbombplacer(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}

	public class BMPlacedBomb : Item
	{
		private PlayerMobile m_Owner;
		private BMbombplacer m_BombPlacer;

		private bool gonorth = true;
		private bool gosouth = true;
		private bool gowest = true;
		private bool goeast = true;
		private ArrayList destroyedwalls = new ArrayList();
		private Direction explosiondirection;
		private int ticks;
		private bool InExplosionProcess;

		private static string[] m_killwords = new string[]{ "fragged", "killed", "blasted", "liquidated", "dominated", "nuked", "owned", "subdued", "defeated", "crushed" };

		public BMPlacedBomb(PlayerMobile owner, BMbombplacer bp): base (2541) //base(5162)
		{
			if (owner == null || owner.Deleted || bp == null || bp.Deleted)
				return;

			Name = "a ticking bomb";
			Movable = false;
			m_Owner = owner;
			m_BombPlacer = bp;
			Hue = 1175;
			InExplosionProcess = false;

			ticks = 0;
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerCallback(TickCallback));
		}

		public BMbombplacer BombPlacer
		{
			get { return m_BombPlacer; }
		}

		public PlayerMobile Owner
		{
			get { return m_Owner; }
		}

		public void TickCallback()
		{
			Hue = (Hue == 1175) ? m_BombPlacer.Hue : 1175;

			if (ticks++ < (m_BombPlacer.DetonationDelay * 2))
				Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerCallback(TickCallback));
			else if (!InExplosionProcess)
				Detonate();
		}

		public void Detonate()
		{
			if (m_Owner == null || m_Owner.Deleted || this.Deleted ||
				m_BombPlacer == null || m_BombPlacer.Game == null ||
				!m_BombPlacer.Game.Running || this.InExplosionProcess)
				return;

			InExplosionProcess = true;

			Effects.PlaySound(this.Location, this.Map, 0x208);

			// go throught the range
			for (int cnt = 1; cnt <= m_BombPlacer.DetonationRange; cnt++)
			{
				ArrayList spots = new ArrayList();
				spots.Add(new Point3D(this.X, this.Y - cnt, this.Z));
				spots.Add(new Point3D(this.X, this.Y + cnt, this.Z));
				spots.Add(new Point3D(this.X + cnt, this.Y, this.Z));
				spots.Add(new Point3D(this.X - cnt, this.Y, this.Z));

				HitLocation(this.Location);
				foreach (Point3D spot in spots)
				{
					if (!(!gonorth && !gosouth && !gowest && !goeast))
					{


						bool IsInGameRegion = false;
						foreach (Rectangle2D rec in m_BombPlacer.Game.RegionControler.MyRegion.Coords)
						{
							if (rec.Contains(spot))
								IsInGameRegion = true;
						}

						explosiondirection = GetExplosionDirection(spot);
						if ((explosiondirection == Direction.North && !gonorth) ||
							(explosiondirection == Direction.South && !gosouth) ||
							(explosiondirection == Direction.West && !gowest) ||
							(explosiondirection == Direction.East && !goeast) ||
							!IsInGameRegion)
							continue;

						HitLocation(spot);
					}
				}
			}

			foreach (BMwall wall in destroyedwalls)
			{
				m_BombPlacer.PlayerStats.DestroyedWalls++;
				m_BombPlacer.Game.MaySpawnGimmick(wall.Location);
				m_BombPlacer.Game.DestroyWall(wall);
			}
			destroyedwalls.Clear();

			this.Delete();
		}

		public void HitLocation(Point3D spot)
		{
			bool showfs = true;
			ArrayList triggeredBombs = new ArrayList();
			IPooledEnumerable eable;
			eable = Map.GetObjectsInRange(spot, 0);
			foreach (object obj in eable)
			{
				if ((obj is BMwall && m_BombPlacer.Game.Walls.Contains((BMwall)obj)) ||
					(obj is PlayerMobile && m_BombPlacer.Game.Participants.Contains((PlayerMobile)obj)) ||
					(obj is Item && ((Item)obj).ItemID == 1822) ||  // nonbreakable wall
					(obj is BMPlacedBomb && obj != this))
				{
					if (obj is BMwall)
						destroyedwalls.Add((BMwall)obj);
					else if (obj is PlayerMobile && ((PlayerMobile)obj).Alive)
					{
						((PlayerMobile)obj).Damage(m_BombPlacer.DetonationDamage);
						if (!((PlayerMobile)obj).Alive && (PlayerMobile)obj != m_Owner)
						{
							m_BombPlacer.PlayerStats.Kills++;
							m_BombPlacer.Game.GameBroadcast(1275, String.Format("{0} {1} {2}!", m_Owner.Name, m_killwords[Utility.RandomMinMax(0,m_killwords.Length-1)] , ((PlayerMobile)obj).Name));
						}
					}
					else if (obj is Item && ((Item)obj).ItemID == 1822) // nonbreakable wall
						showfs = false;
					else if (m_BombPlacer.Game.LinkBombdetonations && obj is BMPlacedBomb && !((BMPlacedBomb)obj).InExplosionProcess)
						triggeredBombs.Add((BMPlacedBomb)obj);

					if (!(obj is PlayerMobile) || (obj is PlayerMobile && ((PlayerMobile)obj).Alive))
					{
						if (explosiondirection == Direction.North)
						{ gonorth = false; }
						else if (explosiondirection == Direction.South)
						{ gosouth = false; }
						else if (explosiondirection == Direction.East)
						{ goeast = false; }
						else if (explosiondirection == Direction.West)
						{ gowest = false; }

						if (spot == this.Location && !(obj is BMPlacedBomb))
						{
							gonorth = false;
							gosouth = false;
							goeast = false;
							gowest = false;
						}
					}
				}
			}

			if (m_BombPlacer.Game.LinkBombdetonations)
			{
				foreach (BMPlacedBomb bomb in triggeredBombs)
					if (bomb != null && !((BMPlacedBomb)bomb).InExplosionProcess)
						((BMPlacedBomb)bomb).Detonate();
			}

			if (showfs)
			{
				Effects.SendLocationEffect(spot, this.Map, 0x3709, 30, m_BombPlacer.Hue, 0);
				if (m_BombPlacer.Game.DamageWhileFS)
				{
					BMbombOnMoveDamager bombOnMoveDamager = new BMbombOnMoveDamager(this);
					bombOnMoveDamager.MoveToWorld(spot, this.Map);
				}
			}

			eable.Free();
		}


		// returns the explosion direction relative to the placed bomb
		public Direction GetExplosionDirection(Point3D loc)
		{
			int x = loc.X;
			int y = loc.Y;

			if (x == this.X)
			{
				return (y < this.Y) ? Direction.North : Direction.South;
			}
			else if (y == this.Y)
			{
				return (x < this.X) ? Direction.West : Direction.East;
			}

			// invalid
			return Direction.Running;
		}

		public override void OnAfterDelete()
		{
			if (m_BombPlacer != null)
				m_BombPlacer.CurrentPlacedBombs--;
			base.OnAfterDelete();
		}

		public BMPlacedBomb(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}

	public class BMbombOnMoveDamager : Item
	{
		private BMPlacedBomb m_PlacedBomb;

		public BMbombOnMoveDamager(BMPlacedBomb placedBomb) : base(7194)
		{
			if (placedBomb == null)
				return;

			Visible = false;
			m_PlacedBomb = placedBomb;
			Timer.DelayCall(TimeSpan.FromSeconds(1.2), new TimerCallback(DeleteCallback));
		}

		public void DeleteCallback()
		{
			this.Delete();
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m == null || !(m is PlayerMobile))
				return true;

			PlayerMobile from = (PlayerMobile)m;
			if (!m.Alive || !m_PlacedBomb.BombPlacer.Game.Participants.Contains(from))
				return true;

			from.Damage(m_PlacedBomb.BombPlacer.DetonationDamage);
			if (!from.Alive && from != m_PlacedBomb.Owner)
				m_PlacedBomb.BombPlacer.PlayerStats.Kills++;

			return true;
		}

		public BMbombOnMoveDamager(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}
}