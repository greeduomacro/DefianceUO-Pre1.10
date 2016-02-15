using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using Server;
using Server.Network;
using Server.Regions;

namespace Server.Misc
{
	public class AutoRestart : Timer
	{
        //Configuration ********************************************************************************************************************************************
		public static bool Enabled = false; // is the script enabled?
		private static bool DisableGuards = false; // Disable guards on server war?

		public static TimeSpan RestartTime = TimeSpan.FromHours( 7.0 ); // time of day at which to restart
        public static DayOfWeek RestartDay = DayOfWeek.Wednesday; // day of week to restart.

        public static TimeSpan RestartDelay = TimeSpan.FromMinutes( 30.0 ); // how long the server should remain active before restart (period of 'server wars')
		private static int[] Warnings = new int[]{ 900, 600, 300, 180, 60, 10, 5, 4, 3, 2, 1 }; //Warning intervals in seconds BEFORE any given restart time
        private static TimeSpan LoginWarnDelay = TimeSpan.FromHours(12); //How many hours before a scheduled restart do players get a sysmessage upon login
        //**********************************************************************************************************************************************************

        public const int SystemHue = 0x38A; // Hue used for system broadcasts - 0x38A
        private static int m_WarningCount = 0;
        private static bool m_Restarting = false;
		private static DateTime m_RestartTime;
        private static DateTime m_WarnAfter;

        #region Properties
        public static bool Restarting
		{
			get{ return m_Restarting; }
		}

		public static DateTime RestartDateTime
		{
			get{ return m_RestartTime; }
        }
        #endregion

        #region Broadcast methods
        //Use only ASCII messages, not unicode.
		public static void Broadcast( AccessLevel level, int font, int hue, string text, string channel )
		{
			ArrayList mobiles = NetState.Instances;

			for( int i = 0; i < mobiles.Count; i++ )
				BroadcastTo( (NetState)mobiles[i], level, font, hue, text );
		}

		public static void Broadcast( AccessLevel level, int font, int hue, string text )
		{
			Broadcast( level, font, hue, text, null );
		}

		public static void BroadcastTo( NetState to, AccessLevel level, int font, int hue, string text )
		{
			if ( to != null && to.Mobile != null && to.Mobile.AccessLevel >= level )
				to.Send( new AsciiMessage( Serial.MinusOne, -1, 0, hue, font, "System", text ) );
        }
        #endregion

        public static void Initialize()
		{
            Commands.Register( "SystemBCast", AccessLevel.Administrator, new CommandEventHandler( SystemBroadcast_OnCommand ) );
			Commands.Register( "SBC", AccessLevel.Administrator, new CommandEventHandler( SystemBroadcast_OnCommand ) );

            if (Enabled)
            {
                Commands.Register("ScheduleServerRestart", AccessLevel.Administrator, new CommandEventHandler(ScheduleServerRestart_OnCommand));
                Commands.Register("SchedRestart", AccessLevel.Administrator, new CommandEventHandler(ScheduleServerRestart_OnCommand));
                new AutoRestart().Start();
            }
		}

		[Aliases( "SBC" )]
		[Usage( "SystemBcast <text>" )]
		[Description( "Displays the message as a grey system message to the whole shard." )]
		private static void SystemBroadcast_OnCommand( CommandEventArgs e )
		{
			if ( e.Length > 0 )
				Broadcast( AccessLevel.Player, 0x0, SystemHue, String.Format( "[SYSTEM]: {0}", e.ArgString ) );
		}

