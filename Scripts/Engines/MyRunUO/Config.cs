using System;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace Server.Engines.MyRunUO
{
	public class Config
	{
		/* Some configuration options have moved to the StaticConfiguration.cs! */

		public static bool Enabled = StaticConfiguration.MyRunUOActive;
		public static string DatabaseServer = GetDatabaseServer();

		// Details required for database connection string
		public static string DatabaseDriver			= "{MySQL}";
		public static string DatabaseDSN			= "JP";

		// Should the database use transactions? This is recommended
		public static bool UseTransactions = false;

		// Use optimized table loading techniques? (LOAD DATA INFILE)
		public static bool LoadDataInFile = true;

		// This must be enabled if the database server is on a remote machine.
		public static bool DatabaseNonLocal = ( DatabaseServer != "localhost" );

		// Text encoding used
		public static Encoding EncodingIO = Encoding.ASCII;

		// Database communication is done in a seperate thread. This value is the 'priority' of that thread, or, how much CPU it will try to use
		public static ThreadPriority DatabaseThreadPriority = ThreadPriority.BelowNormal;

		// Any character with an AccessLevel equal to or higher than this will not be displayed
		public static AccessLevel HiddenAccessLevel	= AccessLevel.Counselor;

		// Export character database every 30 minutes
		public static TimeSpan CharacterUpdateInterval = TimeSpan.FromHours( 24.0 );

		// Export online list database every 5 minutes
		//public static TimeSpan StatusUpdateInterval = TimeSpan.FromMinutes( 5.0 );

		public static string CompileConnectionString()
		{
			return StaticConfiguration.MyRunUODatabaseConnectString;
		}

		public static string GetDatabaseServer()
		{
			Regex dbregex = new Regex(@"^Server=(?<db>.*);.*;.*;.*;.*$");
			Match dbmatch = dbregex.Match(StaticConfiguration.MyRunUODatabaseConnectString);
			return dbmatch.Groups["db"].Value.ToString();
		}
	}
}