using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a royal lizard corpse")]
    public class LizardDuke : BaseCreature
    {
        [Constructable]
        public LizardDuke()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Duke of Lizards";
            Body = 35;
            BaseSoundID = 417;
            Hue = 1644;
            Kills = 100;
            SetStr(732, 789);
            SetDex(191, 201);
            SetInt(391, 435);
            SetHits(3750, 4250);
            SetDamage(25, 28);
            SetSkill(SkillName.EvalInt, 120.1, 135.0);
            SetSkill(SkillName.Magery, 115.1, 120.0);
            SetSkill(SkillName.Meditation, 120.2, 150.0);
            SetSkill(SkillName.Poisoning, 130.1, 150.0);
            SetSkill(SkillName.MagicResist, 75.2, 90.0);
            SetSkill(SkillName.Tactics, 115.1, 135.0);
            SetSkill(SkillName.Wrestling, 110.1, 125.0);
            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 200;
            PackGold(400, 700);
            PackGem();
            PackGem();
            PackScroll(1, 7);
            PackSlayer();

            switch (Utility.Random(2))
            {
                case 0: PackWeapon(0, 5); break;
                case 1: PackArmor(0, 5); break;
            }

            switch (Utility.Random(2))
            {
                case 0: PackWeapon(0, 5); break;
                case 1: PackArmor(0, 5); break;
            }

            switch (Utility.Random(5))
            {
                case 0: PackWeapon(0, 5); break;
                case 1: PackArmor(0, 5); break;
            }

            switch (Utility.Random(15))
            {
                case 0: PackWeapon(1, 5); break;
                case 1: PackArmor(1, 5); break;
            }

            switch (Utility.Random(125))
            {
                case 0: PackItem(new BattleStandard()); break;
            }
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override double HitPoisonChance { get { return 0.75; } }
        public override int TreasureMapLevel { get { return 5; } }

        public LizardDuke(Serial serial)
            : base(serial)
        {

        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)from;

                if (bc.Controlled || bc.BardTarget == this)
                    damage /= 3;
            }
        }

        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (to is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)to;

                if (bc.Controlled || bc.BardTarget == this)
                    damage *= 5;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}