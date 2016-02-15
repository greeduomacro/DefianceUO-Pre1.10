using System;
using Server;

namespace Server.Items
{
	public class LargeWhiteBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeWhiteBedSouthDeed(); } }

		[Constructable]
		public LargeWhiteBedSouthAddon()
		{
			AddComponent( new AddonComponent( 0xA89 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xA8B ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xA88 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xA8A ), 1, 1, 0 );
		}

		public LargeWhiteBedSouthAddon( Serial serial ) : base( serial )
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

	public class LargeWhiteBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeWhiteBedSouthAddon(); } }
		//public override int LabelNumber{ get{ return 1044323; } } // large bed (south)

		[Constructable]
		public LargeWhiteBedSouthDeed()
		{

                Name = "large white bed (south)";

		}

		public LargeWhiteBedSouthDeed( Serial serial ) : base( serial )
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