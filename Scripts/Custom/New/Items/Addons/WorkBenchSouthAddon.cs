using System;
using Server;

namespace Server.Items
{
	public class WorkBenchSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new WorkBenchSouthDeed(); } }

		public override bool RetainDeedHue{ get{ return true; } }

		[Constructable]
		public WorkBenchSouthAddon() : this( 0 )
		{
		}

		[Constructable]
		public WorkBenchSouthAddon( int hue )
		{
			AddComponent( new AddonComponent( 0x6646 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x6647 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x6645 ), 0, 2, 0 );
			Hue = hue;
		}

		public WorkBenchSouthAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class WorkBenchSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new WorkBenchSouthAddon( this.Hue ); } }
		public override int LabelNumber{ get{ return 1026642; } } // woodworker's bench

		[Constructable]
		public WorkBenchSouthDeed()
		{
			Name = "woodworker's bench (south)";
		}

		public WorkBenchSouthDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}