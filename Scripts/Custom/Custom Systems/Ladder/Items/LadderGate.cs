/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							LadderGAte.cs
*						-------------------------
*
*	File Description:	The Ladder Gate provides people with
*						transportation between Ladder Areas.
*
*/

using System;
using System.Collections;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Regions;

namespace Server.Ladder
{
	[DispellableFieldAttribute]
	public class LadderGate : Item
	{
		private bool m_bDispellable;


		[CommandProperty( AccessLevel.GameMaster )]
		public bool Dispellable
		{
			get
			{
				return m_bDispellable;
			}
			set
			{
				m_bDispellable = value;
			}
		}

		public virtual bool ShowFeluccaWarning{ get{ return false; } }


		[Constructable]
		public LadderGate() : base( 0xF6C )
		{

			Movable = false;
			Light = LightType.Circle300;
			Name = "Ladder Acess Gate";
			Hue = 1281;
		}

		public LadderGate(Serial serial) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.Player )
				return;

			if ( from.InRange( GetWorldLocation(), 1 ) )
				CheckGate( from, 1 );
			else
				from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m.Player )
				CheckGate( m, 0 );

			return true;
		}

		public virtual void CheckGate( Mobile m, int range )
		{
			new DelayTimer( m, this, range ).Start();
		}

		public virtual void OnGateUsed( Mobile m )
		{
		}

		public virtual void UseGate( Mobile m )
		{
			int flags = m.NetState == null ? 0 : m.NetState.Flags;

			if ( Factions.Sigil.ExistsOn( m ) )
			{
				m.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			}
			else if ( m.Spell != null )
			{
				m.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
			}
			else
			{
				m.CloseGump(typeof(LadderGateGump));
				m.SendGump(new LadderGateGump(0,50,50, this));


			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			// Version 1
			writer.Write( m_bDispellable );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_bDispellable = reader.ReadBool();
		}

		public virtual bool ValidateUse( Mobile from, bool message )
		{
			if ( from.Deleted || this.Deleted )
				return false;

			if ( from.Map != this.Map || !from.InRange( this, 1 ) )
			{
				if ( message )
					from.SendLocalizedMessage( 500446 ); // That is too far away.

				return false;
			}

			return true;
		}

		public virtual void DelayCallback( Mobile from, int range )
		{
			if ( !ValidateUse( from, false ) || !from.InRange( this, range ) )
				return;

			UseGate(from);

		}

		private class DelayTimer : Timer
		{
			private Mobile m_From;
			private LadderGate m_Gate;
			private int m_Range;

			public DelayTimer(Mobile from, LadderGate gate, int range) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_From = from;
				m_Gate = gate;
				m_Range = range;
			}

			protected override void OnTick()
			{
				m_Gate.DelayCallback( m_From, m_Range );
			}
		}
	}
}