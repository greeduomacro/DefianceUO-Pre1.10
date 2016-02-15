using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Scripts.Commands;

namespace Server.Misc
{
	public class AutoVoteLink : Timer
	{
		public static bool Enabled = false; // is the script enabled?

		private static readonly TimeSpan[] VoteTimes = new TimeSpan[]{ TimeSpan.FromMinutes( 720.0 ), TimeSpan.FromMinutes( 1.0 ) };
		private static readonly TimeSpan VoteDelay = TimeSpan.Zero;

		private static readonly string VoteMessage = "Please vote for DefianceUO - Keep us alive and prospering!";
		private static readonly string VoteURL = "http://www.defianceuo.com/vote.htm";
		private static readonly int VoteFontColor = 0x22;

		private static DateTime m_VoteTime;

		public static void Initialize()
		{
			Commands.Register( "SendVoteLink", AccessLevel.Administrator, new CommandEventHandler( VoteLink_OnCommand ) );
			new AutoVoteLink().Start();
		}

		public static void VoteLink_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "You have initiated a player vote link." );
			Enabled = true;
			m_VoteTime = DateTime.Now;
		}

		public AutoVoteLink() : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;

			int index = 0;

			while ( m_VoteTime < DateTime.Now )
			{
				DateTime date = DateTime.Now.Date;
				if ( index < VoteTimes.Length )
					m_VoteTime = date + VoteTimes[index++];
				else
					m_VoteTime = (date + VoteTimes[0]).AddDays( 1.0 );
			}
		}

		private void Vote_Callback()
		{
			foreach ( NetState state in NetState.Instances )
			{
				if ( state.Mobile != null )
				{
					Mobile m = state.Mobile;
					if ( m.AccessLevel < AccessLevel.Administrator )
					{
						m.SendMessage( VoteFontColor, VoteMessage );
						m.SendGump( new WarningGump( 1060637, 30720, String.Format( "{0}:<br>{1}", VoteMessage, VoteURL ), 0xFFC000, 320, 240, new WarningGumpCallback( OpenBrowser_Callback ), new object[]{ VoteURL } ) );
					}
				}
			}
			if ( Enabled )
				new AutoVoteLink().Start();

			Stop();
		}

		public static void OpenBrowser_Callback( Mobile from, bool okay, object state )
		{
			object[] states = (object[])state;
			string url = (string)states[0];

			if ( okay )
				from.LaunchBrowser( url );
			else
				from.SendMessage( "You have chosen not to open your web browser." );
		}

		protected override void OnTick()
		{
			if ( Enabled && DateTime.Now >= m_VoteTime )
				Timer.DelayCall( VoteDelay, new TimerCallback( Vote_Callback ) );
		}
	}
}