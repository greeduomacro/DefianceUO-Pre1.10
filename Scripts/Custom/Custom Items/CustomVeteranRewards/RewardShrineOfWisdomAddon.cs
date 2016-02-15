using System;
using Server;

namespace Server.Items
{
	public class RewardShrineOfWisdomAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				RewardShrineOfWisdomAddonDeed deed = new RewardShrineOfWisdomAddonDeed();
				deed.IsRewardItem = m_IsRewardItem;
				return deed;
			}
		}

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public RewardShrineOfWisdomAddon()
		{
			AddComponent( new AddonComponent( 0x14C3 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x14D4 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x14C6 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0x14D5 ), 1, 1, 0 );
		}

		public RewardShrineOfWisdomAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 1 ); // Version

			writer.Write( m_IsRewardItem );
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1: m_IsRewardItem = reader.ReadBool(); break;
			}
		}
	}

	public class RewardShrineOfWisdomAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				RewardShrineOfWisdomAddon addon = new RewardShrineOfWisdomAddon();
				addon.IsRewardItem = m_IsRewardItem;
				return addon;
			}
		}

        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		//public override int LabelNumber{ get{ return 1062046; } } // Shrine of Wisdom

		[Constructable]
		public RewardShrineOfWisdomAddonDeed()
		{

			Name = "Shrine Of Wisdom Deed";
			LootType = LootType.Blessed;

		}

		public RewardShrineOfWisdomAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 1 ); // Version

			writer.Write( m_IsRewardItem );
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1: m_IsRewardItem = reader.ReadBool(); break;
			}
		}
	}
}