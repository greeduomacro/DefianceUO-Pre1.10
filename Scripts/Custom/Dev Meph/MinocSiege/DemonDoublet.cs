using System;
using Server;

namespace Server.Items
{
	public class DemonDoublet : Doublet
	{
		[Constructable]
		public DemonDoublet()
		{

			Name = "doublet of the chaos realm";
			Hue = 1359;

		}

		public DemonDoublet( Serial serial ) : base( serial )
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