        [Aliases ( "SchedRestart" )]
        [Usage("ScheduleServerRestart <true | false>")]
        [Description("Schedules a server restart.")]
        private static void ScheduleServerRestart_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                if (e.GetBoolean(0))
                {
                    m_RestartTime = DateTime.Now.Date + RestartTime;
                    if (m_RestartTime < DateTime.Now) m_RestartTime += TimeSpan.FromDays(1.0);
                    e.Mobile.SendMessage("A server restart has been scheduled for {0} {1}.", m_RestartTime.ToLongDateString(), m_RestartTime.ToLongTimeString());
                }
                else
                {
                    CalculateRegularRestartTime();
                    e.Mobile.SendMessage("The additional restart has been canceled. The server will restart on {0} {1}.", m_RestartTime.ToLongDateString(), m_RestartTime.ToLongTimeString());
                }
            }
            else
                e.Mobile.SendMessage("The server will restart on {0} at {1}.", m_RestartTime.ToLongDateString(), m_RestartTime.ToLongTimeString());
        }

        public AutoRestart()
            : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
		{
			Priority = TimerPriority.OneSecond;
            CalculateRegularRestartTime();

            if (LoginWarnDelay != TimeSpan.MinValue)
            {
                m_WarnAfter = m_RestartTime - LoginWarnDelay;
                EventSink.Login += new LoginEventHandler(EventSink_Login);
            }
        }

        private static void CalculateRegularRestartTime()
        {
            //Calculate restart date for next 'DayOfWeek', e.g. next Wednesday
            m_RestartTime = DateTime.Now.Date + RestartTime;
            m_RestartTime = m_RestartTime.AddDays(RestartDay >= m_RestartTime.DayOfWeek ? ((int)RestartDay - (int)m_RestartTime.DayOfWeek) : 7 - (int)m_RestartTime.DayOfWeek + (int)RestartDay);
            if (m_RestartTime < DateTime.Now) m_RestartTime += TimeSpan.FromDays(7.0);
        }

		private void Warning_Callback()
		{
			Broadcast( AccessLevel.Player, 0x0, SystemHue, String.Format( "Server War in {0}", FormatTimeSpan( TimeSpan.FromSeconds( Warnings[m_WarningCount++] ) ) ) );
		}

		private static void Restart_Callback()
		{
			Process.Start( Core.ExePath );
			Core.Process.Kill();
		}

		protected override void OnTick()
		{
			if ( m_Restarting || !Enabled )
				return;

			if ( m_WarningCount < Warnings.Length && DateTime.Now >= ( m_RestartTime - TimeSpan.FromSeconds( Warnings[m_WarningCount] ) ) )
			{
				Warning_Callback();
				return;
			}
			else if ( DateTime.Now < m_RestartTime )
				return;

			AutoSave.Save();

			m_Restarting = true;
			AutoSave.SavesEnabled = false;

			Timer.DelayCall( RestartDelay, new TimerCallback( Restart_Callback ) );
			if ( RestartDelay > TimeSpan.Zero )
			{
				Broadcast( AccessLevel.Player, 0x0, SystemHue, String.Format("---- SERVER WAR ----") );
				Broadcast( AccessLevel.Player, 0x0, SystemHue, String.Format("Server restarting in {0}", FormatTimeSpan( RestartDelay )) );
				PrepareServerWar();
			}
		}

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile == null) return;
            if (DateTime.Now >= m_WarnAfter)
                e.Mobile.SendMessage("NOTE: A server restart has been scheduled for {0}.", m_RestartTime.ToShortTimeString());
        }

		public static string FormatTimeSpan( TimeSpan ts )
		{
			//Based on a regular scale of 365 days to a year, 30 days to a month, 24 hours to a day, 60 minutes to an hour, and 60 seconds to a minute.
			int years = ts.Days / 365;
			string year = (ts.Days / 365) > 1 ? "years " : (ts.Days / 365 <= 0) ? "" : "year ";
			string yspace = String.Format("{0}{1}{2}",years > 0 ? years.ToString() : "", years > 0 ? " " : "", year);
			int months = (ts.Days % 365) / 30;
			string month = ((ts.Days % 365) / 30) > 1 ? "months " : ((ts.Days % 365) / 30 <= 0) ? "" : "month ";
			string mspace = String.Format("{0}{1}{2}",months > 0 ? months.ToString() : "", months > 0 ? " " : "", month);
			int days = ((ts.Days % 365) % 30);
			string day = ((ts.Days % 365) % 30) > 1 ? "days " : ((ts.Days % 365) % 30) <= 0 ? "" : "day ";
			string dspace = String.Format("{0}{1}{2}",days > 0 ? days.ToString() : "", days > 0 ? " " : "", day);
			int hours = ts.Hours;
			string hour = ts.Hours > 1 ? "hours " : ts.Hours <= 0 ? "" : "hour ";
			string hspace = String.Format("{0}{1}{2}",hours > 0 ? hours.ToString() : "", hours > 0 ? " " : "", hour);
			int minutes = ts.Minutes;
			string minute = ts.Minutes > 1 ? "minutes " : ts.Minutes  <= 0 ? "" : "minute ";
			string nspace = String.Format("{0}{1}{2}",minutes > 0 ? minutes.ToString() : "", minutes > 0 ? " " : "", minute);
			int seconds = ts.Seconds;
			string second = ts.Seconds > 1 ? "seconds" : ts.Seconds <= 0 ? "" : "second";
			string sspace = String.Format("{0} {1}",seconds > 0 ? seconds.ToString() : "",second);
			return String.Format("{0}{1}{2}{3}{4}{5}", yspace, mspace, dspace, hspace, nspace, sspace);
		}
		public static void PrepareServerWar()
		{
			if ( DisableGuards )
			{
				Map[] maps = Map.Maps;

				for( int i = 0; i < maps.Length; i++)
					if ( maps[i] != null )
						foreach( Region region in maps[i].Regions )
							if ( region != null && region is GuardedRegion )
								((GuardedRegion)region).Disabled = true;
			}
		}
	}
}