using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Engines.Quests;
using Xanthos.Evo;
using Server.EventPrizeSystem;

namespace Server.Engines.BulkOrders
{
    public class HuntBodUtility
    {
        public static int HuntBodDeedHue = 0x1B0;

        public class HuntBodTarget : Target
        {
            private SmallBOD m_deed;

            public HuntBodTarget(SmallBOD deed)
                : base(18, false, TargetFlags.None)
            {
                m_deed = deed;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (m_deed == null || m_deed.Deleted || !m_deed.IsChildOf(from.Backpack) || !from.CanSee(o))
                    return;

                if (o is Corpse && !((Corpse)o).Deleted)
                {
                    Corpse corpse = (Corpse)o;

                    if (!from.InRange(corpse.Location, 4))
                    {
                        from.SendLocalizedMessage(500446); // That is too far away.
                        return;
                    }

                    if (CheckCorpseType(from, corpse, m_deed) && CanUseWithDeed(from, corpse) && CheckUseWithDeedTime(from, corpse))
                    {
                        corpse.Delete();
                        m_deed.AmountCur++;
                        from.SendMessage("The corpse has been combined with the deed.");

                        from.SendGump(new SmallBODGump(from, m_deed));
                    }
                }
                else
                    from.SendLocalizedMessage(1042600); // That is not a corpse!
            }

            public static bool CheckCorpseType(Mobile from, Corpse corpse, SmallBOD deed)
            {
                if (corpse != null && corpse.Owner != null)
                {
                    Type corpseOwnerType = corpse.Owner.GetType();

                    if (corpseOwnerType != null && deed.Type != null && corpseOwnerType == deed.Type)
                        return true;
                    else
                        from.SendMessage("The creature is not in the request.");
                }
                return false;
            }

            public static bool CheckUseWithDeedTime(Mobile from, Corpse corpse)
            {
                DateTime dtCannAdd = corpse.TimeOfDeath + TimeSpan.FromSeconds(30.0);
                if (dtCannAdd > DateTime.Now)
                {
                    TimeSpan ts = dtCannAdd - DateTime.Now;

                    if (ts < TimeSpan.Zero)
                        ts = TimeSpan.Zero;

                    from.SendMessage("This corpse cannot be added yet, try again in {0} seconds.", ts.Seconds);
                    return false;
                }
                else return true;
            }

            public static bool CanUseWithDeed(Mobile from, Corpse corpse)
            {
                if (corpse.IsCriminalAction(from))
                {
                    from.SendMessage("You did not earn the right to use this creature in the deed.");
                    return false;
                }
                else return true;
            }
        }

        public static void GetSmallBodProps(double skill, out int levelMax, out int amountMax)
        {
            levelMax = 0;
            amountMax = 0;

            // *** Very Hard***
            if (skill >= 100)
            {
                levelMax = 4;
                amountMax = Utility.RandomList(10, 15, 20, 20);
            }
            // *** Hard ***
            else if (skill >= 90)
            {
                levelMax = 3;
                amountMax = Utility.RandomList(10, 15, 15, 20);
            }
            // *** Medium ***
            else if (skill >= 80)
            {
                levelMax = 2;
                amountMax = Utility.RandomList(10, 10, 15, 20);
            }
            // *** Easy ***
            else
            {
                levelMax = 1;
                amountMax = Utility.RandomList(10, 10, 15, 20);
            }
        }

        public static void GetLargeBodProps(double skill, out int levelMax, out int amountMax)
        {
            levelMax = 0;
            amountMax = 0;

            // *** Very Hard***
            if (skill >= 100)
            {
                levelMax = 4;
                amountMax = Utility.RandomList(10, 15, 20, 20);
            }
            // *** Hard ***
            else if (skill >= 90)
            {
                levelMax = 3;
                amountMax = Utility.RandomList(10, 15, 15, 20);
            }
            // *** Medium ***
            else if (skill >= 80)
            {
                levelMax = 2;
                amountMax = Utility.RandomList(10, 10, 15, 20);
            }
            // *** Easy ***
            else
            {
                levelMax = 1;
                amountMax = Utility.RandomList(10, 10, 15, 20);
            }
        }

