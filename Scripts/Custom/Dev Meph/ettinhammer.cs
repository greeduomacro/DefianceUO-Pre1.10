using System;
using Server;

namespace Server.Items
{
	public class ettinhammer : Item
	{
		[Constructable]
		public ettinhammer() : this( 9557 )
		{
		}

		[Constructable]
		public ettinhammer( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 11;
			Name = "hammer of an elder ettin";

		}

		public ettinhammer( Serial serial ) : base( serial )
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