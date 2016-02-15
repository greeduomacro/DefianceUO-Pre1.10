using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
    public class SmallHuntBOD : SmallBOD
    {
        public int Level { get { return (int)this.Material; } }

        public override int ComputeFame()
        {
            return 10 + Utility.Random((Level + 1) * 10);
        }

        public override int ComputeGold()
        {
            return (Level + 1) * 500 + AmountMax * 10 + Utility.Random(500);
        }

        public override ArrayList ComputeRewards()
        {
            ArrayList list = new ArrayList();

            Container cont = new Bag();

            int minProp = Level + 1;
            if (minProp > 3)
                minProp = 3;

            //EUtility.AddRandomLoot(cont, (Level + 1) * 3, (int)(Level * 300 + AmountMax / 20.0 * 300), ScaleTypes.PlayerLuck, minProp, 5, 10, 100);

            cont.DropItem(HuntBodUtility.GetSmallRewardItem(Level));

            list.Add(cont);

            return list;
        }

        public static SmallHuntBOD CreateRandomFor(Mobile m, double skill)
        {
            int curLevel = 0;
            int levelMax = 0;
            int amountMax = 0;

            HuntBodUtility.GetSmallBodProps(skill, out levelMax, out amountMax);
            SmallBulkEntry[] entries = HuntBodUtility.GetSmallEntry(out curLevel, levelMax);

            return new SmallHuntBOD(entries[Utility.Random(entries.Length)], curLevel, amountMax, false);
        }

        private SmallHuntBOD(SmallBulkEntry entry, int level, int amountMax, bool reqExceptional)
        {
            this.Hue = HuntBodUtility.HuntBodDeedHue;
            this.AmountMax = amountMax;
            this.Type = entry.Type;
            this.Number = entry.Number;
            this.Graphic = entry.Graphic;
            this.RequireExceptional = reqExceptional;
            this.Material = (BulkMaterialType)level;
        }

        [Constructable]
        public SmallHuntBOD()
        {
            int curLevel = 0;
            int levelMax = 0;
            int amountMax = 0;

            HuntBodUtility.GetSmallBodProps(Utility.RandomMinMax(80, 120), out levelMax, out amountMax);
            SmallBulkEntry[] entries = HuntBodUtility.GetSmallEntry(out curLevel, levelMax);

            if (entries.Length > 0)
            {
                SmallBulkEntry entry = entries[Utility.Random(entries.Length)];

                this.Hue = HuntBodUtility.HuntBodDeedHue;
                this.AmountMax = amountMax;
                this.Type = entry.Type;
                this.Number = entry.Number;
                this.Graphic = entry.Graphic;
                this.RequireExceptional = false;
                this.Material = (BulkMaterialType)curLevel;
            }
        }

        public SmallHuntBOD(int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional, int level)
        {
            this.Hue = HuntBodUtility.HuntBodDeedHue;
            this.AmountMax = amountMax;
            this.AmountCur = amountCur;
            this.Type = type;
            this.Number = number;
            this.Graphic = graphic;
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

        public SmallHuntBOD(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}