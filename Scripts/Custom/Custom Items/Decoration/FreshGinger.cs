using System;
using Server;

namespace Server.Items
{
	public class FreshGinger : Item
	{
		[Constructable]
		public FreshGinger() : this( Utility.RandomList( 11236, 11236 ) )
		{
		}

		[Constructable]
		public FreshGinger( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "fresh ginger";
		}

		public FreshGinger( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}


	}
}