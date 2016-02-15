using System;
using System.IO;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using System.Threading;
using System.Globalization;

namespace Server.Items
{
	public class MoveSpawner : Item
	{
		private bool m_Active;
		private string m_Creature;
		private ArrayList m_Creatures;
		private string m_Message;
		private Point3D m_Location;
		private int m_HomeRange;
		private int m_Team;
		private int m_Limit;
		private DateTime m_NextSpawn;
		private TimeSpan m_Delay;
		private double m_Chance;

		[CommandProperty( AccessLevel.Seer )]
		public int SpawnChance
		{
			get { return (int)( m_Chance * 100 ); }
			set
			{
				int num = value;

				if ( num > 100 )
					num = 100;
				else if ( num < 1 )
				{
					m_Chance = 0;
					m_Active = false;
					return;
				}

                m_Chance = (double)num / 100 ;
			}
		}

		[CommandProperty(AccessLevel.Seer)]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.Seer)]
		public String Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}

		[CommandProperty(AccessLevel.Seer)]
		public Point3D SpawnPoint
		{
			get { return m_Location; }
			set { m_Location = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.Seer)]
		public int HomeRange
		{
			get { return m_HomeRange; }
			set { m_HomeRange = value; }
		}

		[CommandProperty(AccessLevel.Seer)]
		public int Limit
		{
			get { return m_Limit; }
			set { m_Limit = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.Seer)]
		public int Team
		{
			get { return m_Team; }
			set { m_Team = value; }
		}

		[CommandProperty(AccessLevel.Seer)]
		public String Creature
		{
			get { return m_Creature; }
			set
			{
				string str = value;

				if ( str != null )
				{
					str = str.ToLower();
					str = str.Trim();

					Type type = SpawnerType.GetType( str );

					if ( type != null )
						m_Creature = str;
					else
						m_Creature = "-invalid-";
				}
				else
					m_Creature = null;

				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.Seer)]
		public DateTime NextSpawn
		{
			get { return m_NextSpawn; }
		}

		[CommandProperty(AccessLevel.Seer)]
		public TimeSpan Delay
		{
			get { return m_Delay; }
			set { m_Delay = value;  }
		}

		[Constructable]
		public MoveSpawner() : this( null, null, 1, 1, 4, 0, TimeSpan.FromMinutes( 5 ), false )
		{
		}
