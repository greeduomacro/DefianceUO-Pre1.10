using System;
using Server;

namespace Server.Items
{
	public class LevelFiveDoor: Item
	{
		[Constructable]
		public LevelFiveDoor() : base ( 0x241F )
		{
			Name = "a mystical door";
			Movable = false;
		}

		public LevelFiveDoor( Serial serial ) : base( serial )
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