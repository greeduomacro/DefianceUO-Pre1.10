using System;
using Server;

namespace Server.Items
{
	public class UprightWoodenBenchSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{ 
			get
			{ 
				UprightWoodenBenchSouthAddonDeed deed = new UprightWoodenBenchSouthAddonDeed();
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
		public UprightWoodenBenchSouthAddon()
		{		
			AddComponent( new AddonComponent( 0xB92 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xB91 ), 1, 0, 0 );

		}

		public UprightWoodenBenchSouthAddon( Serial serial ) : base( serial )
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
	public class UprightWoodenBenchSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{ 
			get
			{ 
				
				UprightWoodenBenchSouthAddon addon = new UprightWoodenBenchSouthAddon();
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
		public UprightWoodenBenchSouthAddonDeed()
		{
			Name = "upright wooden bench (south)";
			LootType = LootType.Blessed;
		}

		public UprightWoodenBenchSouthAddonDeed( Serial serial ) : base( serial )
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