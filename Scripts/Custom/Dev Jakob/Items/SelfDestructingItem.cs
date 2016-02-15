using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1E5E, 0x1E5F )]
	public class SelfDestructingItem : Item
	{
		int m_TimeLeft;
		bool m_ShowTimeLeft;
		bool m_Running;
		SelfDestructTimer m_Timer;

		[Constructable]
		public SelfDestructingItem() : base( 0xFF1 )
		{
			Name = "Self destructing item";
			Movable = true;
		}

		public SelfDestructingItem( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version

			writer.Write( m_TimeLeft );
			writer.Write( m_ShowTimeLeft );
			writer.Write( m_Running );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_TimeLeft = reader.ReadInt();
			m_ShowTimeLeft = reader.ReadBool();
			m_Running = reader.ReadBool();

			if ( m_Running )
			{
				if ( m_TimeLeft <= 0 )
					Delete();
				else
				{
					m_Timer = new SelfDestructTimer( this );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TimeLeft
		{
			get
			{
				return m_TimeLeft;
			}
			set
			{
				m_TimeLeft = value;
				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
					m_Timer = new SelfDestructTimer( this );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowTimeLeft
		{
			get
			{
				return m_ShowTimeLeft;
			}
			set
			{
				m_ShowTimeLeft = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Running
		{
			get
			{
				return m_Running;
			}
			set
			{
				m_Running = value;
				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
				}
				if ( m_Running )
				{
					m_Timer = new SelfDestructTimer( this );
				}
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_ShowTimeLeft && m_Running )
			{
				int minutes = m_TimeLeft / 60;
				this.LabelTo( from, String.Format( "This item will vanish in {0} hours and {1} minutes.", minutes / 60, minutes % 60 ) );
			}
			base.OnSingleClick( from );
		}

		private class SelfDestructTimer : Timer
		{
			SelfDestructingItem m_Item;

			public SelfDestructTimer( SelfDestructingItem item ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Item = item;
				Start();
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Item == null || m_Item.Deleted )
				{
					Stop();
					return;
				}
				if ( m_Item.TimeLeft <= 0 )
				{
					m_Item.Delete();
					Stop();
					return;
				}
				m_Item.TimeLeft--;
			}
		}
	}
}