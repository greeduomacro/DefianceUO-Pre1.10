using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Misc
{
	public class SaveGump
	{
		public static void ShowSaveGump()
		{
			for ( int i = 0; i < NetState.Instances.Count; ++i )
			{
				Mobile m = ((NetState)NetState.Instances[i]).Mobile;
				if( m != null && !m.Deleted && m.NetState != null && m.Player )
				{
					m.SendGump( new ESaveGump() );
				}
			}
		}

		public static void CloseSaveGump()
		{
			for ( int i = 0; i < NetState.Instances.Count; ++i )
			{
				Mobile m = ((NetState)NetState.Instances[i]).Mobile;
				if( m != null && !m.Deleted && m.NetState != null && m.Player )
				{
					m.CloseGump( typeof( ESaveGump ) );
				}
			}
		}
	}
}

namespace Server.Gumps
{
	public class ESaveGump : Gump
	{
		private string GetRandomHint(){return m_sHints[Utility.Random( m_sHints.Length )];}
		private static string[] m_sHints = new string[]
		{
			"Using the below voting link will help increase the playerbase.",
			"Using the donation link will provide you with many useful options.",
			"Some dangerous creatures have a chance to drop special rares.",
			"You can craft blessed weapons and armor in Factions.",
			"Evolution eggs can be received as prizes for Hunter Bulk Order Deeds.",
			"Murder counts decay at 2hours short and 10hours long.",
			"Saying [helpinfo will give you a list of player commands.",
			"Blacksmiths and Tailors can get BODs from NPCs and reach 120 skill.",
			"You must set a MagicWord on your account at our homepage.",
			"At champion spawns you can obtain special tickets with prizes.",
			"Starting a guild gives you access to a guild chat command [g chat.",
			"Developers read and post at the DefianceUO forums daily.",
			"By just clicking our ads you help the server.",
			"Saying Toggle My Pvp Title will show your rank .",
			"In factions you can win control of whole cities.",
			"Show your legiance and join one of the 4 factions.",
			"Don't forget to signup at our forums and voice yourself.",
			"With a custom house you get to design how you live.",
			"To show a certain Grandmaster skill - point it up and lock all others.",
			"It is extremly important you have a magicword set on your account.",
			"In Bucanner's Den region you gain no shortterm murder counts.",
			"You can name containers by opening the toggle menu on them.",
			"Faction warhorses have 60% chance to evade bola balls.",
			"Casting beneficial spells on players engaged in PVP flags you to their enemies.",
			"Hunter bulk orders can be obtained from Hunter NPCs through sales.",
			"Every Sunday on Defiance we host a lot of events and hand out prizes.",
			"Type [forum and register so you can voice your opinion among players and staff.",
		};

		private static string[] Links = new string[]
		{
			"http://www.defianceuo.com/vote.htm",
			"http://www.defianceuo.com/donationcart/cart.php",
			"http://www.defianceuo.com/forums",
			"http://accounts.defianceuo.com/",
		};

		private static string[] Descriptions = new string[]
		{
			"VOTING PAGE",
			"DONATION SHOP",
			"FORUM AREA",
			"ACCOUNT MANAGEMENT",
		};

		public ESaveGump() : base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;

			this.AddPage(0);

			//this.AddBackground(312, 269, 381, 295, 9200);
			//this.AddBackground(349, 253, 301, 32, 9200);
			//this.AddLabel(477, 259, 152, @"Defiance");
			//this.AddImage(660, 226, 10410);
			//this.AddImage(262, 463, 10402);
			//this.AddLabel(444, 295, 152, @"The world is saving...");
			//this.AddLabel(467, 318, 152, @"Please wait...");
			//this.AddBackground(335, 382, 342, 71, 9450);
			//this.AddLabel(340, 366, 152, @"Did you know that...");
			//this.AddLabel(340, 465, 152, @"Links");
			//this.AddBackground(334, 481, 342, 72, 9450);
			//this.AddHtml( 345, 389, 321, 57, GetRandomHint(), (bool)false, (bool)false);



			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(198, 123, 381, 318, 9390);
			this.AddLabel(362, 158, 0, @"Defiance");
			this.AddImage(532, 91, 10410);
			this.AddImage(164, 342, 10402);
			this.AddLabel(325, 177, 0, @"The world is saving...");
			this.AddLabel(348, 198, 0, @"Please wait...");
			this.AddBackground(237, 246, 309, 64, 9350);
			this.AddLabel(226, 220, 0, @"Did you know that...");
			this.AddLabel(228, 318, 0, @"Links");
			this.AddBackground(256, 339, 275, 69, 9350);


			this.AddHtml( 239, 248, 321, 57, GetRandomHint(), (bool)false, (bool)false);

			for ( int i = 0; i < Links.Length; i++ )
				this.AddHtml( 266, 345 + 16 * i, 321, 30, String.Format( "<a href=\"{0}\">{1}</a>", Links[i], Descriptions[i] ), (bool)false, (bool)false);
		}
	}
}