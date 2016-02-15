using System;
using Server;

namespace Server.Items
{
	public class OreCartEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				OreCartEastDeed deed = new OreCartEastDeed();
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
		public OreCartEastAddon()
		{		
			AddComponent( new AddonComponent( 0x1A88 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x1A87 ), 1, 0, 0 );
		}

		public OreCartEastAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_IsRewardItem );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 1: m_IsRewardItem = reader.ReadBool(); break;
			}

		}
	}

	public class OreCartEastDeed : BaseAddonDeed
	{

		public override BaseAddon Addon
		{
			get
			{
				OreCartEastAddon addon = new OreCartEastAddon();
				addon.IsRewardItem = m_IsRewardItem;
				return addon;
			}
		}

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get{ return m_IsRewardItem; }
			set{ m_IsRewardItem = value; }
		}

		
		[Constructable]
		public OreCartEastDeed()
		{	
			LootType = LootType.Blessed;
			Name = "ore cart deed (east)";
		}

		public OreCartEastDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			
			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
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
