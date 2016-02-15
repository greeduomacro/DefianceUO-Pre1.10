using System;
using System.Collections;
using System.IO;
using Server.Accounting;
using System.Globalization;

namespace Server.Misc
{
	public class DonationSystem
	{
		private static Hashtable m_DonatorAccountSettingsTable;
		private static Hashtable DonatorAccountSettingsTable
		{
			get
			{
				if ( m_DonatorAccountSettingsTable == null )
					m_DonatorAccountSettingsTable = new Hashtable();
				return m_DonatorAccountSettingsTable;
			}
		}

		private class DonatorAccountSettings
		{
			public Account m_Account;
			public int m_HorseHue;
			public int m_SandalHue;
			public int m_BandanaHue;
			public int m_RobeHue;

			public DonatorAccountSettings()
			{
				m_Account = null;
			}

			public DonatorAccountSettings( Account acc )
			{
				m_Account = acc;
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int)0 );

				writer.Write( m_Account.ToString() );
				writer.Write( m_HorseHue );
				writer.Write( m_SandalHue );
				writer.Write( m_BandanaHue );
				writer.Write( m_RobeHue );
				for ( int i = 0; i < 9; i++ )
					writer.Write( (int)0 );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 0:
					{
						m_Account = Accounts.GetAccount( reader.ReadString() );
						m_HorseHue = reader.ReadInt();
						m_SandalHue = reader.ReadInt();
						m_BandanaHue = reader.ReadInt();
						m_RobeHue = reader.ReadInt();
						for ( int i = 0; i < 9; i++ )
							reader.ReadInt();
						break;
					}
				}
			}
		}

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );
			//EventSink.WorldLoad += new WorldLoadEventHandler( EventSink_WorldLoad );
			Load();
		}

		private static void EventSink_WorldSave( WorldSaveEventArgs e )
		{
			Save();
		}

		private static void EventSink_WorldLoad()
		{
			Load();
		}

		public static void Load()
		{
			Console.Write("DonatorAccountSettings: Loading...");

			string idxPath = Path.Combine( "Saves/Donation", "DonatorAccountSettings.idx" );
			string binPath = Path.Combine( "Saves/Donation", "DonatorAccountSettings.bin" );

			if (File.Exists(idxPath) && File.Exists(binPath))
			{
				// Declare and initialize reader objects.
				FileStream idx = new FileStream(idxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				FileStream bin = new FileStream(binPath, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryReader idxReader = new BinaryReader(idx);
				BinaryFileReader binReader = new BinaryFileReader(new BinaryReader(bin));

				// Start by reading the number of duels from an index file
				int orderCount = idxReader.ReadInt32();

				for (int i = 0; i < orderCount; ++i)
				{

					DonatorAccountSettings das = new DonatorAccountSettings();
					// Read start-position and length of current order from index file
					long startPos = idxReader.ReadInt64();
					int length = idxReader.ReadInt32();
					// Point the reading stream to the proper position
					binReader.Seek(startPos, SeekOrigin.Begin);

					try
					{
						das.Deserialize(binReader);

						if (binReader.Position != (startPos + length))
							throw new Exception(String.Format("***** Bad serialize on DonatorAccountSettings[{0}] *****", i));
					}
					catch
					{
						//handle
					}
					if ( das != null && das.m_Account != null )
						DonatorAccountSettingsTable.Add( das.m_Account, das );
				}
				// Remember to close the streams
				idxReader.Close();
				binReader.Close();
			}

			Console.WriteLine("done");
		}


		public static void Save()
		{
			Console.Write("DonatorAccountSettings: Saving...");

			if (!Directory.Exists("Saves/Donation/"))
				Directory.CreateDirectory("Saves/Donation/");

			string idxPath = Path.Combine( "Saves/Donation", "DonatorAccountSettings.idx" );
			string binPath = Path.Combine( "Saves/Donation", "DonatorAccountSettings.bin" );


			GenericWriter idx = new BinaryFileWriter(idxPath, false);
			GenericWriter bin = new BinaryFileWriter(binPath, true);

			idx.Write( (int)DonatorAccountSettingsTable.Values.Count );
			foreach ( DonatorAccountSettings das in DonatorAccountSettingsTable.Values )
			{
				long startPos = bin.Position;
				das.Serialize( bin );
				idx.Write( (long)startPos );
				idx.Write( (int)(bin.Position - startPos) );
			}
			idx.Close();
			bin.Close();
			Console.WriteLine("done");
		}

		public static int [] DonationHorseHues = new int[] { 0, 1230, 1303, 1420, 1501, 1619, 1640, 2001, 2012, };
		public static int [] DonationSandalHues = new int[] { 0, 1177, };
		public static int [] DonationBandanaHues = new int[] { 0, 1177, };
		public static int [] DonationRobeHues = new int[] { 0, 1365, 2213, 1158, 1154, 1109, 1257 };

		private static DonatorAccountSettings GetSettings( Account acc )
		{
			DonatorAccountSettings settings;
			if ( (settings = (DonatorAccountSettings)DonatorAccountSettingsTable[acc]) == null )
			{
				settings = new DonatorAccountSettings( acc );
				DonatorAccountSettingsTable.Add( acc, settings );
			}
			return settings;
		}

		public static int GetHorseHue( Account acc )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			return settings.m_HorseHue;
		}

		public static void SetHorseHue( Account acc, int hue )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			settings.m_HorseHue = hue;
		}

		public static int GetSandalHue( Account acc )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			return settings.m_SandalHue;
		}

		public static void SetSandalHue( Account acc, int hue )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			settings.m_SandalHue = hue;
		}

		public static int GetBandanaHue( Account acc )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			return settings.m_BandanaHue;
		}

		public static void SetBandanaHue( Account acc, int hue )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			settings.m_BandanaHue = hue;
		}

		public static int GetRobeHue( Account acc )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			return settings.m_RobeHue;
		}

		public static void SetRobeHue( Account acc, int hue )
		{
			DonatorAccountSettings settings = GetSettings( acc );
			settings.m_RobeHue = hue;
		}


		public static void SetSubscriptionStatus( Account acc, DateTime started, TimeSpan duration )
		{
			acc.SetTag( "DonationStart", started.ToString() );
			acc.SetTag( "DonationDuration", duration.ToString() );
		}

		public static bool AddToSubscription( Account acc, TimeSpan duration )
		{
			DateTime DonationStart = DateTime.MinValue;
			TimeSpan DonationDuration = TimeSpan.Zero;

			try
			{
				DonationStart = DateTime.Parse( acc.GetTag( "DonationStart" ) );
				DonationDuration = TimeSpan.Parse( acc.GetTag( "DonationDuration" ) );
			}
			catch
			{
			}


			if (DonationStart == DateTime.MinValue && DonationDuration == TimeSpan.Zero)
			{
				try
				{
					acc.SetTag( "DonationStart", DateTime.Now.ToString() );
					acc.SetTag( "DonationDuration", duration.ToString() );
					//from.SendMessage("Your donation status has been updated.");
					//this.Delete();
					return true;
				}
				catch
				{
					//from.SendMessage("An error ocurred trying to update your donation status. Contact an Administrator.");
					return false;
				}
			}
			else if (DonationDuration == TimeSpan.MaxValue)
			{
				//already at max
				//from.SendMessage("You are already at permanent membership status.");
				return false;
			}
			else
			{
				//existing donation


				try
				{
					//Avoid overflow
					if (duration == TimeSpan.MaxValue)
						DonationDuration = duration;
					else
					{
						// Make sure that expired subscriptions don't cause people to get "negative time".
						if(DonationStart + DonationDuration < DateTime.Now)
							DonationDuration = (DateTime.Now - DonationStart) + duration;
						else
							DonationDuration += duration;
					}

					// Old stuff (next two lines) Should probably never be reintroduced - caleb
					//if ( DonationStart + DonationDuration < DateTime.Now + duration )
					//	DonationDuration = DateTime.Now + duration - DonationStart;

					acc.SetTag("DonationDuration", DonationDuration.ToString());
					//from.SendMessage("Your donation status has been updated.");
					//this.Delete();
					return true;
				}
				catch
				{
					//from.SendMessage("An error ocurred trying to update your donation status. Contact an Administrator.");
					return false;
				}
			}
		}

		public static bool AddToSubscription( Mobile m, TimeSpan duration )
		{
			if ( (Account)m.Account != null )
				return AddToSubscription( (Account)m.Account, duration );
			else
				return false;
		}

		public static bool HasDonated( Account acc )
		{
			if ( acc == null )
				return false;

			DateTime DonationStart;
			TimeSpan DonationDuration;

			try
			{
				DonationStart = DateTime.Parse( acc.GetTag( "DonationStart" ) );
				DonationDuration = TimeSpan.Parse( acc.GetTag( "DonationDuration" ) );

				if ( acc.Username == "magerin" )
				{
					Console.WriteLine( "Here it is: {0}", DonationStart );
				}
			}
			catch
			{
				return false;
			}

			return DateTime.Now - DonationStart < DonationDuration;
		}

		public static DateTime DonationStart( Account acc )
		{
			IFormatProvider culture = new CultureInfo("en-GB", true);
			DateTime DonationStart;
			try
			{
				DonationStart = DateTime.Parse( acc.GetTag( "DonationStart" ), culture );
				//donationstart = DateTime.Parse( acc.GetTag( "DonationStart" ) );
			}
			catch
			{
				DonationStart = DateTime.MinValue;
			}
			finally
			{

			}
			return DonationStart;
		}

		public static TimeSpan DonationDuration( Account acc )
		{

			TimeSpan donationduration = TimeSpan.Zero;
			try
			{
				donationduration = TimeSpan.Parse( acc.GetTag( "DonationDuration" ) );
			}
			catch
			{
				return TimeSpan.Zero;
			}
			return donationduration;
		}
	}
}