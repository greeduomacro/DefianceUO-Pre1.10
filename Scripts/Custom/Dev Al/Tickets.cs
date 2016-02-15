using System;
using System.Text;

namespace Server.Items
{

    public class TCTicket : Item
    {
        [Constructable]
        public TCTicket() : base(0x14F0)
        {
            Hue = 1367;
            Name = "a Team Clash ticket";
            Weight = 11.0;
        }

        public TCTicket(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from) { }

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

    public class CSTicket : Item
    {
        [Constructable]
        public CSTicket()
            : base(0x14F0)
        {
            Hue = 1367;
            Name = "a Counterstrike ticket";
            Weight = 11.0;
        }

        public CSTicket(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from) { }

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