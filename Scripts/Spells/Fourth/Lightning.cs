using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Fourth
{
    public class LightningSpell : AbilitySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Lightning", "Por Ort Grav",
				SpellCircle.Fourth,
				239,
				9021,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public LightningSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return true; } }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				OnHit( Caster, m );

				/*double damage;

				if ( Core.AOS )
				{
					damage = GetAosDamage( 6, 3, 5.0 );
				}
				else
				{
					damage = Utility.Random( 16, 12 );

					if ( CheckResisted( m ) )
					{
						damage *= 0.75;

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					damage *= GetDamageScalar( m );
				}

				m.BoltEffect( 0 );

				InternalTimer t = new InternalTimer( this, damage, m );
				t.Start();*/
			}

			FinishSequence();
		}

		public override void OnHit( Mobile caster, Mobile target )
		{
			double damage;

			damage = Utility.Random( 10, 7 );

			if ( CheckResisted( target ) )
			{
				damage *= 0.75;

				target.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
			}

			damage *= GetDamageScalar( target );

			target.BoltEffect( 0 );

			SpellHelper.Damage( this, target, damage, 0, 0, 0, 0, 100 );
		}


		private class InternalTimer : Timer
		{
			private Spell m_Spell;
			private Double m_Damage;
			private Mobile m_Defender;

			public InternalTimer( Spell spell, Double damage, Mobile defender ) : base( TimeSpan.FromSeconds( 0.5 ) )
			{
				m_Spell = spell;
				m_Damage = damage;
				m_Defender = defender;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				// Deal the damage
				SpellHelper.Damage( m_Spell, m_Defender, m_Damage, 0, 0, 0, 0, 100 );
			}
		}

		private class InternalTarget : Target
		{
			private LightningSpell m_Owner;

			public InternalTarget( LightningSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}