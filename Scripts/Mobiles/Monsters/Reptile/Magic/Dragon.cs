using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class Dragon : BaseCreature
	{
		[Constructable]
		public Dragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dragon";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;

			SetStr( 792, 825 );
			SetDex( 88, 110 );
			SetInt( 437, 475 );

			SetDamage( 16, 22 );

			SetSkill( SkillName.Anatomy, 11.7, 14.4 );
			SetSkill( SkillName.Meditation, 32.5, 35.0 );
			SetSkill( SkillName.EvalInt, 10.1, 15.3 );
			SetSkill( SkillName.Magery, 40.1, 49.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 91.0, 93.2 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 93.9;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			PackGold( 600, 800 );
			PackGem( 2, 5 );
			PackSlayer();
                        PackJewel( 0.01 );

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}
			
			if ( Utility.Random( 100 ) == 0 )
				PackItem( new RareCreamCarpet( PieceType.NECorner ));
         }

                public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 30; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 7; } }
		public override ScaleType ScaleType{ get{ return ( Body == 12 ? ScaleType.Yellow : ScaleType.Red ); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }

		public Dragon( Serial serial ) : base( serial )
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