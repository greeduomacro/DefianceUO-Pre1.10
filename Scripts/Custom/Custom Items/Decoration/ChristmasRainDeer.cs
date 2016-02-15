using System;
using Server;

namespace Server.Items
{
	public class ChristmasRein : Item
	{
		[Constructable]
		public ChristmasRein() : this( Utility.RandomList( 14933, 14935, 14934, 14944 ) )
		{
		}

		[Constructable]
		public ChristmasRein( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 11;
			Name = "a christmas reindeer";
		}

		public ChristmasRein( Serial serial ) : base( serial )
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