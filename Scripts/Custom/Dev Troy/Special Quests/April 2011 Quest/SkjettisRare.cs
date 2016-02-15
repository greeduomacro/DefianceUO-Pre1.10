using System;

namespace Server.Items
{
	public class SkjettisRare : Item
	{
		[Constructable]
		public SkjettisRare() : base( 0x185B )
		{
			Weight = 9.0;
			Name = "Vial of Skjettis' poison";
			Hue = 0x36F;
		}

		public SkjettisRare(Serial serial) : base(serial)
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