using System;

namespace Server.Items
{
	public class Rock : Item
	{

		[Constructable]
		public Rock() : base( Utility.RandomList( Utility.Random( 0x1364, 11 ), 0x1773, 0x1774, 0x1777, 0x1778, 0x177B, 0x177C ) )
		{
			Name = "rock";
			Weight = 15.0;
		}

		public Rock(Serial serial) : base(serial)
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