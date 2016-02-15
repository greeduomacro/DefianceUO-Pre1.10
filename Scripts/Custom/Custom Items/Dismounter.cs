using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class Dismounter : Item
	{
		private bool m_Active;
		private Direction m_Direction;
		private string m_Message;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Direction Facing
		{
			get { return m_Direction; }
			set { m_Direction = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public String Message
		{
			get { return m_Message; }
			set { m_Message = value; InvalidateProperties(); }
		}

		[Constructable]
		public Dismounter() : this( Direction.Down, null, true )
		{
		}

		[Constructable]
		public Dismounter( Direction dir ) : this( dir, null, true )
		{
		}

		[Constructable]
		public Dismounter( Direction dir, string msg ) : this( dir, msg, true )
		{
		}

		[Constructable]
		public Dismounter( Direction dir, bool active ) : this( dir, null, active )
		{
		}

		[Constructable]
		public Dismounter( Direction dir, string msg, bool active ) : base( 0x1B7A )
		{
			Movable = false;
			Visible = false;
			Name = "Dismounter";

			m_Active = active;
			m_Direction = dir;
			m_Message = msg;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( this.Name );

			if ( m_Active )
				list.Add( 1060742 ); // active
			else
				list.Add( 1060743 ); // inactive

			list.Add( (m_Direction == Direction.Mask) ? "Up" : ((Direction)m_Direction).ToString() );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( m_Active )
			{
				LabelTo( from, "Facing " + ((m_Direction == Direction.Mask) ? "Up" : ((Direction)m_Direction).ToString()) );
			}
			else
			{
				LabelTo( from, "(inactive)" );
			}
		}

		public override bool OnMoveOver( Mobile m )
		{
            if (m_Active && m.Player && m.AccessLevel < AccessLevel.GameMaster && m.Mounted)
			{
				IMount mount = (IMount)m.Mount;
				mount.Rider = null;
                if (mount is BaseMount)
                {
                    ((BaseMount)mount).Direction = m_Direction;
                    if (((BaseMount)mount).ControlMaster != null)
                        ((BaseMount)mount).ControlOrder = OrderType.Stay;
                }

				if ( m_Message != null )
					m.SendMessage( m_Message );
			}
			return true;
		}

		public Dismounter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Message );
			writer.Write( m_Active );
			writer.Write( (int)m_Direction );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Message = reader.ReadString();
					goto case 0;
				}
				case 0:
				{
					m_Active = reader.ReadBool();
					m_Direction = (Direction)reader.ReadInt();
					break;
				}
			}
		}
	}
}