using System;
using Server;

namespace Server.Items
{
	public class Brigandbandana : Bandana
	{
		[Constructable]
		public Brigandbandana()
		{

			Name = "brigand's bandana";

		}

		public Brigandbandana( Serial serial ) : base( serial )
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