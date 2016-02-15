using System;
using Server;

namespace Server.Items
{
	public class ShimmeringKey : Item
	{
		[Constructable]
		public ShimmeringKey() : this( 1 )
		{
		}

		[Constructable]
		public ShimmeringKey( int amount ) : base( 0x1010 )
		{
			Stackable = false;
			Weight = 1;
			Name = "A Shimmering Key";
                        Hue = 1159;
			LootType = LootType.Regular;
                        Amount = amount;
		}

		public ShimmeringKey( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ShimmeringKey( amount ), amount );
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

			// tmp (convertation of existing keys)
			LootType = LootType.Regular;
		}
	}
}