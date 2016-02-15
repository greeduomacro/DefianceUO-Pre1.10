using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.IdolSystem;

namespace Server.Mobiles
{
	public class DecBoss : BaseMiniBoss
	{
		static bool m_Active;
		IdolType m_Type;
		MagicalRareType m_Rare;

		[CommandProperty( AccessLevel.GameMaster )]
		public static bool Active
		{
			get{ return m_Active; }
			set{ m_Active = value; }
		}

		[Constructable]
		public DecBoss() : base( AIType.AI_Melee )
		{
			Name = "Idol Keeper";
			Title = "of Deceit";
			Hue = 1250;
			Body = 154;
			BaseSoundID = 471;
			m_Active = true;
			m_Type = IdolType.Deceit;
			m_Rare = MagicalRareType.Two;

			SetStr( 300, 350 );
			SetDex( 150, 200 );
			SetInt( 150, 200 );

			SetHits( 6000 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 170.0 );
			SetSkill( SkillName.Tactics, 170.0 );
			SetSkill( SkillName.Wrestling, 190.0 );

			Fame = 17000;
			Karma = -17000;

			VirtualArmor = 50;

		}

		public override void OnDeath( Container c )
		{
			m_Active = false;

			if ( Utility.Random( 5 ) < 1 )
				c.DropItem( new Idol( m_Type ) );

			if ( Utility.Random( 8 ) < 1 )
				c.DropItem( new MagicalRare( m_Rare ) );

			base.OnDeath( c );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.80; } }
		public override int DoMoreDamageToPets { get { return 10; } }
		public override int DoLessDamageFromPets { get { return 10; } }
		private DateTime m_NextAbilityTime;

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 2.5 );

			this.Say( true, "All this life force around me...I must have it!" );
			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

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
			{
				this.Say( true, "Come back here and give me your life force!  You do not need it!" );
			}
			else
			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.50;
				else if ( list.Count == 2 )
					scalar = 0.40;
				else
					scalar = 0.20;

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile)list[i];

					int damage = (int)(m.Hits * scalar);

					damage += Utility.RandomMinMax( -5, 5 );

					if ( damage < 1 )
						damage = 1;

					m.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535,    1, 0, (EffectLayer)255, 0x100 );
					m.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

					this.DoHarmful( m );
					this.Hits += AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
				}

				this.Say( true, "Ah the joys of feasting on such life!" );
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
				this.MovingParticles( combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1,    0, (EffectLayer)255, 0x100 );
				this.MovingParticles( combatant, 0x0001, 1, 0, false,  true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0 );
				this.PlaySound( 0x1FB );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( DoFocusedLeech_Stage2 ), combatant );
			}
		}

		private void DoFocusedLeech_Stage2( object state )
		{
			Mobile combatant = (Mobile)state;

			if ( this.CanBeHarmful( combatant ) )
			{
				combatant.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1,    0, (EffectLayer)255, 0x100 );
				combatant.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

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
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 30 ) );

					int ability = Utility.Random( 5 );

					switch ( ability )
					{
						case 0: DoFocusedLeech( combatant, "I shall help you join the undead hordes!" ); break;
						case 1: DoFocusedLeech( combatant, "Do you feel weaker?" ); break;
						case 2: DoFocusedLeech( combatant, "I feed off your life!" ); break;
						case 3: DoFocusedLeech( combatant, "Your life is mine!" ); break;
						case 4: DoAreaLeech(); break;
					}
				}
			}

			base.OnThink();}

		public DecBoss( Serial serial ) : base( serial )
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

            m_Active = true;
		}

        public override void OnAfterDelete()
        {
            m_Active = false;
            base.OnAfterDelete();
        }
	}
}