using System;
using Server.Network;

namespace Server.Items
{


	public class SerpentCrest : Item
	{
		[Constructable]
		public SerpentCrest() : base( 0x1514 )
		{
			Movable = true;
			Name = "a donation serpent crest";
		}

		public SerpentCrest( Serial serial ) : base( serial )
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