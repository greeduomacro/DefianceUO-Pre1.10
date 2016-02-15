using System;
using Server;

namespace Server.Items
{
	public class ChristmasCastleAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new ChristmasCastleAddonDeed(); } }

		[Constructable]
		public ChristmasCastleAddon()
		{
			AddonComponent comp = new AddonComponent( 0x2317 );
                        comp.Name = "gingerbread castle";
                        AddComponent( comp, 0, 0, 0 );

                        comp = new AddonComponent( 0x2318 );
                        comp.Name = "gingerbread castle";
			AddComponent( comp, 1, 0, 0 );

                        comp = new AddonComponent( 0x2319 );
                        comp.Name = "gingerbread castle";
			AddComponent( comp, 1, -1, 0 );
		}

		public ChristmasCastleAddon( Serial serial ) : base( serial )
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

	public class ChristmasCastleAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new ChristmasCastleAddon(); } }
		//public override int LabelNumber{ get{ return 1044323; } } // large bed (south)

		[Constructable]
		public ChristmasCastleAddonDeed()
		{

                Name = "christmas gingerbread castle deed";

		}

		public ChristmasCastleAddonDeed( Serial serial ) : base( serial )
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