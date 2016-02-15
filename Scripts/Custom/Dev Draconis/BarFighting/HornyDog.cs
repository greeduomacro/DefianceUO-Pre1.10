using System;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
	public class HornyDog : TimberWolf
	{
		[Constructable]
		public HornyDog() : base()
		{
			Name = "Nystal";
			Title = "the horny mutt";

			Blessed = true;


			Tamable = false;

			m_NextHumpTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );
		}

		private DateTime m_NextHumpTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextHumpTime )
			{
				m_NextHumpTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 45, 60 ) );

				ArrayList list = new ArrayList();

				foreach ( Mobile m in this.GetMobilesInRange( 5 ) )
				{
					if ( m == this || CanBeHarmful( m ) )
						continue;

					if ( m.Player && m.Alive && m.AccessLevel == AccessLevel.Player )
						list.Add( m );
				}

				if ( list.Count >= 1 )
				{
					for ( int i = 0; i < list.Count; ++i )
				    	{
					   	Mobile m = (Mobile)list[i];
						this.Say( "*pounces at the leg*" );
						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( Hump ), m );
					}
				}
			}

			base.OnThink();
		}

		private void Hump( object state )
		{
			Mobile m = (Mobile)state;

			this.MoveToWorld( m.Location, m.Map );
			this.Freeze( TimeSpan.FromSeconds( 10 ) );
			m.Freeze( TimeSpan.FromSeconds( 10 ) );
			m.SendMessage( "You cannot move while Nystal humps your leg!" );
		}

		public HornyDog(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}