using System;

namespace Server.Items
{
	public class WoodenBoard : Item
	{

		[Constructable]
		public WoodenBoard() : base( Utility.RandomList( 0x1BD8, 0x1BD9, 0x1BDB, 0x1BDC, 0x1BDE, 0x1BE1, 0x1BDF, 0x1BE2 ) )
		{
			Name = "boards";
			Weight = 9.0;
		}

		public WoodenBoard(Serial serial) : base(serial)
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