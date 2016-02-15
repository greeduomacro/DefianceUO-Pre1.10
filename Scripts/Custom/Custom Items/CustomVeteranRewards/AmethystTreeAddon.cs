using System;
using Server;
using Server.Items;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
	public class AmethystTreeAddon : BaseAddon, IRewardItem
	{
        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		public override BaseAddonDeed Deed
		{
			get
			{
				AmethystTreeAddonDeed deed = new AmethystTreeAddonDeed();
				deed.IsRewardItem = m_IsRewardItem;
				return deed;
			}
		}

		[ Constructable ]
		public AmethystTreeAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 3302 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 3207 );
			AddComponent( ac, -1, 1, 20 );
			ac = new AddonComponent( 3207 );
			AddComponent( ac, 1, -1, 18 );
			ac = new AddonComponent( 3207 );
			AddComponent( ac, 1, 1, 33 );
			ac = new AddonComponent( 3207 );
			AddComponent( ac, -1, 1, 10 );
			ac = new AddonComponent( 3207 );
			AddComponent( ac, 1, -1, 10 );
			ac = new AddonComponent( 3207 );
			AddComponent( ac, 1, 1, 20 );

		}

		public AmethystTreeAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
            writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            m_IsRewardItem = reader.ReadBool();
		}
	}

	public class AmethystTreeAddonDeed : BaseAddonDeed, IRewardItem
	{
        private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		public override BaseAddon Addon
		{
			get
			{
				AmethystTreeAddon addon = new AmethystTreeAddon();
				addon.IsRewardItem = m_IsRewardItem;
				return addon;
			}
		}

		[Constructable]
		public AmethystTreeAddonDeed()
		{
            LootType = LootType.Blessed;
			Name = "Amethyst Tree Deed";
		}

		public AmethystTreeAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
                        writer.Write( m_IsRewardItem );
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
                        m_IsRewardItem = reader.ReadBool();
		}
	}
}