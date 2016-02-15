//BeneathTheStars, aka [Charon]

using System;
using Server;
using Server.Scripts.Commands;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Misc
{
	public class FindChar
	{
		public static void Initialize()
		{
			Server.Commands.Register( "FindChar", AccessLevel.Seer, new CommandEventHandler( FindChar_OnCommand ) );
		}

		[Usage( "FindChar <name>" )]
		[Description( "Finds the account name of a character." )]
		public static void FindChar_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
			{
				string name = e.GetString( 0 ).ToLower();

				foreach ( Account pm in Accounts.Table.Values )
				{
				  if ( pm == null )
						return;

				int index = 0;

					for ( int i = 0; i < pm.Length; ++i )
					{
						Mobile m = pm[i];
                        if ( m == null )
							continue;
						if (m.Name.ToLower().IndexOf( name ) >= 0)
						 {
						e.Mobile.SendMessage( "Character: {0}, Belongs to Account: {1}", m.Name, pm );

						 }
						else
						 continue;

						 ++index;

				    }
				}
			e.Mobile.SendMessage( "Done.");
			}
			else
			{
				e.Mobile.SendMessage( "Format: FindChar <name>" );
			}
		}
	}
}