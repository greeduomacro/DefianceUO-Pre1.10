using System;
using Server;

namespace Xanthos.Evo
{
	public sealed class HiryuEvoSpec : BaseEvoSpec
	{
		// This class implements a singleton pattern; meaning that no matter how many times the
		// Instance attribute is used, there will only ever be one of these created in the entire system.
		// Copy this template and give it a new name.  Assign all of the data members of the EvoSpec
		// base class in the constructor.  Your subclass must not be abstract.
		// Never call new on this class, use the Instance attribute to get the instance instead.

		HiryuEvoSpec()
		{
			m_Tamable = true;
			m_MinTamingToHatch = 99.9;
			m_AlwaysHappy = true;
			m_ProducesYoung = false;
			m_PregnancyTerm = 0.10;
			m_AbsoluteStatValues = false;

			m_Skills = new SkillName[4] { SkillName.MagicResist, SkillName.Tactics, SkillName.Wrestling, SkillName.Anatomy };
			m_MinSkillValues = new int[4] { 50, 50, 50, 15, };
			m_MaxSkillValues = new int[4] { 100, 110, 120, 110 };


			m_Stages = new BaseEvoStage[] { new HiryuStageOne(), new HiryuStageTwo(), new HiryuStageThree(),
											  new HiryuStageFour(), new HiryuStageFive(), new HiryuStageSix() };
		}

		// These next 2 lines  facilitate the singleton pattern.  In your subclass only change the
		// BaseEvoSpec class name to your subclass of BaseEvoSpec class name and uncomment both lines.
		public static HiryuEvoSpec Instance { get { return Nested.instance; } }
		class Nested { static Nested() { } internal static readonly HiryuEvoSpec instance = new HiryuEvoSpec();}
	}

	// Define a subclass of BaseEvoStage for each stage in your creature and place them in the
	// array in your subclass of BaseEvoSpec.  See the example classes for how to do this.
	// Your subclass must not be abstract.

	public class HiryuStageOne : BaseEvoStage
	{
		public HiryuStageOne()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 50000; EpMinDivisor = 10; EpMaxDivisor = 5; DustMultiplier = 20;
			BaseSoundID = 0x4FD;
			Hue = 2406;
			BodyValue = 201; ControlSlots = 3; MinTameSkill = 99.9; VirtualArmor = 30;

			DamagesTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinDamages = new int[1] { 100 };
			MaxDamages = new int[1] { 100 };

			ResistanceTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinResistances = new int[1] { 15 };
			MaxResistances = new int[1] { 15 };

			DamageMin = 11; DamageMax = 15; HitsMin = 1; HitsMax = 1;
			StrMin = 75; StrMax = 85; DexMin = 95; DexMax = 105; IntMin = 80; IntMax = 100;
		}
	}

	public class HiryuStageTwo : BaseEvoStage
	{
		public HiryuStageTwo()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 150000; EpMinDivisor = 20; EpMaxDivisor = 10; DustMultiplier = 20;
			BaseSoundID = 0x4FD;
			BodyValue = 217; VirtualArmor = 40;
			Hue = 2406;

			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 20, 20, 20, 20, 20 };
			MaxDamages = new int[5] { 20, 20, 20, 20, 20 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 25, 25, 25, 25, 25 };
			MaxResistances = new int[5] { 25, 25, 25, 25, 25 };

			DamageMin = 2; DamageMax = 2; HitsMin= 1; HitsMax = 1;
			StrMin = 65; StrMax = 75; DexMin = 40; DexMax = 45; IntMin = 40; IntMax = 50;
		}
	}

	public class HiryuStageThree : BaseEvoStage
	{
		public HiryuStageThree()
		{
			EvolutionMessage = "has evolved and will now only gain experience in dungeons and certain unsaid regions";
			NextEpThreshold = 20000000; EpMinDivisor = 30; EpMaxDivisor = 20; DustMultiplier = 20;
			BaseSoundID = 0x5A;
			Hue = 2402;
			BodyValue = 214; VirtualArmor = 50;

			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 20, 20, 20, 20 };
			MaxDamages = new int[5] { 100, 20, 20, 20, 20 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 35, 35, 35, 35, 35 };
			MaxResistances = new int[5] { 35, 35, 35, 35, 35 };

			DamageMin = 5; DamageMax = 5; HitsMin= 1; HitsMax = 1;
			StrMin = 25; StrMax = 35; DexMin = 40; DexMax = 45; IntMin = 35; IntMax = 40;
		}
	}

	public class HiryuStageFour : BaseEvoStage
	{
		public HiryuStageFour()
		{
			EvolutionMessage = "has evolved but is yet too young and wild to be ridden";
			NextEpThreshold = 40000000; EpMinDivisor = 50; EpMaxDivisor = 40; DustMultiplier = 20;
			BaseSoundID = 0x4FD;
			Hue = 2403;
			BodyValue = 204; ControlSlots = 3; MinTameSkill = 99.9; VirtualArmor = 85;

			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 40, 40, 40, 40, 40 };
			MaxResistances = new int[5] { 40, 40, 40, 40, 40 };

			DamageMin = 5; DamageMax = 5; HitsMin= 1; HitsMax = 1;
			StrMin = 35; StrMax = 45; DexMin = 30; DexMax = 40; IntMin = 90; IntMax = 100;
		}
	}

	public class HiryuStageFive : BaseEvoStage
	{
		public HiryuStageFive()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 80000000; EpMinDivisor = 60; EpMaxDivisor = 50; DustMultiplier = 20;
			BaseSoundID = 362; ControlSlots = 3;
			Hue = 2406;
			BodyValue = 0x319; VirtualArmor = 140;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 55, 70, 25, 40, 40 };
			MaxResistances = new int[5] { 70, 80, 45, 50, 50 };

			DamageMin = 10; DamageMax = 10; HitsMin= 50; HitsMax = 50;
			StrMin = 100; StrMax = 105; DexMin = 50; DexMax = 60; IntMin = 150; IntMax = 200;
                        MountID = 0x3EBB;
		}
	}

        public class HiryuStageSix : BaseEvoStage
	{
		public HiryuStageSix()
		{
			Title = "The lich steed";
			EvolutionMessage = "has evolved to its highest form and is now a Lich steed";
			NextEpThreshold = 0; EpMinDivisor = 160; EpMaxDivisor = 60; DustMultiplier = 20;
			BaseSoundID = 362; ControlSlots = 3;
			Hue = 1175;
			BodyValue = 0x11C; VirtualArmor = 180;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 55, 70, 25, 40, 40 };
			MaxResistances = new int[5] { 70, 80, 45, 50, 50 };

			DamageMin = 5; DamageMax = 5; HitsMin= 250; HitsMax = 325;
			StrMin = 30; StrMax = 40; DexMin = 15; DexMax = 25; IntMin = 50; IntMax = 60;
                        MountID = 0x3E92;
		}
	}
}