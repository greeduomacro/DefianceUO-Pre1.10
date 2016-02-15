using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBSpecial : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBSpecial()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				//Add( new GenericBuyInfo( typeof( TribalPaint ), 5000, 100, 0x9EC, 0x835 ) );
                                Add( new GenericBuyInfo( typeof( SeedBox ), 2500000, 100, 0x990, 0x0 ) );
                                Add( new GenericBuyInfo( typeof( HousePlacementTool ), 25995, 100, 0x14F6, 0x0 ) );
                                //Add( new GenericBuyInfo( typeof( DarkTribalPaint ), 9995, 100, 0x9EC, 0x497 ) );
                                Add( new GenericBuyInfo( typeof( EtherealHorse ), 800000, 100, 0x20DD, 0x0 ) );
                                Add( new GenericBuyInfo( typeof( EtherealLlama ), 800000, 100, 0x20F6, 0x0 ) );
                                Add( new GenericBuyInfo( typeof( EtherealOstard ), 800000, 100, 0x2135, 0x0 ) );


							}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}