using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Multis;

namespace Server.Items
{


	public class CarpetAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new CarpetAddonDeed(); } }

		#region Constructors
		[Constructable]
		public CarpetAddon( CarpetType type, int width, int height ) : this( (int)type, width, height )
		{
		}

		public CarpetAddon( int type, int width, int height )
		{
			CarpetInfo info = CarpetInfo.GetInfo( type );

			AddComponent( new AddonComponent( info.GetItemPart( Position.Top ).ItemID ), 0, 0, 0 );
			AddComponent( new AddonComponent( info.GetItemPart( Position.Right ).ItemID ), width, 0, 0 );
			AddComponent( new AddonComponent( info.GetItemPart( Position.Left ).ItemID ), 0, height, 0 );
			AddComponent( new AddonComponent( info.GetItemPart( Position.Bottom ).ItemID ), width, height, 0 );

			int w = width - 1;
			int h = height - 1;

			for ( int y = 1; y <= h; ++y )
				AddComponent( new AddonComponent( info.GetItemPart( Position.West ).ItemID ), 0, y, 0 );

			for ( int x = 1; x <= w; ++x )
				AddComponent( new AddonComponent( info.GetItemPart( Position.North ).ItemID ), x, 0, 0 );

			for ( int y = 1; y <= h; ++y )
				AddComponent( new AddonComponent( info.GetItemPart( Position.East ).ItemID ), width, y, 0 );

			for ( int x = 1; x <= w; ++x )
				AddComponent( new AddonComponent( info.GetItemPart( Position.South ).ItemID ), x, height, 0 );

			for ( int x = 1; x <= w; ++x )
				for ( int y = 1; y <= h; ++y )
					AddComponent( new AddonComponent( info.GetItemPart( Position.Center ).ItemID ), x, y, 0 );
		}

		public CarpetAddon( Serial serial ) : base( serial )
		{
		}
		#endregion

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public enum CarpetType
	{
		PlainBlue,
		FloweredBlue1,
		FloweredBlue2,
		PlainRed,
		FloweredRed1,
		FloweredRed2,
		RedBlue1,
		RedBlue2,
		BrownBlue,
		BrownishRed,
		TanBrown
	}

	public enum Position
	{
		Top,
		Bottom,
		Left,
		Right,
		West,
		North,
		East,
		South,
		Center
	}

	public class CarpetInfo
	{
		private ItemPart[] m_Entries;

		public ItemPart[] Entries{ get{ return m_Entries; } }

		public CarpetInfo( ItemPart[] entries )
		{
			m_Entries = entries;
		}

		public ItemPart GetItemPart( Position pos )
		{
			int i = (int)pos;

			if ( i < 0 || i >= m_Entries.Length )
				i = 0;

			return m_Entries[i];
		}

		public static CarpetInfo GetInfo( int type )
		{
			if ( type < 0 || type >= m_Infos.Length )
				type = 0;

			return m_Infos[type];
		}

		#region CarpetInfo definitions
		private static CarpetInfo[] m_Infos = new CarpetInfo[] {
/* PlainBlue */		new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAC3, Position.Top, 44, 0 ),
						new ItemPart( 0xAC2, Position.Bottom, 44, 68 ),
						new ItemPart( 0xAC4, Position.Left, 0, 28 ),
						new ItemPart( 0xAC5, Position.Right, 88, 28 ),
						new ItemPart( 0xAF6, Position.West, 22, 12 ),
						new ItemPart( 0xAF7, Position.North, 66, 12 ),
						new ItemPart( 0xAF8, Position.East, 66, 46 ),
						new ItemPart( 0xAF9, Position.South, 22, 46 ),
						new ItemPart( 0xABE, Position.Center, 44, 24 )
					}),
/* FloweredBlue1 */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAC3, Position.Top, 44, 0 ),
						new ItemPart( 0xAC2, Position.Bottom, 44, 68 ),
						new ItemPart( 0xAC4, Position.Left, 0, 28 ),
						new ItemPart( 0xAC5, Position.Right, 88, 28 ),
						new ItemPart( 0xAF6, Position.West, 22, 12 ),
						new ItemPart( 0xAF7, Position.North, 66, 12 ),
						new ItemPart( 0xAF8, Position.East, 66, 46 ),
						new ItemPart( 0xAF9, Position.South, 22, 46 ),
						new ItemPart( 0xABD, Position.Center, 44, 24 )
					}),
/* FloweredBlue2 */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAC3, Position.Top, 44, 0 ),
						new ItemPart( 0xAC2, Position.Bottom, 44, 68 ),
						new ItemPart( 0xAC4, Position.Left, 0, 28 ),
						new ItemPart( 0xAC5, Position.Right, 88, 28 ),
						new ItemPart( 0xAF6, Position.West, 22, 12 ),
						new ItemPart( 0xAF7, Position.North, 66, 12 ),
						new ItemPart( 0xAF8, Position.East, 66, 46 ),
						new ItemPart( 0xAF9, Position.South, 22, 46 ),
						new ItemPart( 0xABF, Position.Center, 44, 24 )
					}),
/* PlainRed */		new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xACA, Position.Top, 44, 0 ),
						new ItemPart( 0xAC9, Position.Bottom, 44, 68 ),
						new ItemPart( 0xACB, Position.Left, 0, 28 ),
						new ItemPart( 0xACC, Position.Right, 88, 28 ),
						new ItemPart( 0xACD, Position.West, 22, 11 ),
						new ItemPart( 0xACE, Position.North, 66, 12 ),
						new ItemPart( 0xACF, Position.East, 66, 46 ),
						new ItemPart( 0xAD0, Position.South, 22, 46 ),
						new ItemPart( 0xAC8, Position.Center, 44, 24 )
					}),
