using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a drake corpse" )]
	public class Drake : BaseCreature
	{
		[Constructable]
		public Drake () : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "a drake";
			Body = Utility.RandomList( 60, 61 );
			BaseSoundID = 362;

			SetStr( 401, 430 );
			SetDex( 133, 152 );
			SetInt( 101, 140 );

			SetDamage( 11, 17 );

			SetSkill( SkillName.Anatomy, 11.7, 14.4 );
			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 65.1, 88.5 );
			SetSkill( SkillName.Wrestling, 68.1, 81.7 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 46;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new Garlic( Utility.Random( 5, 10 ) ) ); break;
				case 1: PackScroll( 4, 7 ); break;
				case 2: PackGem(); break;
				case 3: PackGem(); break;
				case 4: PackGem(); break;
			}

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 84.3;

			PackGold( 300, 350 );
			PackWeapon( 0, 4 );
			PackWeapon( 0, 4 );
				
				if ( Utility.Random( 125 ) == 0 )
				PackItem( new RareCreamCarpet( PieceType.Centre ));
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int TreasureMapLevel{ get{ return 2; } }
		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 25; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override int Scales{ get{ return 4; } }
		public override ScaleType ScaleType{ get{ return ( Body == 60 ? ScaleType.Yellow : ScaleType.Red ); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public Drake( Serial serial ) : base( serial )
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