using System;
using Server;

namespace Server.Items
{
	public class DemonSandals : Sandals
	{
		[Constructable]
		public DemonSandals()
		{
			Name = "sandals of the chaos realm";
			Hue = 1359;
		}

		public DemonSandals( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}