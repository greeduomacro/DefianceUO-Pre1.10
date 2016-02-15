using System;
using Server;

namespace Server.Items
{
	public class KiranHeart : Item
	{
		[Constructable]
		public KiranHeart() : this( 1 )
		{
		}

		[Constructable]
		public KiranHeart( int amount ) : base( 0x1CED )
		{
			Stackable = false;
			Weight = 0.1;
			Name = "Heart of Kiran";
                        Amount = amount;
		}

		public KiranHeart( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new KiranHeart( amount ), amount );
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