/*
		[Constructable]
		public MoveSpawner( string creature ) : this( creature, null, 1, 1, 4, 0, TimeSpan.FromMinutes( 5 ), true )
		{
		}

		[Constructable]
		public MoveSpawner( string creature, string msg ) : this( creature, msg, 1, 1, 4, 0, TimeSpan.FromMinutes( 5 ), true )
		{
		}

		[Constructable]
		public MoveSpawner( string creature, string msg, int limit ) : this( creature, msg, limit, 1, 4, 0, TimeSpan.FromMinutes( 5 ), true )
		{
		}

		[Constructable]
		public MoveSpawner( string creature, string msg, int limit, double chance ) : this( creature, msg, limit, chance, 4, 0, TimeSpan.FromMinutes( 5 ), true )
		{
		}

		[Constructable]
		public MoveSpawner( string creature, string msg, int limit, int range, int team ) : this( creature, msg, limit, 1, range, team, TimeSpan.FromMinutes( 5 ), true )
		{
		}

		[Constructable]
		public MoveSpawner( string creature, string msg, int limit, double chance, int range, int team ) : this( creature, msg, limit, chance, range, team, TimeSpan.FromMinutes( 5 ), true )
		{
		}
*/
		public MoveSpawner( string creature, string msg, int limit, double chance, int range, int team, TimeSpan delay, bool active ) : base( 0x1B72 )
		{
			Movable = false;
			Visible = false;
			Name = "Movement Spawner";

			if ( creature != null )
				m_Creature = creature.ToLower();
			else
				m_Creature = null;

			m_Creatures = new ArrayList();
			m_Message = msg;
			m_Limit = limit;
			m_HomeRange = range;
			m_Team = team;
			m_Delay = delay;
			m_Active = active;
			m_NextSpawn = DateTime.Now;
			m_Chance = chance;

			if ( m_Chance > 1 )
				m_Chance = 1;
			else if ( m_Chance <= 0 )
			{
				m_Chance = 0;
				m_Active = false;
			}

		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m_Active )
			{
				if ( m.Player )
				{
					if ( Utility.RandomDouble() > m_Chance )
					{
						//PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "Failed on chance to spawn" )); // debugging
						return true;
					}

					if ( m_NextSpawn > DateTime.Now )
					{
						//PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "Not time to spawn" )); // debugging
						return true;
					}

					Defrag();

					if ( m_Creatures.Count >= m_Limit )
					{
						//PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "Spawn Limit exceeded" )); // debugging
						return true;
					}

					Spawn();
				}
			}
			return true;
		}

		public void Spawn()
		{
			Map map = Map;

			if ( map == null || map == Map.Internal || m_Location == Point3D.Zero || m_Creature == null || m_Creature == "-invalid-" )
				return;

			Type type = SpawnerType.GetType( (string)m_Creature );

			if ( type != null )
			{
				m_NextSpawn = DateTime.Now + m_Delay;

				try
				{
					object o = Activator.CreateInstance( type );

					if ( o is Mobile )
					{
						Mobile m = (Mobile)o;

						m_Creatures.Add( m );
						InvalidateProperties();

						m.Map = map;
						m.Location = m_Location;

						if ( m_Message != null )
							m.PublicOverheadMessage( MessageType.Regular, 0x76C, false, m_Message);

						if ( m is BaseCreature )
						{
							BaseCreature c = (BaseCreature)m;

							c.RangeHome = m_HomeRange;

							if ( m_Team > 0 )
								c.Team = m_Team;

							c.Home = m_Location;
						}
					}
					else if ( o is Item )
					{
						Item item = (Item)o;

						m_Creatures.Add( item );
						InvalidateProperties();

						item.MoveToWorld( m_Location, map );

						if ( m_Message != null )
							item.PublicOverheadMessage( MessageType.Regular, 0x76C, false, m_Message);
					}
				}
				catch
				{
					PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "Exception Caught" )); // debugging
				}
			}
		}

		public void Defrag()
		{
			bool removed = false;

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Item )
				{
					Item item = (Item)o;

					if ( item.Deleted || item.Parent != null )
					{
						m_Creatures.RemoveAt( i );
						--i;
						removed = true;
					}
				}
				else if ( o is Mobile )
				{
					Mobile m = (Mobile)o;

					if ( m.Deleted )
					{
						m_Creatures.RemoveAt( i );
						--i;
						removed = true;
					}
					else if ( m is BaseCreature )
					{
						if ( ((BaseCreature)m).Controlled || ((BaseCreature)m).IsStabled )
						{
							m_Creatures.RemoveAt( i );
							--i;
							removed = true;
						}
					}
				}
				else
				{
					m_Creatures.RemoveAt( i );
					--i;
					removed = true;
				}
			}

			if ( removed )
				InvalidateProperties();
		}

		public override void OnDelete()
		{
			Defrag();

			base.OnDelete();

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Item )
					((Item)o).Delete();
				else if ( o is Mobile )
					((Mobile)o).Delete();
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			Defrag();

			base.GetProperties( list );

			list.Add( this.Name );

			if ( m_Location == Point3D.Zero )
				list.Add( "Spawn Point is not set!" );
			else if ( m_Creature != null )
				list.Add( string.Format( "{0}, {1} of {2}", m_Creature, m_Creatures.Count, m_Limit ));

			if ( m_Active )
				list.Add( 1060742 ); // active
			else
				list.Add( 1060743 ); // inactive
		}

		public override void OnSingleClick( Mobile from )
		{
			Defrag();

			base.OnSingleClick( from );

			if ( m_Location == Point3D.Zero )
			{
				LabelTo( from, "Spawn Point is not set!" );
			}
			else if ( m_Active )
			{
				LabelTo( from, m_Creature );
				LabelTo( from, string.Format( "{0} of {1}", m_Creatures.Count, m_Limit ) );
			}
			else
			{
				LabelTo( from, "(inactive)" );
			}
		}

		public MoveSpawner( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Chance );

			writer.Write( m_Active );
			writer.Write( m_Creature );
			writer.Write( m_Message );
			writer.Write( m_Location );
			writer.Write( m_HomeRange );
			writer.Write( m_Team );
			writer.Write( m_Limit );
			writer.Write((string) m_NextSpawn.ToString());
			writer.Write( m_Delay );

			writer.Write( m_Creatures.Count );

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Item )
					writer.Write( (Item)o );
				else if ( o is Mobile )
					writer.Write( (Mobile)o );
				else
					writer.Write( Serial.MinusOne );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Chance = reader.ReadDouble();
					goto case 0;
				}
				case 0:
				{
					m_Active = reader.ReadBool();
					m_Creature = reader.ReadString();
					m_Message = reader.ReadString();
					m_Location = reader.ReadPoint3D();
					m_HomeRange = reader.ReadInt();
					m_Team = reader.ReadInt();
					m_Limit = reader.ReadInt();
					m_NextSpawn = DateTime.Parse(reader.ReadString());
					m_Delay = reader.ReadTimeSpan();

					int size = reader.ReadInt();
					m_Creatures = new ArrayList( size );

					for ( int i = 0; i < size; ++i )
					{
						IEntity e = World.FindEntity( reader.ReadInt() );

						if ( e != null )
							m_Creatures.Add( e );
					}

					break;
				}
			}
			if ( version == 0 )
				m_Chance = 1;
		}
	}
}