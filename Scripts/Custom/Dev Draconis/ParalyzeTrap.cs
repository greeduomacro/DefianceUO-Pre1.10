using System;

namespace Server.Items
{
	public class ParalyzeTrap : BaseTrap
	{
		private int m_Duration;

		[Constructable]
		public ParalyzeTrap( int duration ) : base( 0x1AE1 )
		{
			m_Duration = duration;
		}

		public override void OnTrigger( Mobile from )
		{
			if ( from.AccessLevel > AccessLevel.Player )
				return;

			Effects.SendLocationEffect( this.Location, this.Map, 14186, 32, 20, 300, 0 );

			Effects.PlaySound( Location, Map, 0x204 );

			if ( from.Alive && CheckRange( from.Location, 0 ) && from.CanBeDamaged() )
			{
				from.Freeze( TimeSpan.FromSeconds( m_Duration ) );
				from.SendMessage( "You have been paralyzed!" );
			}
		}

		public ParalyzeTrap( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Duration );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Duration = reader.ReadInt();
		}
	}
}