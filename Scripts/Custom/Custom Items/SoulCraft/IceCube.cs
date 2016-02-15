using System;
using Server;

namespace Server.Items
{
	public class IceCube : Item
	{
		[Constructable]
		public IceCube() : this( 1 )
		{
		}

		[Constructable]
		public IceCube( int amount ) : base( 0xF21 )
		{
			Stackable = false;
			Weight = 30;
			Name = "Icecube";
                        Hue = 1153;
                        Amount = amount;
		}

		public IceCube( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new IceCube( amount ), amount );
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