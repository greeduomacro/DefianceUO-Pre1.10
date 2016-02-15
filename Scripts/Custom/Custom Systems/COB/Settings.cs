// WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING
// WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING
// WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING
//
// This is the new settings file, please be careful when changing options,
// For now (version 1.8d and above) is just experimenting with the system to
// see how well it does, im pretty sure it will hold up, but just checking :)

using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Misc
{
	public class TokenSettings
	{
		// Set the colour of your tokens here
		// Default is my "purple" ones 1170
		// You need to find the Hue code for the colour you want
		public static int Token_Colour=2413;

		// What % distrobution would you like on the loot? (25 default)
		// i.e a lich will drop 750-1000 tokens, at 25% difference.
		// bigger the number the bigger the %, remember no more than 100!
		public static int Loot_Difference=25;

		// Use Extended Currency Converter? (default yes)
		// Yes : uses Dupre's Extended Currency format "100k"
		// No : uses the standard format "100000"
		public static string Currency_Format="yes";

		// Places.. represents the number of decimal places
		// to use for Extended Currency
		// 1 = 1.1, 2 = 1.22, 3 = 1.333 and so on...
		public static int Places_Thousand=1;
		public static int Places_Million=3;

		// Use Gump for Abacus? (default yes)
		// Yes : uses a Gump
		// No : uses a Command Line
		public static string Use_Abacus_Gump="yes";

		// For DeBugging purposes ONLY!
		// DO NOT CHANGE THIS or i wont be able to help
		// with any problems you encounter.
		/************************************************************************************************/
		/* DO NOT CHANGE THIS LINE*/public static string Token_Version="1.8i"; /*DO NOT CHANGE THIS LINE*/
		/************************************************************************************************/
	}
}