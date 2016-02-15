using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class NecroCrystal : Item
	{
		[Constructable]
		public NecroCrystal() : this( 1 )
		{
		}

		[Constructable]
		public NecroCrystal( int amount ) : base( 0x1EA7 )
		{
			Weight = 4.0;
			Name = "Necromatic Crystal";
			Stackable = true;
			Hue = 33;
			Amount = amount;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "This Must be in your Pack to use" ); // The bola must be in your pack to use it.
			}
			else if ( !from.CanBeginAction( typeof( NecroCrystal ) ) )
			{
				from.SendMessage( "You must wait a few moments" ); // You have to wait a few moments before you can use another bola!
			}
			else if ( from.Target is CrystalTarget )
			{
				from.SendMessage( "This is already being used" ); // This bola is already being used.
			}
			else if ( from.FindItemOnLayer( Layer.OneHanded ) != null || from.FindItemOnLayer( Layer.TwoHanded ) != null )
			{
				from.SendMessage( "Your hands must be free!" ); // Your hands must be free to use this
			}
			else
			{
				from.Target = new CrystalTarget( this );
				from.SendMessage( "You Focus your energy on the necromatic crystal" ); // * You begin to swing the bola...*
			}
		}

		private static void ReleaseNecroLock( object state )
		{
			((Mobile)state).EndAction( typeof( NecroCrystal ) );
		}

		//private static void ReleaseMountLock( object state )
		//{
		//	((Mobile)state).EndAction( typeof( BaseMount ) );
		//}

		private static void FinishThrow( object state )
		{

			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Mobile to = (Mobile)states[1];
			to.FixedParticles( 0x36FE, 20, 10, 5044, EffectLayer.Head );
			to.PlaySound( 0x300 );

			if ( to is EvilNecromancer )
				to.Damage( 200, from );


			else
			{
			from.SendMessage( "The Crystal has been used up." );
			}



//			IMount mt = to.Hits;
			//if ( Core.AOS )
			//	new Bola().MoveToWorld( to.Location, to.Map );



			//if to = EvilNecromancer to.Hits = 0;


			//to.BeginAction( typeof( BaseMount ) );

			//to.SendLocalizedMessage( 1040023 ); // You have been knocked off of your mount!

			//Timer.DelayCall( TimeSpan.FromSeconds( 3.0 ), new TimerStateCallback( ReleaseMountLock ), to );
			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( ReleaseNecroLock ), from );
		}

		private class CrystalTarget : Target
		{
			private NecroCrystal m_NecroCrystal;

			public CrystalTarget( NecroCrystal necrocrystal ) : base( 8, false, TargetFlags.Harmful )
			{
				m_NecroCrystal = necrocrystal;
			}

			protected override void OnTarget( Mobile from, object obj )
			{
				if ( m_NecroCrystal.Deleted )
					return;

				if ( obj is Mobile )
				{
					Mobile to = (Mobile)obj;

					if ( !m_NecroCrystal.IsChildOf( from.Backpack ) )
					{
						from.SendMessage( "This must be in your pack" ); // The bola must be in your pack to use it.
					}
					else if ( from.FindItemOnLayer( Layer.OneHanded ) != null || from.FindItemOnLayer( Layer.TwoHanded ) != null )
					{
						from.SendMessage( "Your hands must be free!" ); // Your hands must be free to use this
					}
					//else if ( from.Mounted )
					//{
					//	from.SendLocalizedMessage( 1040016 ); // You cannot use this while riding a mount
					//}
					//else if ( !to.Mounted )
					//{
					//	from.SendLocalizedMessage( 1049628 ); // You have no reason to throw a bola at that.
					//}
					else if ( !from.CanBeHarmful( to ) )
					{
					}
					else if ( from.BeginAction( typeof( NecroCrystal ) ) )
					{
					}
					else if ( to is NecromancerChamp )
					{
					from.SendMessage( "The Crystal weakens the monster!" );
					to.Damage( 50, from );
					}



						m_NecroCrystal.Consume();

						from.Direction = from.GetDirectionTo( to );
						//from.Animate( 11, 5, 1, true, false, 0 );
						from.MovingEffect( to, 0x1EA7, 10, 0, false, false );

						Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( FinishThrow ), new object[]{ from, to } );
					}
					else
					{
						from.SendMessage( "You have to wait a few moments before you can use another Crystal!" ); // You have to wait a few moments before you can use another bola!
					}
				//else
				//{
				//	from.SendMessage( "You cannot throw a crystal at that!" ); // You cannot throw a bola at that.
				//}
			}
		}

		public NecroCrystal( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new NecroCrystal( amount ), amount );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}