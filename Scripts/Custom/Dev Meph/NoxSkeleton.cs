using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a poisoned skeletal corpse" )]
	public class NoxSkeleton : BaseCreature
	{
		[Constructable]
		public NoxSkeleton() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 3.0, 5.0)
		{
			Name = "a nox skeleton";
			Body = 50;
			BaseSoundID = 0x48D;
			Hue = 1164;
			Kills = 5;

			SetStr( 500, 600 );
			SetDex( 200, 250 );
			SetInt( 10, 10 );

			SetHits( 600, 600 );

			SetDamage( 35, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 5, 15 );

			SetSkill( SkillName.MagicResist, 0.0, 0.0 );
			SetSkill( SkillName.Tactics, 100.0, 100.0 );
			SetSkill( SkillName.Wrestling, 500.0, 500.0 );
			SetSkill( SkillName.Anatomy, 100.0, 100.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 1;

			if ( Utility.Random( 300 ) < 1 ) PackItem( new RareSwampTile() );
			PackMagicItems( 1, 5, 0.80, 0.75 );
			PackMagicItems( 3, 5, 0.60, 0.45 );

		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BardImmune{ get{ return true; } }

		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override double HitPoisonChance{ get{ return 0.75; } }

		private DateTime m_NextAttack;

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 10.0 + (10.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x36B0, 10, 25, 9540, 567, 0, EffectLayer.Waist );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound( 0x4CF );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 1, 10 ), 90, 10, 0, 0, 0 );
			}
		}

		public NoxSkeleton( Serial serial ) : base( serial )
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