using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LargeBannerSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new LargeBannerSouthAddonDeed();
			}
		}

		[ Constructable ]
		public LargeBannerSouthAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 5516 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 5517 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 5518 );
			AddComponent( ac, 1, 0, 0 );

		}

		public LargeBannerSouthAddon( Serial serial ) : base( serial )
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

	public class LargeBannerSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new LargeBannerSouthAddon();
			}
		}

		[Constructable]
		public LargeBannerSouthAddonDeed()
		{
			Name = "A Large Golden Banner Deed Facing South";
			LootType = LootType.Blessed;
		}

		public LargeBannerSouthAddonDeed( Serial serial ) : base( serial )
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