using System;
using Server;

namespace Server.Items
{
	public class SoulGem : Item
	{
		[Constructable]
		public SoulGem() : this( 1 )
		{
		}

		[Constructable]
		public SoulGem( int amount ) : base( 0xF21 )
		{
			Stackable = false;
			Weight = 30;
			Name = "SoulGem";
                        Hue = 1287;
                        Amount = amount;
		}

		public SoulGem( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new SoulGem( amount ), amount );
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