using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LilacBushTreeAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				LilacBushTreeAddonDeed deed = new LilacBushTreeAddonDeed();
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
		public LilacBushTreeAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 3230 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 3146 );
			AddComponent( ac, 0, 1, 21 );
			ac = new AddonComponent( 3146 );
			AddComponent( ac, 1, 0, 20 );
			ac = new AddonComponent( 3146 );
			AddComponent( ac, 1, 1, 20 );
			ac = new AddonComponent( 3146 );
			AddComponent( ac, 1, 1, 31 );
			ac = new AddonComponent( 3145 );
			AddComponent( ac, -1, 1, 10 );
			ac = new AddonComponent( 3142 );
			AddComponent( ac, 1, 0, 15 );
		}

		public LilacBushTreeAddon( Serial serial ) : base( serial )
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

	public class LilacBushTreeAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				LilacBushTreeAddon addon = new LilacBushTreeAddon();
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
		public LilacBushTreeAddonDeed()
		{
            LootType = LootType.Blessed;
			Name = "Lilac Bush Tree Deed";
		}

		public LilacBushTreeAddonDeed( Serial serial ) : base( serial )
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