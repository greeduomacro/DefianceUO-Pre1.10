
using System;
using System.Collections;

namespace Server.Items
{
	public class Quiver : BaseContainer
	{
		public override int DefaultGumpID { get { return 65; } }
		public override int DefaultDropSound { get { return 0x48; } }
		public override int MaxWeight { get { return 250; } }
		public override int DefaultMaxWeight { get { return 250; } }


		public override bool OnEquip(Mobile from)
		{
			return base.OnEquip(from);
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved(parent);
		}

		public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
		{
			if (item is Arrow || item is Bolt)
				return base.OnDragDropInto(from, item, p);
			else
				return false;
		}

		[Constructable]
		public Quiver() : base( 11011 )
		{
			Name = "quiver";
			Weight = 1;
			Layer = Layer.Cloak;
			LootType = LootType.Blessed;
		}

		public Quiver(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}