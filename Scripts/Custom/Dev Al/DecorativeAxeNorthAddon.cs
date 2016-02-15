using System;
using Server;

namespace Server.Items
{
	public class DecorativeAxeNorthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DecorativeAxeNorthDeed(); } }

		[Constructable]
		public DecorativeAxeNorthAddon()
		{
			AddComponent(new AddonComponent(0x1568), 0, 0, 0);
			AddComponent(new AddonComponent(0x1569), 1, 0, 0);
		}

		public DecorativeAxeNorthAddon(Serial serial)
			: base(serial)
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

	public class DecorativeAxeNorthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DecorativeAxeNorthAddon(); } }

		[Constructable]
		public DecorativeAxeNorthDeed()
		{
			Name = "decorative weapons (north)";
		}

		public DecorativeAxeNorthDeed(Serial serial) : base(serial)
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