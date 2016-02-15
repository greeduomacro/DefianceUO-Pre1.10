
using System;
using System.Collections;
using Server.Items;

namespace Server.Items
{
	public class SpecialQuestSandals : Sandals
	{
		[Constructable]
		public SpecialQuestSandals() : base()
		{
			Weight = 9.0;

			switch (Utility.Random( 5 ))
			{
				case 0:	Name = "Eagle's sandals";break;
				case 1:	Name = "Wolf's sandals";break;
				case 2:	Name = "Qwert's sandals";break;
				case 3:	Name = "Phoenix's sandals";break;
				case 4: Name = "Troy's sandals";break;
			}

			Hue = Utility.RandomList( 1365, 38, 2006 );

		}

		public SpecialQuestSandals(Serial serial) : base(serial)
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