using System;
using System.Reflection;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class CTFKick
	{

		public static void Initialize()
		{
			Server.Commands.Register( "CTFKick", AccessLevel.GameMaster, new CommandEventHandler( CTFKick_OnCommand ) );
		}

		[Usage( "CTFKick" )]
		[Description( "Kicks a player from CTF." )]
		private static void CTFKick_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new CTFKickTarget();
			e.Mobile.SendMessage( "Select a player to kick from CTF." );
			for ( int i = 0; i < 5; i++ )
				e.Mobile.SendMessage( "BE EXTREMELY CAREFUL WITH THIS! ALL OF THE TARGET'S BELONGINGS WILL BE WIPED!" );
		}

		private class CTFKickTarget : Target
		{
			public CTFKickTarget() : base( 15, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( !(targ is PlayerMobile) )
				{
					from.SendMessage( "You can only target players." );
					return;
				}

				CTFTeam team = CTFGame.FindTeamFor( (PlayerMobile)targ );

				if ( team == null )
				{
					from.SendMessage( "This player is not in a CTF game." );
				}
				else
				{
					team.Game.LeaveGame( (PlayerMobile)targ );
					LeaveGameGate.Strip( (PlayerMobile)targ );
					from.SendMessage( "Player kicked from CTF." );
				}
			}
		}
	}
}