using System;
using Server.Items;

namespace Server.Items
{
    public class ChampionNecklace : BaseNecklace, IOldLowerRegCost
    {
        [Constructable]
        public ChampionNecklace() : base(0x1085)
        {
            Name = "Amulet of the Champion imbued with arcanum level 3";
            Weight = 0.0;
            Hue = 1157;
            LootType = LootType.Blessed;
        }

        public ChampionNecklace(Serial serial) : base(serial) { }

        public int OldLowerRegCost { get { return 3; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Name = "Amulet of the Champion imbued with arcanum level 3";
        }
    }
}