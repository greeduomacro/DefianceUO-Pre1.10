using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x27A1, 0x27EC )]
	public class AncientCoat : BaseMiddleTorso
	{
		[Constructable]
		public AncientCoat() : this( 0 )
		{
		}

		[Constructable]
		public AncientCoat( int hue ) : base( 0x27A1, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;
			Name = "an ancient coat";
		}

		public AncientCoat( Serial serial ) : base( serial )
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