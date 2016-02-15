using System;
using System.Collections;
using Server.Accounting;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;



namespace Server.Scripts.Commands
{
	public class FriendCommands
	{
		public static void Initialize()
		{
			Server.Commands.Register( "FriendList", AccessLevel.Player, new CommandEventHandler( FriendList_OnCommand ) );
			Server.Commands.Register( "FTell", AccessLevel.Player, new CommandEventHandler( FTell_OnCommand ) );
		}

		[Usage( "FriendList" )]
		[Description( "Shows your friend list" )]
		private static void FriendList_OnCommand( CommandEventArgs e )
		{
			e.Mobile.CloseGump( typeof( FriendListGump ) );
			e.Mobile.SendGump( new FriendListGump( e.Mobile ) );
		}

		[Usage( "FTell" )]
		[Description( "Talks to a friend (use _ instead of spaces in your friend's name)" )]
		private static void FTell_OnCommand( CommandEventArgs e )
		{
			string args = e.ArgString;
			Mobile from = e.Mobile;
			int spaceindex = args.IndexOf( ' ' );
			if ( spaceindex != -1 )
			{
				string name = args.Substring( 0, spaceindex );
				string text = args.Substring( spaceindex + 1 );

				if ( name.Length < 2 )
				{
					from.SendMessage( 133, "That name is too short. You need to specify at least 2 characters." );
					return;
				}

				name = name.Replace( '_', ' ' );

				ArrayList friends = FriendSystem.GetFriendList( from ).MutualFriends;
				int sentto = 0;
				foreach( Mobile friend in friends )
				{
					string friendname = friend.Name;
					//if ( friend.Name.ToLower() == name.ToLower() && friend.NetState != null )
					if ( name.Length <= friendname.Length && name.ToLower() == friendname.Substring( 0, name.Length ).ToLower() && friend.NetState != null )
					{
						FriendSystem.Tell( from, friend, text );
						sentto++;
					}
				}
				if ( sentto == 0 )
					from.SendMessage( 133, "No friend named {0} is online.", name );
			}
			else
				from.SendMessage( 133, "Invalid format. Try something like \"[FTell My_Best_Friend Hello!\"." );
		}
	}
}