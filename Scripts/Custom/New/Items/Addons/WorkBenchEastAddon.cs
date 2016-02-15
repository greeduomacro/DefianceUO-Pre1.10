using System;
using Server;

namespace Server.Items
{
	public class WorkBenchEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new WorkBenchEastDeed(); } }

		public override bool RetainDeedHue{ get{ return true; } }

		[Constructable]
		public WorkBenchEastAddon() : this( 0 )
		{
		}

		[Constructable]
		public WorkBenchEastAddon( int hue )
		{
			AddComponent( new AddonComponent( 0x6642 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x6643 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x6641 ), 0, 2, 0 );
			Hue = hue;
		}

		public WorkBenchEastAddon( Serial serial ) : base( serial )
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

	public class WorkBenchEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new WorkBenchEastAddon( this.Hue ); } }
		public override int LabelNumber{ get{ return 1026641; } } // woodworker's bench

		[Constructable]
		public WorkBenchEastDeed()
		{
			Name = "woodworker's bench (east)";
		}

		public WorkBenchEastDeed( Serial serial ) : base( serial )
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