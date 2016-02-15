using System;
using Server;

namespace Server.Items
{
	public class UprightWoodenBenchEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{ 
			get
			{ 
				UprightWoodenBenchEastAddonDeed deed = new UprightWoodenBenchEastAddonDeed();
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
		public UprightWoodenBenchEastAddon()
		{		
			AddComponent( new AddonComponent( 0xB94 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xB93 ), 0, 1, 0 );

		}

		public UprightWoodenBenchEastAddon( Serial serial ) : base( serial )
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
	public class UprightWoodenBenchEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{ 
			get
			{ 
				
				UprightWoodenBenchEastAddon addon = new UprightWoodenBenchEastAddon();
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
		public UprightWoodenBenchEastAddonDeed()
		{
			Name = "upright wooden bench (east)";
			LootType = LootType.Blessed;
		}

		public UprightWoodenBenchEastAddonDeed( Serial serial ) : base( serial )
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