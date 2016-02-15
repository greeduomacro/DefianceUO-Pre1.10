using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{

[Furniture]
	[Flipable( 0x2DF3, 0x2DF4 )]
	public class OrnateElvenBox : LockableContainer
	{
		public override int DefaultGumpID{ get{ return 0x43; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 16, 51, 168, 73 ); }
		}

		[Constructable]
		public OrnateElvenBox() : base( 0x2DF3 )
		{
			Weight = 0.0;
		}

		public OrnateElvenBox( Serial serial ) : base( serial )
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