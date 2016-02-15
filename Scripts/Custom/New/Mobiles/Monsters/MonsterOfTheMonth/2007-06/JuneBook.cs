using System;
using Server;

namespace Server.Items
{
	public class JuneBook : RedBook
	{
		[Constructable]
		public JuneBook() : base( "a riddle book", "Lead Seer Draconis", 10, false )
		{
            Hue = 78;
            Pages[0].Lines = new string[]
				{
					"On the edge of the",
					"mountains of wind will",
					"you find the opposite",
					"of order",
				};

			Pages[1].Lines = new string[]
				{
					"Walk into the desert",
					"and you shall find that",
					"empathy grows",
				};

			Pages[2].Lines = new string[]
				{
					"Walk through the ice and",
					"you shall find truth",
				};

			Pages[3].Lines = new string[]
				{
					"Travel to the island and",
					"you might just find the code",
				};

			Pages[4].Lines = new string[]
				{
					"Walk though the land of",
					"fire and you discover you",
					"are from the earth",
				};

			Pages[5].Lines = new string[]
				{
					"Somewhere around the large",
					"circular lake is where you",
					"will find the reason for",
					"punishment",
				};

			Pages[6].Lines = new string[]
				{
					"Far to the north is",
					"where I will slaughter",
					"this bull in your name",
					"my lord",
				};

			Pages[7].Lines = new string[]
				{
					"Above a small pool of",
					"water I connect to",
					"something greater then",
					"my self",
				};

			Pages[8].Lines = new string[]
				{
					"Travel to an island in",
					"the south and you will",
					"find a sword etched in marble",
					"marble,"
				};

			Pages[9].Lines = new string[]
				{
					"Only deep within contempt",
					" will one find good",
					"judgment",
				};
		}

		public JuneBook( Serial serial ) : base( serial )
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