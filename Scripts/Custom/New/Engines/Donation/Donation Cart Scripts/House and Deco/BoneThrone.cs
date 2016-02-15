///////////////////
//  Bone Throne
//  by The Magi
//  Version 1.0
///////////////////

using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0x2A58,0x2A59)]
	public class BoneThrone : Item
	{
		[Constructable]
		public BoneThrone() : base(0x2A58)
		{
			Name = "a Bone Throne";
			Weight = 1.0;
		}

		public BoneThrone(Serial serial) : base(serial)
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