using System;
using Server.Items;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Regions;
using Server.Spells.Necromancy;
using Server.Engines.Poker;


namespace Server.Spells.Fourth
{
	public class RecallSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Recall", "Kal Ort Por",
				SpellCircle.Fourth,
				239,
				9031,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		private RunebookEntry m_Entry;
		private Runebook m_Book;

		public RecallSpell( Mobile caster, Item scroll ) : this( caster, scroll, null, null )
		{
		}

		public RecallSpell( Mobile caster, Item scroll, RunebookEntry entry, Runebook book ) : base( caster, scroll, m_Info )
		{
			m_Entry = entry;
			m_Book = book;
		}

		public override void GetCastSkills( out double min, out double max )
		{
			if ( TransformationSpell.UnderTransformation( Caster, typeof( WraithFormSpell ) ) )
				min = max = 0;
			else if( Core.SE && m_Book != null )	//recall using Runebook charge
				min = max = 0;
			else
				base.GetCastSkills( out min, out max );
		}

		public override void OnCast()
		{
			if ( m_Entry == null )
				Caster.Target = new InternalTarget( this );
			else
				Effect( m_Entry.Location, m_Entry.Map, true );
		}

		public override bool CheckCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
				Caster.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			else if ( Caster.Criminal )
				Caster.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
			else if (PokerDealer.IsPokerPlayer(Caster) >= 0)
				Caster.SendMessage("You cannot travel while playing poker.");
			else if ( Items.EventFlag.ExistsOn( Caster ) )
				Caster.SendLocalizedMessage( 1061632 ); // EventFlag Add by XLX.
			else if ( SpellHelper.CheckCombat( Caster ) )
				Caster.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
			else if (SpellHelper.CheckFactionAggressed(Caster))
				Caster.SendMessage("Thou art engaged in a Faction fight!");
			else if (Server.Misc.WeightOverloading.IsOverloaded(Caster))
				Caster.SendLocalizedMessage( 502359, "", 0x22 ); // Thou art too encumbered to move.
			else
				return SpellHelper.CheckTravel( Caster, TravelCheckType.RecallFrom );

			return false;
		}

		public void Effect( Point3D loc, Map map, bool checkMulti )
		{
			Region region = Region.Find(loc, map);
			CustomRegion customRegion = region as CustomRegion;
			if (Factions.Sigil.ExistsOn(Caster))
				Caster.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
			else if (map == null || (!Core.AOS && Caster.Map != map))
				Caster.SendLocalizedMessage(1005569); // You can not recall to another facet.
			else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom))
			{
			}
			else if (!SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.RecallTo))
			{
			}
			else if (map == Map.Felucca && Caster is PlayerMobile && ((PlayerMobile)Caster).Young)
				Caster.SendLocalizedMessage(1049543); // You decide against traveling to Felucca while you are still young.
			/*
			else if (Caster.Kills >= 5 && map != Map.Felucca)
			{
				Caster.SendLocalizedMessage(1019004); // You are not allowed to travel there.
			}
			 */
			else if (Caster.Criminal)
				Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
			else if (SpellHelper.CheckCombat(Caster))
				Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
			else if (SpellHelper.CheckFactionAggressed(Caster))
				Caster.SendMessage("Thou art engaged in a Faction fight!");
			else if (Server.Misc.WeightOverloading.IsOverloaded(Caster))
				Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
			else if (customRegion != null && customRegion.CannotEnter) //Al: Block if CannotEnter is set to true
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			else if (region != null && region is GreenAcres) //Al: Block if GreenAcres
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			else if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			else if ((checkMulti && SpellHelper.CheckMulti(loc, map)))
				Caster.SendLocalizedMessage(501942); // That location is blocked.
			else if (m_Book != null && m_Book.CurCharges <= 0)
				Caster.SendLocalizedMessage(502412); // There are no charges left on that item.
			else if (CheckSequence())
			{
				BaseCreature.TeleportPets(Caster, loc, map, true);

				if (m_Book != null)
					--m_Book.CurCharges;

				Caster.PlaySound(0x1FC);
				Caster.MoveToWorld(loc, map);
				Caster.PlaySound(0x1FC);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private RecallSpell m_Owner;

			public InternalTarget( RecallSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;

				owner.Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501029 ); // Select Marked item.
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					RecallRune rune = (RecallRune)o;

					if ( rune.Marked )
						m_Owner.Effect( rune.Target, rune.TargetMap, true );
					else
						from.SendLocalizedMessage( 501805 ); // That rune is not yet marked.
				}
				else if ( o is Runebook )
				{
					RunebookEntry e = ((Runebook)o).Default;

					if ( e != null )
						m_Owner.Effect( e.Location, e.Map, true );
					else
						from.SendLocalizedMessage( 502354 ); // Target is not marked.
				}
				else if ( o is Key && ((Key)o).KeyValue != 0 && ((Key)o).Link is BaseBoat )
				{
					BaseBoat boat = ((Key)o).Link as BaseBoat;

					if ( !boat.Deleted && boat.CheckKey( ((Key)o).KeyValue ) )
						m_Owner.Effect( boat.GetMarkedLocation(), boat.Map, false );
					else
						from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 502357, from.Name, "" ) ); // I can not recall from that object.
				}
				else
				{
					from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 502357, from.Name, "" ) ); // I can not recall from that object.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}