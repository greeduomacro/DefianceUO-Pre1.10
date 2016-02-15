using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a jukan corpse" )]
	public class JukaWarrior : BaseCreature
	{
		[Constructable]
		public JukaWarrior() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a juka warrior";
			Body = 764;

			SetStr( 251, 350 );
			SetDex( 61, 80 );
			SetInt( 101, 150 );

			SetHits( 151, 210 );

			SetDamage( 7, 9 );

			SetSkill( SkillName.Anatomy, 80.1, 90.0 );
			SetSkill( SkillName.Fencing, 80.1, 90.0 );
			SetSkill( SkillName.Macing, 80.1, 90.0 );
			SetSkill( SkillName.MagicResist, 120.1, 130.0 );
			SetSkill( SkillName.Swords, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 80.1, 90.0 );

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 22;

			PackGold( 150, 250 );
			PackGem();
			PackArmor( 0, 3 );
			PackWeapon( 0, 3 );
		}

		public override int GetIdleSound()
		{
			return 0x1CD;
		}

		public override int GetAngerSound()
		{
			return 0x1CD;
		}

		public override int GetHurtSound()
		{
			return 0x1D0;
		}

		public override int GetDeathSound()
		{
			return 0x28D;
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }

		public JukaWarrior( Serial serial ) : base( serial )
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