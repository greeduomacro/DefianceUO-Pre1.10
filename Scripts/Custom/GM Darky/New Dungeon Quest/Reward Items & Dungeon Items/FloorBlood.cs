using System;

namespace Server.Items
{
	public class FloorBlood : Item
	{

		[Constructable]
		public FloorBlood() : base( Utility.Random( 0x1D92, 5 ) )
		{
			Name = "blood";
			Weight = 9.0;
		}

		public FloorBlood(Serial serial) : base(serial)
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