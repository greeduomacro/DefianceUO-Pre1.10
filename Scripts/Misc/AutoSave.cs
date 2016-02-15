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
using Server.Factions;

namespace Server.Misc
{
	public class AutoSave : Timer
	{
		private static TimeSpan m_Delay = TimeSpan.FromMinutes( 60.0 );
		//private static TimeSpan m_Warning = TimeSpan.Zero;
		private static TimeSpan m_Warning = TimeSpan.FromSeconds( 30.0 );

		public static void Initialize()
		{
			new AutoSave().Start();
			Commands.Register( "SetSaves", AccessLevel.Administrator, new CommandEventHandler( SetSaves_OnCommand ) );
		}

		public static bool m_SavesEnabled = true;

		public static bool SavesEnabled
		{
			get{ return m_SavesEnabled; }
			set{ m_SavesEnabled = value; }
		}

		[Usage( "SetSaves <true | false>" )]
		[Description( "Toggles automatic shard saving." )]
		public static void SetSaves_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
			{
				m_SavesEnabled = e.GetBoolean( 0 );
				e.Mobile.SendMessage( "Saves have been {0}.", m_SavesEnabled ? "enabled" : "disabled" );
			}
			else
			{
				e.Mobile.SendMessage( "Format: SetSaves <true | false>" );
			}
		}

		public AutoSave() : base( m_Delay - m_Warning, m_Delay )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			if ( !m_SavesEnabled || AutoRestart.Restarting )
				return;

			if ( m_Warning == TimeSpan.Zero )
				Save();
			else
			{
				int s = (int)m_Warning.TotalSeconds;
				int m = s / 60;
				s %= 60;

				if ( m > 0 && s > 0 )
					World.Broadcast( 0x35, true, "The world will save in {0} minute{1} and {2} second{3}.", m, m != 1 ? "s" : "", s, s != 1 ? "s" : "" );
				else if ( m > 0 )
					World.Broadcast( 0x35, true, "The world will save in {0} minute{1}.", m, m != 1 ? "s" : "" );
				else
					World.Broadcast( 0x35, true, "The world will save in {0} second{1}.", s, s != 1 ? "s" : "" );

				Timer.DelayCall( m_Warning, new TimerCallback( Save ) );
			}
		}

		public static void Save()
		{
			if ( AutoRestart.Restarting )
				return;

			//try
			//{
				ArrayList mobs = new ArrayList( World.Mobiles.Values );

try{
				Backup();
} catch {}

			foreach ( Mobile m in mobs )
			{
				if ( m == null )
					continue;
				else if ( m is PlayerMobile )
				{
					if ( (Account)m.Account == null )
					{
						Console.WriteLine( "*** Warning: Orphan Deleted - {0} [{1}]: {2}", m.Location, m.Map, m.Name );
						m.Delete();
					}
				}
			}
			// *********************************
			SaveGump.ShowSaveGump();
			// *********************************

			World.Save(false);
			Ladder.Ladder.SaveLadder(); // Ladder mod
			Donation.Donation.Save(); // Auto-Donation mod

			// *********************************
			SaveGump.CloseSaveGump();
			// *********************************

			//}
			//catch{}
		}

		private static string[] m_Months = new string[]
			{
				"January", "February", "March", "April", "May",
				"June", "July", "August", "September", "October",
				"November", "December"
			};

		private static void Backup()
		{
			DateTime timestamp = DateTime.Now;

			string root = Path.Combine( Core.BaseDirectory, Path.Combine("Backups", "Automatic") );
			string datepath = Path.Combine( timestamp.Year.ToString(), Path.Combine(m_Months[timestamp.Month - 1].ToString(), timestamp.Day.ToString("D2")));
			root = Path.Combine( root, datepath);

			string folder = Path.Combine( root, String.Format( "{0:D2}-{1:D2}-{2:D2}", timestamp.Hour, timestamp.Minute, timestamp.Second ) );

			if ( !Directory.Exists( root ) )
				Directory.CreateDirectory( root );

			if ( Directory.Exists( folder ) )
				folder = folder + "-" + timestamp.Millisecond.ToString("D3");

			string saves = Path.Combine( Core.BaseDirectory, "Saves" );

			if ( Directory.Exists( saves ) )
				Directory.Move( saves, folder );
		}
	}
}