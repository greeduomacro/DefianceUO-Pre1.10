using System;

namespace Server.FSPvpPointSystem
{
	public class PvpRankInfo
	{
		private string m_Title;
		private string m_Abbreviation;
		private int m_Rank;
		private int m_Required;

		public string Title{ get{ return m_Title; } }
		public string Abbreviation{ get{ return m_Abbreviation; } }
		public int Rank{ get{ return m_Rank; } }
		public int Required{ get{ return m_Required; } }

		public PvpRankInfo( string title, string abbreviation, int rank, int required )
		{
			m_Title = title;
			m_Abbreviation = abbreviation;
			m_Rank = rank;
			m_Required = required;
		}

		public static int MaxRank = 13;

		private static PvpRankInfo[] m_Table = new PvpRankInfo[]
			{
				new PvpRankInfo( String.Empty, String.Empty, 0, -1 ),
				new PvpRankInfo( "Peasant", "(P)", 1, 500 ),
				new PvpRankInfo( "Mercenary", "(M)", 2, 1000 ),
				new PvpRankInfo( "Sergeant", "(S)", 3, 2500 ),
				new PvpRankInfo( "Lieutenant", "(L)", 4, 3500 ),
				new PvpRankInfo( "Captain", "(CT)", 5, 5000 ),
				new PvpRankInfo( "Grand Captain", "(GC)", 6, 6500 ),
				new PvpRankInfo( "Commander", "(CM)", 7, 7500 ),
				new PvpRankInfo( "Grand Commander", "(GCM)", 8, 8500 ),
				new PvpRankInfo( "General", "(GRN)", 9, 10000 ),
				new PvpRankInfo( "Grand General", "(GGR)", 10, 12000 ),
				new PvpRankInfo( "Marshal", "(MSH)", 11, 13500 ),
				new PvpRankInfo( "Grand Marshal", "(GMS)", 12, 15000 ),
				new PvpRankInfo( "Emperor", "(EMP)", 13, 17500 )
			};

		public static PvpRankInfo[] Table{ get{ return m_Table; } }

		public static PvpRankInfo GetInfo( int rank )
		{
			return m_Table[rank];
		}
	}
}