using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using System.Collections;

namespace Server.Items
{
	public class SpidersWeb : Item
	{
		[Constructable]
		public SpidersWeb()	: base( 0x10D2 )
		{
			Movable = false;
			new InternalTimer( this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Item m_Delete;

			public InternalTimer( Item delete ) : base( TimeSpan.FromSeconds( 20.0 ) )
			{
				Priority = TimerPriority.OneSecond;

				m_Delete = delete;
			}

			protected override void OnTick()
			{
				m_Delete.Delete();
			}
		}

		public SpidersWeb( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
		}
	}
}