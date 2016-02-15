using System;
using Server;

namespace Server.Items
{
	public class ChanHeart : Item
	{
		[Constructable]
		public ChanHeart() : this( 1 )
		{
		}

		[Constructable]
		public ChanHeart( int amount ) : base( 0x1CED )
		{
			Stackable = false;
			Weight = 0.1;
			Name = "Heart of Chan";
                        Amount = amount;
		}

		public ChanHeart( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ChanHeart( amount ), amount );
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