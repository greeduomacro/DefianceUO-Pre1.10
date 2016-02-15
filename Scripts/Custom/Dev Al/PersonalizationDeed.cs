//Al@2006-07-20
using System;
using Server.Targeting;
using Server.Targets;

namespace Server.Items
{
    public class PersonalizationDeed : Item
    {
        [Constructable]
        public PersonalizationDeed() : base(0x14F0)
        {
            Hue = 0;
            Name = "a deed for personalization";
            Weight = 1.0;
        }

        public PersonalizationDeed(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack == null || !IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                //Note: Due to the localized item name of clothing I don't have any elegant idea
                //      to to this automatically.
                //from.SendMessage("Please page a GM to personalize your item.");
				from.SendMessage("Please target the clothing you want to personalize.");
				from.Target = new PersonalizeTarget(this);
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
			Name = "a deed for personalization";
        }

		private class PersonalizeTarget : Target
		{
			private PersonalizationDeed m_Deed;

			public PersonalizeTarget(PersonalizationDeed deed)
				: base(1, false, TargetFlags.None)
			{
				m_Deed = deed;
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (m_Deed.Deleted) return;

				if (!m_Deed.IsChildOf(from.Backpack))
				{
					from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				}
				else if (target is BaseClothing)
				{
					Item item = (Item)target;
					if (item.RootParent != from)
					{
						from.SendMessage("The item has to be on your character.");
					}
					else
					{
						if (item.Name == null)
							item.Name = String.Format("{0}'s {1}", from.Name, item.ItemData.Name);
						else
							item.Name = String.Format("{0}'s {1}", from.Name, item.Name);
						m_Deed.Delete();
					}
				}
				else
				{
					from.SendMessage("This deed only works on clothing.");
				}
			}
		}
	}
}