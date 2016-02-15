using System;
using Server;

namespace Server.Items
{
	public class ZendellSoul : Item
	{
		[Constructable]
		public ZendellSoul() : this( 1 )
		{
		}

		[Constructable]
		public ZendellSoul( int amount ) : base( 0xF21 )
		{
			Stackable = false;
			Weight = 30;
			Name = "Zendella's Soulstone";
                        Hue = 2118;
                        Amount = amount;
		}

		public ZendellSoul( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ZendellSoul( amount ), amount );
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