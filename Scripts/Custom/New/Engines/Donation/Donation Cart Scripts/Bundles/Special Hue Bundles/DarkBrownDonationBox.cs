using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class DarkBrownDonationBox : MetalBox
	{
		[Constructable]
		public DarkBrownDonationBox()
		{
			Weight = 1.0;
			Hue = 1736;
			Item item = null;
			Name = "Defiance Dark Brown Member Box";

			PlaceItemIn( 16, 60, (item = new SkillBall( 25 )) );
			item.Hue = 5;
			PlaceItemIn( 28, 60, (item = new SkillBall( 25 )) );
			item.Hue = 5;
			PlaceItemIn( 41, 58, (item = new SevenGMSkillBall()) );
                        item.Hue = 1161;
                        PlaceItemIn( 53, 58, (item = new StatsBall()) );
                        item.Hue = 1161;

			PlaceItemIn( 16, 81, (item = new HoodedShroudOfShadows()) );
			item.Hue = 1736;
			item.Name = "Dark Brown Shroud of Shadows";
                        item.LootType = LootType.Blessed;

			BaseContainer cont;
			PlaceItemIn( 58, 57, (cont = new Backpack()) );
			cont.Hue = 1736;
			cont.Name = "a dark brown backpack";

			cont.PlaceItemIn( 44, 65, new SulfurousAsh(10000) );
			cont.PlaceItemIn( 77, 65, new Nightshade(10000) );
			cont.PlaceItemIn( 110, 65, new SpidersSilk(10000) );
			cont.PlaceItemIn( 143, 65, new Garlic(10000) );

			cont.PlaceItemIn( 44, 128, new Ginseng(10000) );
			cont.PlaceItemIn( 77, 128, new Bloodmoss(10000) );
			cont.PlaceItemIn( 110, 128, new BlackPearl(10000) );
			cont.PlaceItemIn( 143, 128, new MandrakeRoot(10000) );

			PlaceItemIn( 90, 58, (item = new AncientCoat()) );
			item.Hue = 1736;
			item.Name = "Dark Brown Ancient Coat";
                        item.LootType = LootType.Blessed;

		        PlaceItemIn( 74, 64, (item = new WizardGlasses()) );
                        item.Hue = Utility.RandomList(1736);
			PlaceItemIn( 103, 58, (item = new Sandals()) );
			item.Hue = Utility.RandomList(1736);
                        item.Name = "Polar Sandals";
			item.LootType = LootType.Blessed;

			PlaceItemIn( 122, 53, new SpecialDonateDye() );
			PlaceItemIn( 133, 53, new SpecialDonateDyeBeard() );

			PlaceItemIn( 156, 55, (item = new EtherealLongManeHorse()) );
			item.Hue = 1736;

			PlaceItemIn( 34, 83, (item = new HolyDeedofBlessing()) );
			item.Hue = 1736;
	                PlaceItemIn( 43, 83, (item = new CursedClothingBlessDeed()) );
			item.Hue = 1736;
			PlaceItemIn( 58, 83, (item = new SpecialHairRestylingDeed()) );
			item.Hue = 1736;
			PlaceItemIn( 73, 83, (item = new SmallBrickHouseDeed()) );
			item.Hue = 1736;
			PlaceItemIn( 88, 83, (item = new NameChangeDeed()) );
			item.Hue = 1736;
			PlaceItemIn( 103, 83, (item = new AntiBlessDeed()) );
			item.Hue = 1736;
			PlaceItemIn( 118, 83, (item = new BankCheck(100000)) );
			item.Hue = 1736;
			PlaceItemIn(130, 83, (item = new MembershipTicket()));
			item.Hue = 1736;
			((MembershipTicket)item).MemberShipTime = TimeSpan.FromDays(730);
		}

		public DarkBrownDonationBox( Serial serial ) : base( serial )
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