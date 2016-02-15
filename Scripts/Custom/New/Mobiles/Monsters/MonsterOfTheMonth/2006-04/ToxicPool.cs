using System;
using Server;

namespace Server.Items
{
	public class ToxicPool : Item
	{
		[Constructable]
		public ToxicPool() : this( 4650 )
		{
		}

		[Constructable]
		public ToxicPool( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 1;
			Name = "a toxic pool";
			Hue = 72;

		}

		public ToxicPool( Serial serial ) : base( serial )
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