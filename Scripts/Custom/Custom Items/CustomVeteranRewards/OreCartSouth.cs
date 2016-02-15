using System;
using Server;

namespace Server.Items
{
	public class OreCartSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				OreCartSouthDeed deed = new OreCartSouthDeed();
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
		public OreCartSouthAddon()
		{		
			AddComponent( new AddonComponent( 0x1A83 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x1A82 ), 0, 1, 0 );
		}

		public OreCartSouthAddon( Serial serial ) : base( serial )
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

	public class OreCartSouthDeed : BaseAddonDeed
	{

		public override BaseAddon Addon
		{
			get
			{
				OreCartSouthAddon addon = new OreCartSouthAddon();
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
		public OreCartSouthDeed()
		{	
			LootType = LootType.Blessed;
			Name = "ore cart deed (south)";
		}

		public OreCartSouthDeed( Serial serial ) : base( serial )
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