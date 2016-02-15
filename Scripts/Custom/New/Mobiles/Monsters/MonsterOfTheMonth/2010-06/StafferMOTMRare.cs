
using System;
using System.Collections;
using Server.Items;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xB4F, 0xB4E, 0xB50, 0xB51 )]
	public class StafferMOTMRare : Item
	{
		[Constructable]
		public StafferMOTMRare() : base( 0xB4F )
		{
			Weight = 9.0;

			switch (Utility.Random( 5 ))
			{
				case 0:	Name = "a councelor's luxury chair";
						Hue = 1156; break;
				case 1:	Name = "a gamemaster's luxury chair";
						Hue = 38; break;
				case 2:	Name = "a seer's luxury chair";
						Hue = 1367; break;
				case 3:	Name = "an admin's luxury chair";
						Hue = 1150; break;
				case 4:	Name = "a developer's luxury chair";
						Hue = 1175; break;
			}

		}

		public StafferMOTMRare(Serial serial) : base(serial)
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