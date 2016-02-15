using System;
using Server;

namespace Server.Items
{
	public class LevelFourDoor: Item
	{
		[Constructable]
		public LevelFourDoor() : base ( 0x2420 )
		{
			Name = "a mystical door";
			Movable = false;
		}

		public LevelFourDoor( Serial serial ) : base( serial )
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