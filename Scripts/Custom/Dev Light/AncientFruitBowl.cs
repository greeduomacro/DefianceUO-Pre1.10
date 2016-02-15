using System;
using Server.Network;

namespace Server.Items
{

	public class AncientFruitBowl : Item
	{
		[Constructable]
		public AncientFruitBowl() : base( 0x2D4F )
		{
			Movable = true;
			Name = "a fruit bowl";
		}

		public AncientFruitBowl( Serial serial ) : base( serial )
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