using System;
using Server;

namespace Server.Items
{
	public class RedStocking : Item
	{
		[Constructable]
		public RedStocking() : this( Utility.RandomList( 11228, 11228 ) )
		{
		}

		[Constructable]
		public RedStocking( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "a red stocking";
		}

		public RedStocking( Serial serial ) : base( serial )
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