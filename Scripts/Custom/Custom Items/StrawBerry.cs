using System;
using Server;

namespace Server.Items
{
	public class StrawBerry : Item
	{
		[Constructable]
		public StrawBerry() : this( 1 )
		{
		}

		[Constructable]
		public StrawBerry( int amount ) : base( 0x9D1 )
		{
			Stackable = true;
                        Name = "strawberry";
                        Hue = 37;
			Weight = 1;
			Amount = amount;
		}

		public StrawBerry( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new StrawBerry( amount ), amount );
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