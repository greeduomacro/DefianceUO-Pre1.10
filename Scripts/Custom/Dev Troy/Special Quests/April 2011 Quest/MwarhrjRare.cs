using System;

namespace Server.Items
{
	public class MwarhrjRare : Lantern
	{
		[Constructable]
		public MwarhrjRare()
		{
			Weight = 9.0;
			Name = "Mwarhrj lantern of wisdom";
			Hue = 0x235;
			Layer = Layer.Invalid;
		}

		public MwarhrjRare(Serial serial) : base(serial)
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