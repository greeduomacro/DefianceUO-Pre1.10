using System;
using Server;

namespace Server.Items
{
	public class GingerbreadCookie : Item
	{
		[Constructable]
		public GingerbreadCookie() : this( Utility.RandomList( 11233, 11234 ) )
		{
		}

		[Constructable]
		public GingerbreadCookie( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "a gingerbread cookie";
		}

		public GingerbreadCookie( Serial serial ) : base( serial )
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