using System;
using Server;
using Server.Misc;
using System.Collections;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a king lich corpse" )]
	public class KingLichLord : BaseCreature
	{
		public Timer m_DeathTimer;

		[Constructable]
		public KingLichLord () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a king lich lord";
			Body = 310;
			BaseSoundID = 362;
			Hue = 1154;
			//DamageMax 25;

			SetStr( 1300, 1400 );
			SetDex( 125, 195 );
			SetInt( 45006, 46206 );
			SetMana( 46000, 46000 );
			SetHits( 5500, 5500 );

			SetDamage( 19, 22 );

			SetDamageType( ResistanceType.Physical, 100 );
			SetDamageType( ResistanceType.Fire, 100 );
			SetDamageType( ResistanceType.Cold, 100 );
			SetDamageType( ResistanceType.Poison, 100 );
			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Physical, 99 );
			SetResistance( ResistanceType.Fire, 99 );
			SetResistance( ResistanceType.Cold, 99 );
			SetResistance( ResistanceType.Poison, 99 );
			SetResistance( ResistanceType.Energy, 99 );

			SetSkill( SkillName.EvalInt, 155.1, 130.0 );
			SetSkill( SkillName.Magery, 145.1, 130.0 );
			SetSkill( SkillName.Meditation, 155.1, 130.0 );
			SetSkill( SkillName.MagicResist, 160.5, 130.5 );
			SetSkill( SkillName.Tactics, 210.1, 180.0 );
			SetSkill( SkillName.Wrestling, 110.1, 110.0 );

			Fame = 50000;
			Karma = -50000;

			VirtualArmor = 0;

			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackGem();
			PackPotion();
			PackPotion();
			PackPotion();
			PackGold( 4000, 5000 );
			PackScroll( 12, 12 );
			PackMagicItems( 3, 5, 0.95, 0.95 );
			PackMagicItems( 3, 5, 0.80, 0.65 );
			PackMagicItems( 4, 5, 0.80, 0.65 );
			PackSlayer();


			switch ( Utility.Random( 300 ))
			{
			case 0: PackItem( new BronzeStatueMaker() ); break;

			}
		}

		public override bool CanBeControlledBy( Mobile m )
 		{
 		if ( m.Skills[SkillName.AnimalTaming].Base < 95 )
 		return false;
 		return base.CanBeControlledBy( m );
 		}

		public override int GetIdleSound()
		{
			return 0x2D3;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public KingLichLord( Serial serial ) : base( serial )
		{
		}

		private DateTime m_NextBreathe;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextBreathe )
			{
				Breathe( combatant );

				m_NextBreathe = DateTime.Now + TimeSpan.FromSeconds( 12.0 + (3.0 * Utility.RandomDouble()) ); // 12-15 seconds
			}
		}

		public void Breathe( Mobile m )
		{
			DoHarmful( m );

			new BreatheTimer( m, this ).Start();

			this.Frozen = true;

			//experimental by light
			this.MovingParticles( m, 0x374A, 1, 0, false, true, Utility.RandomList( 1153 ), 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );


			//original code.
			//this.MovingParticles( m, 0x1FBE, 1, 0, false, true, Utility.RandomList( 1157, 1175, 1172, 1171, 1170, 1169, 1168, 1167, 1166, 1165 ), 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );
		}

		private class BreatheTimer : Timer
		{
			private KingLichLord d;
			private Mobile m_Mobile;

			public BreatheTimer( Mobile m, KingLichLord owner ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				d = owner;
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				d.Frozen = false;

				m_Mobile.PlaySound( 0x11D );
				AOS.Damage( m_Mobile, Utility.RandomMinMax( 40, 50 ), 0, 100, 0, 0, 0 );
				Stop();
			}
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