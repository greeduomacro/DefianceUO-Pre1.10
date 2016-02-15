using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a glowing ratman corpse" )]
	public class RatmanMage : BaseCreature
	{
		[Constructable]
		public RatmanMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ratman" );
			Body = 0x8F;
			BaseSoundID = 437;

			SetStr( 146, 180 );
			SetDex( 101, 130 );
			SetInt( 186, 210 );

			SetHits( 88, 108 );

			SetDamage( 7, 14 );

			SetSkill( SkillName.EvalInt, 70.1, 95.0 );
			SetSkill( SkillName.Magery, 70.1, 95.0 );
			SetSkill( SkillName.MagicResist, 65.1, 90.0 );
			SetSkill( SkillName.Tactics, 50.1, 75.0 );
			SetSkill( SkillName.Wrestling, 50.1, 75.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 44;

			PackGold( 250, 300 );
			PackReg( 1, 10 );
			PackArmor( 0, 3 );
			PackWeapon( 0, 3 );

                        switch ( Utility.Random( 50 ))
        	 {
           		case 0: PackItem( new StatueEast() ); break;
			case 1: PackItem( new StatueEast2() ); break;
			case 2: PackItem( new StatueNorth() ); break;
			case 3: PackItem( new StatuePegasus() ); break;
			case 4: PackItem( new StatuePegasus2() ); break;
			case 5: PackItem( new StatueSouth() ); break;
			case 6: PackItem( new StatueSouth2() ); break;
        	 }

		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public RatmanMage( Serial serial ) : base( serial )
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

			if ( Body == 42 )
			{
				Body = 0x8F;
				Hue = 0;
			}
		}
	}
}