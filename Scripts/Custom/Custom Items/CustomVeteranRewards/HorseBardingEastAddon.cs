using System;
using Server;

namespace Server.Items
{
	public class HorseBardingEastAddon : BaseAddon
	{
		
		public override BaseAddonDeed Deed
		{ 
			get
			{ 
				HorseBardingEastDeed deed = new HorseBardingEastDeed();
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
		public HorseBardingEastAddon()
		{
			AddComponent( new AddonComponent( 0x1379 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x1378 ), 0, 1, 0 );
		}

		public HorseBardingEastAddon( Serial serial ) : base( serial )
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

	public class HorseBardingEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{ 
			get
			{ 
				
				HorseBardingEastAddon addon = new HorseBardingEastAddon();
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

		[Constructable]
		public HorseBardingEastDeed()
		{
			Name = "horse barding (east)";
			LootType = LootType.Blessed;
		}

		public HorseBardingEastDeed( Serial serial ) : base( serial )
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