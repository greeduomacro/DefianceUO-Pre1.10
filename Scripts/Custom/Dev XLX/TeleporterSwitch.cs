/***********************************
 * Script: TeleporterSwitch.cs     *
 * Author: Aaron Sithsong [GOD]    *
 * Version: 1.2                    *
 ***********************************/

using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class TeleporterSwitch : Item
	{

		private Point3D m_SymbolAppears;
		private Point3D m_TeleportsTo;
		private Map m_SymbolMap;
		private Map m_TeleportsToMap;
		private TimeSpan m_LastsFor;
		private static bool IsUsable;
		private Mobile m_from;
		private Item tele;
		private Item marker;

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D SymbolAppears
		{
			get { return m_SymbolAppears; }
			set { m_SymbolAppears = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D TeleportsTo
		{
			get { return m_TeleportsTo; }
			set { m_TeleportsTo = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map SymbolMap
		{
			get { return m_SymbolMap; }
			set { m_SymbolMap = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map TeleportsToMap
		{
			get { return m_TeleportsToMap; }
			set { m_TeleportsToMap = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan LastsFor
		{
			get { return m_LastsFor; }
			set { m_LastsFor = value; InvalidateProperties(); }
		}

		[Constructable]
		public TeleporterSwitch() : this( 0x1093 )
		{
			Name = "switch";
			Movable = false;
			IsUsable = true;
		}

		protected TeleporterSwitch( int itemID ) : base( itemID )
		{
			Movable = false;
		}

		public override void OnDoubleClick( Mobile m )
		{
			m_from = m;

			if ( !m.InRange( this, 2 ) )
			{
				m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			Flip();
		}

		protected virtual void Flip()
		{
			if ( m_from.BeginAction( typeof( TeleporterSwitch ) ) && IsUsable )
			{
				ItemID = 0x1095;
				Effects.PlaySound( Location, Map, 0x3E8 );
				IsUsable = false;
				tele = new Teleporter( TeleportsTo, TeleportsToMap );
            	tele.MoveToWorld(new Point3D( SymbolAppears ), SymbolMap);
            	marker = new TSalchsymbol();
            	marker.MoveToWorld(new Point3D( SymbolAppears ), SymbolMap);

				Timer.DelayCall( LastsFor , new TimerStateCallback( ReleaseSwitchField ), m_from );
				StartResetTimer( LastsFor );
			}
			else
			{
				m_from.SendMessage( "The switch is magically held in place, you notice small alchemical symbols counting the field's time." );
			}
		}

		private static void ReleaseSwitchField( object state )
		{
			((Mobile)state).EndAction( typeof( TeleporterSwitch ) );
		}

		private ResetTimer m_ResetTimer;

		protected void StartResetTimer( TimeSpan delay )
		{
			StopResetTimer();

			m_ResetTimer = new ResetTimer( this, delay );
			m_ResetTimer.Start();
		}

		protected void StopResetTimer()
		{
			if ( m_ResetTimer != null )
			{
				m_ResetTimer.Stop();
				m_ResetTimer = null;
			}
		}

		protected virtual void Reset()
		{
			if ( ItemID != 0x1093 )
			{
				ItemID = 0x1093;
				Effects.PlaySound( Location, Map, 0x3E8 );
			}

			if ( tele != null )
				tele.Delete();
			if ( marker != null )
				marker.Delete();

			IsUsable = true;
		}

		private class ResetTimer : Timer
		{
			private TeleporterSwitch m_TeleporterSwitch;

			public ResetTimer( TeleporterSwitch teleporterSwitch, TimeSpan delay ) : base( delay )
			{
				m_TeleporterSwitch = teleporterSwitch;

				Priority = ComputePriority( delay );
			}

			protected override void OnTick()
			{
				if ( m_TeleporterSwitch.Deleted )
					return;

				m_TeleporterSwitch.m_ResetTimer = null;

				m_TeleporterSwitch.Reset();
			}
		}

		public TeleporterSwitch( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( m_SymbolAppears );
			writer.Write( m_TeleportsTo );
			writer.Write( m_SymbolMap );
			writer.Write( m_TeleportsToMap );
			writer.Write( (TimeSpan) m_LastsFor );
			writer.Write( IsUsable );
			writer.Write( m_from );
			writer.Write( tele );
			writer.Write( marker );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_SymbolAppears = reader.ReadPoint3D();
			m_TeleportsTo = reader.ReadPoint3D();
			m_SymbolMap = reader.ReadMap();
			m_TeleportsToMap = reader.ReadMap();
			m_LastsFor = reader.ReadTimeSpan();
			IsUsable = reader.ReadBool();
			m_from = reader.ReadMobile();
			tele = reader.ReadItem();
			marker = reader.ReadItem();

			Reset();
		}
	}

/*************************************************************************
******* This is for the alchemy symbol. Just un-comment the line *********
******** [Constructable] if you want to be able to add it in game*********
**************************************************************************/

	public class TSalchsymbol : Item
	{

		//[Constructable]
		public TSalchsymbol() : base( 0x1822 )
		{
			Weight = 1;
			Movable = false;
			Name = "teleporter";
		}

		public TSalchsymbol( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}