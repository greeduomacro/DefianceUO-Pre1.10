using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x2DF3, 0x2DF4 )]
	public class GardenDonationBox : OrnateElvenBox
	{
		[Constructable]
		public GardenDonationBox()
		{
			Weight = 1.0;
			Hue = 1154;
			Item item = null;
			Name = "garden decoration chest";

			PlaceItemIn( 16, 51, (item = new FountainOfLife()) );
			PlaceItemIn( 58, 70, (item = new Beehive()) );
                        PlaceItemIn( 62, 44, (item = new PloughAddonDeed()) );

			PlaceItemIn( 44, 57, (new RareVase()) );
			PlaceItemIn( 89, 53, (new AncientFruitBowl()) );
			PlaceItemIn( 90, 81, (new LargeShell()) );
                        PlaceItemIn( 116, 33, (new WhisperingRoseDeed()) );
                        PlaceItemIn( 116, 64, (new WhisperingRoseDeed()) );

                        BaseContainer cont;
			PlaceItemIn( 131, 121, (cont = new Bag()) );
			cont.Hue = 2413;

			cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );
                        cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );
                        cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );
                        cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );
                        cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );
                        cont.PlaceItemIn( 131, 75, (item = new FlaxFlower()) );

			cont.PlaceItemIn( 140, 51, (item = new FlaxBundle()) );
                        cont.PlaceItemIn( 140, 51, (item = new FlaxBundle()) );
                        cont.PlaceItemIn( 140, 51, (item = new FlaxBundle()) );
                        cont.PlaceItemIn( 140, 51, (item = new FlaxBundle()) );

			cont.PlaceItemIn( 58, 83, (item = new PottedTree1()) );
                        cont.PlaceItemIn( 58, 83, (item = new PottedTree1()) );
			cont.PlaceItemIn( 73, 83, (item = new PottedPlant()) );
                        cont.PlaceItemIn( 73, 83, (item = new PottedPlant()) );
                        cont.PlaceItemIn( 29, 68, (item = new PottedTree()) );
                        cont.PlaceItemIn( 29, 68, (item = new PottedTree()) );
                        cont.PlaceItemIn( 73, 83, (item = new PottedPlant1()) );
                        cont.PlaceItemIn( 73, 83, (item = new PottedPlant1()) );

			cont.PlaceItemIn( 47, 53, (item = new LillyPad1()) );
                        cont.PlaceItemIn( 47, 53, (item = new LillyPad1()) );
                        cont.PlaceItemIn( 47, 53, (item = new LillyPad1()) );
                        cont.PlaceItemIn( 47, 53, (item = new LillyPad1()) );
                        cont.PlaceItemIn( 47, 53, (item = new LillyPad1()) );
                        cont.PlaceItemIn( 47, 53, (item = new LillyPad1()) );

                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );
                        cont.PlaceItemIn( 29, 34, (item = new RoseVine()) );

		}

		public GardenDonationBox( Serial serial ) : base( serial )
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