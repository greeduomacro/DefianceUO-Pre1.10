using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class SummonCreatureSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Creature", "Kal Xen",
				SpellCircle.Fifth,
				266,
				9040,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public SummonCreatureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		// TODO: Get real list
		private static Type[] m_Types = new Type[]
			{
				typeof( PolarBear ),
				typeof( GrizzlyBear ),
				typeof( BlackBear ),
				typeof( BrownBear ),
				typeof( Horse ),
				typeof( Walrus ),
				typeof( GreatHart ),
				typeof( Hind ),
				typeof( Dog ),
				typeof( Boar ),
				typeof( Chicken ),
				typeof( Rabbit )
			};

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				try
				{
					BaseCreature creature = (BaseCreature)Activator.CreateInstance( m_Types[Utility.Random( m_Types.Length )] );

					SpellHelper.Summon( creature, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[SkillName.Magery].Value ), false, false );
				}
				catch
				{
				}
			}

			FinishSequence();
		}

		public override int CastDelayBase
		{
			get{ return base.CastDelayBase + 28; }
		}

		public override int CastDelayFastScalar
		{
			get{ return 5; }
		}

		public override TimeSpan GetCastDelay()
		{
			if ( Core.AOS )
				return base.GetCastDelay();

			return base.GetCastDelay() + TimeSpan.FromSeconds( 6.0 );
		}
	}
}