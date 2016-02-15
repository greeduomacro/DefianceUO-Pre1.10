using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a white wyrm corpse" )]
	public class WhiteWyrm : BaseCreature
	{
		[Constructable]
		public WhiteWyrm () : base( AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Body = Utility.RandomList( 49, 180 );
			Name = "a white wyrm";
			BaseSoundID = 362;

			SetStr( 721, 760 );
			SetDex( 102, 130 );
			SetInt( 384, 425 );

			SetHits( 400, 436 );

			SetDamage( 18, 26 );

			SetSkill( SkillName.Anatomy, 11.7, 14.4 );
			SetSkill( SkillName.Meditation, 32.5, 35.0 );
			SetSkill( SkillName.EvalInt, 10.1, 15.3 );
			SetSkill( SkillName.Magery, 99.2, 100.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 99.8 );
			SetSkill( SkillName.Wrestling, 91.1, 100.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 70;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 96.3;

			int gems = Utility.RandomMinMax( 1, 5 );

			for ( int i = 0; i < gems; ++i )
				PackGem();

			PackGold( 600, 1000 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
                        PackJewel( 0.01 );


			switch ( Utility.Random( 200 ))
        		 {
           			case 0: PackItem( new LampPost1() ); break;
        		 }
			
			if ( Utility.Random( 100 ) == 0 )
			PackItem( new RareCreamCarpet( PieceType.NWCorner ));
	}

		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 30; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 9; } }
		public override ScaleType ScaleType{ get{ return ScaleType.White; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Gold; } }

		public WhiteWyrm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}