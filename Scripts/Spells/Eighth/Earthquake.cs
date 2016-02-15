using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Eighth
{
	public class EarthquakeSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Earthquake", "In Vas Por",
				SpellCircle.Eighth,
				233,
				9012,
				false,
				Reagent.Bloodmoss,
				Reagent.Ginseng,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public EarthquakeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool DelayedDamage{ get{ return !Core.AOS; } }

		public override void OnCast()
		{
			if ( SpellHelper.CheckTown( Caster, Caster ) && CheckSequence() )
			{
				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				if ( map != null )
				{
					foreach ( Mobile m in Caster.GetMobilesInRange( 1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0) ) )
					{
						if ( Caster != m && SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) && (!Core.AOS || Caster.InLOS( m )) )
							targets.Add( m );
					}
				}

				Caster.PlaySound( 0x2F3 );

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					double damage = Core.AOS ? m.Hits - (m.Hits / 3.0) : m.Hits * 0.5;

					if ( !m.Player && damage < 10.0 )
						damage = 10.0;
					else if ( damage > (Core.AOS ? 100.0 : 75.0) )
						damage = Core.AOS ? 100.0 : 75.0;

					if ( damage > (double)m.Hits )
						damage = (double)m.Hits;

					Caster.DoHarmful( m );
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 100, 0, 0, 0, 0 );
				}
			}

			FinishSequence();
		}
	}
}