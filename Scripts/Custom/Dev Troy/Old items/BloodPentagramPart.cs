using System;
using Server;

namespace Server.Items
{
    public class BloodPentagramPart : Item
    {
        [Constructable]
        public BloodPentagramPart() : this(Utility.Random(34))
        {
        }

        [Constructable]
        public BloodPentagramPart(int partnum) : base()
        {
            Movable = true;
            Weight = 10;
            Name = "a blood pentagram";
            if (partnum >= 0 && partnum <= 33)
                ItemID = 7409 + partnum;
            else
                ItemID = 7409 + Utility.Random(34);
        }

        public BloodPentagramPart(Serial serial) : base(serial)
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
			if (Weight == 11) Weight = 10;
        }
    }
}