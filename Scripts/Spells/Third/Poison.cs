using System;
using Server.Targeting;
using Server.Network;
using Server.Regions;

namespace Server.Spells.Third
{
	public class PoisonSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Poison", "In Nox",
				SpellCircle.Third,
				203,
				9051,
				Reagent.Nightshade
			);

		public PoisonSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
			return base.GetResistPercentForCircle( target, circle ) - Caster.Skills[SkillName.Poisoning].Value * 0.2;
		}

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

				if ( m.Spell != null )
					m.Spell.OnCasterHurt();

				m.Paralyzed = false;

				if ( CheckResisted( m ) )
				{
					m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
				}
				else
				{
                    double total = Caster.Skills[SkillName.Magery].Value;
                    if (!(Caster.Region is CustomRegion && ((CustomRegion)Caster.Region).NoPoisonSkillEffects))
                        total += Caster.Skills[SkillName.Poisoning].Value;

					double dist = Caster.GetDistanceToSqrt( m );

					if ( dist >= 3.0 )
						total -= (dist - 3.0) * 10.0;

					int level;

					if ( total >= 200.0 && (/*Core.AOS ||*/ Utility.Random( 1, 100 ) <= 10) )
						level = 3;
					else if ( total > (/*Core.AOS ? 170.1 :*/ 170.0) )
						level = 2;
					else if ( total > (/*Core.AOS ? 130.1 :*/ 130.0) )
						level = 1;
					else
						level = 0;

					m.ApplyPoison( Caster, Poison.GetPoison( level ) );
				}

				m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
				m.PlaySound( 0x474 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private PoisonSpell m_Owner;

			public InternalTarget( PoisonSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}