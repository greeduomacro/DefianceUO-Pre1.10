//By Rokam
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Misc
{
	public class SpecialHueEntry
	{
		private int m_Hue;

		public int Hue
		{
			get
			{
				return m_Hue;
			}
		}

		public SpecialHueEntry( int hue )
		{
			m_Hue = hue;
		}
	}

	public class SpecialHueList
	{
		private string m_HueGroup;
		private SpecialHueEntry[] m_Entries;

		public string HueGroup
		{
			get
			{
				return m_HueGroup;
			}
		}

		public SpecialHueEntry[] Entries
		{
			get
			{
				return m_Entries;
			}
		}

		public SpecialHueList( string hueGroup, SpecialHueEntry[] entries )
		{
			m_HueGroup = hueGroup;
			m_Entries = entries;
		}

		public static readonly SpecialHueList Violet =
			new SpecialHueList( "Violet", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1221 ), // Hue 1
					new SpecialHueEntry( 1222 ), // Hue 2
					new SpecialHueEntry( 1223 ), // Hue 3
					new SpecialHueEntry( 1224 ), // Hue 4
					new SpecialHueEntry( 1225 ), // Hue 5
					new SpecialHueEntry( 1226 ), // Hue 6
				} );

		public static readonly SpecialHueList Tan =
			new SpecialHueList( "Tan", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1501 ), // Hue 1
					new SpecialHueEntry( 1502 ), // Hue 2
					new SpecialHueEntry( 1503 ), // Hue 3
					new SpecialHueEntry( 1504 ), // Hue 4
					new SpecialHueEntry( 1505 ), // Hue 5
					new SpecialHueEntry( 1506 ), // Hue 6
					new SpecialHueEntry( 1507 ), // Hue 7
					new SpecialHueEntry( 1508 ), // Hue 8
				} );

		public static readonly SpecialHueList Brown =
			new SpecialHueList( "Brown", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1853 ), // Hue 1
					new SpecialHueEntry( 1854 ), // Hue 2
					new SpecialHueEntry( 1855 ), // Hue 3
					new SpecialHueEntry( 1856 ), // Hue 4
					new SpecialHueEntry( 1857 ), // Hue 5
					new SpecialHueEntry( 1858 ), // Hue 6
				} );

		public static readonly SpecialHueList DarkBlue =
			new SpecialHueList( "Dark Blue", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1303 ), // Hue 1
					new SpecialHueEntry( 1304 ), // Hue 2
					new SpecialHueEntry( 1305 ), // Hue 3
					new SpecialHueEntry( 1306 ), // Hue 4
					new SpecialHueEntry( 1307 ), // Hue 5
					new SpecialHueEntry( 1308 ), // Hue 6
				} );

		public static readonly SpecialHueList ForestGreen =
			new SpecialHueList( "Forest Green", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1420 ), // Hue 1
					new SpecialHueEntry( 1421 ), // Hue 2
					new SpecialHueEntry( 1422 ), // Hue 3
					new SpecialHueEntry( 1423 ), // Hue 4
					new SpecialHueEntry( 1424 ), // Hue 5
					new SpecialHueEntry( 1425 ), // Hue 6
					new SpecialHueEntry( 1426 ), // Hue 7
				} );

		public static readonly SpecialHueList Pink =
			new SpecialHueList( "Pink", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1600 ), // Hue 1
					new SpecialHueEntry( 1601 ), // Hue 2
					new SpecialHueEntry( 1602 ), // Hue 3
					new SpecialHueEntry( 1603 ), // Hue 4
					new SpecialHueEntry( 1604 ), // Hue 5
					new SpecialHueEntry( 1605 ), // Hue 6
					new SpecialHueEntry( 1606 ), // Hue 7
					new SpecialHueEntry( 1607 ), // Hue 8
				} );

		public static readonly SpecialHueList Red =
			new SpecialHueList( "Red", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1631 ), // Hue 1
					new SpecialHueEntry( 1632 ), // Hue 2
					new SpecialHueEntry( 1633 ), // Hue 3
					new SpecialHueEntry( 1634 ), // Hue 4
					new SpecialHueEntry( 1635 ), // Hue 5
				} );

		public static readonly SpecialHueList Olive =
			new SpecialHueList( "Olive", new SpecialHueEntry[]
				{
					new SpecialHueEntry( 1446 ), // Hue 1
					new SpecialHueEntry( 1447 ), // Hue 2
					new SpecialHueEntry( 1448 ), // Hue 3
					new SpecialHueEntry( 1449 ), // Hue 4
					new SpecialHueEntry( 1450 ), // Hue 5
				} );

		public static readonly SpecialHueList[] SpecialHueLists = new SpecialHueList[]{ Violet, Tan, Brown, DarkBlue, ForestGreen, Pink, Red, Olive };
	}
}