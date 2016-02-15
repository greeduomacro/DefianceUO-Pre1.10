using System;
using Server;

namespace Server.Items
{
	public class VenomKey : Item
	{
		[Constructable]
		public VenomKey() : this( 1 )
		{
		}

		[Constructable]
		public VenomKey( int amount ) : base( 0x1010 )
		{
			Stackable = false;
			Weight = 1;
			Name = "A Venom Key";
                        Hue = 1372;
			LootType = LootType.Regular;
                        Amount = amount;
		}

		public VenomKey( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new VenomKey( amount ), amount );
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