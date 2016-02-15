using System;
using Server;
using Server.Engines.IdolSystem;

namespace Server.Items
{
	public class SnowDrop : Item
	{
		[Constructable]
		public SnowDrop( int amount ) : base( 0xF7A )
		{
			Name = "corrupted snow";
			Hue = 1150;
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public SnowDrop( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new SnowDrop( amount ), amount );
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