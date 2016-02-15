using Server;
using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Engines.IdolSystem;

namespace Server.Items
{
    public enum WhichHalf
    {
        Left,
        Right
    }

    public class Half : Item
    {
        private WhichHalf m_Half;

        public WhichHalf Halfs { get { return m_Half; } set { m_Half = value; InvalidateProperties(); } }

        [Constructable]
        public Half(WhichHalf halfs)
            : base(7955)
        {
            Weight = 5.0;
            Name = "a disfigured idol";
            Hue = 2010;
            m_Half = halfs;
            switch (halfs)
            {
                case WhichHalf.Left: Name = "a disfigured idol"; break;
                case WhichHalf.Right: Name = "a damaged idol"; break;
            }
        }

        public Half(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_Half);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Half = (WhichHalf)reader.ReadInt(); ;

                        break;
                    }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null)
                return;
            else
            {
                from.RevealingAction();
                from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(Half_OnTarget));
            }
        }

        public void Half_OnTarget(Mobile from, object o)
        {
            if (from == null || from.Backpack == null || o == null || !(o is Item))
                return;

            if (!IsChildOf(from.Backpack) || !(((Item)o).IsChildOf(from.Backpack)))
            {
                from.SendMessage("Both items must be in your backpack.");
                return;
            }

            if (o is Half && this.m_Half == WhichHalf.Left || this.m_Half == WhichHalf.Right)
            {
                Half a_half = (Half)o;

                if (this.m_Half == WhichHalf.Left && a_half.m_Half == WhichHalf.Right || this.m_Half == WhichHalf.Right && a_half.m_Half == WhichHalf.Left)
                {
                    from.SendMessage("You carefully stick both halves together and create an idol");
                    from.PlaySound(550);
                    from.AddToBackpack(new Idol(IdolType.Despise));

                    this.Delete();
                    a_half.Delete();
                }
                else
                {
                    from.SendMessage("It does not appear you can combine two of the same halves together");
                }
            }
            else
            {
                from.SendMessage("They do not seem to fit well together");
            }
        }
    }
}