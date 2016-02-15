//By Shaft
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Misc
{

	public class HueEntry
	{
		private int m_Hue;

		public int Hue
		{
			get
			{
				return m_Hue;
			}
		}

		public HueEntry( int hue )
		{
			m_Hue = hue;
		}
	}

	public class HueList
	{
		private string m_HueGroup;
		private HueEntry[] m_Entries;

		public string HueGroup
		{
			get
			{
				return m_HueGroup;
			}
		}

		public HueEntry[] Entries
		{
			get
			{
				return m_Entries;
			}
		}

		public HueList( string hueGroup, HueEntry[] entries )
		{
			m_HueGroup = hueGroup;
			m_Entries = entries;
		}

		public static readonly HueList DullCopper =
			new HueList( "Dull Copper", new HueEntry[]
				{
					new HueEntry( 2418 ), // Hue 1
					new HueEntry( 2419 ), // Hue 2
					new HueEntry( 2420 ), // Hue 3
					new HueEntry( 2421 ), // Hue 4
					new HueEntry( 2422 ), // Hue 5
					new HueEntry( 2423 ), // Hue 6
				} );

		public static readonly HueList ShadowIron =
			new HueList( "Shadow Iron", new HueEntry[]
				{
					new HueEntry( 2405 ), // Hue 1
					new HueEntry( 2406 ), // Hue 2
					new HueEntry( 2407 ), // Hue 3
					new HueEntry( 2408 ), // Hue 4
					new HueEntry( 2409 ), // Hue 5
					new HueEntry( 2410 ), // Hue 6
					new HueEntry( 2411 ), // Hue 7
				} );

		public static readonly HueList Copper =
			new HueList( "Copper", new HueEntry[]
				{
					new HueEntry( 2412 ), // Hue 1
					new HueEntry( 2413 ), // Hue 2
					new HueEntry( 2414 ), // Hue 3
					new HueEntry( 2415 ), // Hue 4
					new HueEntry( 2416 ), // Hue 5
					new HueEntry( 2417 ), // Hue 6
				} );

		public static readonly HueList Bronze =
			new HueList( "Bronze", new HueEntry[]
				{
					new HueEntry( 2413 ), // Hue 1
					new HueEntry( 2414 ), // Hue 2
					new HueEntry( 2415 ), // Hue 3
					new HueEntry( 2416 ), // Hue 4
					new HueEntry( 2417 ), // Hue 5
				} );

		public static readonly HueList Golden =
			new HueList( "Golden", new HueEntry[]
				{
					new HueEntry( 2212 ), // Hue 1
					new HueEntry( 2213 ), // Hue 2
					new HueEntry( 2214 ), // Hue 3
					new HueEntry( 2215 ), // Hue 4
					new HueEntry( 2216 ), // Hue 5
					new HueEntry( 2217 ), // Hue 6
				} );

		public static readonly HueList Agapite =
			new HueList( "Agapite", new HueEntry[]
				{
					new HueEntry( 2424 ), // Hue 1
					new HueEntry( 2425 ), // Hue 2
					new HueEntry( 2426 ), // Hue 3
					new HueEntry( 2427 ), // Hue 4
					new HueEntry( 2428 ), // Hue 5
					new HueEntry( 2429 ), // Hue 6
				} );

		public static readonly HueList Verite =
			new HueList( "Verite", new HueEntry[]
				{
					new HueEntry( 2206 ), // Hue 1
					new HueEntry( 2207 ), // Hue 2
					new HueEntry( 2208 ), // Hue 3
					new HueEntry( 2209 ), // Hue 4
					new HueEntry( 2210 ), // Hue 5
					new HueEntry( 2211 ), // Hue 6
				} );

		public static readonly HueList Valorite =
			new HueList( "Valorite", new HueEntry[]
				{
					new HueEntry( 2218 ), // Hue 1
					new HueEntry( 2219 ), // Hue 2
					new HueEntry( 2220 ), // Hue 3
					new HueEntry( 2221 ), // Hue 4
					new HueEntry( 2222 ), // Hue 5
					new HueEntry( 2223 ), // Hue 6
				} );

		public static readonly HueList Reds =
			new HueList( "Reds", new HueEntry[]
				{
					new HueEntry( 2112 ), // Hue 1
					new HueEntry( 2113 ), // Hue 2
					new HueEntry( 2114 ), // Hue 3
					new HueEntry( 2115 ), // Hue 4
					new HueEntry( 2116 ), // Hue 5
					new HueEntry( 2117 ), // Hue 6
				} );

		public static readonly HueList Blues =
			new HueList( "Blues", new HueEntry[]
				{
					new HueEntry( 2118 ), // Hue 1
					new HueEntry( 2119 ), // Hue 2
					new HueEntry( 2120 ), // Hue 3
					new HueEntry( 2121 ), // Hue 4
					new HueEntry( 2122 ), // Hue 5
					new HueEntry( 2123 ), // Hue 6
				} );

		public static readonly HueList Greens =
			new HueList( "Greens", new HueEntry[]
				{
					new HueEntry( 2125 ), // Hue 1
					new HueEntry( 2126 ), // Hue 2
					new HueEntry( 2127 ), // Hue 3
					new HueEntry( 2128 ), // Hue 4
					new HueEntry( 2129 ), // Hue 5
				} );

		public static readonly HueList Yellows =
			new HueList( "Yellows", new HueEntry[]
				{
					new HueEntry( 2212 ), // Hue 1
					new HueEntry( 2213 ), // Hue 2
					new HueEntry( 2214 ), // Hue 3
					new HueEntry( 2215 ), // Hue 4
					new HueEntry( 2216 ), // Hue 5
					new HueEntry( 2217 ), // Hue 6
				} );

		public static readonly HueList[] HueLists = new HueList[]{ DullCopper, ShadowIron, Copper, Bronze, Golden, Agapite, Verite, Valorite, Reds, Blues, Greens, Yellows };
	}
}