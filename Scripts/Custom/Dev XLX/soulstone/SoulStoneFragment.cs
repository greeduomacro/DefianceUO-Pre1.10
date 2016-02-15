using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Accounting;

namespace Server.Items
{
    public class SoulStoneFragment : SoulStone
    {
        public override bool UseCharges { get { return true; } }
        public override bool ChangeItemIdWhenFilled { get { return false; }
}
        public override int LabelNumber { get { return 1071000; } } // soulstone fragment

        [Constructable]
        public SoulStoneFragment()
            : this(null)
        {
        }

        [Constructable]
        public SoulStoneFragment(string account)
            : base(account, 0x2AA1+Utility.Random(9))
        {
            Charges = 5;
        }

        public SoulStoneFragment(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}