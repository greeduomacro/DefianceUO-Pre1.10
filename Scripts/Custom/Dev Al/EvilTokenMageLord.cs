using System;
using Server;
using Server.Items;
using Server.EventPrizeSystem;

namespace Server.Mobiles
{
    class EvilTokenMageLord : EvilMageLord
    {

        public EvilTokenMageLord(int hue, string title) : base()
        {
            Name = NameList.RandomName("golem controller"); //'evil mage lord' also contains a title
            Title = String.Format("the {0} Wizard", title);

            Robe robe = FindItemOnLayer(Layer.OuterTorso) as Robe;
            if (robe != null) { robe.Hue = hue; robe.LootType = LootType.Newbied; }

            WizardsHat hat = FindItemOnLayer(Layer.Helm) as WizardsHat;
            if (hat != null) { hat.Hue = hue; hat.LootType = LootType.Newbied; }

            Sandals sandals = FindItemOnLayer(Layer.Shoes) as Sandals;
            if (sandals != null) { sandals.Hue = hue; sandals.LootType = LootType.Newbied; }

            Str += 10;
            Int += 10;
            Dex += 10;

            SetSkill(SkillName.EvalInt, 85.2, 110.0);
            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.Meditation, 32.5, 60.0);
            SetSkill(SkillName.MagicResist, 82.5, 110.0);

            SetSkill(SkillName.Tactics, 75.0, 87.5);
            SetSkill(SkillName.Wrestling, 30.3, 80.0);
        }

        public EvilTokenMageLord( Serial serial ) : base( serial ) { }

        public override bool OnBeforeDeath()
        {
            Robe robe = FindItemOnLayer(Layer.OuterTorso) as Robe;
            if (robe != null) { robe.Hue = Utility.RandomList(1102, 1107); robe.LootType = LootType.Regular; }

            WizardsHat hat = FindItemOnLayer(Layer.Helm) as WizardsHat;
            if (hat != null) { hat.Hue = robe.Hue; robe.LootType = LootType.Regular; }

            Sandals sandals = FindItemOnLayer(Layer.Shoes) as Sandals;
            if (sandals != null) { sandals.Hue = Utility.RandomBlueHue(); sandals.LootType = LootType.Regular; }

            return base.OnBeforeDeath();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.Random(20) < 1) c.DropItem(new SpecialHairDye());
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

    class EvilBronzeMageLord : EvilTokenMageLord
    {
        [Constructable]
        public EvilBronzeMageLord()
            : base(0x972, "Bronze")
        {
            SetHits(132, 140);
            VirtualArmor += 10;
        }

        public EvilBronzeMageLord(Serial serial) : base(serial) { }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (Utility.Random(100) < 80) c.DropItem(new BronzePrizeToken());
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

    class EvilSilverMageLord : EvilTokenMageLord
    {
        [Constructable]
        public EvilSilverMageLord()
            : base(0x961, "Silver")
        {
            SetHits(280, 322);
            VirtualArmor += 20;
        }

        public EvilSilverMageLord(Serial serial) : base(serial) { }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (Utility.Random(100) < 80) c.DropItem(new SilverPrizeToken());
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

    class EvilGoldenMageLord : EvilTokenMageLord
    {
        [Constructable]
        public EvilGoldenMageLord()
            : base(0x8a5, "Golden")
        {
            SetHits(592, 720);
            VirtualArmor += 50;
        }

        public EvilGoldenMageLord(Serial serial) : base(serial) { }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (Utility.Random(100) < 80) c.DropItem(new GoldenPrizeToken());
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