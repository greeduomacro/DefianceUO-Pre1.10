using System;
using Server.Network;

namespace Server.Items
{

	public class AncientBedPart1 : Item
	{
		[Constructable]
		public AncientBedPart1() : base( 0x304D )
		{
			Movable = true;
			Name = "an ancient bed";
		}

		public AncientBedPart1( Serial serial ) : base( serial )
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