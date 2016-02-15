using System;
using Server.Misc;

namespace Server.Items
{
	public class FactionSilver : Item
	{
		[Constructable]
		public FactionSilver() : this( 1 )
		{
		}

		[Constructable]
		public FactionSilver( int amount ) : base( 0xEF0 )
		{
			Stackable = true;
			Weight = 0.02;
			Amount = amount;
			LootType = LootType.Regular;
		}

		public FactionSilver( Serial serial ) : base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount <= 1 )
				return 0x2E4;
			else if ( Amount <= 5 )
				return 0x2E5;
			else
				return 0x2E6;
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new FactionSilver( amount ), amount );
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

			if ( Weight != 0.02 )
				Weight = 0.02;
		}
	}
}