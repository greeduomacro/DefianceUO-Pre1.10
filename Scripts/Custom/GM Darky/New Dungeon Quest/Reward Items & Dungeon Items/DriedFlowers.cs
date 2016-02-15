using System;

namespace Server.Items
{
	public class DriedFlowers : Item
	{

		[Constructable]
		public DriedFlowers() : base( Utility.Random( 0xC3B, 8 ) )
		{
			Name = "dried flowers";
			Weight = 9.0;
		}

		public DriedFlowers( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}