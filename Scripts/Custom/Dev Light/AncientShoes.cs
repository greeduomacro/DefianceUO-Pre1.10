using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2796, 0x27E1 )]
	public class AncientShoes : BaseShoes
	{
		[Constructable]
		public AncientShoes() : this( 0 )
		{
		}

		[Constructable]
		public AncientShoes( int hue ) : base( 0x2796, hue )
		{
			Weight = 2.0;
			LootType = LootType.Blessed;
			Name = "ancient shoes";
		}

		public AncientShoes( Serial serial ) : base( serial )
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