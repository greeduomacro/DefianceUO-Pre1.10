//Al@2006-07-20
using System;
using Server.Targeting;
using Server.Targets;

namespace Server.Items
{
    public class HueTicket : Item
    {
        [Constructable]
        public HueTicket() : base(0x14F0)
        {
            Hue = 0;
            Name = "Special Hue Ticket";
            Weight = 1.0;
        }

        public HueTicket(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack == null || !IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.SendMessage("Which item do you want to rehue? (Clothes only)");
                from.Target = new HueTicketTarget(this);
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

    public class HueTicketTarget : Target
    {
        private HueTicket m_Deed;

        public HueTicketTarget(HueTicket deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
        {
            if (m_Deed == null || from.Backpack == null || !m_Deed.IsChildOf(from.Backpack))
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            else if (target is BaseClothing)
            {
                Item item = (Item)target;
                if (!item.IsChildOf(from.Backpack) || item is HoodedShroudOfShadows || item is ElvenRobe)
                    from.SendMessage("You cannot rehue this item.");
                else
                {
                    item.Hue = m_Deed.Hue;
                    m_Deed.Delete();
                    from.SendMessage("The item has successfully been hued.");
                }
            }
            else
            {
                from.SendMessage("You cannot rehue this item.");
            }
        }
    }

    public class BloodHueTicket : HueTicket
    {
        [Constructable]
        public BloodHueTicket() : base()
        {
            Hue = 1157;
            Name = "Blood Hue Ticket";
        }

        public BloodHueTicket(Serial serial) : base(serial) { }

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

    public class FireHueTicket : HueTicket
    {
        [Constructable]
        public FireHueTicket() : base()
        {
            Hue = 1358;
            Name = "Fire Hue Ticket";
        }

        public FireHueTicket(Serial serial) : base(serial) { }

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

    public class DarkPurpleHueTicket : HueTicket
    {
        [Constructable]
        public DarkPurpleHueTicket() : base()
        {
            Hue = 1158;
            Name = "Dark Purple Hue Ticket";
        }

        public DarkPurpleHueTicket(Serial serial) : base(serial) { }

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