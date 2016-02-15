using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a giant snow minion corpse" )]
	public class SnowMinion : BaseCreature
	{
		[Constructable]
		public SnowMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant snow minion";
			Body = 999;
			Hue = 1150;
			BaseSoundID = 1415;

			SetStr( 600 );
			SetDex( 100 );
			SetInt( 400 );

			SetHits( 2000 );
			SetMana( 200 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 0.0 );
			SetSkill( SkillName.Wrestling, 50.0 );
			SetSkill( SkillName.Anatomy, 0.0 );

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 0;

			PackItem( new Gold( 400, 600 ) );

			if ( Utility.Random( 200 ) < 1 ) PackItem( new SnowMinionStatue() );
		}
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.Gems, Utility.Random( 1, 2 ) );
		}

		public override int GetHurtSound()
		{
			return 1417;
		}

		public override int GetDeathSound()
		{
			return 1430;
		}

		public override int GetAttackSound()
		{
			return 1414;
		}

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 1157, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel weaker as the life drains out of you!" );

				int toDrain = Utility.RandomMinMax( 40, 50 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if ( attacker is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)attacker;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
					DrainLife();
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( caster is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)caster;

				if ( bc.Controlled || bc.BardTarget == this )
				scalar = 0.75;
			}
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage /= 8;
			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)to;

				if ( bc.Controlled || bc.Summoned || bc.BardTarget == this )
				damage *= 3;
			}
		}

		public SnowMinion( Serial serial ) : base( serial )
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