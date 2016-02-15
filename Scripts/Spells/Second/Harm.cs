using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Second
{
	public class HarmSpell : AbilitySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Harm", "An Mani",
				SpellCircle.Second,
				212,
				Core.AOS ? 9001 : 9041,
				Reagent.Nightshade,
				Reagent.SpidersSilk
			);

		public HarmSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		//public override TimeSpan GetCastDelay()
		//{
		//return base.GetCastDelay() + TimeSpan.FromSeconds( 0.3 );
		//}

                //protected override TimeSpan NextSpellDelay { get{ return TimeSpan.FromSeconds( 1.0 ); } }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return false; } }

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
					damage = GetAosDamage( 6, 3, 6.5 );
				}
				else
				{
					damage = Utility.Random( 6, 7 );

					if ( CheckResisted( m ) )
					{
						damage *= 0.75;

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					damage *= GetDamageScalar( m );
				}

				if ( !m.InRange( Caster, 2 ) )
					damage *= 0.25; // 1/4 damage at > 2 tile range
				else if ( !m.InRange( Caster, 1 ) )
					damage *= 0.50; // 1/2 damage at 2 tile range

				if ( Core.AOS )
				{
					m.FixedParticles( 0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist );
					m.PlaySound( 0x0FC );
				}
				else
				{
					m.FixedParticles( 0x374A, 10, 15, 5013, EffectLayer.Waist );
					m.PlaySound( 0x1F1 );
				}

				SpellHelper.Damage( this, m, damage, 0, 0, 100, 0, 0 );*/
			}

			FinishSequence();
		}


		public override void OnHit( Mobile caster, Mobile target )
		{
			double damage;

            damage = Utility.Random( 7, 6);

			if ( CheckResisted( target ) )
			{
				damage *= 0.90;

				target.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
			}

			damage *= GetDamageScalar( target );

            if (!target.InRange(caster, 2))
                damage *= 0.25; // 1/4 damage at > 2 tile range
            else if (!target.InRange(caster, 1))
                damage *= 0.50; // 1/2 damage at 2 tile range

			target.FixedParticles( 0x374A, 10, 15, 5013, EffectLayer.Waist );
			target.PlaySound( 0x1F1 );

			SpellHelper.Damage( this, target, damage, 0, 0, 100, 0, 0 );
		}

		private class InternalTarget : Target
		{
			private HarmSpell m_Owner;

			public InternalTarget( HarmSpell owner ) : base( 12, false, TargetFlags.Harmful )
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