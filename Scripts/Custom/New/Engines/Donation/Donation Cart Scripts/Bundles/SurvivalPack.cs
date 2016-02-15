using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class SurvivalPack : Backpack
	{
		[Constructable]
		public SurvivalPack()
		{
			Weight = 1.0;
			Hue = 1871;
			Item item = null;
			Name = "a survival pack";


			PlaceItemIn( 60, 131, (item = new SkillBall( 10 )) );
			item.Hue = 1871;
                        item.Name = "a +10 SkillBall - Works on skills 90.0 and below";

			PlaceItemIn( 110, 103, (item = new SkillBall( 50 )) );
			item.Hue = 1871;
                        item.Name = "a +50 SkillBall - Works on skills 50.0 and below";

			PlaceItemIn( 46, 65, (item = new SkillBall( 25 )) );
			item.Hue = 1871;
			item.Name = "a +25 SkillBall - Works on skills 75.0 and below";

			BaseContainer cont;
			PlaceItemIn( 131, 121, (cont = new Bag()) );
			cont.Hue = 2413;

			cont.PlaceItemIn( 29, 39, new SulfurousAsh(5000) );
			cont.PlaceItemIn( 29, 64, new Nightshade(5000) );
			cont.PlaceItemIn( 29, 89, new SpidersSilk(5000) );

			cont.PlaceItemIn( 60, 64, new Garlic(5000) );
			cont.PlaceItemIn( 60, 89, new Ginseng(5000) );

			cont.PlaceItemIn( 88, 39, new Bloodmoss(5000) );
			cont.PlaceItemIn( 88, 64, new BlackPearl(5000) );
			cont.PlaceItemIn( 88, 89, new MandrakeRoot(5000) );
		}

		public SurvivalPack( Serial serial ) : base( serial )
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