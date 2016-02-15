using System;

namespace Server.Items
{
	public class SpiderWeb : Item
	{

		[Constructable]
		public SpiderWeb() : base( Utility.Random( 0x10D2, 6 ) )
		{
			Name = "a small web";
			Weight = 9.0;
		}

		public SpiderWeb(Serial serial) : base(serial)
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