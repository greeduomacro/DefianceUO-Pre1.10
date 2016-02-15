using System;

namespace Server.Items
{
	public class ChaosToken : Item
	{
		[Constructable]
		public ChaosToken() : base( 0x14F0 )
		{
			Name = "a chaos token";
			Weight = 1.0;
			Hue = 1358;
		}

		public ChaosToken( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ChaosToken( amount ), amount );
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