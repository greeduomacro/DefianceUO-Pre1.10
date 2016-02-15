using System;
using System.Reflection;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class Mydefiance
	{
		public static void Initialize()
		{
			Server.Commands.Register( "Mydefiance", AccessLevel.Player, new CommandEventHandler( Mydefiance_OnCommand ) );
		}

		[Usage( "Mydefiance" )]
		[Description( "Visit the MyDefiance pages to see guild and faction warfare statistics." )]
		public static void Mydefiance_OnCommand( CommandEventArgs e )
		{
			string url = "my.defianceuo.com";

			Mobile m = e.Mobile;
			m.LaunchBrowser( url );
		}
	}
}