/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							ConfigGump.cs
*						-------------------------
*
*	File Description:	This gump provides the player with
*						the options for configuring a duel.
*
*/


using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;
using Server.Network;

namespace Server.Ladder
{
	public class ConfigGump : Gump
	{
		private int m_Selected;
		public ConfigGump(int x, int y, int selected)
			: base( x, y )
		{
			m_Selected = selected;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(50, 50, 425, 375, 9270);
			this.AddLabel(90, 70, 1259, @"Welcome to Defiance Ladder ");

			this.AddButton(80, 100, m_Selected == (int)Buttons.MageX5 ? 2153 : 2151, m_Selected == (int)Buttons.MageX5 ? 2151 : 2153, (int)Buttons.MageX5, GumpButtonType.Reply, 0);
			this.AddLabel(125, 105, 1149, @"Standard 5x Mage");
			this.AddButton(80, 140, m_Selected == (int)Buttons.MageX7 ? 2153 : 2151, m_Selected == (int)Buttons.MageX7 ? 2151 : 2153, (int)Buttons.MageX7, GumpButtonType.Reply, 0);
			this.AddLabel(125, 145, 1149, @"Standard 7x Mage");
			this.AddLabel(80, 190, 1259, @"Extra options:");

			this.AddButton(250, 100, m_Selected == (int)Buttons.DexMonkey ? 2153 : 2151, m_Selected == (int)Buttons.DexMonkey ? 2151 : 2153, (int)Buttons.DexMonkey, GumpButtonType.Reply, 0);
			this.AddLabel(295, 105, 1149, @"Standard Dex Monkey");

			this.AddCheck(80, 215, 2151, 2153, false, (int)Buttons.AllowPotions);
			this.AddLabel(125, 220, 1149, @"Allow Potions");
			this.AddCheck(80, 250, 2151, 2153, false, (int)Buttons.AllowSummoning);
			this.AddLabel(125, 255, 1149, @"Allow Summoning");
			this.AddCheck(80, 285, 2151, 2153, false, (int)Buttons.AllowLooting);
			this.AddLabel(125, 290, 1149, @"Allow Looting");

			this.AddCheck(250, 215, 2151, 2153, false, (int)Buttons.AllowPoisonedWeaps);
			this.AddLabel(295, 220, 1149, @"Allow Poisoned Weapons");
			this.AddCheck(250, 250, 2151, 2153, false, (int)Buttons.AllowMagicWeaps);
			this.AddLabel(295, 255, 1149, @"Allow Magic Weapons");
			this.AddCheck(250, 285, 2151, 2153, !(selected == (int)Buttons.DexMonkey) && !(selected == 0), (int)Buttons.AllowMagery);
			this.AddLabel(295, 290, 1149, @"Allow Magery");

			this.AddLabel(80, 335, 1259, @"Wager gold:");
			this.AddBackground(80, 355, 150, 20, 3000);
			this.AddTextEntry(80, 355, 150, 20, 1149, (int)Buttons.Wager, "0");

			string dex;
			switch (selected)
			{
				case (int)Buttons.MageX5:
					dex = "60";
					break;
				case (int)Buttons.MageX7:
					dex = "60";
					break;
				case (int)Buttons.DexMonkey:
					dex = "100";
					break;
				default:
					dex = "0";
					break;
			}

			this.AddLabel(250, 335, 1259, @"Maximum dex:");
			this.AddBackground(250, 355, 150, 20, 3000);
			this.AddTextEntry(250, 355, 150, 20, 1149, (int)Buttons.MaxDex, dex);

			this.AddButton(160, 380, 247, 248, (int)Buttons.OKButton, GumpButtonType.Reply, 0);



		}

		public enum Buttons
		{
			MageX5 = 1,
			MageX7,
			DexMonkey,
			AllowPotions,
			AllowSummoning,
			AllowLooting,
			AllowPoisonedWeaps,
			AllowMagicWeaps,
			AllowMagery,
			OKButton,
			Wager,
			MaxDex
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			TextRelay wager = info.GetTextEntry((int)Buttons.Wager);
			TextRelay maxDex = info.GetTextEntry((int)Buttons.MaxDex);

