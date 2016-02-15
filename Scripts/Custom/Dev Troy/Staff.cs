using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class AccessLevelMod
	{
		public AccessLevel Level;

		public AccessLevelMod( AccessLevel level )
		{
			Level = level;
		}
	}

	public class AccessLevelToggler
	{
		public static Dictionary<PlayerMobile, AccessLevelMod> m_Mobiles = new Dictionary<PlayerMobile, AccessLevelMod>();

		public static AccessLevelMod RawAccessLevel( PlayerMobile from )
		{
			AccessLevelMod mod = null;
			m_Mobiles.TryGetValue( from, out mod );

			return mod;
		}

		public static void Initialize()
		{
			Server.Commands.Register( "Staff", AccessLevel.Player, new CommandEventHandler( Staff_OnCommand ) );
		}

		[Usage( "Staff [<AccessLevel>]" )]
		[Description( "Switches a staff member between their own access levels" )]
		public static void Staff_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = (PlayerMobile)e.Mobile;

			AccessLevelMod mod = RawAccessLevel( from );

			if ( mod != null )
			{
				if ( e.Length == 0 )
				{
					from.AccessLevel = mod.Level;
					m_Mobiles.Remove( from );
				}
				else
				{
					AccessLevel level;
					if ( !ArgumentToAccessLevel( e.Arguments[0], out level ) )
						from.SendMessage( "Invalid AccessLevel: " + e.Arguments[0] );
					else
					{
						if ( mod.Level < level )
							from.SendMessage( "Invalid AccessLevel: " + e.Arguments[0] );
						else
						{
							if ( mod.Level == level )
							{
								m_Mobiles.Remove( from );
							}
							from.AccessLevel = level;
						}
					}
				}
			}
			else if ( from.AccessLevel == AccessLevel.Player )
				from.Say( e.ArgString );
			else
			{
				if ( e.Length == 0 )
				{
					m_Mobiles.Add( from, new AccessLevelMod( from.AccessLevel ) );
					from.AccessLevel = AccessLevel.Player;
				}
				else
				{
					AccessLevel level;
					if ( !ArgumentToAccessLevel( e.Arguments[0], out level ) )
						from.SendMessage( "Invalid AccessLevel: " + e.Arguments[0] );
					else
					{
						if ( from.AccessLevel <= level )
							from.SendMessage( "Invalid AccessLevel: " + e.Arguments[0] );
						else
						{
							m_Mobiles.Add( from, new AccessLevelMod( from.AccessLevel ) );
							from.AccessLevel = level;
						}
					}
				}
			}
		}

		public static bool ArgumentToAccessLevel( string argument, out AccessLevel accesslevel )
		{
			switch ( argument.ToLower() )
			{
				case "Player": case "0":
					accesslevel = AccessLevel.Player;
					return true;
			//	case "TrialCounselor": case "1":
			//		accesslevel = AccessLevel.TrialCounselor;
			//		return true;
				case "Counselor": case "1":
					accesslevel = AccessLevel.Counselor;
					return true;
				case "Gamemaster": case "2":
					accesslevel = AccessLevel.GameMaster;
					return true;
				case "Seer": case "3":
					accesslevel = AccessLevel.Seer;
					return true;
			//	case "Lead": case "5":
			//		accesslevel = AccessLevel.Lead;
			//		return true;
			//	case "Manager": case "6":
			//		accesslevel = AccessLevel.Manager;
			//		return true;
				case "Administrator": case "4":
					accesslevel = AccessLevel.Administrator;
					return true;
				default:
					accesslevel = AccessLevel.Player;
					return false;
			}
		}
	}
}