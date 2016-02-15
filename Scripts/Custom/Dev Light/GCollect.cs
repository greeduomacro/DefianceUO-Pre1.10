using Server;
using System;
using Server.Items;

namespace Server.Scripts.Commands
{
	public class GCCollect
	{
		public static void Initialize()
		{
			Server.Commands.Register( "Collect", AccessLevel.Administrator, new CommandEventHandler( Collect_OnCommand ) );

                }

		[Usage( "Collect" )]
		[Description( "Collects Memory Garbage." )]
		private static void Collect_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			if ( from == null  )
				return;

			try
			{
				System.GC.Collect();
				from.SendMessage( "Collected" );
			}
			catch
			{
				from.SendMessage( "Wrong format you fool!" );
			}
		}
	}
}