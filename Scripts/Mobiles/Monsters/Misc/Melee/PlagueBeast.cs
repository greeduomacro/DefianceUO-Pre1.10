using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a plague beast corpse" )]
	public class PlagueBeast : BaseCreature
	{
		[Constructable]
		public PlagueBeast() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a plague beast";
			Body = 775;

			SetStr( 302, 500 );
			SetDex( 80 );
			SetInt( 16, 20 );

			SetHits( 318, 404 );

			SetDamage( 20, 24 );

			SetSkill( SkillName.MagicResist, 35.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 13000;
			Karma = -13000;

			VirtualArmor = 40;

			PackGold( 300, 600 );
			PackGem();
			PackGem();
			PackArmor( 0, 4 );
			PackArmor( 0, 2 );
			PackArmor( 0, 3 );
			PackWeapon( 0, 4 );

			// TODO: jewelry, dungeon chest, healthy gland
		}

		// TODO: Poison attack

		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster != this && 0.25 > Utility.RandomDouble() )
			{
				BaseCreature spawn = new PlagueSpawn( this );

				spawn.Team = this.Team;
				spawn.Location = this.Location;
				spawn.Map = this.Map;
				spawn.Combatant = caster;

				Say( 1053034 ); // * The plague beast creates another beast from its flesh! *
			}

			base.OnDamagedBySpell( caster );
		}

		public override bool AutoDispel{ get{ return true; } }

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if ( attacker != this && 0.25 > Utility.RandomDouble() )
			{
				BaseCreature spawn = new PlagueSpawn( this );

				spawn.Team = this.Team;
				spawn.Location = this.Location;
				spawn.Map = this.Map;
				spawn.Combatant = attacker;

				Say( 1053034 ); // * The plague beast creates another beast from its flesh! *
			}

			base.OnGotMeleeAttack( attacker );
		}

		public PlagueBeast( Serial serial ) : base( serial )
		{
		}

		public override int GetIdleSound()
		{
			return 0x1BF;
		}

		public override int GetAttackSound()
		{
			return 0x1C0;
		}

		public override int GetHurtSound()
		{
			return 0x1C1;
		}

		public override int GetDeathSound()
		{
			return 0x1C2;
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