using System;
using Server;

namespace Server.Items
{
	public class EnchantedWood : Item
	{
		[Constructable]
		public EnchantedWood() : this( 1 )
		{
		}

		[Constructable]
		public EnchantedWood( int amount ) : base( 0xF90 )
		{
			Stackable = true;
			Weight = 1;
			Name = "Enchanted Wood";
                        Amount = amount;
			LootType = LootType.Newbied;
		}

		public EnchantedWood( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new EnchantedWood( amount ), amount );
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