using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a balron corpse" )]
	public class Balron : BaseCreature
	{
		[Constructable]
		public Balron () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Name = NameList.RandomName( "balron" );
			Body = 9;
                        Hue = Utility.RandomList(1175 , 2412);
			BaseSoundID = 357;

			SetStr( 1005, 1183 );
			SetDex( 187, 238 );
			SetInt( 151, 247 );

			SetDamage( 29, 35 );

			SetSkill( SkillName.Anatomy, 25.1, 35.0 );
			SetSkill( SkillName.EvalInt, 80.1, 93.6 );
			SetSkill( SkillName.Magery, 96.5, 99.1 );
			SetSkill( SkillName.Meditation, 27.1, 50.8 );
			SetSkill( SkillName.MagicResist, 116.5, 122.0 );
			SetSkill( SkillName.Tactics, 90.1, 99.7 );
			SetSkill( SkillName.Wrestling, 91.2, 99.7 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 82;

                               switch ( Utility.Random( 100 ))
        	             {
           			case 0: PackItem( new PicnicBasket() ); break;

            		     }

			switch ( Utility.Random( 7 ) )
			{
				case 0: PackItem( new GreaterCurePotion() ); break;
				case 1: PackItem( new GreaterPoisonPotion() ); break;
				case 2: PackItem( new GreaterHealPotion() ); break;
				case 3: PackItem( new GreaterStrengthPotion() ); break;
				case 4: PackItem( new GreaterAgilityPotion() ); break;
			}

                        PackGold( 1400, 2000 );
			PackSlayer();
			PackSlayer();
                        PackJewel( 0.01 );
                        PackJewel( 0.01 );

			switch ( Utility.Random( 7 ) )
			{
				case 0: PackReg( 29 ); break;
				case 1: PackScroll( 8, 8 ); break;
				case 2: PackScroll( 8, 8 ); break;
				case 3: PackReg( 37 ); break;
				case 4: PackReg( 15 ); break;
			}
			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 0, 5 ); break;
				case 1: PackArmor( 0, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
			}
			switch ( Utility.Random( 10 ) )
			{
				case 0: PackWeapon( 2, 5 ); break;
				case 1: PackArmor( 2, 5 ); break;
			}

				if ( 0.01 > Utility.RandomDouble() )
					PackItem( new IDWand() );
	       	}

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 75 ) <  1 )
				c.DropItem( new BasicPinkCarpet( PieceType.Centre ) );

			base.OnDeath( c );
	  	}


                public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
		}

		public Balron( Serial serial ) : base( serial )
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