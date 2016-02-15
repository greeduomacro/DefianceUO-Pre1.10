using System;
using Server.Network;

namespace Server.Items
{

	public class IronMaiden : Item
	{
		[Constructable]
		public IronMaiden() : base( 0x124D )
		{
			Movable = true;
			Name = "a donation iron maiden";
		}

		public IronMaiden( Serial serial ) : base( serial )
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