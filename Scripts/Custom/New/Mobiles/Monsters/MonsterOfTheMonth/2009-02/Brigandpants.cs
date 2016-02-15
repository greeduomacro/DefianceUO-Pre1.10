using System;
using Server;

namespace Server.Items
{
	public class Brigandpants : ShortPants
	{
		[Constructable]
		public Brigandpants()
		{

			Name = "brigand's short pants";
			Hue = Utility.RandomNeutralHue();

		}

		public Brigandpants( Serial serial ) : base( serial )
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