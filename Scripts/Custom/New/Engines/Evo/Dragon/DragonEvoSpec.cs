using System;
using Server;

namespace Xanthos.Evo
{
	public sealed class RaelisDragonSpec : BaseEvoSpec
	{
		// This class implements a singleton pattern; meaning that no matter how many times the
		// Instance attribute is used, there will only ever be one of these created in the entire system.
		// Copy this template and give it a new name.  Assign all of the data members of the EvoSpec
		// base class in the constructor.  Your subclass must not be abstract.
		// Never call new on this class, use the Instance attribute to get the instance instead.

		RaelisDragonSpec()
		{
			m_Tamable = true;
			m_MinTamingToHatch = 99.9;
			m_AlwaysHappy = true;
			m_ProducesYoung = false;
			m_PregnancyTerm = 0.10;
			m_AbsoluteStatValues = false;

			m_Skills = new SkillName[7] { SkillName.Magery, SkillName.EvalInt, SkillName.Meditation, SkillName.MagicResist,
										  SkillName.Tactics, SkillName.Wrestling, SkillName.Anatomy };
			m_MinSkillValues = new int[7] { 50, 50, 50, 15, 19, 19, 19 };
			m_MaxSkillValues = new int[7] { 100, 100, 100, 100, 120, 120, 100 };

			m_Stages = new BaseEvoStage[] { new RaelisDragonStageOne(), new RaelisDragonStageTwo(),
											  new RaelisDragonStageThree(), new RaelisDragonStageFour(),
											  new RaelisDragonStageFive(), new RaelisDragonStageSix(),
											  new RaelisDragonStageSeven() };
		}

		// These next 2 lines  facilitate the singleton pattern.  In your subclass only change the
		// BaseEvoSpec class name to your subclass of BaseEvoSpec class name and uncomment both lines.
		public static RaelisDragonSpec Instance { get { return Nested.instance; } }
		class Nested { static Nested() { } internal static readonly RaelisDragonSpec instance = new RaelisDragonSpec();}
	}

	// Define a subclass of BaseEvoStage for each stage in your creature and place them in the
	// array in your subclass of BaseEvoSpec.  See the example classes for how to do this.
	// Your subclass must not be abstract.

	public class RaelisDragonStageOne : BaseEvoStage
	{
		public RaelisDragonStageOne()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 75000; EpMinDivisor = 10; EpMaxDivisor = 5; DustMultiplier = 20;
			BaseSoundID = 0xDB;
			BodyValue = 52; ControlSlots = 2; MinTameSkill = 99.9; VirtualArmor = 25;

			DamagesTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinDamages = new int[1] { 100 };
			MaxDamages = new int[1] { 100 };

			ResistanceTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinResistances = new int[1] { 15 };
			MaxResistances = new int[1] { 15 };

			DamageMin = 11; DamageMax = 17; HitsMin = 250; HitsMax = 350;
			StrMin = 50; StrMax = 60; DexMin = 56; DexMax = 75; IntMin = 26; IntMax = 36;
		}
	}

	public class RaelisDragonStageTwo : BaseEvoStage
	{
		public RaelisDragonStageTwo()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 175000; EpMinDivisor = 20; EpMaxDivisor = 40; DustMultiplier = 20;
			BaseSoundID = 219;
			BodyValue = 89; VirtualArmor = 30;

			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 25, 25, 25, 25 };
			MaxDamages = new int[5] { 100, 25, 25, 25, 25 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 20, 20, 20, 20, 20 };
			MaxResistances = new int[5] { 20, 20, 20, 20, 20 };

			DamageMin = 1; DamageMax = 1; HitsMin= 500; HitsMax = 550;
			StrMin = 60; StrMax = 70; DexMin = 20; DexMax = 30; IntMin = 10; IntMax = 15;
		}
	}

	public class RaelisDragonStageThree : BaseEvoStage
	{
		public RaelisDragonStageThree()
		{
			EvolutionMessage = "has evolved and will now only gain experience in dungeons and certain unsaid regions";
			NextEpThreshold = 25000000; EpMinDivisor = 30; EpMaxDivisor = 50; DustMultiplier = 20;
			BaseSoundID = 0x5A;
			BodyValue = 0xCE; VirtualArmor = 50;

			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 40, 40, 40, 40, 40 };
			MaxResistances = new int[5] { 40, 40, 40, 40, 40 };

			DamageMin = 1; DamageMax = 1; HitsMin= 100; HitsMax = 125;
			StrMin = 70; StrMax = 75; DexMin = 10; DexMax = 15; IntMin = 10; IntMax = 15;
		}
	}

	public class RaelisDragonStageFour : BaseEvoStage
	{
		public RaelisDragonStageFour()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 50000000; EpMinDivisor = 50; EpMaxDivisor = 40; DustMultiplier = 20;
			BaseSoundID = 362;
			BodyValue = 60; ControlSlots = 3; MinTameSkill = 100.0; VirtualArmor = 60;

			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 60, 60, 60, 60, 60 };
			MaxResistances = new int[5] { 60, 60, 60, 60, 60 };

			DamageMin = 1; DamageMax = 1; HitsMin= 100; HitsMax = 125;
			StrMin = 80; StrMax = 90; DexMin = 10; DexMax = 10; IntMin = 20; IntMax = 30;
		}
	}

	public class RaelisDragonStageFive : BaseEvoStage
	{
		public RaelisDragonStageFive()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 75000000; EpMinDivisor = 160; EpMaxDivisor = 40; DustMultiplier = 20;
			BodyValue = 59; VirtualArmor = 75;

			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 50, 50, 50, 50 };
			MaxDamages = new int[5] { 100, 50, 50, 50, 50 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 80, 80, 80, 80, 80 };
			MaxResistances = new int[5] { 80, 80, 80, 80, 80 };

			DamageMin = 5; DamageMax = 5; HitsMin= 110; HitsMax = 90;
			StrMin = 95; StrMax = 100; DexMin = 20; DexMax = 20; IntMin = 40; IntMax = 50;
		}
	}

	public class RaelisDragonStageSix : BaseEvoStage
	{
		public RaelisDragonStageSix()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 135000000; EpMinDivisor = 540; EpMaxDivisor = 480; DustMultiplier = 20;
			BodyValue = 106; VirtualArmor = 100; Hue = 16385;

			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 98, 98, 98, 98, 98 };
			MaxResistances = new int[5] { 98, 98, 98, 98, 98 };

			DamageMin = 10; DamageMax = 10; HitsMin= 165; HitsMax = 100;
			StrMin = 100; StrMax = 100; DexMin = 20; DexMax = 20; IntMin = 55; IntMax = 60;
		}
	}

	public class RaelisDragonStageSeven : BaseEvoStage
	{
		public RaelisDragonStageSeven()
		{
			Title = "The Ancient Dragon";
			EvolutionMessage = "has evolved to its highest form and is now an Ancient Dragon";
			NextEpThreshold = 0; EpMinDivisor = 740; EpMaxDivisor = 660; DustMultiplier = 20;
			BaseSoundID = 362;
			BodyValue = 46; ControlSlots = 4; VirtualArmor = 150; Hue = 16385;

			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 75, 75, 75, 75 };
			MaxDamages = new int[5] { 100, 75, 75, 75, 75 };

			ResistanceTypes = null;
			MinResistances = null;
			MaxResistances = null;

			DamageMin = 10; DamageMax = 10; HitsMin= 550; HitsMax = 750;
			StrMin = 110; StrMax = 120; DexMin = 125; DexMax = 135; IntMin = 70; IntMax = 80;
		}
	}
}