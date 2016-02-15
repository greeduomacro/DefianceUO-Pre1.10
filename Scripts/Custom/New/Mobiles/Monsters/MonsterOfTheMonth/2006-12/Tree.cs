using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a fallen tree" )]
	public class Tree : BaseCreature
	{
		[Constructable]
		public Tree() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a tree";
			Body = 47;
			BaseSoundID = 442;

			SetStr( 800, 1000 );
			SetDex( 80, 100 );
			SetInt( 200, 300 );

			SetHits( 5000, 6000 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.MagicResist, 300.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Tactics, 100 );
			SetSkill( SkillName.Wrestling, 80.0, 90.0 );

			Fame = 13500;
			Karma = -13500;

			VirtualArmor = 40;

			PackGold( 3000, 4000 );
			PackItem( new Log( 200 ) );
			if ( Utility.Random( 200 ) < 1 ) PackItem( new PixieStatue() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool DisallowAllMoves{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }

		private const double HealChance = 1.00; // 100% chance to heal at gm magery

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is PlayerMobile )
			{
				BaseAxe BA = from.FindItemOnLayer( Layer.TwoHanded ) as BaseAxe;
				if ( BA != null )
				damage *= 3;
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			if ( caster is PlayerMobile )
				reflect = true;
		}

		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster is PlayerMobile )
			{
				DoHarmful( caster );

				caster.MovingParticles( this, 0x36F4, 1, 0, false,  true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0x100 );
				caster.PlaySound( 0x108 );

				int toDrain = (int)(caster.Hits);

				Hits += toDrain;
				caster.Damage( toDrain, this );

				caster.SendMessage( "The life is sucked out of you!" );
			}

			base.OnDamagedBySpell( caster );
		}


		public override void OnThink()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 18 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				int toDrain = 50;

				Hits += toDrain;
				m.Damage( toDrain, this );

			}

			base.OnThink();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.25 >= Utility.RandomDouble() )
				SpillAcid( TimeSpan.FromSeconds( 10 ), 20, 30, attacker );
		}

		public void SpillAcid( TimeSpan duration, int minDamage, int maxDamage, Mobile target)
		{
			if ( Map != null && target != null )
			{
				if ( target.Map != null )
				{
					PoolOfSap sap = new PoolOfSap( duration, minDamage, maxDamage );

					sap.MoveToWorld( target.Location, target.Map );
					this.PlaySound( 37 );
				}
			}
		}

		public Tree( Serial serial ) : base( serial )
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