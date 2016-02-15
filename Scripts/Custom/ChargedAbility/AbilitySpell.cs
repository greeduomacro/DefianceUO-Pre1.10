using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Second;
using Server.Spells.Necromancy;

namespace Server.Spells
{
	public abstract class AbilitySpell : Spell
	{
		public AbilitySpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public abstract void OnHit( Mobile caster, Mobile target );
	}
}