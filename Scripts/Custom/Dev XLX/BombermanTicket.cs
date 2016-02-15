using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class BombermanTicket : Item
    {
        [Constructable]
        public BombermanTicket() : base(0x14F0)
        {
            Weight = 11.0;
            Hue = 1367;
            Name = "a Bomberman ticket";
        }

        public BombermanTicket(Serial serial) : base(serial)
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
        }
    }
}