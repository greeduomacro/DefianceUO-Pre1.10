using System;
using Server;

namespace Server.Items
{
	public class LargeShell : Item
	{
		[Constructable]
		public LargeShell() : this( Utility.RandomList( 15122, 15122 ) )
		{
		}

		[Constructable]
		public LargeShell( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "a mysterious large shell";
		}

		public LargeShell( Serial serial ) : base( serial )
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