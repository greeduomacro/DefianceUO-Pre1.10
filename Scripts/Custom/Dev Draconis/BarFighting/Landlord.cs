using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Landlord : BaseBoss
	{
		[Constructable]
		public Landlord() : base( AIType.AI_Melee )
		{
			Name = NameList.RandomName( "male" );
			Title = "the Landlord";
			Hue = Utility.RandomSkinHue();
			BodyValue = 400;

			ActiveSpeed = 1.5;

			SetStr( 200 );
			SetDex( 100 );
			SetInt( 200 );

			SetHits( 350 );

			SetDamage( 20, 30 );

            		SetSkill( SkillName.Tactics, 100.0 );
            		SetSkill( SkillName.Macing, 120.0 );

			AddItem( new HalfApron() );
			AddItem( new FancyShirt() );
			AddItem( new Shoes() );
			AddItem( new LongPants() );

			Club w = new Club();
			w.Movable = false;
			w.LootType = LootType.Blessed;
			w.Name = "old baseball bat";
			AddItem( w );
		}

		public override bool CanDestroyObstacles { get { return true; } }
		public override int CanBandageSelf{ get { return 50; } }

		public Landlord( Serial serial ) : base( serial )
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