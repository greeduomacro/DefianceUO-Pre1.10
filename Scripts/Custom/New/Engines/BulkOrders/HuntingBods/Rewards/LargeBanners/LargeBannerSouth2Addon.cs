using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LargeBannerSouth2Addon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new LargeBannerSouth2AddonDeed();
			}
		}

		[ Constructable ]
		public LargeBannerSouth2Addon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 5524 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 5525 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 5526 );
			AddComponent( ac, 1, 0, 0 );

		}

		public LargeBannerSouth2Addon( Serial serial ) : base( serial )
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

	public class LargeBannerSouth2AddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new LargeBannerSouth2Addon();
			}
		}

		[Constructable]
		public LargeBannerSouth2AddonDeed()
		{
			Name = "A Large Black Banner Deed Facing South";
			LootType = LootType.Blessed;
		}

		public LargeBannerSouth2AddonDeed( Serial serial ) : base( serial )
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