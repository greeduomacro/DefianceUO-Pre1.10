using System;
using Server.Items;

namespace Server.Items
{
    public class ChampionDoublet : Doublet, IOldLowerRegCost
    {
        [Constructable]
        public ChampionDoublet() : base((int)1150)
        {
            Name = "Tunic of the Champion imbued with arcanum level 5";
            Weight = 0.0;
        }

        public ChampionDoublet(Serial serial) : base(serial) { }

        public int OldLowerRegCost { get { return 5; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Name = "Tunic of the Champion imbued with arcanum level 5";
        }
    }
}