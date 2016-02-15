using System;
using Server;

namespace Server.Items
{
	public class StackedArrows : Item
	{
		[Constructable]
		public StackedArrows() : base(7165)
		{
			Name = "stacked bolts";
			Weight = 10;
		}

		public StackedArrows(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}
	public class BronzeIngots : Item
	{
		[Constructable]
		public BronzeIngots()
			: base(7141)
		{
			Name = "bronze ingots";
			Weight = 10;
		}

		public BronzeIngots(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}

	public class StackedShafts : Item
	{
		[Constructable]
		public StackedShafts()
			: base(7126)
		{
			Name = "shafts";
			Weight = 10;
		}

		public StackedShafts(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
			if (Name == "7126") Name = "shafts";
		}
	}
	public class RareFeathers : Item
	{
		[Constructable]
		public RareFeathers()
			: base(7123)
		{
			Name = "feathers";
			Weight = 10;
		}

		public RareFeathers(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}
	public class BoneBenchWestPart : Item
	{
		[Constructable]
		public BoneBenchWestPart()
			: base(10843)
		{
			Name = "bone bench";
			Weight = 10;
		}

		public BoneBenchWestPart(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}
	public class BoneBenchEastPart : Item
	{
		[Constructable]
		public BoneBenchEastPart()
			: base(10842)
		{
			Name = "bone bench";
			Weight = 10;
		}

		public BoneBenchEastPart(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}
	public class MirrorEast : Item
	{
		[Constructable]
		public MirrorEast()
			: base(10877)
		{
			Name = "mirror";
			Weight = 10;
		}

		public MirrorEast(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}
	public class MirrorNorth : Item
	{
		[Constructable]
		public MirrorNorth()
			: base(10875)
		{
			Name = "mirror";
			Weight = 10;
		}

		public MirrorNorth(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (Weight == 11) Weight = 10;
		}
	}





}