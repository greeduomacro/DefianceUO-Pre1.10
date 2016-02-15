#region AuthorHeader
//
//	EvoSystem version 1.6, by Xanthos
//
//
#endregion AuthorHeader
using System;

namespace Xanthos.Evo
{
	public class ShrinkConfig
	{
		public static readonly bool kPetAsStatuette = true;	// Deed or statuette form
		public static readonly bool kAllowLocking = false;	// Allow players to lock the shrunken pet or not
		public static readonly bool kShowPetDetails = true;	// Show stats and skills on the properties of the shrunken pet
		public static readonly double kShrunkenWeight = 25.0;
		public static readonly bool kBlessedLeash = true;
		public static readonly BlessStatus kBlessStatus = BlessStatus.None;	// How the shruken pet should be as loot
		public static readonly double kTamingRequired = 50;	// set to zero for no skill requirement to use shrink tools
		public static readonly int kShrinkCharges = 5;	// set to -1 for infinite uses

		// Add all pack animals for your server.
		public static readonly string[] kPackAnimals = new string []
		{
			"PackHorse",
			"PackLlama",
			"Beetle",
		};
	}

	public enum BlessStatus
	{
		All,		// All shrink items are blessed
		BondedOnly,	// Only shrink items for bonded pets are blessed
		None		// No shrink items are blessed
	}
}