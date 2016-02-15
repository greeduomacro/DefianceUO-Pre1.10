using System;
using Server.Network;

namespace Server.Items
{

	public class Guillotine : Item
	{
		[Constructable]
		public Guillotine() : base( 0x1260 )
		{
			Movable = true;
			Name = "a donation guillotine";
		}

		public Guillotine( Serial serial ) : base( serial )
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