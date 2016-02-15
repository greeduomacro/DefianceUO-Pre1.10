using System;
using Server;

namespace Server.Items
{
	public class MountainBlood : Item
	{
		[Constructable]
		public MountainBlood() : this( 1 )
		{
		}

		[Constructable]
		public MountainBlood( int amount ) : base( 0x122B )
		{
			Stackable = false;
			Weight = 1;
			Name = "Blood of the Mountain";
                        Amount = amount;
			LootType = LootType.Newbied;
		}

		public MountainBlood( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new MountainBlood( amount ), amount );
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