using System;
using Server;

namespace Server.Items
{
	public class SmallMushrooms : Item
	{
		[Constructable]
		public SmallMushrooms() : this( Utility.RandomList( 3348, 3348 ) )
		{
		}

		[Constructable]
		public SmallMushrooms( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "exotic mushrooms";
		}

		public SmallMushrooms( Serial serial ) : base( serial )
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