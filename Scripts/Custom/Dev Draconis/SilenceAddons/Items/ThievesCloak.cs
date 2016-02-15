using System;
using Server.Items;

namespace Server.Items
{
	public class ThievesCloak : BaseCloak
	{
		[Constructable]
		public ThievesCloak() : base(0x1515)
		{
			Name = "Thieves Cloak of Silence";
			Weight = 2.0;
			Hue = 1000;
		}

		public ThievesCloak(Serial serial) : base(serial)
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