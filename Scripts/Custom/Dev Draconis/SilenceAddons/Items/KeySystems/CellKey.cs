using System;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class CellKey : Item, IClaimableLoot
	{
		private bool m_IsLoot;

		[Constructable]
		public CellKey() : base(0x1010)
		{
			Movable = false;
			Weight = 5;
			Name = "Tear's Cell Key";
			Hue = 1160;
			LootType = LootType.Blessed;
			m_IsLoot = true;
		}

		public CellKey(Serial serial) : base(serial)
		{
		}

		public bool IsLoot
		{
			get { return m_IsLoot; }
			set { if (!value) m_IsLoot = value; } //Property can only be used to set IsLoot to false
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Backpack == null)
				return;

			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
				from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(CellKey_OnTarget));
		}

		public void CellKey_OnTarget(Mobile from, object o)
		{
			if (from == null || o == null)
				return;

			CellLock c_lock = o as CellLock;
			if (c_lock != null)
			{
				if (Utility.Random(100) < 1)
					from.SendMessage("You have opened the cell door of Tear.");
				else
				{
					from.SendMessage("This was a fake key and breaks in the attempt of opening Tear's cell door.");
					from.PlaySound(550);
					this.Delete();
				}
			}
			else
				from.SendMessage("This key does not appear to work on that.");
		}


		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
			writer.Write(m_IsLoot);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if (version > 0)
				m_IsLoot = reader.ReadBool();
		}
	}
}