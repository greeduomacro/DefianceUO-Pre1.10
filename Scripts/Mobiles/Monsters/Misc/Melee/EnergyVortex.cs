using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an energy vortex corpse" )]
	public class EnergyVortex : BaseCreature
	{
		[Constructable]
		public EnergyVortex() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Name = "an energy vortex";
			Body = 164;

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 100 );

			SetHits( 500 );
			SetStam( 200 );
			SetMana( 0 );

			SetDamage( 15, 19 );

			SetSkill( SkillName.MagicResist, 99.9 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 0;
			Karma = 0;

			ControlSlots = 1;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }

		public override int GetAngerSound()
		{
			return 0x15;
		}

		public override int GetAttackSound()
		{
			return 0x28;
		}

		public EnergyVortex( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 0;
		}
	}
}