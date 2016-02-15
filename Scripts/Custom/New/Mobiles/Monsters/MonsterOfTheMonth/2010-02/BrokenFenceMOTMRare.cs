using System;

namespace Server.Items
{
	public class BrokenFenceMOTMRare : Item
	{
		[Constructable]
		public BrokenFenceMOTMRare() : base(8783)
		{
			Weight = 9.0;
			Name = "a demonic figure";
			Hue = 2106;
		}

		public BrokenFenceMOTMRare(Serial serial) : base(serial)
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