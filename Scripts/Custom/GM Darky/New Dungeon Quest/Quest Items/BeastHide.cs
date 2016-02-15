using System;
using Server;

namespace Server.Items
{
	public class BeastHide : Item
	{
		[Constructable]
		public BeastHide() : this( 1 )
		{
		}

		[Constructable]
		public BeastHide( int amount ) : base( 0x11F8 )
		{
			Stackable = false;
			Weight = 25;
			Name = "Hide of the Beast";
                        Hue = 1072;
                        Amount = amount;
			LootType = LootType.Newbied;
		}

		public BeastHide( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new BeastHide( amount ), amount );
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