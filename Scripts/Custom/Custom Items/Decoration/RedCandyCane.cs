using System;
using Server;

namespace Server.Items
{
	public class RedCandyCane : Item
	{
		[Constructable]
		public RedCandyCane() : this( Utility.RandomList( 11229, 11229 ) )
		{
		}

		[Constructable]
		public RedCandyCane( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "a red candy cane";
		}

		public RedCandyCane( Serial serial ) : base( serial )
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