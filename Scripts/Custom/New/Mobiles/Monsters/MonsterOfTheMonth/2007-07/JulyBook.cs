using System;
using Server;

namespace Server.Items
{
	public class JulyBook : BlueBook
	{
		[Constructable]
		public JulyBook() : base( "a strange book", "an unknown seer", 1, false )
		{
			Hue = 1000;

			Pages[0].Lines = new string[]
				{
					"*This book appears to be",
					"Empty*",
				};
		}

		public JulyBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}