// By Nerun
// (Thx David for some suggestions.)
using System;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Items
{
	public class Wheat : Item
	{
		[Constructable]
		public Wheat() : this( 1 )
		{
		}

		[Constructable]
		public Wheat( int amount ) : base( 0x1EBD )
		{
			Stackable = true;
			Weight = 4.0;
			Amount = amount;
		}

		public Wheat( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new Wheat( amount ), amount );
		}

		public override void OnDoubleClick( Mobile from )
		{
			ArrayList list = new ArrayList();

			foreach ( Item m in from.GetItemsInRange( 2 ) )
			{
				if ( m is FlourMillEastAddon )
					list.Add( m );

				else if ( m is FlourMillSouthAddon )
					list.Add( m );
			}

			if( IsChildOf( from.Backpack ) && Amount >= 4 && list.Count <= 0 )
			{
				from.SendLocalizedMessage( 1044491 ); // You must be near a flour mill to do that.
			}

			else if( IsChildOf( from.Backpack ) && Amount >= 4 )
			{
				from.SendMessage( "You got a sack of flour." );
				from.AddToBackpack( new SackFlour() );
				this.Consume( 4 );
			}

			else if( IsChildOf( from.Backpack ) && Amount < 4 )
			{
				from.SendMessage( "You need more wheat sheaves." );
			}

			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
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
	}
}