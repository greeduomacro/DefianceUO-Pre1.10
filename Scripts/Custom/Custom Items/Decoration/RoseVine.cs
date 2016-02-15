using System;
using Server;

namespace Server.Items
{
	public class RoseVine : Item
	{
		[Constructable]
		public RoseVine() : this( Utility.RandomList( 11515, 11516, 11514, 11513 ) )
		{
		}

		[Constructable]
		public RoseVine( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "rose vines";
		}

		public RoseVine( Serial serial ) : base( serial )
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