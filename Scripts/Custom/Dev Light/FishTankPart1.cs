using System;
using Server.Network;

namespace Server.Items
{

	public class FishTankPart1 : Item
	{
		[Constructable]
		public FishTankPart1() : base( 0x3061 )
		{
			Movable = true;
			Name = "an ancient fish tank";
		}

		public FishTankPart1( Serial serial ) : base( serial )
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