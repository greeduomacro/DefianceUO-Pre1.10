using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Menus;
using Server.Menus.Questions;
using Server.Accounting;
using Server.Multis;
using Server.Mobiles;

namespace Server.Engines.Help
{
	public class ContainedMenu : QuestionMenu
	{
		private Mobile m_From;

		public ContainedMenu( Mobile from ) : base( "You already have an open help request. We will have someone assist you as soon as possible.  What would you like to do?", new string[]{ "Leave my old help request like it is.", "Remove my help request from the queue." } )
		{
			m_From = from;
		}

		public override void OnCancel( NetState state )
		{
			m_From.SendLocalizedMessage( 1005306, "", 0x35 ); // Help request unchanged.
		}

		public override void OnResponse( NetState state, int index )
		{
			if ( index == 0 )
			{
				m_From.SendLocalizedMessage( 1005306, "", 0x35 ); // Help request unchanged.
			}
			else if ( index == 1 )
			{
				PageEntry entry = PageQueue.GetEntry( m_From );

				if ( entry != null && entry.Handler == null )
				{
					m_From.SendLocalizedMessage( 1005307, "", 0x35 ); // Removed help request.
					PageQueue.Remove( entry );
				}
				else
				{
					m_From.SendLocalizedMessage( 1005306, "", 0x35 ); // Help request unchanged.
				}
			}
		}
	}

	public class HelpGump : Gump
	{
		public static void Initialize()
		{
			EventSink.HelpRequest += new HelpRequestEventHandler( EventSink_HelpRequest );
		}

		private static void EventSink_HelpRequest( HelpRequestEventArgs e )
		{
			foreach ( Gump g in e.Mobile.NetState.Gumps )
			{
				if ( g is HelpGump )
					return;
			}

			if ( !PageQueue.CheckAllowedToPage( e.Mobile ) )
				return;

			if ( PageQueue.Contains( e.Mobile ) )
				e.Mobile.SendMenu( new ContainedMenu( e.Mobile ) );
			else
				e.Mobile.SendGump( new HelpGump( e.Mobile ) );
		}

		private static bool IsYoung( Mobile m )
		{
			if ( m is PlayerMobile )
				return ((PlayerMobile)m).Young;

			return false;
		}

		public static bool CheckCombat( Mobile m )
		{
			for ( int i = 0; i < m.Aggressed.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo)m.Aggressed[i];

				if ( DateTime.Now - info.LastCombatTime < TimeSpan.FromSeconds( 30.0 ) )
					return true;
			}

			return false;
		}

        public HelpGump(Mobile from)
            : base(0, 0)
        {
            from.CloseGump(typeof(HelpGump));

            bool isYoung = IsYoung(from);

            AddBackground(50, 25, 540, 430, 2600);

            AddPage(0);

            AddHtml(150, 50, 360, 40, @"<CENTER><U>Defiance Help Menu</U></CENTER>", false, false);
            AddButton(425, 412, 2073, 2072, 0, GumpButtonType.Reply, 0); // Close

            int x = 80;
            int y = 75;

            AddPage(1);
            if (isYoung)
            {
                AddButton(x, y, 5540, 5541, 9, GumpButtonType.Reply, 2);
                AddHtml(x+30, y, 450, 58, @"<BODY><BASEFONT COLOR=BLACK><u>Young Player Haven Transport.</u> Select this option if you want to be transported to Haven.</BODY>", true, true);
                y += 65;
            }
            AddButton(x, y, 5540, 5541, 0, GumpButtonType.Page, 2);
            AddHtml(x+30, y, 450, 58, @"<u>General Question about Ultima Online or Defiance</u> Select this option if you have a general game play question or request staff assistance.", true, true);
            y += 65;

            AddButton(x, y, 5540, 5541, 2, GumpButtonType.Reply, 0);
            AddHtml(x+30, y, 450, 58, @"<u>My character is physically stuck in game</u> Please use this option only if you are physically stuck. You will only be moved if you are at a location with no way to leave. Please do not use this option and explore your surrounding if you are lost. This will only work twice in 24 hours.", true, true);
            y += 65;

            AddButton(x, y, 5540, 5541, 7, GumpButtonType.Reply, 0);
            AddHtml(x+30, y, 450, 58, @"<u>Report racism</u> Racism is against our terms of service and will not be tolerated on Defiance Networks, please use this to report any racism issue. We will handle it seriously.", true, true);
            y += 65;

            AddButton(x, y, 5540, 5541, 0, GumpButtonType.Page, 3);
            AddHtml(x+30, y, 450, 58, @"<u>Account/Donation problem</u> Please use this option if you have any problem with account or donation.", true, true);

            AddPage(2);
            //4th value = 280
            AddHtml(110, 90, 450, 150, @"<u>Notice:</u> Please note that staffs are not allowed to reveal any information to individual regarding quests and events. All event announcements will be broadcasted shard wide; we do not inform players individually. Event prize will be announced should the event organizer wish to reveal it. <br><br>You might find following web sites helpful.", true, true);

            AddHtml(110, 240, 450, 20, "<a href=\"http://www.defianceuo.com\">Defianceuo Homepage</a>", false, false);
            AddHtml(110, 260, 450, 20, "<a href=\"http://www.defianceuo.com/knowledgevault\">Defianceuo knowledge base</a>", false, false);
            AddHtml(110, 280, 450, 20, "<a href=\"http://www.defianceuo.com/forum\">Defianceuo forum</a>", false, false);
            AddHtml(110, 300, 450, 20, "<a href=\"http://www.mydotdot.com/dfi\">Defianceuo Unofficial FAQ</a>", false, false);
            AddHtml(110, 320, 450, 20, "<a href=\"http://uo.stratics.com\">UO Stratics</a>", false, false);
            AddHtml(110, 340, 450, 20, "<a href=\"http://www.uo.com\">UO.com</a>", false, false);

            AddButton(110, 390, 5540, 5541, 1, GumpButtonType.Reply, 0);
            AddHtml(140, 390, 450, 40, "I have read and understood the statement above. I would like to contact a member of staff team.", false, false);

            AddPage(3);
            AddHtml(110, 90, 450, 240, "<u>Account/Donation problem</u><br>The Standard practice for donation Problems:<br>If you have tried to use the command [claimdonations and there's nothing delivered into your bank box or any other problem concerning donations, please contact our donation service via email: <a href=\"mailto:Admin@defianceuo.com\">Admin@defianceuo.com</a><br><br>If you got account Problems of any kind please also contact us via email: <a href=\"mailto:Admin@defianceuo.com\">Admin@defianceuo.com</a><br><br>We will respond to you as soon as possible.", true, true);

            AddButton(110, 353, 5540, 5541, 5, GumpButtonType.Reply, 0);
            AddHtml(140, 355, 450, 40, "I want to page about a donation or an accounting problem.", false, false);

            AddButton(110, 380, 5540, 5541, 10, GumpButtonType.Reply, 0);
            AddHtml(140, 382, 450, 40, "I have a character transfer ticket and I would like to use it.", false, false);
        }

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			PageType type = (PageType)(-1);

