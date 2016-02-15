using System;
using Server;

namespace Server.Items
{
	public class OrcishBannerSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new OrcishBannerSouthDeed(); } }

		[Constructable]
		public OrcishBannerSouthAddon()
		{
			AddComponent( new AddonComponent( 0x42D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x42C ), 1, 0, 0 );
		}

		public OrcishBannerSouthAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class OrcishBannerSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new OrcishBannerSouthAddon(); } }

		[Constructable]
		public OrcishBannerSouthDeed()
		{
			Name = "an orcish banner deed facing south";
		}

		public OrcishBannerSouthDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class OrcishBannerEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new OrcishBannerEastDeed(); } }

		[Constructable]
		public OrcishBannerEastAddon()
		{
			AddComponent( new AddonComponent( 0x42A ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x42B ), 0, 1, 0 );
		}

		public OrcishBannerEastAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class OrcishBannerEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new OrcishBannerEastAddon(); } }

		[Constructable]
		public OrcishBannerEastDeed()
		{
			Name = "an orcish banner deed facing east";
		}

		public OrcishBannerEastDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}