using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AcidPool : Item
	{
		private Timer BurnTimer;

		[Constructable]
		public AcidPool() : this( null, 0x122A )
		{
		}

		[Constructable]
		public AcidPool( Mobile target ) : this( target, 0x122A )
		{
		}

		[Constructable]
		public AcidPool( Mobile target, int itemID ) : base( itemID )
		{
			Name="acid";
			Movable = false;
			Hue=73;
			new InternalTimer( this ).Start();

			if ( target != null && target.Alive && !(IsAnt(target)) )
			{//if a target is passed, start burning right away
				BurnTimer = new m_Timer( target );
				BurnTimer.Start();
			}
		}

		public AcidPool( Serial serial ) : base( serial )
		{
			new InternalTimer( this ).Start();
		}

		public override bool OnMoveOver( Mobile m )
		{
			if( m.Alive && !(IsDeadPet(m)) && !(IsAnt(m)) )
			{
				m.Damage( Utility.Random( 10, 15 ) );
				m.PlaySound(0x1dE);
				BurnTimer = new m_Timer( m );
				BurnTimer.Start();
			}
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

		private class InternalTimer : Timer
		{
			private Item m_AcidPool;

			public InternalTimer( Item blood ) : base( TimeSpan.FromSeconds( Utility.Random( 10, 15 ) ) )
			{
				Priority = TimerPriority.FiftyMS;
				m_AcidPool = blood;
			}

			protected override void OnTick()
			{
				m_AcidPool.Delete();
			}
		}

		public override void OnAfterDelete()
		{
			if ( BurnTimer != null )
				BurnTimer.Stop();

			BurnTimer = null;

			base.OnAfterDelete();
		}

		public bool IsAnt( Mobile m )
		{
			if ( m is BlackSolenWorker           || m is BlackSolenQueen              || m is BlackSolenWarrior ||
				m is RedSolenWorker             || m is RedSolenQueen                || m is RedSolenWarrior ||
				m is RedSolenInfiltratorQueen   || m is RedSolenInfiltratorWarrior   ||
				m is BlackSolenInfiltratorQueen || m is BlackSolenInfiltratorWarrior || m is BlackDragon || m is AcidElemental ||
				m is TurtleWarrior )
			{
				return true;
			}
			return false;
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
				if( m is BaseCreature && ((BaseCreature)m).IsDeadPet )
					return true;
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