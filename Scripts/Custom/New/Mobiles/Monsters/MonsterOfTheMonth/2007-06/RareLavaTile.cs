using System;
using Server;

namespace Server.Items
{
	public class RareLavaTile : Item
	{
		[Constructable]
		public RareLavaTile() : base( 4868 )
		{
			Movable = true;
			Weight = 5;
			Name = "a rare lava tile";
		}

		public RareLavaTile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}


	}
}