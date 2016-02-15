using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class WindcallerGate : Item, ICarvable
	{
		private SpawnTimer m_Timer;

		[Constructable]
		public WindcallerGate() : base( 8148 )
		{
			Movable = false;
			Name = "Gate of Windcalling";
			Hue = 1237;

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		public void Carve( Mobile from, Item item )
		{
			Effects.PlaySound( GetWorldLocation(), Map, 86 );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0 );

			if ( 0.3 > Utility.RandomDouble() )
			{
				if ( ItemID == 0x2809 )
					from.SendMessage( "You destroy the gate." );
				else
					from.SendMessage( "You destroy the windcalling gate." );

				Gold gold = new Gold( 25, 100 );

				gold.MoveToWorld( GetWorldLocation(), Map );

				Delete();

				m_Timer.Stop();
			}
			else
			{
				if ( ItemID == 0x2809 )
					from.SendMessage( "You damage the gate." );
				else
					from.SendMessage( "You damage the windcalling gate." );
			}
		}

		public WindcallerGate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		private class SpawnTimer : Timer
		{
			private Item m_Item;

			public SpawnTimer( Item item ) : base( TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 10 ) ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_Item = item;
			}

			protected override void OnTick()
			{
				if ( m_Item.Deleted )
					return;

				Mobile spawn;

				switch ( Utility.Random( 2 ) )
				{
					default:
					case 0: spawn = new AirElemental(); break;
					case 1: spawn = new AirElemental(); break;
				}

				spawn.MoveToWorld( m_Item.Location, m_Item.Map );

				m_Item.Delete();
			}
		}
	}
}