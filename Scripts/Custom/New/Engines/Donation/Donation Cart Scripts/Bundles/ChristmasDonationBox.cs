using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	//[DynamicFliping]
	//[Flipable( 0x2DF3, 0x2DF4 )]
	public class ChristmasDonationBox : GiftBox
	{
		[Constructable]
		public ChristmasDonationBox()
		{
			Weight = 1.0;
			//Hue = 38;
			Item item = null;
			Name = "christmas gift box";

			PlaceItemIn( 35, 30, (item = new RedStocking()) );
                        PlaceItemIn( 48, 30, (item = new RedStocking()) );
			PlaceItemIn( 35, 75, (item = new FreshGinger()) );
                        PlaceItemIn( 46, 63, (item = new FreshGinger()) );

                        PlaceItemIn( 80, 26, (item = new GingerbreadCookie()) );
                        PlaceItemIn( 80, 36, (item = new GingerbreadCookie()) );
                        PlaceItemIn( 80, 46, (item = new GingerbreadCookie()) );
                        PlaceItemIn( 80, 56, (item = new GingerbreadCookie()) );
                        PlaceItemIn( 80, 66, (item = new GingerbreadCookie()) );
                        PlaceItemIn( 80, 76, (item = new GingerbreadCookie()) );

                        PlaceItemIn( 146, 31, (item = new RedCandyCane()) );
                        PlaceItemIn( 146, 41, (item = new RedCandyCane()) );
                        PlaceItemIn( 146, 51, (item = new RedCandyCane()) );
                        PlaceItemIn( 146, 61, (item = new RedCandyCane()) );

                        BaseContainer cont;
			PlaceItemIn( 123, 55, (cont = new Bag()) );
			cont.Hue = 38;

			cont.PlaceItemIn( 131, 75, (item = new ChristmasHouseAddonDeed()) );
                        cont.PlaceItemIn( 131, 85, (item = new ChristmasCastleAddonDeed()) );


		}

		public ChristmasDonationBox( Serial serial ) : base( serial )
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