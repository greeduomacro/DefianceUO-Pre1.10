using System;
using Server;

namespace Server.Items
{
	public class SnowMinionStatue : Item
	{
		[Constructable]
		public SnowMinionStatue() : this( 0x2611 )
		{
		}

		[Constructable]
		public SnowMinionStatue( int itemID ) : base( itemID )
		{
			Movable = true;
			Weight = 11;
			Name = "a giant snow minion statuette";

		}

		public SnowMinionStatue( Serial serial ) : base( serial )
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