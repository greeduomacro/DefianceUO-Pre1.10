using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a lich lord corpse" )]
	public class LichLord : BaseCreature
	{
		[Constructable]
		public LichLord() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lich lord";
			Body = 79;
			BaseSoundID = 412;

			SetStr( 416, 505 );
			SetDex( 146, 165 );
			SetInt( 566, 655 );

			SetHits( 250, 303 );

			SetDamage( 14, 19 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 150.5, 200.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 80.0 );

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 50;

			PackGold( 400, 700 );
			PackReg( Utility.RandomMinMax( 10, 20 ) );
			PackScroll( 0, 8 );
			PackScroll( 0, 8 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
			PackSlayer();
                        PackJewel( 0.01 );
        	        PackItem( new IDWand() );

                if (1 > Utility.Random(200)) PackItem(new SeedBox());
                }

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 4; } }

		public LichLord( Serial serial ) : base( serial )
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