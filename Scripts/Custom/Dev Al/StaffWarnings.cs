using System;
using Server.Network;

namespace Server.Misc
{
	public class StaffWarnings
	{
		public const int WARNING_HUE = 33;
		public const int WARNING_FONT = 0x0;

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler(EventSink_Login);
		}

		public static void StaffWarning(Mobile m, AccessLevel minAccessLevel, string text)
		{
			if (m != null && m.NetState != null && m.AccessLevel >= minAccessLevel)
			{
				m.NetState.Send(new AsciiMessage(Serial.MinusOne, -1, 0, WARNING_HUE, WARNING_FONT, "System", "*************************"));
				m.NetState.Send(new AsciiMessage(Serial.MinusOne, -1, 0, WARNING_HUE, WARNING_FONT, "System", "* Staff Warning: "));
				m.NetState.Send(new AsciiMessage(Serial.MinusOne, -1, 0, WARNING_HUE, WARNING_FONT, "System", String.Concat("* ", text)));
				//m.NetState.Send(new AsciiMessage(Serial.MinusOne, -1, 0, WARNING_HUE, WARNING_FONT, "System", "*************************"));
			}
		}

		private static void EventSink_Login(LoginEventArgs args)
		{
			if (args.Mobile == null || args.Mobile.AccessLevel < AccessLevel.Counselor) return;
			Mobile m = args.Mobile;

			if (TestCenter.Enabled)
				StaffWarning(m, AccessLevel.Counselor, "TestCenter is true!");

			if (Donation.Donation.ClaimDonationsBlocked)
				StaffWarning(m, AccessLevel.Administrator, "ClaimDonations is blocked!");

			if (!AutoSave.SavesEnabled)
				StaffWarning(m, AccessLevel.Administrator, "Worldsaves are disabled!");

		}
	}
}