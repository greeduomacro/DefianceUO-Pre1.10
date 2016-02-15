using System;
using Server.Mobiles;
using Server.Multis.Deeds;

namespace Server.Items
{
	[DynamicFliping]
	[Flipable( 0x9A8, 0xE80 )]
	public class PotionBundle : MetalChest
	{
		[Constructable]
		public PotionBundle()
		{
			ItemID = 0x9A8;
			Weight = 1.0;
			Hue = 0;
			Name = "Box of Potions";

			//PlaceItemIn( 16, 60, (item = new SkillBall( 25 )) );
			//item.Hue = 38;

                        PotionKeg keg;
                        PlaceItemIn( 18, 105, (keg =new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 23, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 28, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 33, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 38, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.HealGreater;
                        keg.Held = 100;
                        keg.Hue = 54;
                        PlaceItemIn( 58, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 63, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 68, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 73, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 78, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.CureGreater;
                        keg.Held = 100;
                        keg.Hue = 43;
                        PlaceItemIn( 98, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 103, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 108, 105, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.RefreshTotal;
                        keg.Held = 100;
                        keg.Hue = 38;
                        PlaceItemIn( 18, 129, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.StrengthGreater;
                        keg.Held = 100;
                        keg.Hue = 1001;
                        PlaceItemIn( 28, 129, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.StrengthGreater;
                        keg.Held = 100;
                        keg.Hue = 1001;
                        PlaceItemIn( 48, 129, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.AgilityGreater;
                        keg.Held = 100;
                        keg.Hue = 99;
                        PlaceItemIn( 58, 129, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.AgilityGreater;
                        keg.Held = 100;
                        keg.Hue = 99;
                        PlaceItemIn( 88, 129, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.ExplosionGreater;
                        keg.Held = 100;
                        keg.Hue = 15;
                        PlaceItemIn( 98, 129, (keg = new PotionKeg()) );
                        keg.Type = PotionEffect.PoisonDeadly;
                        keg.Held = 100;
                        keg.Hue = 62;


                        //PlaceItemIn( 103, 58, (item = new Sandals()) );
			//item.Hue = Utility.RandomList(5, 70, 90, 110);
			//item.LootType = LootType.Blessed;

			//PlaceItemIn( 122, 53, new SpecialDonateDye() );

		}

		public PotionBundle( Serial serial ) : base( serial )
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