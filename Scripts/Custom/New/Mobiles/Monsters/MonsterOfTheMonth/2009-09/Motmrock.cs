using System;

namespace Server.Items
{
	public class Motmrock : Item
	{
		[Constructable]
		public Motmrock() : base(6009)
		{
			Weight = 5.0;
			Name = "Gadolinite";
			Hue = 1718;
		}

		public Motmrock(Serial serial) : base(serial)
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