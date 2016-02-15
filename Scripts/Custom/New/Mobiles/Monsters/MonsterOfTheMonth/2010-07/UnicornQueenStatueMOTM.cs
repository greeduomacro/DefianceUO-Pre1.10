using System;

namespace Server.Items
{
	public class UnicornQueenStatueMOTM : Item
	{
		//public override string DefaultName{ get{ return "Queen of Unicorns"; } }

		[Constructable]
		public UnicornQueenStatueMOTM() : base( 0x25CE )
		{
			Name = "Queen of Unicorns";
			Weight = 9.0;
			Hue = 1150;
		}

		public UnicornQueenStatueMOTM( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}