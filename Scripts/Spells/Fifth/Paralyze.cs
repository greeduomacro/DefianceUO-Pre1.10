using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Fifth
{
	public class ParalyzeSpell : AbilitySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Paralyze", "An Ex Por",
				SpellCircle.Fifth,
				218,
				9012,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public ParalyzeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( Core.AOS && (m.Frozen || m.Paralyzed || (m.Spell != null && m.Spell.IsCasting)) )
			{
				Caster.SendLocalizedMessage( 1061923 ); // The target is already frozen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				OnHit( Caster, m );

				/*double duration;

				if ( Core.AOS )
				{
					duration = (GetDamageSkill( Caster ) - GetResistSkill( m )) * 0.3;

					if ( duration < 0.0 )
						duration = 0.0;
				}
				else
				{
					// Algorithm: ((20% of magery) + 7) seconds [- 50% if resisted]

					duration = 7.0 + (Caster.Skills[SkillName.Magery].Value * 0.2);

					if ( CheckResisted( m ) )
						duration *= 0.75;
				}

				m.Paralyze( TimeSpan.FromSeconds( duration ) );

				m.PlaySound( 0x204 );
				m.FixedEffect( 0x376A, 6, 1 );*/
			}

			FinishSequence();
		}

		public override void OnHit( Mobile caster, Mobile target )
		{
			double duration;

			// Algorithm: ((20% of magery) + 4) seconds [- 50% if resisted]

			duration = 4.0 + (Caster.Skills[SkillName.Magery].Value * 0.2);

			if ( CheckResisted( target ) )
			{
				duration /= 2.0;
				target.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
			}

			target.Paralyze( TimeSpan.FromSeconds( duration ) );

			target.PlaySound( 0x204 );
			target.FixedEffect( 0x376A, 6, 1 );
		}

		public class InternalTarget : Target
		{
			private ParalyzeSpell m_Owner;

			public InternalTarget( ParalyzeSpell owner ) : base( 12, false, TargetFlags.Harmful )
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