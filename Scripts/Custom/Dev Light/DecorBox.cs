using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class DecorBox : MetalBox
	{
		[Constructable]
		public DecorBox()
		{
			Weight = 1.0;
			Hue = 1150;
			Item item = null;
			Name = "Defiance Decoration Member Box";


			PlaceItemIn( 10, 53, new FishTankPart1() );
			PlaceItemIn( 25, 53, new FishTankPart2() );
			PlaceItemIn( 50, 53, new AncientBedPart1() );
			PlaceItemIn( 85, 53, new AncientBedPart2() );
			PlaceItemIn( 90, 53, new AncientFruitBowl() );
			PlaceItemIn( 110, 53, new StoneSculpture() );

			BaseContainer cont;
			PlaceItemIn( 58, 57, (cont = new Backpack()) );
			cont.Hue = 2213;
			cont.Name = "a golden backpack";


			PlaceItemIn(130, 83, (item = new MembershipTicket()));
			item.Hue = 2213;
			((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(180);
		}

		public DecorBox( Serial serial ) : base( serial )
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