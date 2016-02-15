using System;
using Server;

namespace Server.Items
{
	public class HorseBardingSouthAddon : BaseAddon
	{
		
		public override BaseAddonDeed Deed
		{ 
			get
			{ 
				HorseBardingSouthDeed deed = new HorseBardingSouthDeed();
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
		public HorseBardingSouthAddon()
		{
			AddComponent( new AddonComponent( 0x1379 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x1378 ), 0, 1, 0 );
		}

		public HorseBardingSouthAddon( Serial serial ) : base( serial )
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

	public class HorseBardingSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{ 
			get
			{ 
				
				HorseBardingSouthAddon addon = new HorseBardingSouthAddon();
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
		public HorseBardingSouthDeed()
		{
			Name = "horse barding (south)";
			LootType = LootType.Blessed;
		}

		public HorseBardingSouthDeed( Serial serial ) : base( serial )
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