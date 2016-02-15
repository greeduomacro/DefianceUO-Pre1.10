using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dark spider corpse" )]
	public class DarkSpider : BaseCreature
	{
		[Constructable]
		public DarkSpider () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dark spider";
			Body = 173;
			BaseSoundID = 1170;

			SetStr( 196, 220 );
			SetDex( 126, 145 );
			SetInt( 286, 310 );

			SetHits( 1000 );

			SetDamage( 18, 22 );


			SetSkill( SkillName.EvalInt, 100.0, 110.0 );
			SetSkill( SkillName.Magery, 100.0, 110.0 );
			SetSkill( SkillName.Meditation, 65.1, 80.0 );
			SetSkill( SkillName.MagicResist, 115.0, 125.0 );
			SetSkill( SkillName.Tactics, 55.1, 70.0 );
			SetSkill( SkillName.Wrestling, 75.0 );

			Fame = 10000;
			Karma = 10000;

			PackItem( new SpidersSilk( 30 ) );

			if ( Utility.Random( 125 ) < 1 ) PackItem( new Leaves() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		//Melee damage from controlled mobiles is divided by 15
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 15;
			}
		}

		//Melee damage to controlled mobiles is multiplied by 9
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 9;
			}
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public DarkSpider( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 1170;
		}
	}
}