//  Last Modified: 13/2-2005 By Caleb - Added a skillstone only usable by people with no other accounts on the server.
using System;
using System.Collections;
using System.Net;
using Server.Network;
using Server.Items;
using Server.Gumps;
using Server.Accounting;

namespace Server.Items
{

	//******* Added by Caleb for special skillstone only available to new players*****  ///
	public class NewPlayerSkillStone : Item
	{
		#region UtilityClass
		private class AccountComparer : IComparer
		{
			public static readonly IComparer Instance = new AccountComparer();

			public AccountComparer()
			{
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
					return 0;
				else if (x == null)
					return -1;
				else if (y == null)
					return 1;

				Account a = x as Account;
				Account b = y as Account;

				if (a == null || b == null)
					throw new ArgumentException();

				AccessLevel aLevel, bLevel;
				bool aOnline, bOnline;

				GetAccountInfo(a, out aLevel, out aOnline);
				GetAccountInfo(b, out bLevel, out bOnline);

				if (aOnline && !bOnline)
					return -1;
				else if (bOnline && !aOnline)
					return 1;
				else if (aLevel > bLevel)
					return -1;
				else if (aLevel < bLevel)
					return 1;
				else
					return Insensitive.Compare(a.Username, b.Username);
			}
		}


		#endregion
		#region UtilityMethod
		public static void GetAccountInfo(Account a, out AccessLevel accessLevel, out bool online)
		{
			accessLevel = a.AccessLevel;
			online = false;

			for (int j = 0; j < 5; ++j)
			{
				Mobile check = a[j];

				if (check == null)
					continue;

				if (check.AccessLevel > accessLevel)
					accessLevel = check.AccessLevel;

				if (check.NetState != null)
					online = true;
			}
		}

		private static ArrayList GetSharedAccounts(IPAddress[] ipAddresses)
		{
			ArrayList list = new ArrayList();

			foreach (Account acct in Accounts.Table.Values)
			{
				IPAddress[] theirAddresses = acct.LoginIPs;
				bool contains = false;

				for (int i = 0; !contains && i < theirAddresses.Length; ++i)
				{
					IPAddress check = theirAddresses[i];

					for (int j = 0; !contains && j < ipAddresses.Length; ++j)
						contains = check.Equals(ipAddresses[j]);
				}

				if (contains)
					list.Add(acct);
			}

			list.Sort(AccountComparer.Instance);
			return list;
		}
		#endregion

		public bool m_Enabled;

		[CommandProperty(AccessLevel.Seer)]
		public bool Enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				m_Enabled = value;
			}
		}

		[Constructable]
		public NewPlayerSkillStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 0x85;
			Name = "a skill stone";
			m_Enabled = false;
		}

		public NewPlayerSkillStone(Serial serial) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_Enabled);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			switch (version)
			{
				case 0:
					{
						m_Enabled = reader.ReadBool();
						break;
					}
			}
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);

			string value;

			if (m_Enabled)
				value = "(Enabled)";
			else
				value = "(Disabled)";

			this.LabelTo(from, value);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.SendLocalizedMessage(500446); // That is too far away.
			}
			else if (!m_Enabled)
			{
				from.SendMessage("The skill stone is disabled");
				return;
			}
			else
			{
				from.SendGump(new NewPlayerSkilloStoneGump(from, from));
				from.SendGump(new NewPlayerStatsoGump(from));
			}
		}
	}
}

namespace Server.Gumps
{
	public class NewPlayerSkilloStoneGump : Gump
	{
		private const int FieldsPerPage = 14;

		private Mobile m_From;
		private Mobile m_Mobile;

