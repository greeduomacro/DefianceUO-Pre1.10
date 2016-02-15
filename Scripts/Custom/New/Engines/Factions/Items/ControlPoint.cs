using System;
using Server.Mobiles;

namespace Server.Factions
{
	public class ControlPoint : Item
	{
		private TownMonolith m_Monolith;
		private Faction m_Owner;
		private Mobile m_Capturer;
		private Timer m_CaptureTimer;
		private Timer m_ResetTimer;

		[CommandProperty( AccessLevel.Administrator )]
		public bool HasNeutralTimerRunning
		{
			get
			{
				return m_ResetTimer != null;
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public Faction Owner
		{
			get
			{
				return m_Owner;
			}
			set
			{
				m_Owner = value;
				UpdateHue();
				if ( m_Owner == null )
					EndNeutralTimer( false );
				else
					StartNeutralTimer();
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TownMonolith TownMonolith
		{
			get
			{
				return m_Monolith;
			}
			set
			{
				// be sure to remove it from the last monolith
				if (m_Monolith != null)
					m_Monolith.RemoveControlPoint( this );

				m_Monolith = value;
				// ...and add it to the new
				m_Monolith.AddControlPoint( this );
			}
		}

		[Constructable]
		public ControlPoint() : base( 1183 )
		{
			Movable = false;
			Name = "Control Point";
			UpdateHue();
		}

		public ControlPoint( Serial serial ) : base( serial )
		{
		}

		public override void OnDelete()
		{
			if ( m_Monolith != null )
				m_Monolith.RemoveControlPoint( this );
			base.OnDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Monolith != null );
			if ( m_Monolith != null )
				writer.Write( m_Monolith );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					if ( reader.ReadBool() )
						m_Monolith = (TownMonolith)reader.ReadItem();

					break;
				}
			}

			UpdateHue();
		}

		public void UpdateHue()
		{
			if ( m_Owner != null )
				Hue = m_Owner.Definition.HuePrimary;
			else
				Hue = 2301; // gray;
		}

		public override bool OnMoveOver( Mobile m )
		{
			// first check: the mobile actually exists and is a live player
			if ( m != null && m is PlayerMobile && m.Alive )
			{
				// second check: connected to monolith, monolith connected to town, and there is nobody currently trying to capture
				if ( m_Monolith != null && m_Monolith.Town != null && m_Capturer == null )
				{
					// third check: there is no sigil at all or a pure sigil on the monolith
					//if ( m_Monolith.Sigil == null || !m_Monolith.Sigil.IsPurifying )
					{
						// fourth check: the player is a faction member and the control point is owned by another faction than the player
						PlayerState ps = PlayerState.Find( m );
						if ( ps != null && !ps.IsLeaving && ps.Faction != m_Owner )
						{
							BeginCapture( m );
						}
					}
				}
			}

			return base.OnMoveOver( m );
		}

		public void BeginCapture( Mobile m )
		{
			m_Capturer = m;
			m_CaptureTimer = new CaptureTimer( m, this );
			m_CaptureTimer.Start();

			Faction captureFaction = Faction.Find( m );

			foreach ( Faction faction in Faction.Factions )
			{
				string factionName, townName;
				if ( faction == captureFaction )
					factionName = "Your faction";
				else
					factionName = captureFaction.Definition.FriendlyName;

				townName = m_Monolith.Town.Definition.FriendlyName;

				string broadcastString = String.Format( "{0} is in progress of capturing a control point in {1}!", factionName, townName);
				faction.Broadcast( 0x27, broadcastString );
			}
			EndNeutralTimer( false );
		}

		public void EndCapture( Mobile m, bool successful )
		{
			if ( successful )
			{
				Faction f = Faction.Find( m );
				if ( f != null )
				{
					Owner = f;

					foreach ( Faction faction in Faction.Factions )
					{
						string factionName, townName;
						if ( faction == f )
							factionName = "Your faction";
						else
							factionName = f.Definition.FriendlyName;

						townName = m_Monolith.Town.Definition.FriendlyName;

						string broadcastString = String.Format( "{0} has gained control over a control point in {1}!", factionName, townName);
						faction.Broadcast( 0x27, broadcastString );
					}
				}
			}
			else if ( Owner != null )
				StartNeutralTimer();

			m_Capturer = null;
			if ( m_CaptureTimer != null )
			{
				m_CaptureTimer.Stop();
				m_CaptureTimer = null;
			}
		}

		public void StartNeutralTimer()
		{
			EndNeutralTimer( false );
			m_ResetTimer = Timer.DelayCall( TimeSpan.FromMinutes( 10.0 ), new TimerCallback( EndNeutralTimer ) );
		}

		public void EndNeutralTimer()
		{
			EndNeutralTimer( true );
		}

		public void EndNeutralTimer( bool success )
		{
			if ( success )
				Owner = null;
			if ( m_ResetTimer != null )
			{
				m_ResetTimer.Stop();
				m_ResetTimer = null;
			}
		}

		public class CaptureTimer : Timer
		{
			private const int SecondsForCapture = 120 * 1; // 2 minutes

			private Mobile m_Capturer;
			private ControlPoint m_ControlPoint;
			private int m_TicksLeft;

			public CaptureTimer(Mobile capturer, ControlPoint controlPoint) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Capturer = capturer;
				m_ControlPoint = controlPoint;
				m_TicksLeft = SecondsForCapture;

				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Capturer.Location != m_ControlPoint.Location )
					m_ControlPoint.EndCapture( m_Capturer, false );
				else
				{
					m_TicksLeft--;

					if ( m_TicksLeft <= 0 )
						m_ControlPoint.EndCapture( m_Capturer, true );
					else if ( m_TicksLeft % 10 == 0 )
						m_Capturer.SendMessage( "There are {0} seconds left...", m_TicksLeft );

					if ( m_TicksLeft == SecondsForCapture / 2 )
					{
						Faction captureFaction = Faction.Find( m_Capturer );

						foreach ( Faction faction in Faction.Factions )
						{
							string factionName, townName;
							if ( faction == captureFaction )
								factionName = "Your faction";
							else
								factionName = captureFaction.Definition.FriendlyName;

							if ( m_ControlPoint.TownMonolith != null && m_ControlPoint.TownMonolith.Town != null )
							{
								townName = m_ControlPoint.TownMonolith.Town.Definition.FriendlyName;

								string broadcastString = String.Format( "{0} will capture a control point in {1} very soon!", factionName, townName);
								faction.Broadcast( 0x27, broadcastString );
							}
						}
					}
				}
			}
		}
	}
}