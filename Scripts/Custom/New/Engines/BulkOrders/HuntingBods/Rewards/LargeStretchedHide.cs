using System;
using Server;

namespace Server.Items
{
	public class LargeStretchedHideSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeStretchedHideSouthDeed(); } }

		[Constructable]
		public LargeStretchedHideSouthAddon()
		{
			AddComponent( new AddonComponent( 0x107D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x107E ), 1, 0, 0 );
		}

		public LargeStretchedHideSouthAddon( Serial serial ) : base( serial )
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

	public class LargeStretchedHideSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeStretchedHideSouthAddon(); } }

		[Constructable]
		public LargeStretchedHideSouthDeed()
		{
			Name = "a large stretched hide deed facing south";
		}

		public LargeStretchedHideSouthDeed( Serial serial ) : base( serial )
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

	public class LargeStretchedHideEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeStretchedHideEastDeed(); } }

		[Constructable]
		public LargeStretchedHideEastAddon()
		{
			AddComponent( new AddonComponent( 0x106C ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x106D ), 0, 1, 0 );
		}

		public LargeStretchedHideEastAddon( Serial serial ) : base( serial )
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

	public class LargeStretchedHideEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeStretchedHideEastAddon(); } }

		[Constructable]
		public LargeStretchedHideEastDeed()
		{
			Name = "a large stretched hide deed facing east";
		}

		public LargeStretchedHideEastDeed( Serial serial ) : base( serial )
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