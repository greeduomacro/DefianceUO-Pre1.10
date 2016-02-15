using System;

namespace Server.Items
{
	public class MongahRare : Item
	{
		[Constructable]
		public MongahRare() : base( 0x19B9 )
		{
			Weight = 9.0;
			Name = "Mon'gah's ore";
			Hue = 0x37;
		}

		public MongahRare(Serial serial) : base(serial)
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