using System;
using Server;

namespace Server.Items
{
	public class ValoriteShards : Item
	{
		[Constructable]
		public ValoriteShards() : this( 1 )
		{
		}

		[Constructable]
		public ValoriteShards( int amount ) : base( 0x2242 )
		{
			Stackable = false;
			Weight = 15;
			Name = "Valorite Shards";
                        Hue = 2219;
                        Amount = amount;
			LootType = LootType.Newbied;
		}

		public ValoriteShards( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ValoriteShards( amount ), amount );
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