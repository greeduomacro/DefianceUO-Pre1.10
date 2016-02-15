using System;

namespace Server.Items
{
	public class KrofinRoyalFlag : Item
	{
		[Constructable]
		public KrofinRoyalFlag() : base(0x15D4)
		{
			Weight = 9.0;
			Name = "a Krofin royal flag";
			Hue = 1157;
		}

		public KrofinRoyalFlag(Serial serial) : base(serial)
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