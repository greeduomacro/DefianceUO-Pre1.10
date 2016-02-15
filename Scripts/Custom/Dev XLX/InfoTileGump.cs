using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class InfoTileGump : Gump
	{
		private Mobile m_From;
		private string m_Message;
		private Point3D m_Location;
		private Timer m_Timer;

		public virtual void StartTimer()
		{
			if ( m_Timer == null )
				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ), new TimerCallback( Refresh ) );
		}

		public virtual void StopTimer()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;
		}

		public InfoTileGump( Mobile from, string message, Point3D location ) : base( 100, 100 )
		{
			m_From = from;
			m_Message = message;
			m_Location = location;

			StartTimer();

			AddPage( 0 );
			AddBackground(400, 30, 375, 400, 9380);
			AddBackground(430, 80, 315, 300, 9300);

			AddHtml( 440, 90, 295, 280, message, false, false );
			//AddHtmlLocalized( 10, 10, 120, 100, 1060198, 0, false, false ); // May your wealth bring blessings to those in need, if tithed upon this most sacred site.
		}

		public virtual void Refresh()
		{
			if (!Utility.InRange(m_From.Location, m_Location, 6))
			{
				StopTimer();
				m_From.CloseGump( typeof( InfoTileGump ) );
			}
		}
	}
}