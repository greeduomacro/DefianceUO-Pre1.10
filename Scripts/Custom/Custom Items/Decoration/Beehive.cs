using System;
using Server;

namespace Server.Items
{
	public class Beehive : Item
	{
		[Constructable]
		public Beehive() : this( Utility.RandomList( 2330, 2330 ) )
		{
		}

		[Constructable]
		public Beehive( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "a busy beehive";
		}

		public Beehive( Serial serial ) : base( serial )
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