using System;
using Server;

namespace Server.Items
{
	public class AMiseryToken : Item
	{
		[Constructable]
		public AMiseryToken() : this( 0x14F0 )
		{
		}

		[Constructable]
		public AMiseryToken( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 11;
			Name = "a misery token";
			Hue = 842;

		}

		public AMiseryToken( Serial serial ) : base( serial )
		{
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