		public NewPlayerSkilloStoneGump(Mobile from, Mobile mobile) : base ( 20, 30 )
		{
			m_From = from;
			m_Mobile = mobile;

			AddPage(0);
			AddBackground(0, 0, 260, 351, 5054);

			AddImageTiled(10, 10, 240, 23, 0x52);
			AddImageTiled(11, 11, 238, 21, 0xBBC);

			AddLabel(65, 11, 0, "Skills");

			AddPage(1);

			int page = 1;

			int index = 0;

			Skills skills = mobile.Skills;

			int number;
			if (Core.AOS)
				number = 0;
			else
				number = 3;

			for (int i = 0; i < (skills.Length - number); ++i)
			{
				if (index >= FieldsPerPage)
				{
					AddButton(231, 13, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1);

					++page;
					index = 0;

					AddPage(page);

					AddButton(213, 13, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page - 1);
				}

				Skill skill = skills[i];
				AddImageTiled(10, 32 + (index * 22), 240, 23, 0x52);
				AddImageTiled(11, 33 + (index * 22), 238, 21, 0xBBC);

				AddLabelCropped(13, 33 + (index * 22), 150, 21, 0, skill.Name);
				AddImageTiled(180, 34 + (index * 22), 50, 19, 0x52);
				AddImageTiled(181, 35 + (index * 22), 48, 17, 0xBBC);
				AddLabelCropped(182, 35 + (index * 22), 234, 21, 0, skill.Base.ToString("F1"));

				if (from.AccessLevel >= AccessLevel.Player && !IsRestrictedSkill((SkillName)i))
					AddButton(231, 35 + (index * 22), 0x15E1, 0x15E5, i + 1, GumpButtonType.Reply, 0);
				else
					AddImage(231, 35 + (index * 22), 0x2622);

				++index;
			}
		}
		private bool IsRestrictedSkill(SkillName i)
		{
			return i == SkillName.Blacksmith || i == SkillName.Peacemaking || i == SkillName.Provocation ||
				i == SkillName.Musicianship || i == SkillName.Tailoring || i == SkillName.AnimalTaming ||
				i == SkillName.Lockpicking || i == SkillName.RemoveTrap || i == SkillName.Mining ||
				i == SkillName.Necromancy || i == SkillName.Focus || i == SkillName.Chivalry ||
				i == SkillName.Bushido || i == SkillName.Carpentry || i == SkillName.Poisoning ||
                                i == SkillName.Snooping || i == SkillName.Fishing || i == SkillName.Hiding ||
                                i == SkillName.Lumberjacking || i == SkillName.Inscribe || i == SkillName.Alchemy ||
                                i == SkillName.Stealing || i == SkillName.Cartography || i == SkillName.Discordance || i == SkillName.Ninjitsu;
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID > 0)
				m_From.SendGump(new NewPlayerEditoSkillsGump(m_From, m_Mobile, info.ButtonID - 1));
		}
	}

	public class NewPlayerEditoSkillsGump : Gump
	{
		private Mobile m_From;
		private Mobile m_Mobile;
		private Skill m_Skill;

		public NewPlayerEditoSkillsGump(Mobile from, Mobile mobile, int skillNo) : base ( 20, 30 )
		{
			m_From = from;
			m_Mobile = mobile;
			m_Skill = mobile.Skills[skillNo];

			if (m_Skill == null)
				return;

			AddPage(0);

			AddBackground(0, 0, 90, 60, 5054);

			AddImageTiled(10, 10, 72, 22, 0x52);
			AddImageTiled(11, 11, 70, 20, 0xBBC);
			AddTextEntry(11, 11, 70, 20, 0, 0, m_Skill.Base.ToString("F1"));
			AddButton(15, 35, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0);
			AddButton(50, 35, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID > 0)
			{
				TextRelay text = info.GetTextEntry(0);

				if (text != null)
				{
					try
					{
						double count = 0;
						for (int i = 0; i < state.Mobile.Skills.Length; ++i)
						{
							if (m_Skill != state.Mobile.Skills[i])
								count = count + state.Mobile.Skills[i].Base;
						}

						double value = Convert.ToDouble(text.Text);
						if (value < 0.0 || value > 90.0)
						//if (value < 0.0 || value > 100.0)
						{
							state.Mobile.SendMessage("Value too high. 0 - 85 only.");
							//state.Mobile.SendLocalizedMessage(502452); // Value too high. 0-100 only
						}
						else if ((count + value) > (state.Mobile.SkillsCap / 10))
						{
							state.Mobile.SendMessage("You can not exceed the skill cap.  Try setting another skill lower first.");
						}
						else
						{
							m_Skill.Base = value;
						}
					}
					catch
					{
						state.Mobile.SendMessage("Bad format. ###.# expected");
					}
				}
			}
			state.Mobile.SendGump(new NewPlayerSkilloStoneGump(m_From, m_Mobile));
		}
	}

	public class NewPlayerStatsoGump : Gump
	{
		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 1: // Change Strength
					{
						TextRelay text = info.GetTextEntry(0);

						if (text.Text != "")
						{
							try
							{
								int value = Convert.ToInt32(text.Text);
								if (value < 0.0 || value > 100.0)
								//if (value < 0.0 || value > 100.0)
								{
									state.Mobile.SendMessage("Value too high. 0 - 100 only.");
									//state.Mobile.SendLocalizedMessage(502452); // Value too high. 0-100 only
								}
								else
								{
									if ((value + from.RawDex + from.RawInt) > from.StatCap)
									{
										from.SendLocalizedMessage(1005629); // You can not exceed the stat cap.  Try setting another stat lower first.
									}
									else
									{
										from.RawStr = value;
										//from.Hits = from.HitsMax;
										from.SendLocalizedMessage(1005630); // Your stats have been adjusted.
									}
								}
							}
							catch
							{
								state.Mobile.SendMessage("Bad format. ### expected");
							}
						}

						from.SendGump(new NewPlayerStatsoGump(from));
						break;
					}

				case 2: // Change Dexterity
					{
						TextRelay text = info.GetTextEntry(0);

						if (text.Text != "")
						{
							try
							{
								int value = Convert.ToInt32(text.Text);
								if (value < 0.0 || value > 100.0)
								//if (value < 0.0 || value > 100.0)
								{
									state.Mobile.SendMessage("Value too high. 0 - 100 only.");
									//state.Mobile.SendLocalizedMessage(502452); // Value too high. 0-100 only
								}
								else
								{
									if ((value + from.RawStr + from.RawInt) > from.StatCap)
									{
										from.SendLocalizedMessage(1005629); // You can not exceed the stat cap.  Try setting another stat lower first.
									}
									else
									{
										from.RawDex = value;
										//from.Stam = from.StamMax;
										from.SendLocalizedMessage(1005630); // Your stats have been adjusted.
									}
								}
							}
							catch
							{
								state.Mobile.SendMessage("Bad format. ### expected");
							}
						}

						from.SendGump(new NewPlayerStatsoGump(from));
						break;
					}

				case 3: // Change Intelligence
					{
						TextRelay text = info.GetTextEntry(0);

						if (text.Text != "")
						{
							try
							{
								int value = Convert.ToInt32(text.Text);
								if (value < 0.0 || value > 100.0)
								//if (value < 0.0 || value > 100.0)
								{
									state.Mobile.SendMessage("Value too high. 0 - 100 only.");
									//state.Mobile.SendLocalizedMessage(502452); // Value too high. 0-100 only
								}
								else
								{
									if ((value + from.RawDex + from.RawStr) > from.StatCap)
									{
										from.SendLocalizedMessage(1005629); // You can not exceed the stat cap.  Try setting another stat lower first.
									}
									else
									{
										from.RawInt = value;
										//from.Mana = from.ManaMax;
										from.SendLocalizedMessage(1005630); // Your stats have been adjusted.
									}
								}
							}
							catch
							{
								state.Mobile.SendMessage("Bad format. ### expected");
							}
						}
						from.SendGump(new NewPlayerStatsoGump(from));
						break;
					}
			}
		}

		public NewPlayerStatsoGump(Mobile from) : this( from, "" )
		{
		}

		public NewPlayerStatsoGump(Mobile from, string initialText) : base( 100, 400 )
		{
			AddPage(0);

			AddBackground(0, 0, 110, 96, 5054);

			AddPage(1);

			int line = 0;
			int x = Convert.ToInt32(from.RawStr);
			string Str = Convert.ToString(x);
			int y = Convert.ToInt32(from.RawDex);
			string Dex = Convert.ToString(y);
			int z = Convert.ToInt32(from.RawInt);
			string Int = Convert.ToString(z);
			AddImageTiled(10, 10 + (line * 22), 90, 22, 0xBBC);
			AddImageTiled(11, 10 + (line * 22) + 1, 88, 20, 0x2426);
			AddTextEntry(11, 10 + (line++ * 22) + 1, 88, 20, 0x480, 0, initialText);
			AddButton(10, 10 + (line * 22), 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0); // Str
			AddButton((10 * 3) + 10, 10 + (line * 22), 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0); // Dex
			AddButton((10 * 6) + 10, 10 + (line++ * 22), 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0); // Int
			AddLabel(10, 10 + (line * 22), 0x481, "Str");
			AddLabel((10 * 3) + 10, 10 + (line * 22), 0x481, "Dex");
			AddLabel((10 * 6) + 10, 10 + (line++ * 22), 0x481, "Int");
			AddLabel(10, 10 + (line * 22), 0x481, Str);
			AddLabel((10 * 3) + 10, 10 + (line * 22), 0x481, Dex);
			AddLabel((10 * 6) + 10, 10 + (line++ * 22), 0x481, Int);
		}
	}
}