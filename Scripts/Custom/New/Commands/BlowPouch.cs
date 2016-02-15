using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
   public class BlowPouch
   {
		public static void Initialize()
		{
			Server.Commands.Register( "BlowPouch", AccessLevel.Player, new CommandEventHandler( BlowPouch_OnCommand ) );
		}

		[Usage( "BlowPouch" )]
		[Description( "Blows a trapped pouch if any available." )]
		public static void BlowPouch_OnCommand( CommandEventArgs e )
		{
			bool pouchfound = false;
			Item[] pouchs = e.Mobile.Backpack.FindItemsByType( typeof( Pouch ) );

			for ( int i = 0; i < pouchs.Length; ++i )
			{
				Pouch pouch = pouchs[i] as Pouch;

				if ( (int)pouch.TrapType == 1 )
				{
					pouch.OnDoubleClick(e.Mobile);
					pouchfound = true;
					break;
				}
			}

			if (!pouchfound)
				e.Mobile.SendMessage( "You have no trapped pouchs left." );
		}
	}
}