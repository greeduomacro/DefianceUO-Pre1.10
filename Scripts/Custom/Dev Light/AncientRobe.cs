using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2799, 0x27E4 )]
	public class AncientRobe : BaseOuterTorso
	{
		[Constructable]
		public AncientRobe() : this( 0 )
		{
		}

		[Constructable]
		public AncientRobe( int hue ) : base( 0x2799, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;
			Name = "an ancient robe";
		}

		public AncientRobe( Serial serial ) : base( serial )
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