using System;

namespace Server.Items
{
	public class BloodCurtain : Item
	{
		[Constructable]
		public BloodCurtain() : base( Utility.RandomBool() ? 0x159E : 0x159F )
		{
			Name = "curtain";
			Weight = 9.0;
			Hue = 1157;
		}

		public BloodCurtain( Serial serial ) : base( serial )
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