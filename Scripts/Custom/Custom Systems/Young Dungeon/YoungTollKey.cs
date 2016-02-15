using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class YoungTollKey1 : SelfDestructingItem
	{
		[Constructable]
		public YoungTollKey1() : base()
		{
			Weight = 5.0;
			Name = "an azure key";
			Hue = 1154;
			LootType = LootType.Blessed;
			ItemID = 0x2002;

			ShowTimeLeft = true;

			TimeLeft = 172800;
			Running = true;


		}

		public YoungTollKey1(Serial serial) : base(serial)
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

public class YoungTollKey2 : SelfDestructingItem
	{
		[Constructable]
		public YoungTollKey2() : base()
		{
			Weight = 5.0;
			Name = "a diamond key";
			Hue = 1150;
			LootType = LootType.Blessed;
			ItemID = 0x2002;

			ShowTimeLeft = true;

			TimeLeft = 172800;
			Running = true;



		}

		public YoungTollKey2(Serial serial) : base(serial)
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

public class YoungTollKey3 : SelfDestructingItem
	{
		[Constructable]
		public YoungTollKey3() : base()
		{
			Weight = 5.0;
			Name = "a dark key";
			Hue = 1175;
			LootType = LootType.Blessed;
			ItemID = 0x2002;

			ShowTimeLeft = true;

			TimeLeft = 172800;
			Running = true;



		}

		public YoungTollKey3(Serial serial) : base(serial)
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