using System;
using Server.Network;

namespace Server.Items
{

	public class FishTankPart2 : Item
	{
		[Constructable]
		public FishTankPart2() : base( 0x3060 )
		{
			Movable = true;
			Name = "an ancient fish tank";
		}

		public FishTankPart2( Serial serial ) : base( serial )
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