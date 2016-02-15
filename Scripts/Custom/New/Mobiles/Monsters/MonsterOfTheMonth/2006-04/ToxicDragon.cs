using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a toxic corpse" )]
	public class ToxicDragon : BaseCreature
	{
		[Constructable]
		public ToxicDragon() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a decaying dragon";
			Body = 104;
			BaseSoundID = 0x488;
			Hue = 72;

			SetStr( 96, 120 );
			SetDex( 66, 85 );
			SetInt( 16, 30 );

			SetHits( 1000 );
			SetMana( 0 );

			SetDamage( 20, 25 );

			SetSkill( SkillName.MagicResist, 110.0, 120.0 );
			SetSkill( SkillName.Tactics, 80.0, 90.0 );
			SetSkill( SkillName.Wrestling, 80.0, 90.0 );
			SetSkill( SkillName.Poisoning, 120.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 20;

			PackReg( 15 );

			if ( Utility.Random( 200 ) < 1 ) PackItem( new ToxicPool() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		//Melee damage from controlled mobiles is divided by 10
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled )
				damage /= 10;
			}
		}

		//Melee damage to controlled mobiles is multiplied by 6
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled )
				damage *= 6;
			}
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public ToxicDragon( Serial serial ) : base( serial )
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