using System;
using Server;

namespace Server.Items
{
	public class MartooSoul : Item
	{
		[Constructable]
		public MartooSoul() : this( 1 )
		{
		}

		[Constructable]
		public MartooSoul( int amount ) : base( 0xF21 )
		{
			Stackable = false;
			Weight = 30;
			Name = "Martoo's Soulstone";
                        Hue = 1367;
                        Amount = amount;
		}

		public MartooSoul( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new MartooSoul( amount ), amount );
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