using System;
using Server;

namespace Server.Items
{
	public class Brigandboots : Boots
	{
		[Constructable]
		public Brigandboots()
		{

			Name = "brigand's boots";
			Hue = Utility.RandomNeutralHue();

		}

		public Brigandboots( Serial serial ) : base( serial )
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