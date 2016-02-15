using System;
using System.Collections;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
	public class SBHouseDeed: SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBHouseDeed()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "deed to a stone-and-plaster house", typeof( StonePlasterHouseDeed ), 93800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a field stone house", typeof( FieldStoneHouseDeed ), 93800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a small brick house", typeof( SmallBrickHouseDeed), 93800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a wooden house", typeof( WoodHouseDeed ), 93800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a wood-and-plaster house", typeof( WoodPlasterHouseDeed ), 93800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a thatched-roof cottage", typeof( ThatchedRoofCottageDeed ), 93800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a brick house", typeof( BrickHouseDeed ), 224500, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a two-story wood-and-plaster house", typeof( TwoStoryWoodPlasterHouseDeed ), 192400, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a two-story stone-and-plaster house", typeof( TwoStoryStonePlasterHouseDeed ), 192400, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a tower", typeof( TowerDeed ), 833200, 20, 0x14F0, 0 ) );
				//Add( new GenericBuyInfo( "deed to a small stone keep", typeof( KeepDeed ), 6065200, 20, 0x14F0, 0 ) );
				//Add( new GenericBuyInfo( "deed to a castle", typeof( CastleDeed ), 1022800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a large house with patio", typeof( LargePatioDeed ), 252800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a marble house with patio", typeof( LargeMarbleDeed ), 292000, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a small stone tower", typeof( SmallTowerDeed ), 168500, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a two story log cabin", typeof( LogCabinDeed ), 197800, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a sandstone house with patio", typeof( SandstonePatioDeed ), 170900, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a two story villa", typeof( VillaDeed ), 186500, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a small stone workshop", typeof( StoneWorkshopDeed ), 133600, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "deed to a small marble workshop", typeof( MarbleWorkshopDeed ), 133000, 20, 0x14F0, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( StonePlasterHouseDeed ), 52528 );
				Add( typeof( FieldStoneHouseDeed ), 52528 );
				Add( typeof( SmallBrickHouseDeed ), 52528 );
				Add( typeof( WoodHouseDeed ), 52528 );
				Add( typeof( WoodPlasterHouseDeed ), 52528 );
				Add( typeof( ThatchedRoofCottageDeed ), 52528 );
				Add( typeof( BrickHouseDeed ), 125720 );
				Add( typeof( TwoStoryWoodPlasterHouseDeed ), 107744 );
				Add( typeof( TowerDeed ), 466592 );
				//Add( typeof( KeepDeed ), 119655 );
				//Add( typeof( CastleDeed ), 2072768 );
				Add( typeof( LargePatioDeed ), 126400 );
				Add( typeof( LargeMarbleDeed ), 146000 );
				Add( typeof( SmallTowerDeed ), 84250 );
				Add( typeof( LogCabinDeed ), 98900 );
				Add( typeof( SandstonePatioDeed ), 85450 );
				Add( typeof( VillaDeed ), 93250 );
				Add( typeof( StoneWorkshopDeed ), 66800 );
				Add( typeof( MarbleWorkshopDeed ), 66800 );
				Add( typeof( SmallBrickHouseDeed ), 52528 );
			}
		}
	}
}