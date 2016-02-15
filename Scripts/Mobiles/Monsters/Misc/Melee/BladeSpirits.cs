using System;
using System.Collections;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a blade spirit corpse" )]
	public class BladeSpirits : BaseCreature
	{
		[Constructable]
		public BladeSpirits() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.2 )
		{
			Name = "a blade spirit";
			Body = 574;

			SetStr( 150 );
			SetDex( 150 );
			SetInt( 100 );

			SetHits( 400 );
			SetStam( 150 );
			SetMana( 0 );

			SetDamage( 14, 17 );

			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.Wrestling, 91.0 );

			Fame = 0;
			Karma = 0;

			ControlSlots = 1;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override int GetAngerSound()
		{
			return 0x23A;
		}

		public override int GetAttackSound()
		{
			return 0x3B8;
		}

		public override int GetHurtSound()
		{
			return 0x23A;
		}

		public BladeSpirits( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}