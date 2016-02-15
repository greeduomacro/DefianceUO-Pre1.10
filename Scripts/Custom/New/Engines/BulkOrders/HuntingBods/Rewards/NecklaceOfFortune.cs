using System;

namespace Server.Items
{
	public class NecklaceOfFortune : BaseNecklace
	{
		[Constructable]
		public NecklaceOfFortune() : base( 0x1088 )
		{
			Name = "Necklace Of Fortune";
			Weight = 0.1;
			Hue = 0x501;
			Attributes.Luck = 200;
			LootType = LootType.Blessed;
		}

		public NecklaceOfFortune( Serial serial ) : base( serial )
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