/* FloweredRed1 */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xACA, Position.Top, 44, 0 ),
						new ItemPart( 0xAC9, Position.Bottom, 44, 68 ),
						new ItemPart( 0xACB, Position.Left, 0, 28 ),
						new ItemPart( 0xACC, Position.Right, 88, 28 ),
						new ItemPart( 0xACD, Position.West, 22, 11 ),
						new ItemPart( 0xACE, Position.North, 66, 12 ),
						new ItemPart( 0xACF, Position.East, 66, 46 ),
						new ItemPart( 0xAD0, Position.South, 22, 46 ),
						new ItemPart( 0xAC7, Position.Center, 44, 24 )
					}),
/* FloweredRed2 */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xACA, Position.Top, 44, 0 ),
						new ItemPart( 0xAC9, Position.Bottom, 44, 68 ),
						new ItemPart( 0xACB, Position.Left, 0, 28 ),
						new ItemPart( 0xACC, Position.Right, 88, 28 ),
						new ItemPart( 0xACD, Position.West, 22, 11 ),
						new ItemPart( 0xACE, Position.North, 66, 12 ),
						new ItemPart( 0xACF, Position.East, 66, 46 ),
						new ItemPart( 0xAD0, Position.South, 22, 46 ),
						new ItemPart( 0xAC6, Position.Center, 44, 24 )
					}),
/* RedBlue1 */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAEF, Position.Top, 44, 0 ),
						new ItemPart( 0xAEE, Position.Bottom, 44, 80 ),
						new ItemPart( 0xAF0, Position.Left, 0, 40 ),
						new ItemPart( 0xAF1, Position.Right, 88, 40 ),
						new ItemPart( 0xAF2, Position.West, 22, 18 ),
						new ItemPart( 0xAF3, Position.North, 66, 18 ),
						new ItemPart( 0xAF4, Position.East, 66, 58 ),
						new ItemPart( 0xAF5, Position.South, 22, 58 ),
						new ItemPart( 0xAEC, Position.Center, 44, 36 )
					}),
/* RedBlue2 */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAEF, Position.Top, 44, 0 ),
						new ItemPart( 0xAEE, Position.Bottom, 44, 80 ),
						new ItemPart( 0xAF0, Position.Left, 0, 40 ),
						new ItemPart( 0xAF1, Position.Right, 88, 40 ),
						new ItemPart( 0xAF2, Position.West, 22, 18 ),
						new ItemPart( 0xAF3, Position.North, 66, 18 ),
						new ItemPart( 0xAF4, Position.East, 66, 58 ),
						new ItemPart( 0xAF5, Position.South, 22, 58 ),
						new ItemPart( 0xAED, Position.Center, 44, 36 )
					}),
/* BrownBlue */		new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAD3, Position.Top, 44, 0 ),
						new ItemPart( 0xAD2, Position.Bottom, 44, 80 ),
						new ItemPart( 0xAD4, Position.Left, 0, 40 ),
						new ItemPart( 0xAD5, Position.Right, 88, 40 ),
						new ItemPart( 0xAD6, Position.West, 22, 18 ),
						new ItemPart( 0xAD7, Position.North, 66, 18 ),
						new ItemPart( 0xAD8, Position.East, 66, 58 ),
						new ItemPart( 0xAD9, Position.South, 22, 58 ),
						new ItemPart( 0xAD1, Position.Center, 44, 36 )
					}),
/* BrownishRed */	new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xAE4, Position.Top, 44, 0 ),
						new ItemPart( 0xAE3, Position.Bottom, 44, 80 ),
						new ItemPart( 0xAE5, Position.Left, 0, 40 ),
						new ItemPart( 0xAE6, Position.Right, 88, 40 ),
						new ItemPart( 0xAE7, Position.West, 22, 18 ),
						new ItemPart( 0xAE8, Position.North, 66, 18 ),
						new ItemPart( 0xAE9, Position.East, 66, 58 ),
						new ItemPart( 0xAEA, Position.South, 22, 58 ),
						new ItemPart( 0xAEB, Position.Center, 44, 36 )
					}),
/* TanBrown */		new CarpetInfo( new ItemPart[] {
						new ItemPart( 0xADC, Position.Top, 44, 0 ),
						new ItemPart( 0xADB, Position.Bottom, 44, 80 ),
						new ItemPart( 0xADD, Position.Left, 0, 40 ),
						new ItemPart( 0xADE, Position.Right, 88, 40 ),
						new ItemPart( 0xADF, Position.West, 22, 18 ),
						new ItemPart( 0xAE0, Position.North, 66, 18 ),
						new ItemPart( 0xAE1, Position.East, 66, 58 ),
						new ItemPart( 0xAE2, Position.South, 22, 58 ),
						new ItemPart( 0xADA, Position.Center, 44, 36 )
					})
			};
			#endregion

		public static CarpetInfo[] Infos{ get{ return m_Infos; } }
	}

	public class ItemPart
	{
		private int m_ItemID;
		private  Position m_Info;
		private int m_OffsetX;
		private int m_OffsetY;

		public int ItemID
		{
			get{ return m_ItemID; }
		}

		public  Position Position
		{
			get{ return m_Info; }
		}

		// For Gump Rendering
		public int OffsetX
		{
			get{ return m_OffsetX; }
		}

		// For Gump Rendering
		public int OffsetY
		{
			get{ return m_OffsetY; }
		}

		public ItemPart( int itemID,  Position info, int offsetX, int offsetY )
		{
			m_ItemID = itemID;
			m_Info = info;
			m_OffsetX = offsetX;
			m_OffsetY = offsetY;
		}
	}
}