using System;
using Server;

namespace Server.Items
{
	public class LanternOfHellfire : Lantern
	{
		[Constructable]
		public LanternOfHellfire()
		{
			Name = "lantern of hellfire";
			Hue = 1258;
			LootType = LootType.Blessed;
		}

		public LanternOfHellfire( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}