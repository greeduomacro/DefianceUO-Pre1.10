using Server;
using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Engines.IdolSystem;
using System.Text;

namespace Server.Items
{
    public class RareCollector : Item
    {
        private MagicalRareType m_Rare;

        private bool m_RareOne;

        public bool RareOne
        {
            get { return m_RareOne; }
            set { m_RareOne = value; }
        }

        private bool m_RareTwo;

        public bool RareTwo
        {
            get { return m_RareTwo; }
            set { m_RareTwo = value; }
        }

        private bool m_RareThree;

        public bool RareThree
        {
            get { return m_RareThree; }
            set { m_RareThree = value; }
        }

        private bool m_RareFour;

        public bool RareFour
        {
            get { return m_RareFour; }
            set { m_RareFour = value; }
        }

        private bool m_RareFive;

        public bool RareFive
        {
            get { return m_RareFive; }
            set { m_RareFive = value; }
        }

        private bool m_RareSix;

        public bool RareSix
        {
            get { return m_RareSix; }
            set { m_RareSix = value; }
        }

        [Constructable]
        public RareCollector()
            : base(0xE2E)
        {
            Weight = 5.0;
            Hue = 2010;
            Light = LightType.Circle225;
            m_RareSix = false;
            m_RareFive = false;
            m_RareFour = false;
            m_RareThree = false;
            m_RareTwo = false;
            m_RareOne = false;
            Name = "This Rare Collector holds:";
        }

        public RareCollector(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_RareOne);
            writer.Write(m_RareTwo);
            writer.Write(m_RareThree);
            writer.Write(m_RareFour);
            writer.Write(m_RareFive);
            writer.Write(m_RareSix);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_RareOne = reader.ReadBool();
                        m_RareTwo = reader.ReadBool();
                        m_RareThree = reader.ReadBool();
                        m_RareFour = reader.ReadBool();
                        m_RareFive = reader.ReadBool();
                        m_RareSix = reader.ReadBool();
                        break;
                    }
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (m_RareOne == false && m_RareTwo == false && m_RareThree == false && m_RareFour == false && m_RareFive == false && m_RareSix == false)
                this.LabelTo(from, String.Format("Nothing, it is Empty"));
            else
            {
                StringBuilder sb = new StringBuilder();
                if (m_RareOne)
                    sb.Append("an odd looking rare");
                if (m_RareTwo)
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.Append("a small rare item");
                }
                if (m_RareThree)
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.Append("a dull rare");
                }
                if (m_RareFour)
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.Append("a funny shaped rare");
                }
                if (m_RareFive)
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.Append("a scratched rare");
                }
                if (m_RareSix)
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.Append("an old rare");
                }
                this.LabelTo(from, sb.ToString());
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null)
                return;
            else
            {
                from.RevealingAction();
                from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(Rare_OnTarget));
            }
        }

        public void Rare_OnTarget(Mobile from, object o)
        {
            if (from == null || from.Backpack == null || o == null || !(o is Item))
                return;

            if (!IsChildOf(from.Backpack) || !(((Item)o).IsChildOf(from.Backpack)))
            {
                from.SendMessage("Both items must be in your backpack.");
                return;
            }
            if (o is MagicalRare)
            {
                MagicalRare item = (MagicalRare)o;

                if (item.rares == MagicalRareType.One && this.m_RareOne == false)
                {
                    from.SendMessage("You place the magical rare in the collector");
                    from.PlaySound(550);
                    this.m_RareOne = true;
                    CheckFull(from);
                    item.Delete();
                }
                if (item.rares == MagicalRareType.Two && this.m_RareTwo == false)
                {
                    from.SendMessage("You place the magical rare in the collector");
                    from.PlaySound(550);
                    this.m_RareTwo = true;
                    CheckFull(from);
                    item.Delete();
                }
                if (item.rares == MagicalRareType.Three && this.m_RareThree == false)
                {
                    from.SendMessage("You place the magical rare in the collector");
                    from.PlaySound(550);
                    this.m_RareThree = true;
                    CheckFull(from);
                    item.Delete();
                }
                if (item.rares == MagicalRareType.Four && this.m_RareFour == false)
                {
                    from.SendMessage("You place the magical rare in the collector");
                    from.PlaySound(550);
                    this.m_RareFour = true;
                    CheckFull(from);
                    item.Delete();
                }
                if (item.rares == MagicalRareType.Five && this.m_RareFive == false)
                {
                    from.SendMessage("You place the magical rare in the collector");
                    from.PlaySound(550);
                    this.m_RareFive = true;
                    CheckFull(from);
                    item.Delete();
                }
                if (item.rares == MagicalRareType.Six && this.m_RareSix == false)
                {
                    from.SendMessage("You place the magical rare in the collector");
                    from.PlaySound(550);
                    this.m_RareSix = true;
                    CheckFull(from);
                    item.Delete();
                }
            }
            else
            {
                from.SendMessage("That is not something to be collected!");
            }
        }

        public void CheckFull(Mobile from)
        {
            if (this.m_RareOne == true && this.m_RareTwo == true && this.m_RareThree == true && this.m_RareFour == true && this.m_RareFive == true && this.m_RareSix == true)
            {
                from.SendMessage("You have collected all the magical rares, heres your reward!");
                from.PlaySound(551);
                this.Delete();
                from.AddToBackpack(new IdolBossLoot());
            }
        }
    }
}