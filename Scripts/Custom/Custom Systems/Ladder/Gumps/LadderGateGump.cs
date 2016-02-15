/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							LadderGateGump.cs
*						-------------------------
*
*	File Description:	This gump shows people info about ladder
*						areas;
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
	public class LadderGateGump : Gump
	{

		private LadderGate m_Gate;
		private enum Color
		{
			yellow = 0,
			black,
			green,
			red,
			white,
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
		public LadderGateGump(int page, int x, int y, LadderGate gate)
			: base( x, y )
		{
			this.m_Gate = gate;
			this.page = page;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);

			//Gump background
			this.AddBackground(100, 100, 500, 310, 9270);

			//Labels
			this.AddLabel(350, 118, 1160, @"Select destination");


			// Formated html
			this.AddHtml(150, 170, 200, 20, Format("Name:", Color.yellow, false), false, false);
			this.AddHtml(300, 170, 70, 20, Format("Arenas:", Color.yellow, true), false, false);
			this.AddHtml(360, 170, 70, 20, Format("In Use:", Color.yellow, true), false, false);
			this.AddHtml(430, 170, 70, 20, Format("Duellers:", Color.yellow, true), false, false);
			this.AddHtml(510, 170, 70, 20, Format("Spectators:", Color.yellow, true), false, false);
/*
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
*/
			// Add scroll buttons
			if (page > 0)
				this.AddButton(120, 120, 9909, 9910, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			if (Ladder.Arenas.Count > page * 10 + 10)
				this.AddButton(560, 120, 9903, 9904, (int)Buttons.Button2, GumpButtonType.Reply, 0);

            int j = 0;          // lame check but ppl use razor to access nonexistant buttons
            for (int i = page * 10; i >= 0 && i < Ladder.Arenas.Count && j < 10; i++)
			{
				LadderAreaControl LAC = (LadderAreaControl)Ladder.Arenas[i];
				if (LAC != null)
				{
					int spectators = 0; ;
					if(LAC.MyRegion != null)
						spectators = LAC.MyRegion.Players.Count;
					int inUse = 0;
					int duellers = 0;
					foreach (ArenaControl AC in LAC.Arenas)
					{
						if (AC != null)
						{
							duellers += AC.MyRegion.Players.Count;
							if (AC.InUse)
								inUse++;
						}
					}

					// Data formated with html
					this.AddButton(120, 190 + j * 20, 9702, 9703, i+3, GumpButtonType.Reply, 0);
					this.AddHtml(150, 190 + j * 20, 200, 20, Format(LAC.Name, Color.white, false), false, false);
					this.AddHtml(300, 190 + j * 20, 70, 20, Format(LAC.Arenas.Count, Color.white, true), false, false);
					this.AddHtml(360, 190 + j * 20, 70, 20, Format(inUse, Color.white, true), false, false);
					this.AddHtml(430, 190 + j * 20, 70, 20, Format(duellers, Color.white, true), false, false);
					this.AddHtml(510, 190 + j * 20, 70, 20, Format(spectators, Color.white, true), false, false);

					j++;
				}
			}
		}

		public enum Buttons
		{
			Button1 = 1,
			Button2 = 2,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (state == null || state.Mobile == null)
				return;

			if (info.ButtonID == (int)Buttons.Button1)
			{
				state.Mobile.SendGump(new LadderGateGump(page - 1, this.X, this.Y, m_Gate));
			}
			else if (info.ButtonID == (int)Buttons.Button2)
			{
				state.Mobile.SendGump(new LadderGateGump(page + 1, this.X, this.Y, m_Gate));
			}
			else if (!state.Mobile.InRange(m_Gate.GetWorldLocation(), 1) || state.Mobile.Map != m_Gate.Map)
			{
				state.Mobile.SendLocalizedMessage(1019002); // You are too far away to use the gate.
			}
			else if (info.ButtonID >= 2)
			{
				int selected = info.ButtonID - 3;
				if (Ladder.Arenas.Count > selected && Ladder.Arenas[selected] != null)
				{
					LadderAreaControl LAC = (LadderAreaControl)Ladder.Arenas[selected];
					if (LAC.Map != null && LAC.GateSpot != Point3D.Zero)
					{

						BaseCreature.TeleportPets(state.Mobile, LAC.GateSpot, LAC.Map);
						state.Mobile.MoveToWorld(LAC.GateSpot, LAC.Map);
						state.Mobile.PlaySound(0x1FE);
						string msg = String.Format("[{0}]:{1}", state.Mobile.Name, LAC.Name);
						m_Gate.PublicOverheadMessage(MessageType.Regular, 0x22, false, msg);
					}
				}
			}

		}
	}
}