using System;

namespace Server.Items
{
	public class Orphystatue : Item
	{
		[Constructable]
		public Orphystatue() : base(9644)
		{
			Weight = 10.0;
			Name = "ophidian statue";
		}

		public Orphystatue(Serial serial) : base(serial)
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