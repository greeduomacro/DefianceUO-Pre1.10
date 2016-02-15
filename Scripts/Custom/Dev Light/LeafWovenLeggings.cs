using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x317F, 0x317F )]
	public class LeafWovenLeggings : BasePants
	{
		[Constructable]
		public LeafWovenLeggings() : this( 0 )
		{
		}

		[Constructable]
		public LeafWovenLeggings( int hue ) : base( 0x317F, hue )
		{
			Weight = 2.0;
			LootType = LootType.Blessed;
			Name = "Leaf Woven Leggings";
		}

		public LeafWovenLeggings( Serial serial ) : base( serial )
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