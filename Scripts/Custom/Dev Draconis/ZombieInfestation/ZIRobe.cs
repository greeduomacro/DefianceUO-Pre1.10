using System;
using Server.Mobiles;
using Server.Items;
using System.Collections;

namespace Server.Items
{
	public class ZIRobe : Robe
	{
		private Timer m_Timer;
		private Mobile m_Owner;

		[Constructable]
		public ZIRobe( Mobile owner ) : base()
		{
			Weight = 3.0;
			Hue = 1367;
			Name = "Zombie Robe";
			Movable = false;
			m_Owner = owner;

			m_Timer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 1 ), new TimerCallback( OnTick ) );
		}

		public override void OnAfterDelete()
		{
			if( m_Timer != null )
				m_Timer.Stop();
		}

		private void OnTick()
		{
			if ( m_Owner != null && m_Owner.Alive )
				m_Owner.Stam += 50;

			if ( m_Owner != null && !m_Owner.Alive )
				this.Delete();

			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 0 ) )
			{
				if ( m is PlayerMobile && ( m.BodyValue == 400 || m.BodyValue == 401 ) && m_Owner.CanBeHarmful(m, false) )
				{
					list.Add( m );
				}
			}

			foreach ( Mobile m in list )
			{
				Item robe = m.FindItemOnLayer( Layer.OuterTorso );

				if ( robe != null && robe.Movable )
					m.AddToBackpack( robe );

				Item[] items = m.Backpack.FindItemsByType( typeof( Spellbook ) );

				foreach ( Spellbook book in items )
				{
					book.Delete();
				}

				m.BodyMod = 155;
				m.NameMod = "an infestation zombie";
				m.Hidden = true;
				m.Combatant = null;
				m.AddItem( new ZIRobe( m ) );
			}
		}

		public ZIRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}