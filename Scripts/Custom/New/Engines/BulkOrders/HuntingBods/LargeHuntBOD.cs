using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
    public class LargeHuntBOD : LargeBOD
    {
        public int Level { get { return (int)this.Material; } }

        public override int ComputeFame()
        {
            return 100 + Utility.Random(20 * (Level + 1));
        }

        public override int ComputeGold()
        {
            return 3000 + Level * 2000 + AmountMax * 40 + this.Entries.Length * 100 + Utility.Random(500);
        }

        public override ArrayList ComputeRewards()
        {
            ArrayList list = new ArrayList();

            double psChance = 0;
            int iPSMin = 0;
            int iPSMax = 0;
            int itemAmount = (Level + 1) * this.Entries.Length;

            switch (Level)
            {
                default:
                case 0: // Easy
                    psChance = 0.03;
                    iPSMin = 5;
                    iPSMax = 5;
                    break;
                case 1: // Medium
                    psChance = 0.05;
                    iPSMin = 5;
                    iPSMax = 5;
                    break;
                case 2: // Hard
                    psChance = 0.10;
                    iPSMin = 5;
                    iPSMax = 5;
                    break;
                case 3: // Very Hard
                    psChance = 0.20;
                    iPSMin = 10;
                    iPSMax = 10;
                    break;
            }

            psChance += this.AmountMax / 200.0;
            psChance += this.Entries.Length / 60.0;

            Container cont = new Bag();

            int minProp = Level + 3;
            if (minProp > 5)
                minProp = 5;

            //EUtility.AddRandomLoot(cont, (Level + 1) * 3, (int)(Level * 300 + AmountMax / 20.0 * 150 + Entries.Length / 6.0 * 150), ScaleTypes.PlayerLuck, minProp, 5, 50, 100);

            cont.DropItem(HuntBodUtility.GetLargeRewardItem(Level));

            if (psChance >= Utility.RandomDouble() && iPSMin > 0 && iPSMax > 0)
                cont.DropItem(PowerScroll.CreateRandomNoCraft(iPSMin, iPSMax));

            list.Add(cont);

            return list;
        }

        public static LargeHuntBOD CreateRandomFor(Mobile m, double skill)
        {
            int curLevel = 0;
            int levelMax = 0;
            int amountMax = 0;

            HuntBodUtility.GetLargeBodProps(skill, out levelMax, out amountMax);

            LargeHuntBOD largeBod = new LargeHuntBOD(amountMax, false, 0, null);

            largeBod.Entries = LargeBulkEntry.ConvertEntries(largeBod, HuntBodUtility.GetLargeEntry(out curLevel, levelMax));
            largeBod.Material = (BulkMaterialType)curLevel;

            return largeBod;
        }

        [Constructable]
        public LargeHuntBOD()
        {
            int curLevel = 0;
            int levelMax = 0;
            int amountMax = 0;

            HuntBodUtility.GetLargeBodProps(Utility.RandomMinMax(80, 120), out levelMax, out amountMax);
            LargeBulkEntry[] entries = LargeBulkEntry.ConvertEntries(this, HuntBodUtility.GetLargeEntry(out curLevel, levelMax));

            this.Hue = HuntBodUtility.HuntBodDeedHue;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = false;
            this.Material = (BulkMaterialType)curLevel;
        }

        public LargeHuntBOD(int amountMax, bool reqExceptional, int level, LargeBulkEntry[] entries)
        {
            this.Hue = HuntBodUtility.HuntBodDeedHue;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = (BulkMaterialType)level;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1042971, string.Format("Difficulty Level: {0}", HuntBodUtility.GetDifficultyLevel(Level))); // ~1_NOTHING~
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) && !BaseHuntContractVendor.CanUseContract(from))
            {
                from.SendMessage("You need to have atleast {0} in a figthing skill to use this.", BaseHuntContractVendor.SkillNeeded);
                return;
            }

            base.OnDoubleClick(from);
        }

        public LargeHuntBOD(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}