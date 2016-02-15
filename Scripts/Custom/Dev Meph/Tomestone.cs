using System;
using Server;

namespace Server.Items
{
	public class Tombstone : Item
	{
		[Constructable]
		public Tombstone() : this( Utility.RandomList( 4475, 4476 ) )
		{
		}

		[Constructable]
		public Tombstone( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 10;
			Name = "a gravestone";
		}

		public Tombstone( Serial serial ) : base( serial )
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