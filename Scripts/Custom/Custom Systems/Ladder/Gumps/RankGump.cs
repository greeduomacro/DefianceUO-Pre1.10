/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							RankGump.cs
*						-------------------------
*
*	File Description:	This gump shows the ranking list of
*						the best duellers on the ladder.
*
*/

using System;
using System.Collections;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Ladder
{
	public class RankGump : Gump
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
		public RankGump(int page, int x, int y)
			: base( x, y )
		{
			this.page = page;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);

			//Gump background
			this.AddBackground(100, 100, 440, 310, 9270);

			//Labels
			this.AddLabel(300, 118, 1160, @"Ladder Rankings");
			//this.AddLabel(206, 147, 1160, @"Top ten duellers at the moment");
			this.AddLabel(130, 170, 1160, @"Rank:");

			// Formated html
			this.AddHtml(170, 170, 140, 20, Format("Name:", Color.yellow, false), false, false);
			this.AddHtml(310, 170, 40, 20, Format("Wins:", Color.yellow, true), false, false);
			this.AddHtml(350, 170, 60, 20, Format("Losses:", Color.yellow, true), false, false);
			this.AddHtml(410, 170, 50, 20, Format("Honor:", Color.yellow, true), false, false);
			this.AddHtml(460, 170, 50, 20, Format("Change:", Color.yellow, true), false, false);

			// Rows background
			this.AddBackground(120, 190, 400, 20, 3000);
			this.AddBackground(120, 210, 400, 20, 3000);
			this.AddBackground(120, 230, 400, 20, 3000);
			this.AddBackground(120, 290, 400, 20, 3000);
			this.AddBackground(120, 270, 400, 20, 3000);
			this.AddBackground(120, 250, 400, 20, 3000);
			this.AddBackground(120, 310, 400, 20, 3000);
			this.AddBackground(120, 330, 400, 20, 3000);
			this.AddBackground(120, 350, 400, 20, 3000);
			this.AddBackground(120, 370, 400, 20, 3000);

			// Add scroll buttons
			if (page > 0)
				this.AddButton(120, 120, 9909, 9910, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			if (Ladder.Players.Count > page * 10 + 10)
				this.AddButton(500, 120, 9903, 9904, (int)Buttons.Button2, GumpButtonType.Reply, 0);

            int j = 0;              // lame check but ppl use razor to access nonexistant buttons
            for (int i = page * 10; i >= 0 && i < Ladder.Players.Count && j < 10; i++)
			{
				PlayerMobile pm = (PlayerMobile)Ladder.Players[i];

				// Data formated with html
				this.AddHtml(130, 190 + j * 20, 30, 20, Format((i + 1), Color.black, true), false, false);
				this.AddHtml(170, 190 + j * 20, 140, 20, Format(pm.Name, Color.black, false), false, false);
				this.AddHtml(310, 190 + j * 20, 40, 20, Format(pm.Wins, Color.green, true), false, false);
				this.AddHtml(350, 190 + j * 20, 60, 20, Format(pm.Losses, Color.red, true), false, false);
				this.AddHtml(410, 190 + j * 20, 50, 20, (pm.Honor >= 0 ? Format(pm.Honor, Color.green, true) : Format(pm.Honor, Color.red, true)), false, false);
				this.AddHtml(450, 190 + j * 20, 50, 20, (pm.HonorChange >= 0 ? Format(pm.HonorChange, Color.green, true) : Format(pm.HonorChange, Color.red, true)), false, false);

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
				state.Mobile.SendGump(new RankGump(page - 1, this.X, this.Y));
			}
			else if (info.ButtonID == (int)Buttons.Button2)
			{
				state.Mobile.SendGump(new RankGump(page + 1, this.X, this.Y));
			}

		}
	}
}