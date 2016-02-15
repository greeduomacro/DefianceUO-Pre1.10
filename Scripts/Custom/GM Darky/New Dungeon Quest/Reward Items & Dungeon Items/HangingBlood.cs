using System;

namespace Server.Items
{
	public class HangingBlood : Item
	{

		[Constructable]
		public HangingBlood() : base( Utility.RandomList( 0x122A, 0x122B, 0x122D, 0x122E ) )
		{
			Name = "blood";
			Weight = 9.0;
		}

		public HangingBlood(Serial serial) : base(serial)
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