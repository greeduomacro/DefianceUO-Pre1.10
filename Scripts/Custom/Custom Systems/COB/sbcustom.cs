
using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBCustom : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBCustom()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{

				AddItem( typeof(WarFork), 46 );
				AddItem( typeof(Hatchet), 33 );

				AddItem( typeof(TribalSpear), 184 );

				AddItem( typeof(BoneChest), 93 );
				AddItem( typeof(BoneLegs), 87 );
				AddItem( typeof(BoneHelm), 62 );
				AddItem( typeof(BoneGloves), 52 );
				AddItem( typeof(BoneArms), 71 );

				AddItem( typeof(OrcHelm), 101 );

			}

			private void AddItem( Type type, int price )
			{
				Add( type, price*10/19 );
			}
		}
	}
}