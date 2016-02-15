using Server;
using System;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class MysticKeyCombinedPart : Item
	{
		private int m_Level;

		public int Level
		{
			get { return m_Level; }
		}

		[Constructable]
		public MysticKeyCombinedPart(int state) : base()
		{
			Movable = true;
			Weight = 5.0;
			LootType = LootType.Regular;

			PartSetup(state);
		}

		public void PartSetup(int state)
		{
			m_Level = state;

			switch (m_Level)
			{
				case 1: //former 12
					Name = "a strange wooden object";
					Hue = 1868;
					ItemID = 0x1425;
					break;
				case 2: // former 123
					Name = "strange shimmering wood";
					Hue = 1159;
					ItemID = 0x1427;
					break;
				case 3: // former 1234
					Name = "a spiked plate";
					Hue = 1437;
					ItemID = 0x26B2;
					break;
				case 4: // former 12345
					Name = "a venomous part";
					Hue = 1372;
					ItemID = 0x1421;
					break;
				default:
					this.Delete();
					break;
			}
		}

		public void RaiseLevel(Mobile from)
		{
			if (from == null || from.Backpack == null)
				return;

			if (++m_Level == 5)
				from.PlaceInBackpack(new MysticKeys());

			PartSetup(m_Level);
		}

		public MysticKeyCombinedPart(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_Level);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Level = reader.ReadInt();
		}
	}


	public class MysticKeySinglePart : Item, IClaimableLoot
	{
		private int m_Level;
		private bool m_IsLoot;

		[Constructable]
		public MysticKeySinglePart(int state) : base()
		{
			Movable = false;
			Weight = 5.0;
			LootType = LootType.Regular;
			m_IsLoot = true;
			PartSetup(state);
		}

		public int Level
		{
			get { return m_Level; }
		}

		public bool IsLoot
		{
			get { return m_IsLoot; }
			set { if (!value) m_IsLoot = value; } //Property can only be used to set IsLoot to false
		}

		public void PartSetup(int state)
		{
			m_Level = state;

			switch (m_Level)
			{
				case 1:
					Name = "a bloodstained rune";
					Hue = 2118;
					ItemID = 0x1423;
					break;
				case 2:
					Name = "a wooden rune";
					Hue = 1868;
					ItemID = 0x1423;
					break;
				case 3:
					Name = "a shimmering rune";
					Hue = 1159;
					ItemID = 0x1423;
					break;
				case 4:
					Name = "a clawed rune";
					Hue = 1437;
					ItemID = 0x1423;
					break;
				case 5:
					Name = "a poisonous rune";
					Hue = 1372;
					ItemID = 0x1423;
					break;
				case 6:
					Name = "a toxic part";
					Hue = 1159;
					ItemID = 0x1420;
					break;
				default:
					this.Delete();
					break;
			}
		}

		public MysticKeySinglePart(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_IsLoot);
			writer.Write(m_Level);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_IsLoot = reader.ReadBool();
			m_Level = reader.ReadInt();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null)
				return;

			from.RevealingAction();
			from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(SingleParts_OnTarget));
		}

		public void SingleParts_OnTarget(Mobile from, object o)
		{
			if (from == null || from.Backpack == null || o == null || !(o is Item))
				return;

			if (!IsChildOf(from.Backpack) || !(((Item)o).IsChildOf(from.Backpack)))
			{
				from.SendMessage("Both items must be in your backpack.");
				return;
			}
			else if (this.Level == 1 && o is MysticKeySinglePart)
			{
				MysticKeySinglePart s_part = (MysticKeySinglePart)o;
				if (s_part.Level == 2)
				{
					if (Utility.Random(10) < 3)
					{
						from.SendMessage("You fail to fuse the two parts together and one explodes.");
						from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
						from.PlaySound(0x307);
						Spells.SpellHelper.Damage(TimeSpan.FromTicks(0), from, 20);
						switch (Utility.Random(2))
						{
							case 0: this.Delete(); break;
							case 1: s_part.Delete(); break;
						}
					}
					else
					{
						from.SendMessage("You carefully fuse both parts together and form a bigger part.");
						from.PlaySound(550);
						from.AddToBackpack(new MysticKeyCombinedPart(1));

						this.Delete();
						s_part.Delete();
					}
				}
				else
					from.SendMessage("They do not seem to fit well together.");
			}
			else if (o is MysticKeyCombinedPart)
			{
				MysticKeyCombinedPart c_part = (MysticKeyCombinedPart)o;
				if (this.Level >= 3 && this.Level-2 == c_part.Level)
				{
					if (Utility.Random(10) < this.Level+2)
					{
						from.SendMessage("You fail to fuse the two parts together and the smaller one explodes");
						from.FixedParticles(0x36B0, 20, 10, 5044, EffectLayer.Head);
						from.PlaySound(0x307);
						Spells.SpellHelper.Damage(TimeSpan.FromTicks(0), from, 40);
						this.Delete();
					}
					else
					{
						from.SendMessage("You carefully fuse both parts together and form a bigger part");
						from.PlaySound(550);
						c_part.RaiseLevel(from);
						this.Delete();
					}
				}
				else
					from.SendMessage("They do not seem to fit well together.");
			}
			else
			{
				from.SendMessage("You can not use this with that.");
			}
		}
	}
}