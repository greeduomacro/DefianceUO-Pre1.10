using System;
using Server;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a vampire's corpse" )]
	public class EvilVampire : BaseCreature
	{
		[Constructable]
		public EvilVampire () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "male" );
			Body = 400;
			Title = "the Vampire Lord";

			BaseSoundID = 0x482;;
			Hue = 0;

			SetStr( 350, 500 );
			SetDex( 100, 105 );
			SetInt( 3000, 4000 );
			SetHits( 2500, 3750 );

			SetDamage( 40, 50 );
			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 150.1, 200.0 );
			SetSkill( SkillName.Magery, 150.1, 200.0 );
			SetSkill( SkillName.MagicResist, 200, 250.0 );
			SetSkill( SkillName.Tactics, 150.0, 200.0 );
			SetSkill( SkillName.Wrestling, 150.0, 200.0 );

			Fame = 20000;
			Karma = -10000;

			VirtualArmor = 90;

			PackGem();
			PackGem();
			PackGold( 5000, 10000 );
			PackScroll( 3, 8 );
			PackScroll( 3, 8 );
			PackMagicItems( 1, 5, 0.80, 0.75 );
			PackMagicItems( 3, 5, 0.60, 0.45 );
			PackSlayer( 1 );

			AddItem( new BlackStaff() );
			Item m_ShortPants = new ShortPants( Utility.RandomRedHue() );
			m_ShortPants.LootType = LootType.Blessed;
			AddItem( m_ShortPants );
			Item m_Doublet = new Doublet( Utility.RandomRedHue() );
			m_Doublet.LootType = LootType.Blessed;
			AddItem( m_Doublet );
			Item m_sandals = new Sandals( Utility.RandomRedHue() );
			m_sandals.LootType = LootType.Blessed;
			AddItem( m_sandals );

			AddItem( new PonyTail( Utility.RandomRedHue() ) );
		}

		public override void GenerateLoot( bool spawning )
		{
			if ( !spawning )
			{
				if ( Utility.Random( 100 ) < 5 ) PackItem( new CandleSkull() );
				if ( Utility.Random( 100 ) < 1 ) PackItem( new SpecialBeardDye() );
				if ( Utility.Random( 100 ) < 1 ) PackItem( new SpecialHairDye() );
			}

			PackItem( new DaemonBone( 60 ) );
		}
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.4; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 3; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool Uncalmable{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			if ( m.Mount != null )
				m.Mount.Rider = null;

			if ( m.BeginAction( typeof( PolymorphSpell) ) )
			{

				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );
				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 74;
				m.HueMod = 1157;
				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;
			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 3.0 ) )
			{
				m_Owner = owner;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}

		public override int GetHurtSound()
		{
			return 0x167;
		}

		public override int GetDeathSound()
		{
			return 0xBC;
		}

		public override int GetAttackSound()
		{
			return 0x28B;
		}

		private DateTime m_NextAbilityTime;

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 2.5 );
			this.Say( true, "Blood! I vant your blood!" );
			this.FixedParticles( 0x376A, 10, 10, 9537, 1150, 0, EffectLayer.Waist );
			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( DoAreaLeech_Finish ) );

		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 6 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			if ( list.Count == 0 )
				this.Say( true, "Ah the chase is on. Your blood will be mine soon!" );
			else
			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.75;
				else if ( list.Count == 2 )
					scalar = 0.50;
				else
					scalar = 0.25;

				for ( int i = 0; i < list.Count; ++i )
				{

					Mobile m = (Mobile)list[i];

					int damage = (int)(m.Hits * scalar);

					damage += Utility.RandomMinMax( -5, 5 );

					if ( damage < 1 )
						damage = 1;

					m.MovingParticles( this, 0x36F4, 1, 0, false, false, 1150, 0, 9535,    1, 0, (EffectLayer)255, 0x100 );
					m.MovingParticles( this, 0x0001, 1, 0, false,  true, 1150, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

					this.DoHarmful( m );

					this.Hits += AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
				}
				this.Say( true, "If I cannot have thine blood then I shall destroy thee!" );
			}
		}

		private void DoFocusedLeech( Mobile combatant, string message )
		{
			this.Say( true, message );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( DoFocusedLeech_Stage1 ), combatant );
		}

		private void DoFocusedLeech_Stage1( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				this.MovingParticles( combatant, 0x36FA, 1, 0, false, false, 1175, 0, 9533, 1,    0, (EffectLayer)255, 0x100 );
				this.MovingParticles( combatant, 0x0001, 1, 0, false,  true, 1175, 0, 9533, 9534, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x1FB );
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage2 ), combatant );
			}
		}

		private void DoFocusedLeech_Stage2( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				combatant.MovingParticles( this, 0x36F4, 1, 0, false, false, 1150, 0, 9535, 1,    0, (EffectLayer)255, 0x100 );
				combatant.MovingParticles( this, 0x0001, 1, 0, false,  true, 1150, 0, 9535, 9536, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x209 );
				this.DoHarmful( combatant );
				this.Hits += AOS.Damage( combatant, this, Utility.RandomMinMax( 30, 40 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );
			}
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 15 ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );
					int ability = Utility.Random( 5 );
					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "I shall bathe in thy blood!" ); break;
						case 1: DoFocusedLeech( combatant, "I rebuke thee, maggot, and consume thy life giving blood!" ); break;
						case 2: DoFocusedLeech( combatant, "I devour thy life's essence to strengthen my being!" ); break;
						case 3: DoFocusedLeech( combatant, "Your blood is mine for the taking!" ); break;
						case 4: DoAreaLeech(); break;
							// TODO: Resurrect ability
					}
				}
			}
			base.OnThink();
		}

		public EvilVampire( Serial serial ) : base( serial )
		{
		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.75 >= Utility.RandomDouble() ) // 75% chance to polymorph attacker into a imp
				Polymorph( target );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			DoSpecialAbility( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			DoSpecialAbility( defender );
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