using System;

namespace Server.Engines.VeteranRewards
{
	public class RewardList
	{
		private TimeSpan m_Age;
		private TimeSpan m_GameTime;
		private RewardEntry[] m_Entries;

		public TimeSpan Age{ get{ return m_Age; } }
		public TimeSpan GameTime { get { return m_GameTime; } }
		public RewardEntry[] Entries{ get{ return m_Entries; } }

		public RewardList( TimeSpan interval, TimeSpan gameTime, int index, RewardEntry[] entries )
		{
			m_Age = TimeSpan.FromDays( interval.TotalDays * index );
			m_GameTime = TimeSpan.FromDays(gameTime.TotalDays * index);
			m_Entries = entries;

			for ( int i = 0; i < entries.Length; ++i )
				entries[i].List = this;
		}
	}
}