using System;
using Server;
using Server.Scripts.Commands;
using Server.Mobiles;

namespace Server.Misc
{
	public class Fastwalk
	{
		private const int  MaxSteps = 6;			// Maximum number of queued steps until fastwalk is detected, default: 4
		private const bool Enabled = true;			// Is fastwalk detection enabled?
		private const bool UOTDOverride = true;	// Should UO:TD clients not be checked for fastwalk?
		private const AccessLevel AccessOverride = AccessLevel.GameMaster; // Anyone with this or higher access level is not checked for fastwalk

		public static void Initialize()
		{
			Mobile.FwdMaxSteps = MaxSteps;
			Mobile.FwdEnabled = Enabled;
			Mobile.FwdUOTDOverride = UOTDOverride;
			Mobile.FwdAccessOverride = AccessOverride;

			if ( Enabled )
				EventSink.FastWalk += new FastWalkEventHandler( OnFastWalk );
		}

		public static void OnFastWalk( FastWalkEventArgs e )
		{
			e.Blocked = true;//disallow this fastwalk
			PlayerMobile pm = e.NetState.Mobile as PlayerMobile;
			if (pm != null && pm.CheckFastWalk())
				CommandHandlers.BroadcastMessage(AccessLevel.Seer, 0x482, String.Format("Possible tile skip ({0}) detected by: {1}.", pm.FastWalkCount, CommandLogging.Format(pm)) );
		}
	}
}