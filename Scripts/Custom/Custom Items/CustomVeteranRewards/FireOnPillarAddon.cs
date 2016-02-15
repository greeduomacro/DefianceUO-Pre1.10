using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FireOnPillarAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				FireOnPillarAddonDeed deed = new FireOnPillarAddonDeed();
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
		public FireOnPillarAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 7978 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 7979 );
			AddComponent( ac, 0, 0, 7 );
			ac = new AddonComponent( 6571 );
			ac.Light = LightType.ArchedWindowEast;
			AddComponent( ac, 0, 0, 10 );
		}

		public FireOnPillarAddon( Serial serial ) : base( serial )
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

	public class FireOnPillarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				FireOnPillarAddon addon = new FireOnPillarAddon();
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
		public FireOnPillarAddonDeed()
		{
			LootType = LootType.Blessed;
			Name = "Obsidian Fire Pillar";
		}

		public FireOnPillarAddonDeed( Serial serial ) : base( serial )
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