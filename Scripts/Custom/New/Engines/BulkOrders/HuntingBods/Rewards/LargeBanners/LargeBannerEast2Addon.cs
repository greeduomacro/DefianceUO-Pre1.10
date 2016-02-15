using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LargeBannerEast2Addon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new LargeBannerEast2AddonDeed();
			}
		}

		[ Constructable ]
		public LargeBannerEast2Addon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 5675 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 5676 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 5677 );
			AddComponent( ac, 0, -1, 0 );

		}

		public LargeBannerEast2Addon( Serial serial ) : base( serial )
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

	public class LargeBannerEast2AddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new LargeBannerEast2Addon();
			}
		}

		[Constructable]
		public LargeBannerEast2AddonDeed()
		{
			Name = "A Large Black Banner Deed Facing East";
			LootType = LootType.Blessed;
		}

		public LargeBannerEast2AddonDeed( Serial serial ) : base( serial )
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