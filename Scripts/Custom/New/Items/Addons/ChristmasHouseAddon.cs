using System;
using Server;

namespace Server.Items
{
	public class ChristmasHouseAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new ChristmasHouseAddonDeed(); } }

		[Constructable]
		public ChristmasHouseAddon()
		{
			AddonComponent comp = new AddonComponent( 0x2BE5 );
                        comp.Name = "gingerbread house";
                        AddComponent( comp, 0, 0, 0 );

                        comp = new AddonComponent( 0x2BE6 );
                        comp.Name = "gingerbread house";
			AddComponent( comp, 1, 0, 0 );

                        comp = new AddonComponent( 0x2BE7 );
                        comp.Name = "gingerbread house";
			AddComponent( comp, 1, -1, 0 );
		}

		public ChristmasHouseAddon( Serial serial ) : base( serial )
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

	public class ChristmasHouseAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new ChristmasHouseAddon(); } }
		//public override int LabelNumber{ get{ return 1044323; } } // large bed (south)

		[Constructable]
		public ChristmasHouseAddonDeed()
		{

                Name = "christmas gingerbread house deed";

		}

		public ChristmasHouseAddonDeed( Serial serial ) : base( serial )
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