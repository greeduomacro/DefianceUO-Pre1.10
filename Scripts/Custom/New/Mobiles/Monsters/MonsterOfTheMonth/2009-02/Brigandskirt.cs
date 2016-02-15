using System;
using Server;

namespace Server.Items
{
	public class Brigandskirt : Skirt
	{
		[Constructable]
		public Brigandskirt()
		{

			Name = "brigand's skirt";
			Hue = Utility.RandomNeutralHue();

		}

		public Brigandskirt( Serial serial ) : base( serial )
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