using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class EmberTree : Item
	{
		[Constructable]
		public EmberTree() : base( 0x2377 )
		{
			Name = "an ember tree";
			Weight = 12.0;
			Hue = 1194;
		}

		public EmberTree( Serial serial ) : base( serial )
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