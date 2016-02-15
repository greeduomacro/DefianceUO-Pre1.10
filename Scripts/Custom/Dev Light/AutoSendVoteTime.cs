using System;
using System.Reflection;
using System.Collections;
using System.IO;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Accounting;
using Server.Gumps;

namespace Server.Misc
{
	public class AutoVoting : Timer
	{
		private static TimeSpan m_Delay = TimeSpan.FromHours( 10.0 );  //change to 12 hours after test
		//private static TimeSpan m_Warning = TimeSpan.Zero;
		private static TimeSpan m_Warning = TimeSpan.FromSeconds( 10.0 ); //keep warning

		public static void Initialize()
		{
			new AutoVoting().Start();
			Commands.Register( "SetVote", AccessLevel.Administrator, new CommandEventHandler( SetVoting_OnCommand ) );
		}

		public static bool m_VotingEnabled = true;

		public static bool VotingEnabled
		{
			get{ return m_VotingEnabled; }
			set{ m_VotingEnabled = value; }
		}

		[Usage( "SetVote <true | false>" )]
		[Description( "Toggles automatic shard voting." )]
		public static void SetVoting_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
			{
				//m_VotingEnabled = e.GetBoolean( 0 );
				e.Mobile.SendMessage( "Voting has been {0}.", m_VotingEnabled ? "enabled" : "disabled" );
			}
			else
			{
				e.Mobile.SendMessage( "Format: SetVote <true | false>" );
			}
		}

		public AutoVoting() : base( m_Delay - m_Warning, m_Delay )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			if ( !m_VotingEnabled || AutoRestart.Restarting )
				return;

			if ( m_Warning == TimeSpan.Zero )
				ShowAutoVoteGump();
			else
			{
				int s = (int)m_Warning.TotalSeconds;
				int m = s / 60;
				s %= 60;

				if ( m > 0 && s > 0 )
					World.Broadcast( 0x35, true, "An auto vote gump will appear in {0} minute{1} and {2} second{3}.", m, m != 1 ? "s" : "", s, s != 1 ? "s" : "" );
				else if ( m > 0 )
					World.Broadcast( 0x35, true, "Auto vote in {0} minute{1}.", m, m != 1 ? "s" : "" );
				else
					World.Broadcast( 0x35, true, "Auto vote in {0} second{1}.", s, s != 1 ? "s" : "" );

				Timer.DelayCall( m_Warning, new TimerCallback( ShowAutoVoteGump ) );
			}
		}

		public static void ShowAutoVoteGump()
		{
			for ( int i = 0; i < NetState.Instances.Count; ++i )
			{
				Mobile m = ((NetState)NetState.Instances[i]).Mobile;
				if( m != null && !m.Deleted && m is PlayerMobile )
					m.SendGump( new AutoVoteGump() );
			}
		}
	}
}