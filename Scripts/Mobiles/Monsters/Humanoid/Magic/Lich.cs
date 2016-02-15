using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a lich corpse" )]
	public class Lich : BaseCreature
	{
		[Constructable]
		public Lich() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lich";
			Body = 24;
			BaseSoundID = 0x19C; // t2a sound

			SetStr( 171, 200 );
			SetDex( 126, 145 );
			SetInt( 276, 305 );

			SetHits( 103, 120 );

			SetDamage( 10, 16 );

			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.Meditation, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 80.1, 100.0 );
			SetSkill( SkillName.Tactics, 70.1, 90.0 );

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 50;

			PackReg( Utility.RandomMinMax( 1, 10 ) );
			PackGem();
			PackGold( 200, 250 );
			PackScroll( 4, 7 );
			PackArmor( 0, 4 );
			PackWeapon( 0, 4 );
			PackSlayer();

                           switch ( Utility.Random( 25 ))
        		 {
           			case 0: PackItem( new IDWand() ); break;
        		 }
    		if (2 > Utility.Random(100)) PackItem(new BloodPentagramPart(Utility.RandomMinMax(29, 33)));
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public Lich( Serial serial ) : base( serial )
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