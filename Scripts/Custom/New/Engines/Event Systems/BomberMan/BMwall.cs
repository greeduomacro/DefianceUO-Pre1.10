using System;

namespace Server.Items
{
	public class BMwall : Item
	{
		[Constructable]
		public BMwall() : base(3645)
		{
			Name = "a bomberman wall";
			Hue = 1341;
			Movable = false;
			Visible = true;
		}

		public BMwall(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
		}
	}
}