using System;
using Server;

namespace Server.Items
{
	public class Brigandshirt : FancyShirt
	{
		[Constructable]
		public Brigandshirt()
		{

			Name = "brigand's shirt";

		}

		public Brigandshirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}