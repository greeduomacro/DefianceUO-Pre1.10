using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class UltimateDecorationDonationBox : MetalBox
	{
		[Constructable]
		public UltimateDecorationDonationBox()
		{
			Weight = 1.0;
			Hue = 1154;
			Item item = null;
			Name = "ultimate decoration box";

			PlaceItemIn( 27, 63, (item = new GardenDonationBox()) );
			PlaceItemIn( 131, 66, (item = new DungeonDonationBox()) );
                        PlaceItemIn( 82, 83, (item = new RareVase()) );

                        //BaseContainer cont;
			//PlaceItemIn( 131, 121, (cont = new Bag()) );
			//cont.Hue = 2413;

			//cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );


		}

		public UltimateDecorationDonationBox( Serial serial ) : base( serial )
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