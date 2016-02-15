using System;
using Server;
using Server.Mobiles;



using System.IO;
using System.Collections;

using Server.Items;

namespace Server.Items
{
	public class MagicBrazier : Item
	{
		private static Type[] m_Types = new Type[]
			{
				typeof( AirElemental ),	typeof( BloodElemental ), typeof( FireElemental ),
				typeof( PoisonElemental ), typeof( WaterElemental ), typeof( EarthElemental ),
				typeof( Balron ), typeof( BoneMagi ), typeof( Daemon ), typeof( ElderGazer ),
				typeof( Gargoyle ),	typeof( Gazer ),typeof( Lich ), typeof ( LichLord ),
				typeof( OrcishMage ), typeof( Shade ), typeof( SkeletalMage ), typeof( Wraith ),
				typeof( BoneKnight ), typeof( Ettin ), typeof( Ghoul ), typeof( HeadlessOne ),
				typeof( Mongbat ), typeof( Ogre ), typeof( OgreLord ),
				typeof( Orc ), typeof( OrcCaptain ), typeof( OrcishLord ), typeof( Ratman ),
				typeof( SkeletalKnight ), typeof( Skeleton ), typeof( Zombie ),	typeof( Slime ),
				typeof( Reaper ), typeof( Corpser ), typeof( AncientWyrm ), typeof( Dragon ),
				typeof( Drake ), typeof( Harpy ), typeof( Lizardman ), typeof(EvilMageLord),
				typeof(Spectre),typeof(Troll),typeof(Wisp),typeof(FireSteed)
			};

		private DateTime m_NextSpawn = DateTime.Now;
		private TimeSpan m_Interval = TimeSpan.FromMinutes( 15 );
		private Mobile m_CurrentMonster;
		private int m_HomeRange;

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Interval
		{
			get{ return m_Interval; }
			set{ m_Interval = value; m_NextSpawn = DateTime.Now + m_Interval; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CurrentMonster
		{
			get{ return m_CurrentMonster.Serial; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HomeRange
		{
			get{ return m_HomeRange; }
			set{ m_HomeRange = value; }
		}

		[Constructable]
		public MagicBrazier() : base(0xE31)
		{
			InitBrazier();
		}

		public MagicBrazier( Serial serial ) : base( serial )
		{
			InitBrazier();
		}

		private void InitBrazier()
		{
			Name = "magic brazier";
			Movable = false;
			Light = LightType.Circle225;
			m_HomeRange = 6;
		}

		private bool MonsterAround()
		{
			if (m_CurrentMonster != null)
			{
				if ( m_CurrentMonster.Deleted )
					return false;
				else if ( m_CurrentMonster is BaseCreature )
					if ( ((BaseCreature)m_CurrentMonster).Controlled || ((BaseCreature)m_CurrentMonster).IsStabled )
						return false;
				return true;
			}
			return false;
		}

		public void BringToHome()
		{
			if (MonsterAround())
				m_CurrentMonster.MoveToWorld(Location , Map);
		}

		//nicked form spawner.cs
		private Point3D GetSpawnPosition()
		{
			Map map = Map;

			if ( map == null )
				return Location;

			for ( int i = 0; i < 10; i++ )
			{
				int x = Location.X + (Utility.Random( (m_HomeRange * 2) + 1 ) - m_HomeRange);
				int y = Location.Y + (Utility.Random( (m_HomeRange * 2) + 1 ) - m_HomeRange);
				int z = Map.GetAverageZ( x, y );

				if ( Map.CanSpawnMobile( new Point2D( x, y ), this.Z ) )
					return new Point3D( x, y, this.Z );
				else if ( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}
			return this.Location;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.Player )
				return;

			if ( !from.InRange( GetWorldLocation(), 1 ) )
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use.
				return;
			}

			if ( DateTime.Now < m_NextSpawn )
			{//time isnt right yet
				from.SendLocalizedMessage( 500760 ); // The brazier fizzes and pops, but nothing seems to happen.
				return;
			}

			if (MonsterAround())
			{ //there's a monster spawned and it hasnt been killed yet
				from.SendLocalizedMessage( 500760 ); // The brazier fizzes and pops, but nothing seems to happen.
			}
			else
			{
				m_CurrentMonster = null;
				//bool validLoc = false;
				Point3D SpawnLocation = new Point3D(GetSpawnPosition());

				int random = Utility.Random( m_Types.Length );
				Type type = m_Types[random] as Type;

				Mobile summoned	= null;

				try{ summoned = Activator.CreateInstance( type ) as Mobile; }
				catch{ Console.WriteLine( "Exception error in Deceit Brazier spawn." );	}

				if ( summoned == null )
				{
					from.SendLocalizedMessage( 500760 ); // The brazier fizzes and pops, but nothing seems to happen.
					return;
				}

				m_CurrentMonster = summoned;

				if ( summoned is BaseCreature )//leaving this in just to be sure
				{
					summoned.Map = this.Map;
					summoned.Location = SpawnLocation;
					summoned.Combatant = from;

					BaseCreature bc = (BaseCreature)summoned;

					bc.Home  = this.Location;	//added some home stuff so it doesnt wander off
					bc.RangeHome = m_HomeRange;

					bc.Combatant = from; //make it attack the summoner

					//bc.Tamable = false; thought they had to be tamable. atleast some of them

					Effects.PlaySound( this.Location, this.Map, bc.GetAngerSound() );

					m_NextSpawn = DateTime.Now + m_Interval;
				}
				else
				{
					from.SendLocalizedMessage( 500760 ); // The brazier fizzes and pops, but nothing seems to happen.
				}
			}
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );

			writer.Write( m_NextSpawn );
			writer.Write( m_Interval );
			writer.Write( m_HomeRange );
			writer.Write( m_CurrentMonster );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_NextSpawn = reader.ReadDateTime();
			m_Interval = reader.ReadTimeSpan();
			m_HomeRange = reader.ReadInt();
			m_CurrentMonster = reader.ReadMobile();
		}
	}
}