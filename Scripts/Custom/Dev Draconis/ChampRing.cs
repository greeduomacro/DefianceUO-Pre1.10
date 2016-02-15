using System;
using Server.Items;

namespace Server.Items
{
    public class ChampionRing : BaseRing, IOldLowerRegCost
    {
        [Constructable]
        public ChampionRing() : base( 0x108A )
        {
            Name = "Band of the Champion imbued with arcanum level 3";
            Weight = 0.0;
            Hue = 1150;
            LootType = LootType.Blessed;
        }

        public ChampionRing(Serial serial) : base(serial) { }

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
            Name = "Band of the Champion imbued with arcanum level 3";
        }
    }
}