			if (info.ButtonID == (int)Buttons.MageX5 || info.ButtonID == (int)Buttons.MageX7 || info.ButtonID == (int)Buttons.DexMonkey)
			{
				state.Mobile.CloseGump(typeof(ConfigGump));
				state.Mobile.SendGump(new ConfigGump(this.X, this.Y, info.ButtonID));
			}
			else if (info.ButtonID == (int)Buttons.OKButton)
			{
				if (m_Selected == 0)
				{
					state.Mobile.SendMessage("You must choose a duel type.");
				}
				else if (info.IsSwitched((int)Buttons.AllowSummoning) && !info.IsSwitched((int)Buttons.AllowMagery))
				{
					state.Mobile.SendMessage("There was a conflict, magery must be allowed for summoning to be allowed.");
				}
				else if (m_Selected == 1 && (info.IsSwitched((int)Buttons.AllowPoisonedWeaps) || info.IsSwitched((int)Buttons.AllowMagicWeaps)))
				{
					state.Mobile.SendMessage("There was a conflict, it does not make sense to allowe magic/poisoned weapons in a 5x Mage duel.");
				}
				else
				{
					try
					{
						int wagerVal = Convert.ToInt32(wager.Text);
						int dexVal = Convert.ToInt32(maxDex.Text);
						if (wagerVal < 0)
						{
							state.Mobile.SendMessage("Bad format. Wager must be a positive interger");
						}
						else if (dexVal < 0 || dexVal > 100)
						{
							state.Mobile.SendMessage("Bad format. Maximum Dex must be an interger between 0 and 100");
						}
						else
						{
							state.Mobile.SendMessage("Select a player to challenge");
							state.Mobile.Target = new ChallengeTarget(m_Selected, info.IsSwitched((int)Buttons.AllowPotions), info.IsSwitched((int)Buttons.AllowSummoning),
								info.IsSwitched((int)Buttons.AllowLooting), info.IsSwitched((int)Buttons.AllowPoisonedWeaps), info.IsSwitched((int)Buttons.AllowMagicWeaps),
								info.IsSwitched((int)Buttons.AllowMagery), wagerVal, dexVal);
						}
					}
					catch(Exception e)
					{
						Console.WriteLine("Source: {0}   Message: {1} Trace: {2}", e.Source, e.Message, e.StackTrace);
						state.Mobile.SendMessage("Bad format. Wager and Maximum  Dex entries must be an interger");
					}
				}
			}
		}
	}

	public class ChallengeTarget : Target
	{
		private int m_DuelType;
		private bool m_Potions;
		private bool m_Summoning;
		private bool m_Looting;
		private bool m_PoisonWeaps;
		private bool m_MagicWeaps;
		private bool m_Magery;
		private int m_Wager;
		private int m_Dex;

		public ChallengeTarget(int duelType, bool potions, bool summoning, bool looting, bool poisonWeaps, bool magicWeaps, bool magery, int wager, int dex) : base( -1, false, TargetFlags.None )
		{
			this.m_DuelType = duelType;
			this.m_Potions = potions;
			this.m_Summoning = summoning;
			this.m_Looting = looting;
			this.m_PoisonWeaps = poisonWeaps;
			this.m_MagicWeaps = magicWeaps;
			this.m_Magery = magery;
			this.m_Wager = wager;
			this.m_Dex = dex;
		}

		protected override void OnTarget(Mobile from, object targ)
		{

			if (targ is PlayerMobile && (PlayerMobile)targ != from)
			{
				PlayerMobile targeted = (PlayerMobile)targ;
				if (!targeted.AllowChallenge)
				{
					from.SendMessage("That person is not accepting challenges");
				}
				else if (Ladder.Duellers.Contains(targeted))
				{
					from.SendMessage("That person is already involved in a duel");
				}
				else
				{
					if (!Ladder.Players.Contains(targeted))
					{
						Ladder.Players.Add(targeted);
						Ladder.Players.Sort();
					}
					from.SendMessage("You challenge it to a duel.");
					new DuelObject(from, targeted, m_DuelType, m_Potions, m_Summoning, m_Looting, m_PoisonWeaps, m_MagicWeaps, m_Magery, m_Wager, m_Dex);
				}
			}
			else
			{
				from.SendMessage("You cannot challenge that.");
			}
		}
	}
}