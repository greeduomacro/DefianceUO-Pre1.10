using System;

namespace Server.Items
{
	public class HojanRoyalFlag : Item
	{
		[Constructable]
		public HojanRoyalFlag() : base(0x15D0)
		{
			Weight = 9.0;
			Name = "a Hojan royal flag";
			Hue = 1176;
		}

		public HojanRoyalFlag(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}
	}
}