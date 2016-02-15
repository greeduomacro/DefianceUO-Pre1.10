using System;
using Server.Items;

namespace Server.Items
{
	public class RoyalCloak : BaseCloak
	{
		[Constructable]
		public RoyalCloak() : base(0x2B05)
		{
			Name = "a royal cloak";
			Weight = 1.0;
			Hue = 0;
		}

		public RoyalCloak(Serial serial) : base(serial)
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