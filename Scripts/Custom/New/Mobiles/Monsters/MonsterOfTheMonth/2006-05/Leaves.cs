using System;
using Server;

namespace Server.Items
{
	public class Leaves : Item
	{
		[Constructable]
		public Leaves() : this( Utility.RandomList( 6949, 6947, 6948, 6950 ) )
		{
		}

		[Constructable]
		public Leaves( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "leaves";
		}

		public Leaves( Serial serial ) : base( serial )
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