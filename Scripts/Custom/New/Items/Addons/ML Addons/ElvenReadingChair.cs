
using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0x2DF5,0x2DF6)]
	public class ElvenReadingChair : Item
	{
		[Constructable]
		public ElvenReadingChair() : base(0x2DF5)
		{
			Name = "a Elven Reading Chair";
			Weight = 0.0;
		}

		public ElvenReadingChair(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
}