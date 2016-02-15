using System;
using Server;

namespace Server.Items
{
	public class WoodenKey : Item
	{
		[Constructable]
		public WoodenKey() : this( 1 )
		{
		}

		[Constructable]
		public WoodenKey( int amount ) : base( 0x1010 )
		{
			Stackable = false;
			Weight = 1;
			Name = "A Wooden Key";
                        Hue = 1868;
			LootType = LootType.Regular;
                        Amount = amount;
		}

		public WoodenKey( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new WoodenKey( amount ), amount );
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