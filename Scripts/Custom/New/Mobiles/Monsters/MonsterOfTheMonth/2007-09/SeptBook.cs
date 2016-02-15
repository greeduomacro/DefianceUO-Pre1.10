using System;
using Server;

namespace Server.Items
{
	public class SeptBook : RedBook
	{
		[Constructable]
		public SeptBook() : base( "a poem", "Seer Jeremy", 6, false )
		{
			Hue = 1000;

			Pages[0].Lines = new string[]
				{
					"As it sits and counts",
					"the days ",
					"In one place it always",
					"stays",
					"While some things",
					"change in many ways",
				};

			Pages[1].Lines = new string[]
				{
					"One stands still while",
					"another plays",
					"The oak is covered in",
					"a stain",
					"Without this it would",
					"be plain",
				};

			Pages[2].Lines = new string[]
				{
					"And it must endure",
					"more pain",
					"Other parts could rust",
					"in the rain",
					"But the wood is strong",
					"and would remain",
				};

			Pages[3].Lines = new string[]
				{
					"This case protects all",
					"sorts of things",
					"Intricate parts like the",
					"springs",
					"And all the sounds they",
					"bring",
				};

			Pages[4].Lines = new string[]
				{
					"Make something plain seem",
					"to sing",
					"And when time breaks a gear",
					"From the stress of days",
					"and of years",
					"It depends more on the one",
					"that's near",
				};

			Pages[5].Lines = new string[]
				{
					"So that even at night the",
					"hours still cheer",
				};
		}

		public SeptBook( Serial serial ) : base( serial )
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