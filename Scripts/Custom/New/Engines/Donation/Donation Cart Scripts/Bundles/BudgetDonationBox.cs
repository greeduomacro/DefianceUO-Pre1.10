using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class BudgetDonationBox : MetalBox
	{
		[Constructable]
		public BudgetDonationBox()
		{
			Weight = 1.0;
			Hue = 1109;
			Item item = null;
			Name = "Defiance Budget Box";

			PlaceItemIn( 16, 60, (item = new SkillBall( 25 )) );
			item.Hue = 38;

			PlaceItemIn( 18, 80, (item = new HoodedShroudOfShadows()) );
                        item.LootType = LootType.Blessed;

			BaseContainer cont;
			PlaceItemIn( 64, 50, (cont = new Backpack()) );
			cont.Hue = 0;
			cont.Name = "a backpack";

			cont.PlaceItemIn( 44, 65, new SulfurousAsh(1000) );
			cont.PlaceItemIn( 77, 65, new Nightshade(1000) );
			cont.PlaceItemIn( 110, 65, new SpidersSilk(1000) );
			cont.PlaceItemIn( 143, 65, new Garlic(1000) );

			cont.PlaceItemIn( 44, 128, new Ginseng(1000) );
			cont.PlaceItemIn( 77, 128, new Bloodmoss(1000) );
			cont.PlaceItemIn( 110, 128, new BlackPearl(1000) );
			cont.PlaceItemIn( 143, 128, new MandrakeRoot(1000) );

			PlaceItemIn( 93, 60, new SpecialDonateDye() );

			PlaceItemIn( 50, 80, new ClothingBlessDeed() );
			PlaceItemIn( 60, 80, new GuildDeed() );
			PlaceItemIn( 70, 80, new SmallBrickHouseDeed() );
			PlaceItemIn( 80, 80, new NameChangeDeed() );

			PlaceItemIn(90, 80, (item = new MembershipTicket()));
			((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(180);

			PlaceItemIn( 110, 50, new BankCheck(100000) );
		}

		public BudgetDonationBox( Serial serial ) : base( serial )
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