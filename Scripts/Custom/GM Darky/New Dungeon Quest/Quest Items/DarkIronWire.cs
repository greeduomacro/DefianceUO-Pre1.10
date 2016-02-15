using System;
using Server;

namespace Server.Items
{
	public class DarkIronWire : Item
	{
		[Constructable]
		public DarkIronWire() : this( 1 )
		{
		}

		[Constructable]
		public DarkIronWire( int amount ) : base( 0x1876 )
		{
			Stackable = true;
			Weight = 15;
			Name = "Dark Iron Wire";
                        Hue = 2106;
                        Amount = amount;
			LootType = LootType.Newbied;
		}

		public DarkIronWire( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new DarkIronWire( amount ), amount );
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