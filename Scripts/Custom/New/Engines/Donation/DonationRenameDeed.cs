using System;
using Server.Targeting;

namespace Server.Items
{
	class DonationDeed : Item
	{
		[Constructable]
		public DonationDeed()
			: base(0x14F0)
		{
			Name = "Donation Deed - Change the name of a Bandana or a Body Sash";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack))
			{
				from.Target = new DonationDeedTarget(this);
				from.SendMessage("Please target a Sash or Bandana that you wish to enhance.");
			}

			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public DonationDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}

		private class DonationDeedTarget : Target
		{
			private DonationDeed m_Deed;

			public DonationDeedTarget(DonationDeed deed)
				: base(30, false, TargetFlags.None)
			{
				m_Deed = deed;
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (target is BodySash)
				{
					BodySash sash = (BodySash)target;

					if (sash.IsChildOf(from.Backpack))
					{
						if (m_Deed != null && !m_Deed.Deleted)
						{
							if (sash.Layer == Layer.Earrings)
								sash.Name = "I do my part to keep the shard Online [Layered]";
							else
								sash.Name = "I do my part to keep the shard Online";
							sash.Hue = 1177;
							from.SendMessage("You have enhanced the item.");
							Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 0x47D, 2, 9962, 0);
							Effects.SendLocationParticles(EffectItem.Create(new Point3D(from.X, from.Y, from.Z - 7), from.Map, EffectItem.DefaultDuration), 0x37C4, 1, 29, 0x47D, 2, 9502, 0);
							from.PlaySound(0x212);
							from.PlaySound(0x206);
							m_Deed.Delete();
						}
					}
					else
						from.SendLocalizedMessage(1061005); // The item must be in your backpack to enhance it.
				}
				else if (target is Bandana)
				{
					Bandana band = (Bandana)target;

					if (band.IsChildOf(from.Backpack))
					{
						if (m_Deed != null && !m_Deed.Deleted)
						{
							band.Name = String.Format( "I do my part to keep the shard Online");
							band.Hue = 1177;
							from.SendMessage("You have enhanced the item.");
							Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 0x47D, 2, 9962, 0);
							Effects.SendLocationParticles(EffectItem.Create(new Point3D(from.X, from.Y, from.Z - 7), from.Map, EffectItem.DefaultDuration), 0x37C4, 1, 29, 0x47D, 2, 9502, 0);
							from.PlaySound(0x212);
							from.PlaySound(0x206);
							m_Deed.Delete();
						}
					}
					else
						from.SendLocalizedMessage(1061005); // The item must be in your backpack to enhance it.
				}
				else
					from.SendMessage("You cannot enhance that item.");
			}
		}
	}
}