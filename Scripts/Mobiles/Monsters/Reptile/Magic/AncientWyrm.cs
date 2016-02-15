using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ancient wyrm corpse" )]
	public class AncientWyrm : BaseCreature
	{
		[Constructable]
		public AncientWyrm () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Name = "an ancient wyrm";
			Body = Utility.RandomList( 46, 172 );
			BaseSoundID = 362;

			SetStr( 1103, 1400 );
			SetDex( 99, 120 );
			SetInt( 734, 752 );

			SetDamage( 29, 35 );

			SetSkill( SkillName.EvalInt, 72.8, 78.2 );
			SetSkill( SkillName.Magery, 85.8, 99.3 );
			SetSkill( SkillName.Meditation, 61.7, 64.4 );
			SetSkill( SkillName.MagicResist, 120.1, 133.0 );
			SetSkill( SkillName.Tactics, 99.6, 100.0 );
			SetSkill( SkillName.Wrestling, 98.6, 99.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 45;

			int gems = Utility.RandomMinMax( 2, 6 );

			for ( int i = 0; i < gems; ++i )
				PackGem();

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
                        PackJewel( 0.02 );
                        PackJewel( 0.01 );
                        //PackJewel( 0.01 );

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

			switch ( Utility.Random( 2 ) )
			{
				case 0: PackWeapon( 1, 5 ); break;
				case 1: PackArmor( 1, 5 ); break;
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
				case 0: PackWeapon( 4, 5 ); break;
				case 1: PackArmor( 4, 5 ); break;
			}

                          switch ( Utility.Random( 250 ))
        		 {
           			case 0: PackItem( new HoodedShroudOfShadows() ); break;
                         }

				if ( 0.01 > Utility.RandomDouble() )
					PackItem( new IDWand() );
	       }

		public override void OnDeath( Container c )
	  	{
			if ( Utility.Random( 100 ) <  1 )
				c.DropItem( new RareCreamCarpet( PieceType.SECorner ) );

			base.OnDeath( c );
	  	}

                public override bool HasBreath{ get{ return true; } } // fire breath enabled
                public override bool AutoDispel{ get{ return true; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Hides{ get{ return 40; } }
		public override int Meat{ get{ return 19; } }
		public override int Scales{ get{ return 25; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 3;
		}

		public AncientWyrm( Serial serial ) : base( serial )
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