using System;
using Server.Items;
using AMA = Server.Items.ArmorMeditationAllowance;

namespace Server.SkillHandlers
{
	class Meditation
	{
	public static void Initialize()
	{
		SkillInfo.Table[46].Callback = new SkillUseCallback( OnUse );
	}

	public static bool CheckOkayHolding( Item item )
	{
		if ( item == null )
		return true;

		if ( item is Spellbook || item is Runebook || item is ChaosShield || item is OrderShield || item is HeroKnightShield || item is LanternOfHellfire )
		return true;

		if ( Core.AOS && item is BaseWeapon && ((BaseWeapon)item).Attributes.SpellChanneling != 0 )
		return true;

		if ( Core.AOS && item is BaseArmor && ((BaseArmor)item).Attributes.SpellChanneling != 0 )
		return true;

		return false;
	}

	//Begin Seg ( 1 of 2 in file )
	public static bool IsMedable( Item item )
	{
		if ( item != null && item is BaseArmor )
		{
			if ( ((BaseArmor)item).DefMedAllowance == AMA.All || ((BaseArmor)item).ArmorAttributes.MageArmor == 1 )
			{
				// All is medable.
				return true;
			}
			if ( ((BaseArmor)item).DefMedAllowance == AMA.None || ((BaseArmor)item).DefMedAllowance == AMA.Half )
			{
				// if its None or Half then we can't meditate in it.
				return false;
			}
			// Because error checking gives me wood...
			return false;
		}
		// if the item doesnt exist or is not armor then assume it's medable.
		return true;
	}
	//End Seg ( 1 of 2 in file)

	public static TimeSpan OnUse( Mobile m )
	{
		m.RevealingAction();
		if ( m.Target != null ) // cant med if cursor is target
		{
			m.SendLocalizedMessage( 501845 ); // You are busy doing something else and cannot focus.
			return TimeSpan.FromSeconds( 5.0 );
		}
		else if ( m.Hits < (m.HitsMax / 10) ) // Less than 10% health
		{
			m.SendLocalizedMessage( 501849 ); // The mind is strong but the body is weak.
			return TimeSpan.FromSeconds( 5.0 );
		}
		else if ( m.Mana >= m.ManaMax ) // already at full mana
		{
			m.SendLocalizedMessage( 501846 ); // You are at peace.
			return TimeSpan.FromSeconds( 5.0 );
		}
		else
		{
//Begin Seg ( 2 of 2 in file )
/* --------------------------------------------------------------------------------------------------
Wearing non-medable armor should give message that player cannot med and not allow meditaion.
Original file no warning was issued, and player was allowed to meditate, just without the regen bonus.

from leather armor...
public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

Studded leather returns ArmorMeditationAllowance.Half

from basearmor.cs
public virtual AMA DefMedAllowance{ get{ return AMA.None; } }
public virtual AMA AosMedAllowance{ get{ return DefMedAllowance; } }
public virtual AMA OldMedAllowance{ get{ return DefMedAllowance; } }

--------------------------------------------------------------------------------------------------- */
			Item neck = m.FindItemOnLayer( Layer.Neck );
			Item hand = m.FindItemOnLayer( Layer.Gloves );
			Item head = m.FindItemOnLayer( Layer.Helm );
			Item arms = m.FindItemOnLayer( Layer.Arms );
			Item legs = m.FindItemOnLayer( Layer.Pants );
			Item chest = m.FindItemOnLayer( Layer.InnerTorso );
			int MedableCheck = 1;

			if ( !IsMedable( neck ) && MedableCheck == 1 )
				MedableCheck += MedableCheck;

			if ( !IsMedable( hand ) && MedableCheck == 1 )
				MedableCheck += MedableCheck;

			if ( !IsMedable( head ) && MedableCheck == 1 )
				MedableCheck += MedableCheck;

			if ( !IsMedable( arms ) && MedableCheck == 1 )
				MedableCheck += MedableCheck;

			if ( !IsMedable( legs ) && MedableCheck == 1 )
				MedableCheck += MedableCheck;

			if ( !IsMedable( chest ) && MedableCheck == 1 )
				MedableCheck += MedableCheck;

			if ( MedableCheck == 1 )
			{
// it's important to check armor first, then hands, dont want to disarm a player if they cant even meditate
// in the armor they are wearing.

				Item oneHanded = m.FindItemOnLayer( Layer.OneHanded );
				Item twoHanded = m.FindItemOnLayer( Layer.TwoHanded );
				if ( Core.AOS )
				{
					if ( !CheckOkayHolding( oneHanded ) )
					m.AddToBackpack( oneHanded );

					if ( !CheckOkayHolding( twoHanded ) )
					m.AddToBackpack( twoHanded );
				}
				else if ( !CheckOkayHolding( oneHanded ) || !CheckOkayHolding( twoHanded ) )
				{
					m.SendLocalizedMessage( 502626 ); // Your hands must be free to cast spells or meditate.
					return TimeSpan.FromSeconds( 2.5 );
				}

				if ( m.CheckSkill( SkillName.Meditation, 0, 100 ) )
				{
					m.SendLocalizedMessage( 501851 ); // You enter a meditative trance.
					m.Meditating = true;

					if ( m.Player || m.Body.IsHuman )
						m.PlaySound( 0xF9 );
				}
				else
				{
					m.SendLocalizedMessage( 501850 ); // You cannot focus your concentration.
				}
			}
			else if ( MedableCheck >= 2 )
			{
				m.SendLocalizedMessage( 500135 ); // Regenative forces cannot penetrate your armor!
				return TimeSpan.FromSeconds( 2.0 );
			}
			else
			{
				m.SendMessage( "Fall through error in meditation.cs. Please report to staff." );
			}
			return TimeSpan.FromSeconds( 5.0 );
			}
			//End Seg: ( 2 of 2 in file )
		}
	}
}