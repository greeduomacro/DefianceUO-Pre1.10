using System;
using Server.Items;

namespace Server.Items
{
    public class ChampionKilt : Kilt, IOldLowerRegCost
    {
        [Constructable]
        public ChampionKilt() : base(1150)
        {
            Name = "Kilt of the Champion imbued with arcanum level 5";
            Weight = 0.0;
        }

        public ChampionKilt(Serial serial) : base(serial) { }
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
            Name = "Kilt of the Champion imbued with arcanum level 5";
        }
    }
}