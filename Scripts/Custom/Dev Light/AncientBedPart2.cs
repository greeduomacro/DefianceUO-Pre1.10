using System;
using Server.Network;

namespace Server.Items
{

	public class AncientBedPart2 : Item
	{
		[Constructable]
		public AncientBedPart2() : base( 0x304C )
		{
			Movable = true;
			Name = "an ancient bed";
		}

		public AncientBedPart2( Serial serial ) : base( serial )
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