using System;

namespace Server.Items
{
public class GrayShirt : BaseShirt
	{
		[Constructable]
		public GrayShirt() : base( 0x1EFD)
		{
			Name = "Gray Shirt of Silence";
			Weight = 1.0;
			Hue = 1000;
		}

		public GrayShirt( Serial serial ) : base( serial )
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
		}
	}
}