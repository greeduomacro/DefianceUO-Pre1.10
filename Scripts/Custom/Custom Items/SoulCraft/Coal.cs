using System;
using Server;

namespace Server.Items
{
	public class Coal : Item
	{
		[Constructable]
		public Coal() : this( 1 )
		{
		}

		[Constructable]
		public Coal( int amount ) : base( 0xF21 )
		{
			Stackable = false;
			Weight = 30;
			Name = "Coal";
                        Hue = 1109;
                        Amount = amount;
		}

		public Coal( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new Coal( amount ), amount );
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