		public static readonly Type[][] SmallRewardItems =
			{
				new Type[] { //Easy Difficulty
				        typeof(BagOfReagents),
					typeof(BagOfReagents)
				},
				new Type[] { //Medium Difficulty
				        typeof(VialOfAgilitySmall),
					typeof(VialOfIntellectSmall),
					typeof(VialOfLifeSmall)
				},
				new Type[] { //Hard Difficulty
				        typeof(RottenEvoCarrot),
					typeof(CharredEvoMeat),
					typeof(ArmorKitSmall),
					typeof(ArmorKitSmall)
				},
				new Type[] { //Very Hard Difficulty
					typeof(BottleOfLifeMedium),
					typeof(BottleOfIntellectMedium),
					typeof(BottleOfAgilityMedium),
					typeof(TreasureMap),
					typeof(TreasureMap)
				}
			};

        public static Item GetSmallRewardItem(int level)
        {
            Type[] itemtypes = SmallRewardItems[level];
			Type itemType = itemtypes[Utility.Random( itemtypes.Length )];

            Item item = null;

            if (itemType == typeof(TreasureMap))
                item = new TreasureMap(level + Utility.RandomMinMax(2, 3), Map.Felucca);
            else
                item = (Item)Activator.CreateInstance(itemType);

            return item;
        }

		public static readonly Type[][] LargeRewardItems =
			{
				new Type[] { //Easy Difficulty
					typeof(BottleOfLifeMedium),
					typeof(BottleOfIntellectMedium),
					typeof(BottleOfAgilityMedium),
					typeof(TenderEvoMeat),
					typeof(FreshEvoCarrot)
				},
				new Type[] { //Medium Difficulty
			                typeof(ArmorKitMedium),
                                        typeof(ArmorKitMedium),
				        typeof(ArmorKitMedium),
			                typeof(RackOfIntellectLarge),
					typeof(RackOfAgilityLarge),
					typeof(RackOfLifeLarge)
				},
				new Type[] { //Hard Difficulty
					typeof(LusciousEvoCarrot),
					typeof(EnrichedEvoMeat),
					typeof(ArmorKitLarge),
					typeof(PetSkillBall),
					typeof(GracedPetSummonBall)
				},
				new Type[] { //Very Hard Difficulty
				        typeof(HiryuEvoEgg),
					typeof(RaelisDragonEgg)
				}
			};

        public static Item GetLargeRewardItem(int level)
        {
            Type[] itemtypes = LargeRewardItems[level];
			Type itemType = itemtypes[Utility.Random( itemtypes.Length )];

            Item item = (Item)Activator.CreateInstance(itemType);

            return item;
        }

        public static Container GetNewContainer()
        {
            Bag bag = new Bag();
            bag.Hue = QuestSystem.RandomBrightHue();
            return bag;
        }

        public static string GetDifficultyLevel(int level)
        {
            switch (level)
            {
                default: return "Easy";
                case 1: return "Medium";
                case 2: return "Hard";
                case 3: return "Very Hard";
            }
        }

        public static SmallBulkEntry[] GetSmallEntry(out int curLevel, int levelMax)
        {
            int iRand = Utility.Random(levelMax);
            curLevel = iRand;

            switch (iRand)
            {
                default:
                case 0: return SmallBulkEntry.GetEntries("Hunting", "smalleasy");
                case 1: return SmallBulkEntry.GetEntries("Hunting", "smallmedium");
                case 2: return SmallBulkEntry.GetEntries("Hunting", "smallhard");
                case 3: return SmallBulkEntry.GetEntries("Hunting", "smallveryhard");
            }
        }

        private static int GetLargeCurLevel(int value)
        {
            if (value < m_iLargeLvl1Amount)
                return 0;
            else if (value < m_iLargeLvl2Amount)
                return 1;
            else if (value < m_iLargeLvl3Amount)
                return 2;
            else if (value < m_iLargeLvl4Amount)
                return 3;

            return 0;
        }

