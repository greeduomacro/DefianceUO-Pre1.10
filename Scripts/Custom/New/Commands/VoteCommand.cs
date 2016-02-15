using System;
using System.Reflection;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class Vote
	{
		public static void Initialize()
		{
			Server.Commands.Register( "Vote", AccessLevel.Player, new CommandEventHandler( Vote_OnCommand ) );
		}

		[Usage( "Vote" )]
		[Description( "Vote here every 7 days to increase the shards playerbase and with that your gameplay experience." )]
		public static void Vote_OnCommand( CommandEventArgs e )
		{
			if (e.Mobile == null || e.Mobile.Deleted)
				return;

			string url = "http://www.defianceuo.com/vote.htm";

			Mobile m = e.Mobile;
			m.LaunchBrowser( url );
		}
	}
}