using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.First
{
	public class MagicArrowSpell : AbilitySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Magic Arrow", "In Por Ylem",
				SpellCircle.First,
				212,
				9041,
				Reagent.SulfurousAsh
			);

		public MagicArrowSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
				Mobile source = Caster;

				SpellHelper.Turn( source, m );

				SpellHelper.CheckReflect( (int)this.Circle, ref source, ref m );

                OnHit(source, m);

				/*double damage;

				if ( Core.AOS )
				{
					damage = GetAosDamage( 3, 1, 10.0 );
				}
				else
				{
					damage = Utility.Random( 1, 1 );

					if ( CheckResisted( m ) )
					{
						damage *= 0.75;

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					damage *= GetDamageScalar( m );
				}

				source.MovingParticles( m, 0x36E4, 5, 0, false, true, 3006, 4006, 0 );
				source.PlaySound( 0x1E5 );

				SpellHelper.Damage( TimeSpan.FromSeconds(1.00), m, Caster, damage, 0, 100, 0, 0, 0 );*/
			}

			FinishSequence();
		}

        public override void OnHit(Mobile caster, Mobile target)
        {
            double damage;

            damage = 3;

            if (CheckResisted(target))
            {
                damage *= 0.75;

                target.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
            }

            damage *= GetDamageScalar(target);

            caster.MovingParticles(target, 0x36E4, 5, 0, false, true, 3006, 4006, 0);
            caster.PlaySound(0x1E5);

            SpellHelper.Damage(TimeSpan.FromSeconds(0.50), target, caster, damage, 0, 100, 0, 0, 0);
        }


		private class InternalTarget : Target
		{
			private MagicArrowSpell m_Owner;

			public InternalTarget( MagicArrowSpell owner ) : base( 12, false, TargetFlags.Harmful )
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