using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a zombie corpse" )] // ?
	public class RottingCorpse : BaseCreature
	{
		[Constructable]
		public RottingCorpse() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rotting corpse";
			Body = 155;
			BaseSoundID = 471;

			SetStr( 301, 350 );
			SetDex( 75 );
			SetInt( 151, 200 );

			SetHits( 1200 );
			SetStam( 150 );
			SetMana( 0 );

			SetDamage( 8, 12 );

			SetSkill( SkillName.Poisoning, 120.0 );
			SetSkill( SkillName.MagicResist, 350.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.1, 120.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 60;

			PackGem( 2, 5 );
			PackScroll( 1, 8 );
			PackGold( 600, 1000 );
			PackArmor( 0, 5 );
			PackWeapon( 0, 5 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public RottingCorpse( Serial serial ) : base( serial )
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