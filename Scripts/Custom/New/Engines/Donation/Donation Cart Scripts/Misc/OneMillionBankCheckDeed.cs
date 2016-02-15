using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class OneMillionBankCheckDeed : Item
    {
        [Constructable]
        public OneMillionBankCheckDeed() : base(0x14F0)
        {
            Weight = 1.0;
            Name = "One million Bankcheck Deed";
        }

        public OneMillionBankCheckDeed(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Deleted || from.Backpack == null)
                return;

            {
                if (!IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }

                if (from.BankBox != null)
                {
                    from.BankBox.AddItem(new BankCheck(1000000));
                    from.SendMessage("Thanks for your support! a bankcheck of 1.000.000 gold pieces has been placed into your bank.");
                    Delete();
                }
            }
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