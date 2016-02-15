/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							LadderStone.cs
*						-------------------------
*
*	File Description:	Players can use the [endduel command
*						if they want to end their duel. (duh)
*
*
*/

using System;
using Server;
using Server.Gumps;
using Server.Scripts.Gumps;
using Server.Network;

namespace Server.Ladder
{

	public class EndDuel
	{
		public static void Initialize()
		{
			Server.Commands.Register("Endduel", AccessLevel.Player, new CommandEventHandler(Endduel_OnCommand));
		}


		[Usage("Endduel")]
		[Description("Stops an ongoing duel")]
		private static void Endduel_OnCommand(CommandEventArgs e)
		{
			DuelObject d = Ladder.GetDuel(e.Mobile);
			if (d != null)
			{
				Mobile opponent = d.Player1 == e.Mobile ? d.Player2 : d.Player1;
				if (opponent != null)
				{
					if (opponent.NetState == null)
					{
						d.Timer.Stop();
						d.Finished(-1, DateTime.Now);
					}
					else
					{
						e.Mobile.SendGump(new EndDuelGump(d));
						e.Mobile.SendMessage("Your opponent is still online.");
					}
				}
				else
				{
					e.Mobile.SendMessage("You are not duelling.");
				}
			}
			else
			{
				e.Mobile.SendMessage("You are not duelling.");
			}
		}
	}
}