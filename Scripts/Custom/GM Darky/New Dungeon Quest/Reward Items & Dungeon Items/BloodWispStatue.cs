using System;

namespace Server.Items
{
	public class BloodWispStatue : Item
	{
		[Constructable]
		public BloodWispStatue() : base( 0x2100 )
		{
			Name = "a bloody wisp statue";
			Weight = 9.0;
			Hue = 232;
		}

		public BloodWispStatue( Serial serial ) : base( serial )
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