using System;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Prompts;
using Server.Targeting;
using System.Collections;
using Server.Mobiles;

namespace Server.FSPvpPointSystem
{
	public class PvpKeywords
	{
		public static void Initialize()
		{
			EventSink.Speech += new SpeechEventHandler( Speech_Event );
		}

		public static void Speech_Event( SpeechEventArgs e )
		{
			string speech = e.Speech.ToLower();
			Mobile from = e.Mobile;

			if ( speech.IndexOf( "toggle my pvp title" ) > -1 )
			{
				FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( from );
				bool toggle = !ps.ShowPvpTitle;
				from.SendMessage( "You toggle your pvp title {0}.", toggle ? "on" : "off" );
				FSPvpSystem.ToggleTitle( from, toggle );
			}
			else if ( speech.IndexOf( "pvp stats" ) > -1 )
			{
				from.Target = new StatTarget();
				from.SendMessage( "Whos pvp record would you like to view?" );
			}
		}

		public class StatTarget : Target
		{
			public StatTarget() : base( -1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is PlayerMobile )
				{
					Mobile m = (Mobile)o;

					FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( m );

					if ( m == from || ps.ShowPvpTitle || from.AccessLevel > AccessLevel.Player )
						from.SendGump( new PvpStatGump( m ) );
					else
						from.SendMessage( "This player has chosen to hide their pvp stats." );
				}
			}
		}
	}
}