			switch ( info.ButtonID )
			{
				case 0: // Close/Cancel
				{
					from.SendLocalizedMessage( 501235, "", 0x35 ); // Help request aborted.

					break;
				}
				case 1: // General question
				{
					type = PageType.Question;
					break;
				}
				case 2: // Stuck
				{
					BaseHouse house = BaseHouse.FindHouseAt( from );

					if ( house != null && house.IsAosRules )
					{
						from.Location = house.BanLocation;
					}
					else if ( from.Region is Server.Regions.Jail )
					{
						from.SendLocalizedMessage( 1041530, "", 0x35 ); // You'll need a better jailbreak plan then that!
					}
					else if ( Factions.Sigil.ExistsOn( from ) )
					{
						from.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
					}
					else if ( from.CanUseStuckMenu() && from.Region.CanUseStuckMenu( from ) && !CheckCombat( from ) && !from.Frozen && !from.Criminal && (Core.AOS || from.Kills < 5) )
					{
						StuckMenu menu = new StuckMenu( from, from, true );

						menu.BeginClose();

						from.SendGump( menu );
					}
					else
					{
						type = PageType.Stuck;
					}

					break;
				}
				case 3: // Report bug or contact Origin
				{
					type = PageType.Bug;
					break;
				}
				case 4: // Game suggestion
				{
					type = PageType.Suggestion;
					break;
				}
				case 5: // Account management
				{
					type = PageType.Account;
					break;
				}
				case 6: // Other
				{
					type = PageType.Other;
					break;
				}
				case 7: // Harassment: verbal/exploit
				{
					type = PageType.VerbalHarassment;
					break;
				}
				case 8: // Harassment: physical
				{
					type = PageType.PhysicalHarassment;
					break;
				}
				case 9: // Young player transport
				{
					if ( IsYoung( from ) )
					{
						if ( from.Region is Regions.Jail )
						{
							from.SendLocalizedMessage( 1041530, "", 0x35 ); // You'll need a better jailbreak plan then that!
						}
/*
						else if ( from.Region.Name == "Haven" )
						{
							from.SendLocalizedMessage( 1041529 ); // You're already in Haven
						}
*/
						else
						{
							from.MoveToWorld( new Point3D( 3618, 2587, 0 ), Map.Felucca );
						}
					}

					break;
				}
                case 10: // char transfer
                {
                    PageQueue.Enqueue(new PageEntry(from, "CharacterTransferTicket usage request", PageType.CharTransfer));
                    break;
                }
			}

			if ( type != (PageType)(-1) && PageQueue.CheckAllowedToPage( from ) )
				from.SendGump( new PagePromptGump( from, type ) );
		}
	}
}