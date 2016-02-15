using System;
using Server;

namespace Server.Items
{
	public class LargeWhiteBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeWhiteBedEastDeed(); } }

		[Constructable]
		public LargeWhiteBedEastAddon()
		{
			AddComponent( new AddonComponent( 0xA85 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xA84 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xA87 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xA86 ), 1, 1, 0 );
		}

		public LargeWhiteBedEastAddon( Serial serial ) : base( serial )
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

	public class LargeWhiteBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeWhiteBedEastAddon(); } }
		//public override int LabelNumber{ get{ return 1044324; } } // large bed (east)

		[Constructable]
		public LargeWhiteBedEastDeed()
		{

                Name = "large white bed (east)";

		}

		public LargeWhiteBedEastDeed( Serial serial ) : base( serial )
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