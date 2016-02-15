using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class MysticKeys : Item
	{
		private bool m_Charged;
		private DateTime m_LastRecharged;

		private static Point3D LevelFourDestination = new Point3D(4, 1268, -11);
		private static Point3D LevelFiveDestination = new Point3D(2114, 829, -11);

		[Constructable]
		public MysticKeys() : base(0x176B)
		{
			Movable = true;
			Weight = 5;
			LootType = LootType.Blessed;

			UnCharge();
		}

		public MysticKeys(Serial serial) : base(serial)
		{
		}

		public DateTime LastRecharged
		{
			get { return m_LastRecharged; }
		}

		public bool Charged
		{
			get { return m_Charged; }
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			writer.Write(m_Charged);
			writer.Write(m_LastRecharged);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Charged = reader.ReadBool();
			m_LastRecharged = reader.ReadDateTime();
		}

		public void Charge()
		{
			m_Charged = true;

			m_LastRecharged = DateTime.Now;
			Name = "a set of charged mystical keys";
			Hue = 1164;
		}

		public void UnCharge()
		{
			m_Charged = false;
			Name = "a set of uncharged mystical keys";
			Hue = 1110;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Backpack == null)
				return;

			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
				from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(UseKey_OnTarget));
		}

		public void UseKey_OnTarget(Mobile from, object o)
		{
			if (from == null || o == null || !(o is Item))
				return;

			if (!from.CanSee(o))
			{
				from.SendMessage("You cannot see this.");
				return;
			}

			if (o is LevelFourDoor)
			{
				from.SendMessage("You unlock the door and proceed to level four.");
				Server.Mobiles.BaseCreature.TeleportPets(from, LevelFourDestination, from.Map);
				from.MoveToWorld(LevelFourDestination, from.Map);
			}
			else if (o is LevelFiveDoor)
			{
				if (m_Charged == false)
				{
					from.SendMessage("Your keys require charging before they can open this door.");
				}
				else
				{
					UnCharge();
					from.SendMessage("You unlock the door and proceed to level five.");
					Server.Mobiles.BaseCreature.TeleportPets(from, LevelFiveDestination, from.Map);
					from.MoveToWorld(LevelFiveDestination, from.Map);
				}
			}
			else
				from.SendMessage("You can not use this on that.");
		}
	}
}