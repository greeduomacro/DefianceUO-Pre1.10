using System;
using Server;

namespace Server.Items
{
	public enum PlantColors
	{
	Red = 0x66D,
	BrightRed = 0x21,
	Blue = 0x53D,
	BrightBlue = 0x5,
	Yellow = 0x8A5,
	BrightYellow = 0x38,
	Plain = 0,
	Purple = 0xD,
	BrightPurple = 0x10,
	Orange = 0x46F,
	BrightOrange = 0x2B,
	Green = 0x59B,
	BrightGreen = 0x42,
	Black = 0x455,
	White = 0x481,
	RareMagenta = 0x486,
	RareAqua = 0x495,
	RarePink = 0x48E,
	RareFireRed = 0x489
	}

	public struct PlantProps
	{
		public static TimeSpan GrowthCheck = TimeSpan.FromHours(24); //Every how often a growth check occurs

		public static bool Bright(int Hue)
		{
		return (Hue == (int)PlantColors.BrightRed || Hue == (int)PlantColors.BrightBlue || Hue == (int)PlantColors.BrightYellow || Hue == (int)PlantColors.BrightPurple || Hue == (int)PlantColors.BrightGreen || Hue == (int)PlantColors.BrightOrange );
		}

		public static bool FatPlant(int SeedType)
		{
		return (SeedType == 2 || SeedType == 5 || SeedType == 6 || SeedType == 7 || SeedType == 8);
		}

		public static string Color( int Hue )
		{
		string color;
		switch (Hue)
			{
			case (int)PlantColors.Plain: color = "#1060813"; break; // plain
			case (int)PlantColors.Red: color = "#1060814"; break; // red
			case (int)PlantColors.BrightRed: color = "#1060814"; break; // red
			case (int)PlantColors.Blue: color = "#1060815"; break; // blue
			case (int)PlantColors.BrightBlue: color = "#1060815"; break; // blue
			case (int)PlantColors.Purple: color = "#1060816"; break; // purple
			case (int)PlantColors.BrightPurple: color = "#1060816"; break; // purple
			case (int)PlantColors.Orange: color = "#1060817"; break; // orange
			case (int)PlantColors.BrightOrange: color = "#1060817"; break; // orange
			case (int)PlantColors.Yellow: color = "#1060818"; break; // yellow
			case (int)PlantColors.BrightYellow: color = "#1060818"; break; // yellow
			case (int)PlantColors.Green: color = "#1060819"; break; // green
			case (int)PlantColors.BrightGreen: color = "#1060819"; break; // green
			case (int)PlantColors.Black: color = "#1060820"; break; // black
			case (int)PlantColors.White: color = "#1060821"; break; // white
			case (int)PlantColors.RareMagenta: color = "#1061852"; break; // rare magenta
			case (int)PlantColors.RareAqua: color = "#1061853"; break; // rare aqua
			case (int)PlantColors.RarePink: color = "#1061854"; break; // rare pink
			case (int)PlantColors.RareFireRed: color = "#1061855"; break; // rare fire red
			default: color = "Error!"; break;
			}
		return color;
		}

		public static string PlantName(int SeedType)
		{
		string name;
		switch ( SeedType + 8 )
			{
			case 0: name = "#1023209"; break; // campion flowers 1st
			case 1: name = "#1023262"; break; // poppies
			case 2: name = "#1023214"; break; // snowdrops
			case 3: name = "#1023220"; break; // bulrushes
			case 4: name = "#1023211"; break; // lilies
			case 5: name = "#1023237"; break; // pampas grass
			case 6: name = "#1023239"; break; // rushes
			case 7: name = "#1023223"; break; // elephant ear plant
			case 8: name = "#1023234"; break; // fern 1st
			case 9: name = "#1023238"; break; // ponytail palm
			case 10: name = "#1023229"; break; // small palm
			case 11: name = "#1023376"; break; // century plant
			case 12: name = "#1023332"; break; // water plant
			case 13: name = "#1023241"; break; // snake plant
			case 14: name = "#1023372"; break; // prickly pear cactus
			case 15: name = "#1023366"; break; // barrel cactus
			case 16: name = "#1023367"; break; // tribarrel cactus 1st
			default: name = "Error!"; break;
			}
		return name;
		}

		public static string Stages( int stage )
		{
		if (stage == 0)
			return "#1026951"; // dirt
		else if (stage < 2)
			return "#1060810"; // seed
		else if (stage > 1 && stage < 4)
			return "#1023305"; // sapling
		else
			return "#1060812"; // plant
		}

	}
}