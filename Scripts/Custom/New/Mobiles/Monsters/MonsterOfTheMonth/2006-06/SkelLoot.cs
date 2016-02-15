using System;
using Server;

namespace Server.Items
{
	public class SkelLoot : Item
	{
		[Constructable]
		public SkelLoot() : this( Utility.RandomList( 7040, 7037 ) )
		{
		}

		[Constructable]
		public SkelLoot( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "skeleton";
		}

		public SkelLoot( Serial serial ) : base( serial )
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