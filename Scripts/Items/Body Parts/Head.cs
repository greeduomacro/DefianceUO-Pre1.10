using System;
using Server;

namespace Server.Items
{
	public class Head : BaseBodyPart
	{
		public override TimeSpan DecayDelay { get { return TimeSpan.FromHours( 24.0 ); } }

		Mobile m_Owner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}

		[Constructable]
		public Head() : this( null )
		{
		}

		[Constructable]
		public Head( Mobile owner ) : base( 0x1DA0 )
		{
			m_Owner = owner;
			Weight = 1.0;
		}

		public Head( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			bool hasowner = m_Owner != null;

			writer.Write( hasowner );
			if ( hasowner )
				writer.Write( m_Owner );


		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch ( version )
			{
				case 1:
				{
					if ( reader.ReadBool() )
						m_Owner = reader.ReadMobile();
					goto case 0;
				}
				case 0:
					break;
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_Owner != null )
				LabelTo( from, String.Format( "head of {0}", m_Owner.Name ) );
			else
				base.OnSingleClick( from );
		}
	}
}