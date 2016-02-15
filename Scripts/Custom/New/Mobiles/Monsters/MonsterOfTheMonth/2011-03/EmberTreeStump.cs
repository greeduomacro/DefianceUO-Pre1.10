using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class EmberTreeStump : Item
	{
		[Constructable]
		public EmberTreeStump() : base( 0xE57 )
		{
			Name = "an ember tree stump";
			Weight = 11.0;
			Hue = 1194;
		}

		public EmberTreeStump( Serial serial ) : base( serial )
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