using System;
using Server;
using Server.Network;
using Server.Accounting;

namespace Server.Misc
{
	public class Profile
	{
		public static void Initialize()
		{
			EventSink.ProfileRequest += new ProfileRequestEventHandler( EventSink_ProfileRequest );
			EventSink.ChangeProfileRequest += new ChangeProfileRequestEventHandler( EventSink_ChangeProfileRequest );
		}

		public static void EventSink_ChangeProfileRequest( ChangeProfileRequestEventArgs e )
		{
			Mobile from = e.Beholder;

			if ( from.ProfileLocked )
				from.SendMessage( "Your profile is locked. You may not change it." );
			else
				from.Profile = e.Text;
		}

		public static void EventSink_ProfileRequest( ProfileRequestEventArgs e )
		{
			Mobile beholder = e.Beholder;
			Mobile beheld = e.Beheld;

			if ( !beheld.Player )
				return;

			if ( beholder.Map != beheld.Map || !beholder.InRange( beheld, 12 ) || !beholder.CanSee( beheld ) )
				return;

			string header = Titles.ComputeTitle( beholder, beheld );

			string footer = "";

			if ( beheld.ProfileLocked )
			{
				if ( beholder == beheld )
					footer = "Your profile has been locked.";
				else if ( beholder.AccessLevel >= AccessLevel.Counselor )
					footer = "This profile has been locked.";
			}

			if ( footer == "" && beholder == beheld )
				footer = GetAccountDuration( beheld );

			string body = beheld.Profile;

			if ( body == null || body.Length <= 0 )
				body = "";

			beholder.Send( new DisplayProfile( beholder != beheld || !beheld.ProfileLocked, beheld, header, body, footer ) );
		}

		private static string GetAccountDuration( Mobile m )
		{
			Account a = m.Account as Account;

			if ( a == null ) return "";

			TimeSpan ts = DateTime.Now - a.Created;

			//return String.Concat(Format(ts, "This account is {0} old."), Format(a.TotalGameTime, " The total game time is {0}."));
			return Format(ts, "This account is {0} old.");
		}

		public static bool Format(double value, string format, out string op)
		{
			if ( value >= 1.0 )
			{
				op = String.Format( format, (int)value, (int)value != 1 ? "s" : "" );
				return true;
			}

			op = null;
			return false;
		}

		public static string Format(TimeSpan ts, string format)
		{
			string v;
			if (Format(ts.TotalDays, "{0} day{1}", out v))
				return String.Format(format, v);
			if (Format(ts.TotalHours, "{0} hour{1}", out v))
				return String.Format(format, v);
			if (Format(ts.TotalMinutes, "{0} minute{1}", out v))
				return String.Format(format, v);
			if (Format(ts.TotalSeconds, "{0} second{1}", out v))
				return String.Format(format, v);
			return "";
		}
	}
}