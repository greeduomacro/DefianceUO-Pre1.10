using System;
using System.Reflection;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class Forum
	{
		public static void Initialize()
		{
			Server.Commands.Register( "Forum", AccessLevel.Player, new CommandEventHandler( Forum_OnCommand ) );
		}

		[Usage( "Forum" )]
		[Description( "Defiance Network needs your daily input, Opens webbrowser to our Forums." )]
		public static void Forum_OnCommand( CommandEventArgs e )
		{
			string url = "http://www.defianceuo.com/forums/";

			Mobile m = e.Mobile;
			m.LaunchBrowser( url );
		}
	}
}