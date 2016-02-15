using System;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.First
{
	public class HealSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Heal", "In Mani",
				SpellCircle.First,
				224,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SpidersSilk
			);

		public HealSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( m is Golem )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 500951 ); // You cannot heal that.
			}
			else if ( m.Poisoned || Server.Items.MortalStrike.IsWounded( m ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, (Caster == m) ? 1005000 : 1010398 );
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				// Algorithm: (10% of magery) + (1-5)

				int toHeal = (int)(Caster.Skills[SkillName.Magery].Value * 0.1);
				toHeal += Utility.Random( 1, 5 );

				int healmessage = Math.Min( m.HitsMax - m.Hits, toHeal );

				m.Heal( toHeal );

				if ( healmessage > 0 )
				{
					m.PrivateOverheadMessage( MessageType.Regular, 0x42, false, healmessage.ToString(), m.NetState );
					if ( Caster != m )
						m.PrivateOverheadMessage( MessageType.Regular, 0x42, false, healmessage.ToString(), Caster.NetState );
				}

				m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
				m.PlaySound( 0x1F2 );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private HealSpell m_Owner;

			public InternalTarget( HealSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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