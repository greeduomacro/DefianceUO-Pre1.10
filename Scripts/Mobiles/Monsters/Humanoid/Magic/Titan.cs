using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a titans corpse" )]
	public class Titan : BaseCreature
	{
		[Constructable]
		public Titan() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a titan";
			Body = 76;
			BaseSoundID = 609;

			SetStr( 536, 585 );
			SetDex( 126, 145 );
			SetInt( 281, 355 );

			SetHits( 322, 351 );

			SetDamage( 13, 16 );

			SetSkill( SkillName.EvalInt, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );
			SetSkill( SkillName.MagicResist, 80.2, 110.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.Wrestling, 40.1, 50.0 );

			Fame = 11500;
			Karma = -11500;

			VirtualArmor = 40;

			PackGold( 300, 600 );
			PackArmor( 0, 4 );
			PackWeapon( 0, 4 );
			PackSlayer();
                        PackJewel( 0.01 );

					switch ( Utility.Random( 500 ))
        		 {
           			case 0: PackItem( new RuinedPainting() ); break;
        		 }
			if (2 > Utility.Random(100)) PackItem(new BloodPentagramPart(Utility.RandomMinMax(19, 23)));
		}

		public override int Meat{ get{ return 4; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public Titan( Serial serial ) : base( serial )
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