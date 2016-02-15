using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	class BMJoinStone : Item
	{
		private BomberManGame m_Game;

		[Constructable]
		public BMJoinStone() : base(0xEDC)
		{
			Name = "Bomber Man SignUp Stone";
			Movable = false;
			Visible = true;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public BomberManGame GameStone
		{
			get { return m_Game; }
			set { m_Game = value; }
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Deleted || !(from is PlayerMobile))
				return;

			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.SendLocalizedMessage(500446); // That is too far away.
				return;
			}
			else if ((!this.Visible) || !from.InLOS(this.GetWorldLocation()))
			{
				from.SendLocalizedMessage(502800); // You can't see that.
				return;
			}

			if (!from.CanSee(this))
			{
				from.SendMessage("You can not see this.");
				return;
			}

			if (m_Game == null)
			{
				from.SendMessage("This SignUp stone needs setup. Please contact a GM.");
				this.Visible = false;
				return;
			}

			if (m_Game.Running)
			{
				from.SendMessage("You cant join the game while its running.");
				return;
			}

			if (!m_Game.OpenJoin)
			{
				from.SendMessage("The SignUp is closed.");
				return;
			}

			if (m_Game.FreePlayerSlots == 0)
			{
				from.SendMessage("The Game is full.");
				return;
			}

			if (((PlayerMobile)from).Young)
			{
				from.SendMessage("You cant enter the game as a young player.");
				return;
			}

			if (((PlayerMobile)from).HueMod != -1 || ((PlayerMobile)from).BodyMod != 0)
			{
				from.SendMessage("You can not enter polymorphed or hue modded.");
				return;
			}

			if (from.Backpack == null)
			{
				from.SendMessage("You cant enter the game with no backpack.");
				return;
			}

			if (m_Game.Participants.Contains((PlayerMobile)from))
			{
				from.SendMessage("You have already joined this game.");
				return;
			}


			if (!PlayerIsNaked(from) || from.Mounted)
			{
				from.SendMessage("You have to be naked and unmounted to join the game.");
				return;
			}

			m_Game.AddParticipant((PlayerMobile)from);
		}

		// This function returns "true" if player is naked
		// This function does not check invalid layers and the mount layer
		public bool PlayerIsNaked(Mobile m)
		{
			if (m.Holding != null)
				return false;

			for (int i = 1; i < 29; i++)
			{
				// ignore mount, invalid or noused layers
				if (i == 9 || i == 15 || i == 24 || i == 11 || i == 16 || i == 25)
					continue;

				Layer layer = (Layer)i;
				Item item = m.FindItemOnLayer(layer);

				// Allow DeathRobe on OuterTorso
				if (i == 22 && item != null && !item.Deleted && item.ItemID == 0x204E)
					continue;

				if (item == null)
					continue;
				else if (!(item is Container))
					return false;
				else if (((Container)item).Items.Count > 0)
					return false;
			}
			return true;
		}

		public BMJoinStone(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_Game);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Game = reader.ReadItem() as BomberManGame;
		}
	}
}