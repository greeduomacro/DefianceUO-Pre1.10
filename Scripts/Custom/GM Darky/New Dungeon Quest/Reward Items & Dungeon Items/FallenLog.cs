using System;

namespace Server.Items
{
	public class FallenLog : Item
	{

		[Constructable]
		public FallenLog() : base( Utility.Random( 0xCF3, 5 ) )
		{
			Name = "a fallen log";
			Weight = 9.0;
		}

		public FallenLog(Serial serial) : base(serial)
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