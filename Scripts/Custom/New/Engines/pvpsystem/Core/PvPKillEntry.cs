using System;

namespace Server.FSPvpPointSystem
{
	public class PvPKillEntry
	{
		public static readonly TimeSpan ExpirePeriod = TimeSpan.FromMinutes( 10.0 );

		private Mobile m_Killed;
		private DateTime m_TimeOfKill;

		public Mobile Killed{ get{ return m_Killed; } }
		public DateTime TimeOfKill{ get{ return m_TimeOfKill; } }

		public bool IsExpired{ get{ return ( m_TimeOfKill + ExpirePeriod ) < DateTime.Now; } }

		public PvPKillEntry( Mobile killed )
		{
			m_Killed = killed;
			m_TimeOfKill = DateTime.Now;
		}
	}
}