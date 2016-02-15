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
*	File Description:	Simple Gump that lets players choose
*						between rankings, fights etc.
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
	public class ChooseGump : Gump
	{
		public ChooseGump()
			: base( 50, 50 )
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(100, 100, 260, 160, 9270);
			this.AddLabel(180, 130, 1160, @"Ladder Rankings");
			this.AddButton(130, 175, 2103, 2104, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(130, 195, 2103, 2104, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddLabel(150, 170, 1160, @"Top duelists on the ladder");
			this.AddLabel(150, 190, 1160, @"List of all your duels");

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
				state.Mobile.SendGump(new RankGump(0, 50, 50));
			}
			else if (info.ButtonID == (int)Buttons.Button2)
			{


				state.Mobile.SendGump(new FightGump(BuildFights(state.Mobile, 100), state.Mobile, 0, 50, 50));
			}
		}
		public ArrayList BuildFights(Mobile from, int size)
		{
			ArrayList fights = new ArrayList();
			for (int i = Ladder.Fights.Count - 1; fights.Count < size && i >= 0; i--)
			{
				if (((Fight)Ladder.Fights[i]).Winner == from || ((Fight)Ladder.Fights[i]).Loser == from)
					fights.Add(Ladder.Fights[i]);
			}
			return fights;
		}
	}
}