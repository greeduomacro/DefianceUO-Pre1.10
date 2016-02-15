using System;

namespace Server.Items
{
	public class InnocentMOTMRare : Item
	{
		[Constructable]
		public InnocentMOTMRare() : base( 0x1079 )
		{
			Weight = 9.0;
			Name = "Hide of the Innocent";
			Hue = 1109;
		}

		public InnocentMOTMRare(Serial serial) : base(serial)
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