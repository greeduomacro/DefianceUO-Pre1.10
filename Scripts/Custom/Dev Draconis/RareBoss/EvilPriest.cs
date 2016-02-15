using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class EvilPriest : BaseRareBoss
	{
		[Constructable]
		public EvilPriest() : base( AIType.AI_Mage )
		{
			Name = NameList.RandomName( "male" );
			Body = 400;
			Title = "the priest";
			Hue = Utility.RandomSkinHue();

			Robe robe = new Robe();
			robe.Hue = 1150;
			robe.LootType = LootType.Blessed;
			AddItem( robe );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override bool DoHealMobiles { get { return true; } }
		public override int CanCastReflect{ get { return 60; } }

		public EvilPriest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}