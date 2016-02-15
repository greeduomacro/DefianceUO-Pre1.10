using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x317B, 0x317B )]
	public class LeafWovenJacket : BaseShirt
	{
		[Constructable]
		public LeafWovenJacket() : this( 0 )
		{
		}

		[Constructable]
		public LeafWovenJacket( int hue ) : base( 0x317B, hue )

		{

			Weight = 5.0;
			Layer = Layer.InnerTorso;
			LootType = LootType.Blessed;
			Name = "Leaf Woven Jacket";
		}

		public LeafWovenJacket( Serial serial ) : base( serial )
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