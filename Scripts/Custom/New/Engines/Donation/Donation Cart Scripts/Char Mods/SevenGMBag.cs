using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	//[DynamicFliping]
	//[Flipable( 0x9A8, 0xE80 )]
	public class SevenGMBag : Bag
	{
		[Constructable]
		public SevenGMBag()
		{
			Weight = 1.0;
			Hue = 1154;
			Item item = null;
			Name = "Grandmasters Bag";

			PlaceItemIn( 16, 51, (item = new StatsBall()) );
                        PlaceItemIn( 16, 60, (item = new SevenGMSkillBall()) );

		}

		public SevenGMBag( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}