using System;
using Server.Network;

namespace Server
{
	public class SE
	{
		public const bool Enabled = false;

//		private const int FLAGS_AOS = 0x801c;
//		private const int FLAGS_SE = 0x0040;
//		private const int FLAGS_ML_9TH_ANNI = 0x280;

		public static void Configure()
		{
			Core.SE = Enabled;
			Server.Network.SupportedFeatures.Value = 0x801c | 0x0040 | 0x280;
		}
	}
}