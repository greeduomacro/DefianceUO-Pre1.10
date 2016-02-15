using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LargeBannerEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new LargeBannerEastAddonDeed();
			}
		}

		[ Constructable ]
		public LargeBannerEastAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 5667 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 5668 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 5669 );
			AddComponent( ac, 0, -1, 0 );

		}

		public LargeBannerEastAddon( Serial serial ) : base( serial )
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

	public class LargeBannerEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new LargeBannerEastAddon();
			}
		}

		[Constructable]
		public LargeBannerEastAddonDeed()
		{
			Name = "A Large Golden Banner Deed Facing East";
			LootType = LootType.Blessed;
		}

		public LargeBannerEastAddonDeed( Serial serial ) : base( serial )
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