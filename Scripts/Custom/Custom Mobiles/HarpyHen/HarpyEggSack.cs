using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class HarpyEggSack : Item, ICarvable
	{
		private SpawnTimer m_Timer;

		[Constructable]
		public HarpyEggSack() : base( 0x10D9 )
		{
			Movable = false;

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		public void Carve( Mobile from, Item item )
		{
			from.SendMessage( "You destroy the egg sack." );

			Delete();

			m_Timer.Stop();
		}

		public HarpyEggSack( Serial serial ) : base( serial )
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

			public SpawnTimer( Item item ) : base( TimeSpan.FromSeconds( 1.5 + (Utility.RandomDouble() * 1.5) ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_Item = item;
			}

			protected override void OnTick()
			{
				if ( m_Item.Deleted )
					return;

				Mobile spawn;

			 	switch ( Utility.Random( 1 ) )
				{
					default:
					case 0: spawn = new Harpy(); break;


				}
				spawn.Map = m_Item.Map;
				spawn.Location = m_Item.Location;

				m_Item.Delete();
			}
		}
	}
}