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
	public class WaitGump : Gump
	{
		private DuelObject duel;
		public WaitGump(DuelObject duel)
			: base( 50, 50 )
		{
			this.duel = duel;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(116, 106, 249, 143, 9270);
			this.AddLabel(209, 112, 1259, @"Challenge");
			this.AddLabel(153, 139, 1149, @"The challenge has been made,");
			this.AddButton(209, 216, 241, 242, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddLabel(139, 193, 1149, @"You can still cancel the challenge.");
			this.AddLabel(130, 160, 1149, @"Waiting for response from opponent.");

		}

		public enum Buttons
		{
			Button1 = 0,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{

			if (info.ButtonID == (int)Buttons.Button1)
			{
				duel.Player1.SendMessage("Your canceled the challenge");
				duel.Player2.CloseGump(typeof(ChallengedGump));
				duel.Player2.SendMessage("Your opponent canceled the challenge");
			}
		}
	}
}