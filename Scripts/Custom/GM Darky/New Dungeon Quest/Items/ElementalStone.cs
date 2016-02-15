using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ElementalStone : Item, ICarvable
	{
		private SpawnTimer m_Timer;

		[Constructable]
		public ElementalStone() : base( 0x2809 )
		{
			Movable = false;
			Hue = 0x4F2;
			Name = "Elemental Summoning Stone";

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		public void Carve( Mobile from, Item item )
		{
			Effects.PlaySound( GetWorldLocation(), Map, 0x48F );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0 );

			if ( 0.3 > Utility.RandomDouble() )
			{
				if ( ItemID == 0x2809 )
					from.SendMessage( "You destroy the stone." );
				else
					from.SendMessage( "You destroy the elemental stone." );

				Gold gold = new Gold( 25, 100 );

				gold.MoveToWorld( GetWorldLocation(), Map );

				Delete();

				m_Timer.Stop();
			}
			else
			{
				if ( ItemID == 0x2809 )
					from.SendMessage( "You damage the stone." );
				else
					from.SendMessage( "You damage the elemental stone." );
			}
		}

		public ElementalStone( Serial serial ) : base( serial )
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

				switch ( Utility.Random( 10 ) )
				{
					default:
					case 0: spawn = new AirElemental(); break;
					case 1: spawn = new EarthElemental(); break;
					case 2: spawn = new WaterElemental(); break;
					case 3: spawn = new FireElemental(); break;
					case 4: spawn = new IceElemental(); break;
					case 5: spawn = new SnowElemental(); break;
					case 6: spawn = new Efreet(); break;
					case 7: spawn = new BloodElemental(); break;
					case 8: spawn = new PoisonElemental(); break;
				}

				spawn.MoveToWorld( m_Item.Location, m_Item.Map );

				m_Item.Delete();
			}
		}
	}
}