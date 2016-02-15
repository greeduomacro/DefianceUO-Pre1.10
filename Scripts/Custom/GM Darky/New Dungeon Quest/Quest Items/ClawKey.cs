using System;
using Server;

namespace Server.Items
{
	public class ClawKey : Item
	{
		[Constructable]
		public ClawKey() : this( 1 )
		{
		}

		[Constructable]
		public ClawKey( int amount ) : base( 0x1010 )
		{
			Stackable = false;
			Weight = 1;
			Name = "A Claw Key";
                        Hue = 1437;
			LootType = LootType.Regular;
                        Amount = amount;
		}

		public ClawKey( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ClawKey( amount ), amount );
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