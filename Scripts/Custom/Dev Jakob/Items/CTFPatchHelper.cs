using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1E5E, 0x1E5F )]
	public class CTFPatchHelper : Item
	{
		[Constructable]
		public CTFPatchHelper() : base( 0x1E5E )
		{
			Hue = 1171;
			Name = "Having problems with the CTF patch? Click this!";
			Movable = false;
		}

		public CTFPatchHelper( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			string url = "http://www.defianceuo.com/forums/viewtopic.php?t=446";
			from.LaunchBrowser( url );
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