using System;

namespace Server.Items
{
	public class UnicornRareMOTM : Item
	{
		//public override string DefaultName{ get{ return "a flask of unicorn perfume"; } }

		[Constructable]
		public UnicornRareMOTM() : base()
		{
			Name = "a flask of unicorn perfume";
			Weight = 9.0;
			switch ( Utility.Random(4) )
			{
				case 0: 	ItemID = 0x182A;
						Hue = 1141;break;
				case 1:	ItemID = 0x182E;
						Hue = 1124;break;
				case 2:	ItemID = 0x1834;
						Hue = 1150;break;
				case 3:	ItemID = 0x1838;
						Hue = 1159;break;
			}
		}

		public UnicornRareMOTM(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}