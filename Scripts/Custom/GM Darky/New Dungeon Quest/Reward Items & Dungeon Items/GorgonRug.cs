using System;

namespace Server.Items
{
	public class GorgonRug : Item
	{

		[Constructable]
		public GorgonRug() : base( Utility.Random( 0x1DEF, 14 ) )
		{
			Name = "gorgon rug";
			Weight = 9.0;
		}

		public GorgonRug(Serial serial) : base(serial)
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