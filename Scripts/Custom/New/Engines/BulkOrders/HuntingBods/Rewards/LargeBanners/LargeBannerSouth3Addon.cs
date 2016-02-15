using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LargeBannerSouth3Addon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new LargeBannerSouth3AddonDeed();
			}
		}

		[ Constructable ]
		public LargeBannerSouth3Addon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 5510 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 5511 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 5512 );
			AddComponent( ac, 1, 0, 0 );

		}

		public LargeBannerSouth3Addon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class LargeBannerSouth3AddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new LargeBannerSouth3Addon();
			}
		}

		[Constructable]
		public LargeBannerSouth3AddonDeed()
		{
			Name = "A Large Silver Banner Deed Facing South";
			LootType = LootType.Blessed;
		}

		public LargeBannerSouth3AddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}