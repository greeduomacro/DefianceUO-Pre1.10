using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a blood elemental corpse" )]
	public class AncientBloodElemental : BaseCreature
	{
		public override bool IsScaryToPets{ get{ return true; } }

		[Constructable]
		public AncientBloodElemental () : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.1, 0.2 )
		{
			Name = "an ancient blood elemental";
			Body = 159;
			BaseSoundID = 278;
			Kills = 10;
			ShortTermMurders = 10;

			SetStr( 750, 750 );
			SetDex( 175, 175 );
			SetInt( 750, 750 );

			SetHits( 850, 1050 );

			SetDamage( 18, 23 );

			SetSkill( SkillName.EvalInt, 75.1, 85.1 );
			SetSkill( SkillName.Magery, 75.1, 85.1 );
			SetSkill( SkillName.Meditation, 30.0, 40.0 );
			SetSkill( SkillName.MagicResist, 100.0, 100.0 );
			SetSkill( SkillName.Tactics, 125.0, 125.0 );
			SetSkill( SkillName.Wrestling, 150.0, 150.0 );

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 75;

			PackGold( 500, 2000 );
			PackReg( 1, 15 );
			PackScroll( 4, 8 );
			PackScroll( 4, 8 );
			PackScroll( 4, 8 );
			PackWeapon( 0, 5 );
			PackWeapon( 0, 5 );
			PackSlayer();

				if ( 0.02 > Utility.RandomDouble() )
					PackItem( new DarkIronWire() );

			}

		public override void OnDeath( Container c )
		{
			if ( Utility.Random( 250 ) <  1 )
				c.DropItem( new BloodCurtain() );

            		base.OnDeath( c );
		}

		public override int TreasureMapLevel{ get{ return 5; } }

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
				damage *= 2;
		}

		public AncientBloodElemental( Serial serial ) : base( serial )
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