        private static int GetLargeLevelAmount(int levelMax)
        {
            switch (levelMax)
            {
                case 1: return m_iLargeLvl1Amount;
                case 2: return m_iLargeLvl2Amount;
                case 3: return m_iLargeLvl3Amount;
                case 4: return m_iLargeLvl4Amount;
            }

            return 0;
        }

        private static int m_iLargeLvl1Amount = 10;
        private static int m_iLargeLvl2Amount = 19;
        private static int m_iLargeLvl3Amount = 22;
        private static int m_iLargeLvl4Amount = 23;

        public static SmallBulkEntry[] GetLargeEntry(out int curLevel, int levelMax)
        {
            int iRand = Utility.Random(GetLargeLevelAmount(levelMax));

            curLevel = GetLargeCurLevel(iRand);

            switch (iRand)
            {
                default:
              /*// Easy
                case 0: return SmallBulkEntry.GetEntries("Hunting", "largeorc");
                case 1: return SmallBulkEntry.GetEntries("Hunting", "largeratmen");
                case 2: return SmallBulkEntry.GetEntries("Hunting", "largesavage");
                case 3: return SmallBulkEntry.GetEntries("Hunting", "largeserpent");
                case 4: return SmallBulkEntry.GetEntries("Hunting", "largeskeleton");
                case 5: return SmallBulkEntry.GetEntries("Hunting", "largesnake");
                case 6: return SmallBulkEntry.GetEntries("Hunting", "largeundead");
                case 7: return SmallBulkEntry.GetEntries("Hunting", "largespider");
                case 8: return SmallBulkEntry.GetEntries("Hunting", "largeplant");
                case 9: return SmallBulkEntry.GetEntries("Hunting", "largeharpy");

                // Medium
                case 10: return SmallBulkEntry.GetEntries("Hunting", "largedragon");
                case 11: return SmallBulkEntry.GetEntries("Hunting", "largeophidian");
                case 12: return SmallBulkEntry.GetEntries("Hunting", "largeelemental");
                case 13: return SmallBulkEntry.GetEntries("Hunting", "largejuka");
                case 14: return SmallBulkEntry.GetEntries("Hunting", "largegargoyle");
                case 15: return SmallBulkEntry.GetEntries("Hunting", "largemeer");
                case 16: return SmallBulkEntry.GetEntries("Hunting", "largeogre");
                case 17: return SmallBulkEntry.GetEntries("Hunting", "largeredsolen");
                case 18: return SmallBulkEntry.GetEntries("Hunting", "largeblacksolen");

                // Hard
                case 19: return SmallBulkEntry.GetEntries("Hunting", "largewyrm");
                case 20: return SmallBulkEntry.GetEntries("Hunting", "largegargoylehard");
              */
                case 21: return SmallBulkEntry.GetEntries("Hunting", "largedoom");
                case 22: return SmallBulkEntry.GetEntries("Hunting", "largeelementalhard");

                // Very Hard
                case 23: return SmallBulkEntry.GetEntries("Hunting", "largeveryhard");
            }
        }

        private static SkillName[] snFightingSkills = new SkillName[]
				{
					SkillName.Archery,
					SkillName.Fencing,
					SkillName.Macing,
					SkillName.Magery,
					SkillName.Necromancy,
                                        SkillName.AnimalTaming,
					SkillName.Swords,
                                        SkillName.Provocation
				};

        public static SkillName GetBestFightingSkill(Mobile from)
        {
            double iHighestSkill = 0;
            SkillName skillName = SkillName.Archery;

            for (int i = 0; i < snFightingSkills.Length; i++)
                if (from.Skills[snFightingSkills[i]].Base > iHighestSkill)
                {
                    iHighestSkill = from.Skills[snFightingSkills[i]].Base;
                    skillName = snFightingSkills[i];
                }

            return skillName;
        }
    }
}