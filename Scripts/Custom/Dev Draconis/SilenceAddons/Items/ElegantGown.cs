using System;

namespace Server.Items
{
	public class ElegantGown : BaseOuterTorso
	{
		[Constructable]
		public ElegantGown() : base(0x1F00)
		{
			Name = "Elegant Gown of Silence";
			Hue = 1000;
			Weight = 3.0;
		}

		public ElegantGown(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}