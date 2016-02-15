/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							ChooseGump.cs
*						-------------------------
*
*	File Description:	This gump displays the last fights a person
*						has fought, and the outcome in detail.
*
*
*/
using System;
using System.Collections;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Ladder
{
	public class FightGump : Gump
	{
		private enum Color
		{
			yellow = 0,
			black,
			green,
			red,
		}
		private string[] colors = Color.GetNames(typeof(Color));

		// Cast ints to string
		private string Format(int s, Color c, bool alignRight) { return Format(s.ToString(), c, alignRight); }

		// Formatter
		private string Format(String s, Color c, bool alignRight)
		{
			String formatString = "<basefont color=\"" + colors[(int)c] + "\">";
			formatString += alignRight ? "<div align=\"right\">" : "<div align=\"left\">";
			formatString += s;
			formatString += "</div></basefont>";
			return formatString;
		}

		private int page;
		private ArrayList fights;
		private Mobile from;
		public FightGump(ArrayList fights, Mobile from, int page, int x, int y)
			: base( x, y )
		{
			this.from = from;
			this.fights = fights;
			this.page = page;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(0, 0, 600, 310, 9270);
			this.AddLabel(207, 25, 1160, @"Your ladder fights");
			this.AddLabel(30, 70, 1160, @"Date & Time:");
			this.AddLabel(190, 70, 1160, @"Opponent:");
			this.AddLabel(310, 70, 1160, @"Duration:");
			this.AddLabel(390, 70, 1160, @"Outcome:");
			this.AddLabel(470, 70, 1160, @"Gain:");
			this.AddLabel(520, 70, 1160, @"Difficulty:");
			this.AddBackground(20, 90, 560, 20, 3000);
			this.AddBackground(20, 110, 560, 20, 3000);
			this.AddBackground(20, 130, 560, 20, 3000);
			this.AddBackground(20, 190, 560, 20, 3000);
			this.AddBackground(20, 170, 560, 20, 3000);
			this.AddBackground(20, 150, 560, 20, 3000);
			this.AddBackground(20, 210, 560, 20, 3000);
			this.AddBackground(20, 230, 560, 20, 3000);
			this.AddBackground(20, 250, 560, 20, 3000);
			this.AddBackground(20, 270, 560, 20, 3000);

			// Add scroll buttons
			if (page > 0)
				this.AddButton(20, 40, 9909, 9910, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			if (fights.Count > page * 10 + 10)
				this.AddButton(560, 40, 9903, 9904, (int)Buttons.Button2, GumpButtonType.Reply, 0);

            int j = 0;          // lame check but ppl use razor to access nonexistant buttons
            for (int i = page * 10; i >= 0 && i < fights.Count && j < 10; i++)
			{

				Fight f = (Fight)fights[i];
				TimeSpan dur = (f.End - f.Start);
				bool win = f.Winner == from;
				this.AddHtml(30, 90 + j * 20, 160, 20, Format(f.Start.ToString(), Color.black, false), false, false);
				this.AddHtml(190, 90 + j * 20, 120, 20, Format((win ? (f.Loser == null ? "-Deleted Char-" : f.Loser.Name) : (f.Winner == null ? "-Deleted Char-" : f.Winner.Name)), Color.black, false), false, false);
				this.AddHtml(310, 90 + j * 20, 50, 20, Format((dur.Minutes + ":" + dur.Seconds), Color.black, true), false, false);
				this.AddHtml(400, 90 + j * 20, 50, 20, Format((win ? "Win" : "Loss"), Color.black, false), false, false);
				this.AddHtml(470, 90 + j * 20, 30, 20, (win ? Format(f.Gain, Color.green, true) : Format(f.Loss, Color.red, true)), false, false);
				this.AddHtml(520, 90 + j * 20, 50, 20, Format((win ? f.Difficulty : 1000 - f.Difficulty), Color.black, true), false, false);

				j++;

			}

		}

		public enum Buttons
		{
			Button1 = 1,
			Button2 = 2,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == (int)Buttons.Button1)
			{
				state.Mobile.SendGump(new FightGump(fights, from, page - 1, this.X, this.Y));
			}
			else if (info.ButtonID == (int)Buttons.Button2)
			{
				state.Mobile.SendGump(new FightGump(fights, from, page + 1, this.X, this.Y));
			}

		}
	}
}