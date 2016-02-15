using System;
using Server;

namespace Server.Items
{
	public class MysticalScroll : Item
	{
		[Constructable]
		public MysticalScroll() : base(0x1F35)
		{
			Movable = true;
			Weight = 5;
			Name = "a mystical scroll";
			Hue = 1159;
			LootType = LootType.Regular;
		}

		public MysticalScroll( Serial serial ) : base( serial )
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