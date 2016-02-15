using System;
using Server;

namespace Server.Items
{
	public class BloodKey : Item
	{
		[Constructable]
		public BloodKey() : this( 1 )
		{
		}

		[Constructable]
		public BloodKey( int amount ) : base( 0x1010 )
		{
			Stackable = false;
			Weight = 1;
			Name = "A Blood Covered Key";
                        Hue = 2118;
			LootType = LootType.Regular;
                        Amount = amount;
		}

		public BloodKey( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new BloodKey( amount ), amount );
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