using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AcidTrap : Item
	{
		private Timer BurnTimer;

		[Constructable]
		public AcidTrap() : this( null, 0x122A )
		{
		}

		[Constructable]
		public AcidTrap( Mobile target ) : this( target, 0x122A )
		{
		}

		[Constructable]
		public AcidTrap( Mobile target, int itemID ) : base( itemID )
		{
			Name="acid";
			Movable = false;
			Hue=73;

			if ( target != null && target.Alive )
			{//if a target is passed, start burning right away
				BurnTimer = new m_Timer( target );
				BurnTimer.Start();
			}
		}

		public AcidTrap( Serial serial ) : base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			/* Al: Disabled because it is bugged. Kills for example a mounted pet that
             * does not stand on the trap any more.
            if( m.Alive && !(IsDeadPet(m)) )
			{
				m.Damage( Utility.Random( 5, 10 ) );
				m.PlaySound(0x1dE);
				BurnTimer = new m_Timer( m );
				BurnTimer.Start();
			}
            */
            return true;
		}

		public override bool OnMoveOff(Mobile m)
		{
			if ( BurnTimer != null )
				BurnTimer.Stop();

			BurnTimer = null;
			return true;
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
		}

		public override void OnAfterDelete()
		{
			if ( BurnTimer != null )
				BurnTimer.Stop();

			BurnTimer = null;

			base.OnAfterDelete();
		}

		public bool IsDeadPet( Mobile m )
		{
			if( m is BaseCreature )
			{
				BaseCreature animal = (BaseCreature)m;
				if( animal.IsDeadPet )
					return true;
			}
			return false;

		}

		private class m_Timer: Timer
		{
			private Mobile m_Mobile;
			private bool IsDeadPet( Mobile m )
			{
				if( m is BaseCreature )
				{
					BaseCreature animal = (BaseCreature)m;
					if( animal.IsDeadPet )
						return true;
				}
				return false;
			}
			public m_Timer( Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ),TimeSpan.FromSeconds( 1.0 ) )
			{
				Priority = TimerPriority.FiftyMS;
				m_Mobile = from;
			}

			protected override void OnTick()
			{
				if( m_Mobile.Alive && !(IsDeadPet(m_Mobile)) )
				{
					m_Mobile.Damage( Utility.Random( 10, 15 ) );
					m_Mobile.PlaySound(0x1dE);
				}
				else
					Stop();
			}
		}
	}
}