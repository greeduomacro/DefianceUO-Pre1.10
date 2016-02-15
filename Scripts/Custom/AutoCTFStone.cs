using System;
using System.Collections;
using Server;

namespace Server.Items
{
	public class AutoCTFStone : Item
	{
		[Constructable]
		public AutoCTFStone() : base( 0xEDC )
		{
			Hue = 1114;
			Movable = false;
			Name = "AUTO-CTF JOIN STONE!";
			Visible = false;
			m_Times = new TimeSpan[2];
		}

		public AutoCTFStone( Serial serial ) : base( serial )
		{
		}

		private CTFGame m_Game;
		private GameJoinStone m_Stone;
		private TimeSpan[] m_Times;
		private bool m_Active;
		private Timer m_Timer;
		private TimeSpan m_Duration;
		private BaseDoor m_Doors;
		private string m_CTFJoinMessage;
		private string m_CTFStartMessage;
		private int m_CTFMessageHue;

		public Timer CTFTimer{ get{ return m_Timer; } set{ m_Timer = value; } }

		public TimeSpan[] Times{ get{ return m_Times; } set{ m_Times = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Duration{ get{ return m_Duration; } set{ m_Duration = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan DayTime{ get{ return m_Times[0]; } set{ m_Times[0] = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NightTime{ get{ return m_Times[1]; } set{ m_Times[1] = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public CTFGame Game{ get{ return m_Game; } set{ m_Game = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public GameJoinStone JoinStone{ get{ return m_Stone; } set{ m_Stone = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active{ get{ return m_Active; } set{ Activate( value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseDoor Doors{ get{ return m_Doors; } set{ m_Doors = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string CTFJoinMessage{ get{ return m_CTFJoinMessage; } set{ m_CTFJoinMessage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string CTFStartMessage{ get{ return m_CTFStartMessage; } set{ m_CTFStartMessage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int CTFMessageHue{ get{ return m_CTFMessageHue; } set{ m_CTFMessageHue = value; } }

		public void Activate( bool newvalue )
		{
			if ( m_Active ) //Currently Active
			{
				if ( !newvalue ) //We are disabling it!
				{
					QuitGame();
					if ( m_Timer != null )
					{
						m_Timer.Stop();
						m_Timer = null;
					}
				}
			}
			else if ( newvalue ) //Currently Deactive, we are enabling it!
			{
				QuitGame();
				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
				}

				m_Timer = new AutoCTFTimer( this );
				m_Timer.Start();
			}

			m_Active = newvalue;
		}

		public bool Validate()
		{
			return m_Stone != null && m_Game != null;
		}

		public void QuitGame()
		{
			if ( m_Game != null )
				m_Game.EndGame();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( m_Game );
			writer.Write( m_Stone );
			writer.WriteEncodedInt( m_Times.Length );
			for ( int i = 0; i < m_Times.Length; i++ )
				writer.Write( m_Times[i] );
			writer.Write( m_Active );
			writer.Write( m_Duration );
			writer.Write( m_Doors );
			writer.Write( m_CTFJoinMessage );
			writer.Write( m_CTFStartMessage );
			writer.Write( m_CTFMessageHue );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_Game = reader.ReadItem() as CTFGame;
			m_Stone = reader.ReadItem() as GameJoinStone;
			int count = reader.ReadEncodedInt();
			m_Times = new TimeSpan[count];
			for ( int i = 0; i < count; i++ )
				m_Times[i] = reader.ReadTimeSpan();
			bool active = reader.ReadBool();
			m_Duration = reader.ReadTimeSpan();
			m_Doors = reader.ReadItem() as BaseDoor;
			m_CTFJoinMessage = reader.ReadString();
			m_CTFStartMessage = reader.ReadString();
			m_CTFMessageHue = reader.ReadInt();

			Activate( active );
		}
	}

	public class AutoCTFTimer : Timer
	{
		public AutoCTFStone m_Stone;
		public Blocker m_Blocker;

		private DateTime m_CTFTime;

		public AutoCTFTimer( AutoCTFStone stone ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;
			m_Stone = stone;
			SetDate();
		}

		private void CTF_Callback()
		{
			if ( m_Stone != null )
			{
				if ( m_Stone.Validate() )
				{
					m_Stone.JoinStone.RandomTeam = true;
					m_Stone.JoinStone.Game = m_Stone.Game;
					m_Stone.Game.OpenJoin = true;

					if ( m_Stone.Doors != null )
					{
						m_Blocker = new Blocker();
						m_Blocker.MoveToWorld( m_Stone.Doors.Location, m_Stone.Doors.Map );

						ArrayList list = m_Stone.Doors.GetChain();

						for ( int i = 0; i < list.Count; ++i )
							((BaseDoor)list[i]).Open = true;
					}
					else
						m_Blocker = null;

					m_Stone.JoinStone.Visible = true;

					new AutoCTFFinTimer( m_Stone, m_Blocker ).Start();

					SetDate();
				}
				else
					m_Stone.Activate( false );
			}
			else
				Stop();
		}

		protected override void OnTick()
		{
			if ( DateTime.Now >= m_CTFTime )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( CTF_Callback ) );
		}

		private void SetDate()
		{
			if ( m_Stone == null )
				Stop();
			else
			{
				int index = 0;

				DateTime datenow = DateTime.Now;

				while ( m_CTFTime < datenow )
				{
					DateTime date = DateTime.Now.Date;
					if ( index < m_Stone.Times.Length )
						m_CTFTime = date + m_Stone.Times[index++];
					else
						m_CTFTime = (date + m_Stone.Times[0]).AddDays( 1.0 );
				}
			}
		}
	}

	public class AutoCTFFinTimer : Timer
	{
		public AutoCTFStone m_Stone;
		public DateTime m_Start;
		public Blocker m_Blocker;

		public AutoCTFFinTimer( AutoCTFStone stone, Blocker blocker ) : base( TimeSpan.Zero, TimeSpan.FromMinutes( 1.0 ) )
		{
			m_Stone = stone;
			m_Start = DateTime.Now + m_Stone.Duration;
			m_Blocker = blocker;
		}

		protected override void OnTick()
		{
			if ( DateTime.Now < m_Start )
				World.Broadcast( m_Stone.CTFMessageHue, true, m_Stone.CTFJoinMessage );
			else if ( m_Stone != null && m_Stone.Validate() )
			{
				if ( m_Blocker != null )
					m_Blocker.Delete();

				if ( m_Stone.Doors != null )
				{
					ArrayList list = m_Stone.Doors.GetChain();

					for ( int i = 0; i < list.Count; ++i )
						((BaseDoor)list[i]).Open = false;
				}

				m_Stone.JoinStone.Visible = false;
				m_Stone.Game.OpenJoin = false;

				World.Broadcast( m_Stone.CTFMessageHue, true, m_Stone.CTFStartMessage );

				m_Stone.Game.StartGame( false );

				Stop();
			}
			else
				Stop();
		}
	}
}