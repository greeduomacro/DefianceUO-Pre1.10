/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							WaitGump.cs
*						-------------------------
*
*	File Description:	Gump that is displayed to a challenger while
*						waiting for response from the 'victim'. The
*						challenge can be canceled from this gump.
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
	public class EndDuelGump : Gump
	{
		private DuelObject duel;
		public EndDuelGump(DuelObject duel)
			: base( 50, 50 )
		{
			this.duel = duel;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(116, 106, 249, 143, 9270);
			this.AddLabel(209, 112, 1259, @"End duel?");
			this.AddButton(150, 216, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(250, 216, 241, 242, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddLabel(130, 160, 1149, @"If you end the duel now you will lose.");

		}

		public enum Buttons
		{
			Button1 = 1,
			Button2 = 0,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{

			if (duel != null && duel.Timer != null && info.ButtonID == (int)Buttons.Button1)
			{
				duel.Timer.Stop();
				if (duel.Player1 == state.Mobile)
					duel.Finished(2, DateTime.Now);
				else if (duel.Player2 == state.Mobile)
					duel.Finished(1, DateTime.Now);
			}
		}
	}
}