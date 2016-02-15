using System;
using System.Collections;
using System.IO;
using Server;
using Server.Scripts.Commands;
using Server.Network;
using Server.Factions;
using Server.Accounting;
using Server.Mobiles;
using Server.Guilds;
//using UODreams.Jail;

namespace Server.Gumps
{
	public enum FactionsManageGumpPage
	{
		Faction_select,
		Main,
		Details,
		AdvancedFilter
	}

	public class FactionsManageGump : Gump
	{
		private Mobile m_From;
		private FactionsManageGumpPage m_PageType;

		private string m_Message;
		private string m_MarkedMessage;

		private ArrayList m_List;
		private ArrayList m_OrigList;
		private ArrayList m_Checked;

		private int m_EntryCount = 12;
		private int m_ListPage = 0;
		private int m_Index = 0;

		private Faction m_Faction = null;

		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;
		private const int RedHue = 0x20;
		private const int GrayHue = 0x76C;
		private const int YellowHue = 0x33;
		private const int OrangeHue = 0x841;
		private const int NameHue = 0x516;

		private const int standardButtonIDUp = 0xFA5;
		private const int standardButtonIDDown = 0xFA7;

		public static void Initialize()
		{
			Server.Commands.Register( "FactionsManage", AccessLevel.Seer, new CommandEventHandler( FactionsManage_OnCommand ) );
		}

		[Usage( "FactionsManage" )]
		[Description( "Opens a gump which allows to manage players in Factions." )]
		public static void FactionsManage_OnCommand( CommandEventArgs arg )
		{
			Mobile from = arg.Mobile;

			from.CloseGump( typeof( FactionsManageGump ) );
			from.SendGump( new FactionsManageGump( from ) );
		}

		public FactionsManageGump( Mobile from ) : this( from, FactionsManageGumpPage.Faction_select, null, null, null, null )
		{
		}

		public FactionsManageGump( Mobile from, ArrayList list, ArrayList rads ) : this( from, FactionsManageGumpPage.Main, list, rads, null, null )
		{
		}

		public FactionsManageGump( Mobile from, FactionsManageGumpPage page, ArrayList list, ArrayList toDelete, ArrayList original, object state ) : base( 50, 40 )
		{
			m_From = from;
			m_PageType = page;
			m_List = ( list != null ? list : new ArrayList() );
			m_Checked = ( toDelete != null ? toDelete : new ArrayList() );
			m_OrigList = ( original != null ? original : new ArrayList( m_List ) );

			if ( state != null )
			{
				object[] states = (object[])state;
				m_ListPage = (int)states[0];
				m_Index = (int)states[1];
				m_Faction = (Faction)states[2];
			}

			m_Message = String.Format( "{0} player{1} found.", m_List.Count == 1 ? "One" : m_List.Count.ToString(), m_List.Count == 1 ? "" : "s" );
			m_MarkedMessage = String.Format( "Selected: {0}/{1}" , m_Checked.Count, m_OrigList == null ? m_List.Count : m_OrigList.Count );

			InitializeGump();
		}

		private void addPrevNextButtons( ArrayList list, int page, int index )
		{
			if ( page > 0 )
				AddButton( 475, 102, 0x15E3, 0x15E7, 31, GumpButtonType.Reply, 0 ); // Previous ( 31 )
			else
				AddImage( 475, 102, 0x25EA );

			if ( index < list.Count )
				AddButton( 492, 102, 0x15E1, 0x15E5, 32, GumpButtonType.Reply, 0 ); // Next ( 32 )
			else
				AddImage( 492, 102, 0x25E6 );
		}

		private void addBottomButtons()
		{
			addBottomButtons( false );
		}

		private void addBottomButtons( bool onlyBack )
		{
			int height = m_PageType == FactionsManageGumpPage.Main ? m_EntryCount * 20 : 240;
			int startY = 130 + height; // 370
			int offset = 22;
			int labelLength = 210;
			int x1 = 10;
			int x2 = 265;
			int xdiff = 35;

			switch ( m_PageType )
			{
				case FactionsManageGumpPage.Faction_select:
				{
					break;
				}
				case FactionsManageGumpPage.Main:
				{
					AddLabelCropped( x1 + xdiff, startY, labelLength, offset, LabelHue, "Mark all" );
					AddButton( x1, startY, standardButtonIDUp, standardButtonIDDown, 1, GumpButtonType.Reply, 0 ); // Mark all ( 1 )

					AddLabelCropped( x2 + xdiff, startY, labelLength, offset, LabelHue, "Force Faction Leaving" );
					AddButton( x2, startY, standardButtonIDUp, standardButtonIDDown, 5, GumpButtonType.Reply, 0 ); // Force faction leaving ( 5 )

					AddLabelCropped( x1 + xdiff, startY + offset, labelLength, offset, LabelHue, "Unmark all" );
					AddButton( x1, startY + offset, standardButtonIDUp, standardButtonIDDown, 2, GumpButtonType.Reply, 0 ); // Unmark all ( 2 )

					AddLabelCropped( x2 + xdiff, startY + offset, labelLength, offset, LabelHue, "Kick from Faction" );
					AddButton( x2, startY + offset, standardButtonIDUp, standardButtonIDDown, 6, GumpButtonType.Reply, 0 ); // Kick from Faction ( 6 )

					AddLabelCropped( x1 + xdiff, startY + 2 * offset, labelLength, offset, LabelHue, "Mark players in Leaving" );
					AddButton( x1, startY + 2 * offset, standardButtonIDUp, standardButtonIDDown, 3, GumpButtonType.Reply, 0 ); // Mark players in Leaving ( 3 )

					AddLabelCropped( x2 + xdiff, startY + 2 * offset, labelLength, offset, LabelHue, "Ban from Faction" );
					AddButton( x2, startY + 2 * offset, standardButtonIDUp, standardButtonIDDown, 7, GumpButtonType.Reply, 0 ); // Ban from Faction ( 7 )

					AddLabelCropped( x1 + xdiff, startY + 3 * offset, labelLength, offset, LabelHue, "Advanced filter" );
					AddButton( x1, startY + 3 * offset, standardButtonIDUp, standardButtonIDDown, 4, GumpButtonType.Reply, 0 ); // Advanced filter ( 4 )

					AddLabelCropped( x2 + xdiff, startY + 3 * offset, labelLength, offset, LabelHue, "Unban from Faction" );
					AddButton( x2, startY + 3 * offset, standardButtonIDUp, standardButtonIDDown, 8, GumpButtonType.Reply, 0 ); // Unban from Faction ( 8 )

					break;
				}
				case FactionsManageGumpPage.Details:
				{
					if ( onlyBack )
						goto default;

					AddButton( x1, startY, standardButtonIDUp, standardButtonIDDown, 111, GumpButtonType.Reply, 0 ); // Props ( 111 )
					AddLabelCropped( x1 + xdiff, startY, labelLength, offset, LabelHue, "Properties" );

					AddButton( x2, startY, standardButtonIDUp, standardButtonIDDown, 114, GumpButtonType.Reply, 0 ); // GoTo ( 114 )
					AddLabelCropped( x2 + xdiff, startY, labelLength, offset, LabelHue, "Go to them" );

					AddButton( x2, startY + offset, standardButtonIDUp, standardButtonIDDown, 116, GumpButtonType.Reply, 0 ); // Skills ( 116 )
					AddLabelCropped( x2 + xdiff, startY + offset, labelLength, offset, LabelHue, "Skills" );

					AddButton( x2, startY + 2 * offset, standardButtonIDUp, standardButtonIDDown, 112, GumpButtonType.Reply, 0 ); // Jail ( 112 )
					AddLabelCropped( x2 + xdiff, startY + 2 * offset, labelLength, offset, LabelHue, "Jail" );

					AddButton( x1, startY + offset, standardButtonIDUp, standardButtonIDDown, 113, GumpButtonType.Reply, 0 ); // Account properties ( 113 )
					AddLabelCropped( x1 + xdiff, startY + offset, labelLength, offset, LabelHue, "Account properties" );

					if ( m_From.AccessLevel >= AccessLevel.Administrator )
					{
						AddButton( x1, startY + 2 * offset, standardButtonIDUp, standardButtonIDDown, 115, GumpButtonType.Reply, 0 ); // Ban account ( 115 )
						AddLabelCropped( x1 + xdiff, startY + 2 * offset, labelLength, offset, LabelHue, "Ban account" );
					}

					goto default;
				}
				default:
				{
					AddLabelCropped( x1 + xdiff, startY + 3 * offset, labelLength, 22, LabelHue, "Back" );
					AddButton( x1, startY + 3 * offset, 0xFAE, 0xFB0, 101, GumpButtonType.Reply, 0 ); // Back ( 101 )
					break;
				}
			}
		}

		private void addSearchButtons()
		{
			if ( m_PageType == FactionsManageGumpPage.Faction_select )
				return;

			AddImageTiled(  285, 15, 220, 25, 0x248A );
			AddAlphaRegion( 286, 16, 218, 23 );
			AddTextEntry( 290, 20, 210, 16, LabelHue, 0, "" ); // insert text here

			AddLabelCropped( 285, 40, 220, 22, LabelHue, "Search for:" );

			AddButton( 285, 65, standardButtonIDUp, standardButtonIDDown, 11, GumpButtonType.Reply, 0 ); // Search for Name ( 11 )
			AddButton( 360, 65, standardButtonIDUp, standardButtonIDDown, 12, GumpButtonType.Reply, 0 ); // Search for Serial ( 12 )
			AddButton( 435, 65, standardButtonIDUp, standardButtonIDDown, 13, GumpButtonType.Reply, 0 ); // Search for Guild ( 13 )

			AddLabelCropped( 315, 65, 40, 22, LabelHue, "Name" );
			AddLabelCropped( 390, 65, 40, 22, LabelHue, "Serial" );
			AddLabelCropped( 465, 65, 40, 22, LabelHue, "Guild" );
		}

		private void addTitleBox()
		{
			if ( m_PageType != FactionsManageGumpPage.Faction_select )
			{
				AddLabelCropped(  15, 10, 250, 19, GreenHue, "Players in current Faction" );
				AddLabelCropped(  15, 30, 250, 19, LabelHue, m_Message );
				AddLabelCropped(  15, 50, 250, 19, LabelHue, m_MarkedMessage );
			}

			switch ( m_PageType )
			{
				case FactionsManageGumpPage.Main:
				{
					string filter = ( m_Faction == null ? "(disabled)" : m_Faction.ToString() );

					AddButton( 15, 68, standardButtonIDUp, standardButtonIDDown, 103, GumpButtonType.Reply, 0 ); // Show factions ( 103 )
					AddLabelCropped(  50, 69,  98, 20, LabelHue, "Faction:" );
					AddLabelCropped( 150, 69, 115, 20, m_Faction == null ? GrayHue : YellowHue, filter );
					break;
				}
				case FactionsManageGumpPage.Details:
				{
					AddLabelCropped(  15, 69,  250, 20, GrayHue, "Selected player's details" );
					break;
				}
				case FactionsManageGumpPage.AdvancedFilter:
				{
					AddLabelCropped(  15, 69,  250, 20, GrayHue, "Advanced selection" );
					break;
				}
				default: break;
			}
		}

		public virtual void InitializeGump()
		{
			int height = m_PageType == FactionsManageGumpPage.Main ? m_EntryCount * 20 : 240;

			AddPage( 0 );

			if ( m_PageType != FactionsManageGumpPage.Faction_select )
			{
				AddBackground( 0, 0, 520, 228 + height, 0x13BE ); // 468

				AddImageTiled(   10,  10, 260,  80, 2624 );
				AddAlphaRegion(  10,  10, 260,  80 ); // top left
				AddImageTiled(  280,  10, 230,  80, 2624 );
				AddAlphaRegion( 280,  10, 230,  80 ); // top right
				AddImageTiled(   10, 130 + height, 500,  88, 2624 );
				AddAlphaRegion(  10, 130 + height, 500,  88 ); // bottom
			}
			else
			{
				AddBackground( 0, 90, 520, 40 + height, 0x13BE );
			}

			AddImageTiled(   10, 100, 500, 20 + height, 2624 );
			AddAlphaRegion(  10, 100, 500, 20 + height ); // middle

			addTitleBox();

			addSearchButtons();

			switch ( m_PageType )
			{
				case FactionsManageGumpPage.Main:
				{
					int l1 = 70, l2 = 70, l3 = 40, l4 = 85, l5 = 45, l6 = 15, l7 = 109;
					int x1 = 32, x2 = x1 + l1 + 2, x3 = x2 + l2 + 2, x4 = x3 + l3 + 2, x5 = x4 + l4 + 2, x6 = x5 + l5 + 2, x7 = x6 + l6 + 2;

					AddLabelCropped( x1, 100, l1, 20, LabelHue, "Name" );
					AddLabelCropped( x2, 100, l2, 20, LabelHue, "Account" );
					AddLabelCropped( x3, 100, l3, 20, LabelHue, "Guild" );
					AddLabelCropped( x4, 100, l4, 20, LabelHue, "Last login" );
					AddLabelCropped( x5, 100, l5, 20, LabelHue, "Hours" );
					AddLabelCropped( x6, 100, l6, 20, LabelHue, "L" );
					AddLabelCropped( x7, 100, l7, 20, LabelHue, "Uselessness" );

					if ( m_List.Count == 0 )
						AddLabel( 12, 120, LabelHue, "No players found." );

					int index = ( m_ListPage * m_EntryCount );
					for ( int i = 0; i < m_EntryCount && index >= 0 && index < m_List.Count; i++, index++ )
					{
						PlayerMobile player = m_List[index] as PlayerMobile;

						if ( player == null )
							continue;

						int offset = 120 + ( i * 20 );
						string name = player.RawName;
						Account acct = player.Account as Account;
						string account = (acct == null ? "" : acct.Username );
						Guild gld = player.Guild as Guild;
						string guild = (gld == null ? "" : gld.Abbreviation );
						string lastLogin = (acct == null ? "N/A" : acct.LastLogin.ToString( "yyyy/MM/dd" ));
						string gameTime = String.Format( "{0}", player.GameTime.TotalHours.ToString( "f1" ));
						PlayerState pl = PlayerState.Find( player );
						string leaving = (pl == null ? "" : (pl.IsLeaving ? "?" : "" ));
						int uselessness = computeUselessness( pl );

						AddLabelCropped( x1, offset, l1, 20, NameHue, name ); // Name
						AddLabelCropped( x2, offset, l2, 20, GrayHue, account ); // Account
						AddLabelCropped( x3, offset, l3, 20, LabelHue, guild ); // Guild
						AddLabelCropped( x4, offset, l4, 20, LabelHue, lastLogin ); // Last login
						AddLabelCropped( x5, offset, l5, 20, YellowHue, gameTime ); // Game time
						AddLabelCropped( x6, offset, l6, 20, pl == null ? LabelHue : pl.Leaving + Faction.LeavePeriod < DateTime.Now ? RedHue : GreenHue, leaving ); // Living

						AddImage( x7, offset + 4, uselessness < 109 ? 0x808 : uselessness < 218 ? 0x809 : 0x805 ); // Uselessness
						AddImageTiled( x7, offset + 4, 109 - uselessness % 109, 11, uselessness < 109 ? 0x806 : uselessness < 218 ? 0x808 : uselessness < 327 ? 0x809 : 0x805 );
						AddHtml( x7, offset, l7, 20, String.Format( "<BASEFONT COLOR=#FFFFFF><CENTER>{0}%</CENTER></BASEFONT>", uselessness * 100 / 327 ), false, false );

						if ( !player.Deleted )
						{
							AddCheck( 10, offset, 0xD2, 0xD3, m_Checked.Contains( player ), index );

							if ( pl != null )
								AddButton( 480, offset - 1, 0xFAB, 0xFAD, 1000 + index, GumpButtonType.Reply, 0 ); // Show details ( 1000 + index )
						}
					}

					addPrevNextButtons( m_List, m_ListPage, index );
					addBottomButtons();
					break;
				}

				case FactionsManageGumpPage.Details:
				{
					bool onlyBack = false;
					int offset = 100;
					int suboffset = 18;
					int dimX1 = 220;
					int dimX2 = 250;
					int dimY = 20;
					int x1 = 20;
					int x2 = 250;

					int index = m_Index;
					PlayerMobile player = m_List[index] as PlayerMobile;
					PlayerState pl = PlayerState.Find( player );

					if ( player == null || player.Deleted )
					{
						AddLabel( 12, offset, LabelHue, "The player does not exist or has been deleted." );
						onlyBack = true;
					}
					else if ( pl == null )
					{
						AddLabel( 12, offset, LabelHue, "The player is not in a Faction." );
						onlyBack = true;
					}
					else
					{
						string name = player.RawName;
						Account acct = player.Account as Account;
						string account = (acct == null ? "" : acct.Username);
						Guild gld = player.Guild as Guild;
						string guild = (gld == null ? "" : String.Format("{0} ({1})", gld.Abbreviation, gld.Name ));
						string guildMaster = (gld == null ? "" : String.Format( "{0} ({1})", gld.Leader, gld.Leader == null ? "0x00000000" : gld.Leader.Serial.ToString() ));
						string serial = player.Serial.ToString();
						int days = (int)(DateTime.Now - acct.LastLogin).TotalDays;
						string lastLogin = acct.LastLogin.ToString( "yyyy/MM/dd HH.mm" ) + String.Format( " [-{0} day{1}]", days, days == 1 ? "" : "s" );
						TimeSpan time = player.GameTime;
						string gameTime = time > TimeSpan.FromMinutes( 1.0 ) ? String.Format( "{0}{1}{2}", time.Days > 0 ? String.Format( "{0} day{1} ", time.Days, time.Days == 1 ? "" : "s" ) : "", time.Hours > 0 ? String.Format( "{0} hour{1} ", time.Hours, time.Hours == 1 ? "" : "s" ) : "", time.Minutes > 0 ? String.Format( "{0} minute{1} ", time.Minutes, time.Minutes == 1 ? "" : "s" ) : "" ) : "less than one minute";
						string leaving = (pl.IsLeaving ? "true" : "false");
						string skills = ((double)player.SkillsTotal / 10).ToString( "f1" ) + String.Format( " [highest: {0} ({1})]", player.Skills.Highest.Name, player.Skills.Highest.Base.ToString( "f1" ) );
						string alive = (player.Alive ? "true" : "false");
						string faction = pl.Faction.ToString();
						int uselessnessLevel = computeUselessnessPercent( pl );

						AddLabel( 230, offset, LabelHue, "Informations" ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Name:" );
						AddLabelCropped( x2, offset, dimX2, dimY, NameHue, name ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Account:" );
						AddLabelCropped( x2, offset, dimX2, dimY, GrayHue, account ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Guild:" );
						AddLabelCropped( x2, offset, dimX2, dimY, LabelHue, guild ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Guild Master:" );
						AddLabelCropped( x2, offset, dimX2, dimY, LabelHue, guildMaster ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Serial:" );
						AddLabelCropped( x2, offset, dimX2, dimY, GreenHue, serial ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Last login:" );
						AddLabelCropped( x2, offset, dimX2, dimY, LabelHue, lastLogin ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Game time:" );
						AddLabelCropped( x2, offset, dimX2, dimY, LabelHue, gameTime ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Leaving from Faction:" );
						AddLabelCropped( x2, offset, dimX2, dimY, pl.IsLeaving ? RedHue : LabelHue, leaving ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Leaving start time:" );
						AddLabelCropped( x2, offset, dimX2, dimY, pl.IsLeaving ? ((pl.Leaving + Faction.LeavePeriod) < DateTime.Now ? RedHue : GreenHue) : LabelHue, pl.IsLeaving ? pl.Leaving.ToString( "yyyy/MM/dd HH.mm" ) : "" ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Total skills:" );
						AddLabelCropped( x2, offset, dimX2, dimY, LabelHue, skills ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Alive:" );
						AddLabelCropped( x2, offset, dimX2, dimY, LabelHue, alive ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Faction:" );
						AddLabelCropped( x2, offset, dimX2, dimY, YellowHue, faction ); offset += suboffset;
						AddLabelCropped( x1, offset, dimX1, dimY, LabelHue, "Uselessness level:" );
						AddLabelCropped( x2, offset, dimX2, dimY, uselessnessLevel <= 0 ? LabelHue : (uselessnessLevel < 34 ? GreenHue : (uselessnessLevel < 67 ? YellowHue : (uselessnessLevel < 100 ? OrangeHue : RedHue))), uselessnessLevel.ToString() + '%' );
					}

					addPrevNextButtons( m_List, index, index + 1 );
					addBottomButtons( onlyBack );
					break;
				}

				case FactionsManageGumpPage.AdvancedFilter:
				{
					int x1 = 20;
					int x2 = 55;
					int y = 150;
					int offset = 20;

					AddHtml( 20, 100, 480, 40, "<basefont color=yellow>Choose one of the following options to filter players or to refine your current filter.</basefont>", false, false );

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 41, GumpButtonType.Reply, 0 ); // Skills filter ( 41 )
					AddLabel( x2, y + 1, LabelHue, "Show all players with less skills than:" );
					AddImageTiled(  335, y, 60, 21, 0x248A );
					AddAlphaRegion( 336, y + 1, 58, 19 );
					AddTextEntry( 340, y + 1, 50, 16, LabelHue, 3, "600" ); // total skills
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 42, GumpButtonType.Reply, 0 ); // Game time filter ( 42 )
					AddLabel( x2, y + 1, LabelHue, "Show all players with less hours than:" );
					AddImageTiled(  335, y, 60, 21, 0x248A );
					AddAlphaRegion( 336, y + 1, 58, 19 );
					AddTextEntry( 340, y, 50, 16, LabelHue, 4, "10" ); // total hours of connect time
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 43, GumpButtonType.Reply, 0 ); // Last login filter ( 43 )
					AddLabel( x2, y + 1, LabelHue, "Show all players not logged in for days:" );
					AddImageTiled(  335, y, 60, 21, 0x248A );
					AddAlphaRegion( 336, y + 1, 58, 19 );
					AddTextEntry( 340, y + 1, 50, 16, LabelHue, 5, "30" ); // days since last login time
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 44, GumpButtonType.Reply, 0 ); // Not in a guild filter ( 44 )
					AddLabel( x2, y + 1, LabelHue, "Show all players who are not in a guild." );
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 45, GumpButtonType.Reply, 0 ); // Dead filter ( 45 )
					AddLabel( x2, y + 1, LabelHue, "Show all players who are not alive." );
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 46, GumpButtonType.Reply, 0 ); // Leaving expired filter ( 46 )
					AddLabel( x2, y + 1, LabelHue, "Show all players whose leaving period is expired." );
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 47, GumpButtonType.Reply, 0 ); // Uselessness filter ( 47 )
					AddLabel( x2, y + 1, LabelHue, "Show all players with uselessness above:" );
					AddImageTiled(  335, y, 60, 21, 0x248A );
					AddAlphaRegion( 336, y + 1, 58, 19 );
					AddTextEntry( 340, y + 1, 50, 16, LabelHue, 6, "67" ); // minimum uselessness
					y += offset;

					AddButton( x1, y, standardButtonIDUp, standardButtonIDDown, 48, GumpButtonType.Reply, 0 ); // Highest skill filter ( 48 )
					AddLabel( x2, y + 1, LabelHue, "Show all players with highest skill below:" );
					AddImageTiled(  335, y, 60, 21, 0x248A );
					AddAlphaRegion( 336, y + 1, 58, 19 );
					AddTextEntry( 340, y + 1, 50, 16, LabelHue, 7, "90" ); // highest skill

					AddCheck( 20, 330, 0xD2, 0xD3, false, -1 ); // refine selection
					AddLabel( 42, 331, LabelHue, "Check this if you want to refine your current filter." );

					addBottomButtons();
					break;
				}

				case FactionsManageGumpPage.Faction_select:
				{
					AddLabel( 55, 111, GrayHue, "Please select one of the following factions." );

					FactionCollection factions = Faction.Factions;

					if ( factions.Count > 0 )
					{
						AddButton( 50, 150, standardButtonIDUp, standardButtonIDDown, 21, GumpButtonType.Reply, 0 ); // Faction 1 ( 21 )
						AddLabel( 85, 151, LabelHue, String.Format( "{0} [Members: {1}]", factions[0].ToString(), factions[0].Members.Count ) );
					}

					if ( factions.Count > 1 )
					{
						AddButton( 50, 200, standardButtonIDUp, standardButtonIDDown, 22, GumpButtonType.Reply, 0 ); // Faction 2 ( 22 )
						AddLabel( 85, 201, LabelHue, String.Format( "{0} [Members: {1}]", factions[1].ToString(), factions[1].Members.Count ) );
					}

					if ( factions.Count > 2 )
					{
						AddButton( 50, 250, standardButtonIDUp, standardButtonIDDown, 23, GumpButtonType.Reply, 0 ); // Faction 3 ( 23 )
						AddLabel( 85, 251, LabelHue, String.Format( "{0} [Members: {1}]", factions[2].ToString(), factions[2].Members.Count ) );
					}

					if ( factions.Count > 3 )
					{
						AddButton( 50, 300, standardButtonIDUp, standardButtonIDDown, 24, GumpButtonType.Reply, 0 ); // Faction 4 ( 24 )
						AddLabel( 85, 301, LabelHue, String.Format( "{0} [Members: {1}]", factions[3].ToString(), factions[3].Members.Count ) );
					}
					break;
				}
				default:
				{
					addBottomButtons();
					break;
				}
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_From.AccessLevel < AccessLevel.Seer )
				return;

			// set selected items
			if ( m_List != null && m_Checked != null && m_PageType == FactionsManageGumpPage.Main )
			{
				for ( int i = 0, index = m_ListPage * m_EntryCount; i < m_EntryCount && index < m_List.Count; i++, index++ )
				{
					PlayerMobile player = m_List[index] as PlayerMobile;

					if ( info.IsSwitched( index ) )
					{
						if ( player == null || player.Deleted )
						{
							m_Checked.Remove( player );
						}
						else if ( !m_Checked.Contains( player ) )
						{
							m_Checked.Add( player );
						}
					}
					else
					{
						m_Checked.Remove( player );
					}
				}
			}

			switch ( info.ButtonID )
			{
				case 1: // Mark all
				{
					ArrayList list = m_List;
					ArrayList rads = new ArrayList( list );

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, list, rads, m_OrigList, new object[]{ m_ListPage, m_Index, m_Faction } ) );
					break;
				}
				case 2: // Unmark all
				{
					ArrayList list = m_List;
					ArrayList rads = new ArrayList();

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, list, rads, m_OrigList, new object[]{ m_ListPage, m_Index, m_Faction } ) );
					break;
				}
				case 3: // Mark players in Leaving
				{
					ArrayList list = m_List;
					ArrayList rads = new ArrayList();

					foreach ( PlayerMobile player in list )
					{
						PlayerState pl = PlayerState.Find( player );
						if ( pl != null && pl.IsLeaving )
						{
							rads.Add( player );
						}
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, list, rads, m_OrigList, new object[]{ m_ListPage, m_Index, m_Faction } ) );
					break;
				}
				case 4: // Advanced filter
				{
					resendGump( FactionsManageGumpPage.AdvancedFilter );
					break;
				}
				case 5: // Force faction leaving
				{
					ArrayList list = m_List;
					ArrayList rads = m_Checked;

					string message = String.Format( "You are about to force the Faction Leaving state to {0} player{1}.<br><br>Would you like to continue?", rads.Count, rads.Count == 1 ? "" : "s" );

					if ( rads.Count > 0 )
						m_From.SendGump( new WarningGump( 1060635, 30720, message, 0xFFC000, 420, 280, new WarningGumpCallback( ForceLeaving_Callback ), new object[]{ list, rads, m_OrigList, m_ListPage, m_Faction } ) );
					else
						m_From.SendGump( new NoticeGump( 1060637, 30720, "You didn't select any player. Please check the boxes left to the players you wish to force in leaving state, then try again.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, m_OrigList, FactionsManageGumpPage.Main, m_ListPage, 0, m_Faction } ) );

					break;
				}
				case 6: // Kick from Faction
				{
					ArrayList list = m_List;
					ArrayList rads = m_Checked;

					string message = String.Format( "You are about to kick from Faction System {0} player{1}.<br><br>Please note that guilded players will be removed from their guild.<br><br>Would you like to continue?", rads.Count, rads.Count == 1 ? "" : "s" );

					if ( rads.Count > 0 )
						m_From.SendGump( new WarningGump( 1060635, 30720, message, 0xFFC000, 420, 280, new WarningGumpCallback( Kick_Callback ), new object[]{ list, rads, m_OrigList, m_ListPage, m_Faction } ) );
					else
						m_From.SendGump( new NoticeGump( 1060637, 30720, "You didn't select any player. Please check the boxes left to the players you wish to kick from Faction System, then try again.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, m_OrigList, FactionsManageGumpPage.Main, m_ListPage, 0, m_Faction } ) );

					break;
				}
				case 7: // Ban from Faction
				{
					ArrayList list = m_List;
					ArrayList rads = m_Checked;

					string message = String.Format( "You are about to ban from Faction System {0} player{1}. In case they are already banned, nothing will happen.<br><br>Please note that all banned players will also be kicked from their Faction. Furthermore, guilded players will be removed from their guild.<br><br>Would you like to continue?", rads.Count, rads.Count == 1 ? "" : "s" );

					if ( rads.Count > 0 )
						m_From.SendGump( new WarningGump( 1060635, 30720, message, 0xFFC000, 420, 280, new WarningGumpCallback( Ban_Callback ), new object[]{ list, rads, m_OrigList, m_ListPage, m_Faction } ) );
					else
						m_From.SendGump( new NoticeGump( 1060637, 30720, "You didn't select any player. Please check the boxes left to the players you wish to ban from Faction System, then try again.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, m_OrigList, FactionsManageGumpPage.Main, m_ListPage, 0, m_Faction } ) );

					break;
				}
				case 8: // Unban from Faction
				{
					ArrayList list = m_List;
					ArrayList rads = m_Checked;

					string message = String.Format( "You are about to unban from Faction System {0} player{1}. In case they are not currently banned, nothing will happen.<br><br>Would you like to continue?", rads.Count, rads.Count == 1 ? "" : "s" );

					if ( rads.Count > 0 )
						m_From.SendGump( new WarningGump( 1060635, 30720, message, 0xFFC000, 420, 280, new WarningGumpCallback( Unban_Callback ), new object[]{ list, rads, m_OrigList, m_ListPage, m_Faction } ) );
					else
						m_From.SendGump( new NoticeGump( 1060637, 30720, "You didn't select any player. Please check the boxes left to the players you wish to unban from Faction System, then try again.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, m_OrigList, FactionsManageGumpPage.Main, m_ListPage, 0, m_Faction } ) );

					break;
				}
				case 11: // Search for name
				case 12: // Search for serial
				case 13: // Search for guild
				{
					ArrayList results = new ArrayList();
					ArrayList list = m_OrigList;

					TextRelay matchEntry = info.GetTextEntry( 0 );
					string match = ( matchEntry == null ? null : matchEntry.Text.Trim().ToLower() );

					if ( match == null || match.Length == 0 )
					{
						m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, list, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
						break;
					}

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						bool isMatch = false;

						switch ( info.ButtonID )
						{
							case 11: // name
							{
								string name = player.RawName;
								isMatch = name.ToLower().IndexOf( match ) >= 0;
								break;
							}
							case 12: // serial (string search)
							{
								string serial = player.Serial.ToString();
								isMatch = serial.ToLower().IndexOf( match ) >= 0;
								break;
							}
							case 13: // guild abbreviation
							{
								string guild = (player.Guild == null ? String.Empty : player.Guild.Abbreviation.ToLower());
								isMatch = guild.CompareTo( match ) == 0;
								break;
							}
						}

						if ( isMatch )
							results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, list, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 21: // Faction 1
				case 22: // Faction 2
				case 23: // Faction 3
				case 24: // Faction 4
				{
					int id = info.ButtonID - 21;
					ArrayList list = new ArrayList();

					FactionCollection factions = Faction.Factions;
					PlayerStateCollection playerStates = ((Faction)factions[id]).Members;
					foreach( PlayerState playerState in playerStates )
					{
						PlayerMobile player = playerState.Mobile as PlayerMobile;
						if ( player != null && !player.Deleted )
							list.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, list, null, null, new object[]{ 0, 0, factions[id] as Faction } ) );
					break;
				}
				case 31: // Previous
				{
					switch ( m_PageType )
					{
						case FactionsManageGumpPage.Main:
							m_ListPage--;
							break;

						case FactionsManageGumpPage.Details:
							m_Index--;
							break;
					}

					m_From.SendGump( new FactionsManageGump( m_From, m_PageType, m_List, m_Checked, m_OrigList, new object[]{ m_ListPage, m_Index, m_Faction } ) );
					break;
				}
				case 32: // Next
				{
					switch ( m_PageType )
					{
						case FactionsManageGumpPage.Main:
							m_ListPage++;
							break;

						case FactionsManageGumpPage.Details:
							m_Index++;
							break;
					}

					m_From.SendGump( new FactionsManageGump( m_From, m_PageType, m_List, m_Checked, m_OrigList, new object[]{ m_ListPage, m_Index, m_Faction } ) );
					break;
				}
				case 41: // Skills filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					TextRelay matchEntry = info.GetTextEntry( 3 );
					string match = ( matchEntry == null ? null : matchEntry.Text.Trim().ToLower() );
					int skills = 0;
					try{ skills = Convert.ToInt32( match ) * 10; }
					catch{}

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						bool isMatch = player.SkillsTotal < skills;
						if ( isMatch )
							results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 42: // Game time filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					TextRelay matchEntry = info.GetTextEntry( 4 );
					string match = ( matchEntry == null ? null : matchEntry.Text.Trim().ToLower() );
					int hours = 0;
					try{ hours = Convert.ToInt32( match ); }
					catch{}

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						bool isMatch = player.GameTime < TimeSpan.FromHours( hours );
						if ( isMatch )
							results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 43: // Last login filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					TextRelay matchEntry = info.GetTextEntry( 5 );
					string match = ( matchEntry == null ? null : matchEntry.Text.Trim().ToLower() );
					int days = 120;
					try{ days = Convert.ToInt32( match ); }
					catch{}

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						Account acct = player.Account as Account;
						if ( acct == null )
							continue;

						bool isMatch = acct.LastLogin < DateTime.Now - TimeSpan.FromDays( days );
						if ( isMatch )
							results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 44: // Not in a guild filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null || player.Guild != null )
							continue;

						results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 45: // Dead filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null || player.Alive )
							continue;

						results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 46: // Leaving filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						PlayerState pl = PlayerState.Find( player );
						if ( pl != null && pl.IsLeaving && (pl.Leaving + Faction.LeavePeriod) < DateTime.Now )
                            results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 47: // Uselessness filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					TextRelay matchEntry = info.GetTextEntry( 6 );
					string match = ( matchEntry == null ? null : matchEntry.Text.Trim().ToLower() );
					int percent = 100;
					try{ percent = Convert.ToInt32( match ); }
					catch{}

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						PlayerState pl = PlayerState.Find( player );
						if ( pl != null && computeUselessnessPercent( pl ) >= percent )
							results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 48: // Highest skill filter
				{
					ArrayList results = new ArrayList();
					ArrayList list = info.IsSwitched( -1 ) ? m_List : m_OrigList;

					TextRelay matchEntry = info.GetTextEntry( 7 );
					string match = ( matchEntry == null ? null : matchEntry.Text.Trim().ToLower() );
					int skill = 120;
					try{ skill = Convert.ToInt32( match ); }
					catch{}

					for ( int i = 0; i < list.Count; i++ )
					{
						PlayerMobile player = list[i] as PlayerMobile;
						if ( player == null )
							continue;

						Skill highest = player.Skills.Highest;
						if ( highest.Base < skill )
							results.Add( player );
					}

					m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Main, results, m_Checked, m_OrigList, new object[]{ 0, 0, m_Faction } ) );
					break;
				}
				case 101: // Back
				{
					resendGump( FactionsManageGumpPage.Main );
					break;
				}
				case 103: // Show factions
				{
					resendGump( FactionsManageGumpPage.Faction_select );
					break;
				}
				case 111: // Props
				{
					ArrayList list = m_List;
					PlayerMobile player = list[m_Index] as PlayerMobile;

					resendGump( m_PageType );

					if ( player != null && !player.Deleted )
					{
						m_From.SendGump( new PropertiesGump( m_From, player ) );
					}
					else
					{
						m_From.SendMessage( 0x23, "The player does not exist or has been deleted." );
					}
					break;
				}
				case 112: // Jail
				{
					//ArrayList list = m_List;
					//PlayerMobile player = list[m_Index] as PlayerMobile;

					m_From.SendMessage( 0x23, "This feature is not implemented." );
					resendGump( m_PageType );

					/*if ( player != null && !player.Deleted && player.AccessLevel == AccessLevel.Player )
					{
						ArrayList players = new ArrayList();
						players.Add( player );
						m_From.SendGump( new JailSearchGump( players, m_From, null ) );
					}
					else
					{
						m_From.SendMessage( 0x23, "The player does not exist or has been deleted or cannot be jailed." );
					}*/

					break;
				}
				case 113: // Account properties
				{
					ArrayList list = m_List;
					PlayerMobile player = list[m_Index] as PlayerMobile;

					resendGump( m_PageType );

					if ( player != null && !player.Deleted )
					{
						Account acc = player.Account as Account;
						if ( acc != null )
							m_From.SendGump( new AdminGump( m_From, AdminGumpPage.AccountDetails_Information, 0, null, "Returned from the factions management system", acc ) );
					}
					break;
				}
				case 114: // GoTo
				{
					ArrayList list = m_List;
					PlayerMobile player = list[m_Index] as PlayerMobile;

					if ( player != null && !player.Deleted )
					{
						Map map = player.Map;
						Point3D loc = player.Location;

						if ( map == null || map == Map.Internal )
						{
							map = player.LogoutMap;
							loc = player.LogoutLocation;
						}

						if ( map != null && map != Map.Internal )
						{
							m_From.MoveToWorld( loc, map );
						}

						m_From.SendMessage( "You have been teleported to their location." );
					}
					else
					{
						m_From.SendMessage( 0x23, "The player is unreachable." );
					}

					resendGump( m_PageType );
					break;
				}
				case 115: // Ban account
				{
					if ( m_From.AccessLevel < AccessLevel.Administrator )
						break;

					PlayerMobile player = m_List[m_Index] as PlayerMobile;
					Account acct = player.Account as Account;
					if ( acct != null )
					{
						if ( !acct.Banned )
						{
							CommandLogging.WriteLine( m_From, "{0} {1} banning account {2}", m_From.AccessLevel, CommandLogging.Format( m_From ), acct.Username );
							acct.SetUnspecifiedBan( m_From );
							acct.Banned = true;

							m_From.SendGump( new NoticeGump( 1060637, 30720, "You banned the account.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ m_List, m_Checked, m_OrigList, m_PageType, m_ListPage, m_Index, m_Faction } ) );
							m_From.SendGump( new BanDurationGump( acct ) );
						}
						else
						{
							m_From.SendMessage( "This account is already banned." );
							resendGump( m_PageType );
						}
					}
					else
					{
						m_From.SendMessage( "Unable to ban the account." );
						resendGump( m_PageType );
					}
					break;
				}
				case 116: // Skills
				{
					PlayerMobile player = m_List[m_Index] as PlayerMobile;

					resendGump( m_PageType );

					if ( player != null && !player.Deleted )
						m_From.SendGump( new Server.Scripts.Gumps.SkillsGump( m_From, player ) );
					break;
				}
				default: // Show details
				{
					if ( info.ButtonID == 0 )
						break;

					int index;

					index = info.ButtonID - 1000;

					switch ( m_PageType )
					{
						case FactionsManageGumpPage.Main: // Show details
						{
							m_From.SendGump( new FactionsManageGump( m_From, FactionsManageGumpPage.Details, m_List, m_Checked, m_OrigList, new object[]{ m_ListPage, index, m_Faction } ) );
							break;
						}
					}
					break;
				}
			}
		}

		private void resendGump( FactionsManageGumpPage page )
		{
			m_From.SendGump( new FactionsManageGump( m_From, page, m_List, m_Checked, m_OrigList, new object[]{ m_ListPage, m_Index, m_Faction } ) );
		}

		private static void ResendGump_Callback( Mobile from, object state )
		{
			object[] states = (object[])state;
			ArrayList list = (ArrayList)states[0];
			ArrayList rads = (ArrayList)states[1];
			ArrayList original = (ArrayList)states[2];
			FactionsManageGumpPage gumpPage = (FactionsManageGumpPage)states[3];
			int page = (int)states[4];
			int index = (int)states[5];
			Faction faction = (Faction)states[6];

			from.SendGump( new FactionsManageGump( from, gumpPage, list, rads, original, new object[]{ page, index, faction } ) );
		}

		private static void ForceLeaving_Callback( Mobile from, bool ok, object state )
		{
			object[] states = (object[])state;
			ArrayList list = (ArrayList)states[0];
			ArrayList rads = (ArrayList)states[1];
			ArrayList original = (ArrayList)states[2];
			int page = (int)states[3];
			Faction faction = (Faction)states[4];

			int forced = 0;

			if ( ok )
			{
				for ( int i = 0; i < rads.Count; i++ )
				{
					PlayerMobile player = rads[i] as PlayerMobile;
					PlayerState pl = PlayerState.Find( player );
					if ( pl != null && !pl.IsLeaving )
					{
						Guild guild = player.Guild as Guild;

						if ( guild == null )
						{
							pl.Leaving = DateTime.Now;
							forced++;

							if ( Faction.LeavePeriod == TimeSpan.FromDays( 3.0 ) )
								player.SendLocalizedMessage( 1005065 ); // You will be removed from the faction in 3 days
							else
								player.SendMessage( "You will be removed from the faction in {0} days.", Faction.LeavePeriod.TotalDays );
						}
						else if ( guild.Leader != player )
						{
							// Cannot force leave from Faction on a single member of a guild who is not the guild master
						}
						else
						{
							player.SendLocalizedMessage( 1042285 ); // Your guild is now quitting the faction.

							for ( int j = 0; j < guild.Members.Count; ++j )
							{
								Mobile mob = (Mobile) guild.Members[j];
								PlayerState pl2 = PlayerState.Find( mob );

								if ( pl2 != null )
								{
									pl2.Leaving = DateTime.Now;
									forced++;

									if ( Faction.LeavePeriod == TimeSpan.FromDays( 3.0 ) )
										mob.SendLocalizedMessage( 1005060 ); // Your guild will quit the faction in 3 days
									else
										mob.SendMessage( "Your guild will quit the faction in {0} days.", Faction.LeavePeriod.TotalDays );
								}
							}
						}
					}
				}

				if ( forced > 0 )
				{
					string message  = String.Format( "Operation complete: {0} selected player{1} {2} been forced to leave Factions. Please note that, if you selected a guild master, the entire guild has been forced to leave.", forced == 1 ? "One" : forced.ToString(), forced == 1 ? "" : "s", forced == 1 ? "has" : "have" );
					from.SendGump( new NoticeGump( 1060637, 30720, message, 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
				}
				else
					from.SendGump( new NoticeGump( 1060637, 30720, "It was not possible to force Factions leaving to the selected players.<br><br>Maybe the selected players are already leaving or have already left a Faction.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
			else
			{
				from.SendGump( new NoticeGump( 1060637, 30720, "Aborted.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
		}

		private static void Kick_Callback( Mobile from, bool ok, object state )
		{
			object[] states = (object[])state;
			ArrayList list = (ArrayList)states[0];
			ArrayList rads = (ArrayList)states[1];
			ArrayList original = (ArrayList)states[2];
			int page = (int)states[3];
			Faction faction = (Faction)states[4];

			int kicked = 0;

			if ( ok )
			{
				for ( int i = 0; i < rads.Count; i++ )
				{
					PlayerMobile player = rads[i] as PlayerMobile;
					PlayerState pl = PlayerState.Find( player );
					if ( pl != null )
					{
						pl.Faction.RemoveMember( player );

						Guild guild = player.Guild as Guild;
						if ( guild != null && !pl.IsLeaving )
							guild.RemoveMember( player );

						player.SendMessage( "You have been kicked from your faction." );
						kicked++;
					}
				}

				if ( kicked > 0 )
				{
					string message  = String.Format( "Operation complete: {0} selected player{1} {2} been kicked from Factions.", kicked == 1 ? "One" : kicked.ToString(), kicked == 1 ? "" : "s", kicked == 1 ? "has" : "have" );
					from.SendGump( new NoticeGump( 1060637, 30720, message, 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
				}
				else
					from.SendGump( new NoticeGump( 1060637, 30720, "It was not possible to kick from Factions the selected players.<br><br>Maybe the selected players are not in a Faction.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
			else
			{
				from.SendGump( new NoticeGump( 1060637, 30720, "Aborted.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
		}

		private static void Ban_Callback( Mobile from, bool ok, object state )
		{
			object[] states = (object[])state;
			ArrayList list = (ArrayList)states[0];
			ArrayList rads = (ArrayList)states[1];
			ArrayList original = (ArrayList)states[2];
			int page = (int)states[3];
			Faction faction = (Faction)states[4];

			int banned = 0;

			if ( ok )
			{
				for ( int i = 0; i < rads.Count; i++ )
				{
					PlayerMobile player = rads[i] as PlayerMobile;
					if ( !Faction.IsFactionBanned( player ) )
					{
						Account acct = player.Account as Account;
						if ( acct != null )
						{
							acct.SetTag( "FactionBanned", "true" );

							for ( int j = 0; j < acct.Length; ++j )
							{
								player = acct[j] as PlayerMobile;

								if ( player != null )
								{
									PlayerState pl = PlayerState.Find( player );

									if ( pl != null )
									{
										pl.Faction.RemoveMember( player );
										player.SendMessage( "You have been kicked from your faction." );

										Guild guild = player.Guild as Guild;
										if ( guild != null && !pl.IsLeaving )
											guild.RemoveMember( player );
									}
								}
							}

							banned++;
						}
					}
				}

				if ( banned > 0 )
				{
					string message  = String.Format( "Operation complete: {0} selected player{1} {2} been banned from Factions.", banned == 1 ? "One" : banned.ToString(), banned == 1 ? "" : "s", banned == 1 ? "has" : "have" );
					from.SendGump( new NoticeGump( 1060637, 30720, message, 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
				}
				else
					from.SendGump( new NoticeGump( 1060637, 30720, "It was not possible to ban from Factions the selected players.<br><br>Maybe the selected players are already banned from Factions.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
			else
			{
				from.SendGump( new NoticeGump( 1060637, 30720, "Aborted.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
		}

		private static void Unban_Callback( Mobile from, bool ok, object state )
		{
			object[] states = (object[])state;
			ArrayList list = (ArrayList)states[0];
			ArrayList rads = (ArrayList)states[1];
			ArrayList original = (ArrayList)states[2];
			int page = (int)states[3];
			Faction faction = (Faction)states[4];

			int unbanned = 0;

			if ( ok )
			{
				for ( int i = 0; i < rads.Count; i++ )
				{
					PlayerMobile player = rads[i] as PlayerMobile;
					if ( Faction.IsFactionBanned( player ) )
					{
						Account acct = player.Account as Account;
						if ( acct != null )
						{
							acct.RemoveTag( "FactionBanned" );
							unbanned++;
						}
					}
				}

				if ( unbanned > 0 )
				{
					string message  = String.Format( "Operation complete: {0} selected player{1} {2} been unbanned from Factions.", unbanned == 1 ? "One" : unbanned.ToString(), unbanned == 1 ? "" : "s", unbanned == 1 ? "has" : "have" );
					from.SendGump( new NoticeGump( 1060637, 30720, message, 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
				}
				else
					from.SendGump( new NoticeGump( 1060637, 30720, "It was not possible to unban from Factions the selected players.<br><br>Maybe the selected players are not currently banned from Factions.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
			else
			{
				from.SendGump( new NoticeGump( 1060637, 30720, "Aborted.", 0xFFC000, 420, 280, new NoticeGumpCallback( ResendGump_Callback ), new object[]{ list, rads, original, FactionsManageGumpPage.Main, 0, 0, faction } ) );
			}
		}

		private static int computeUselessness( PlayerState pl )
		{
			int max = 327, meanWeight = 12000;

			if ( pl == null )
				return max;

			PlayerMobile player = pl.Mobile as PlayerMobile;
			if ( player == null || player.Deleted )
				return max;

			Account acct = player.Account as Account;

			// counting uselessness (min 0, max 109 * 3 = 327)
			// raw maximum = 23184 (if 60 days from last login)
			int skillsWeight = 0, gameTimeWeight = 0, leavingWeight = 0, lastLoginWeight = 0;

			if ( player.SkillsTotal < 7000 )
				skillsWeight = (7000 - player.SkillsTotal); // range 0 - 7000

			if ( player.Skills.Highest.BaseFixedPoint < 1000 )
				skillsWeight += (1000 - player.Skills.Highest.BaseFixedPoint) * 5; // range 0 - 5000

			if ( player.GameTime < TimeSpan.FromHours( 20 ) )
				gameTimeWeight = (int)Math.Pow((30 - player.GameTime.TotalHours) * 2.4, 2); // range 196 - 5184
			else if ( player.GameTime < TimeSpan.FromHours( 100 ) )
				gameTimeWeight = (int)(100 - player.GameTime.TotalHours) * 72 / 10; // range 0 - 576

			if ( pl.IsLeaving )
			{
				if ( pl.Leaving + Faction.LeavePeriod < DateTime.Now )
					leavingWeight = Int32.MaxValue; // these people will leave on next login, so uselessness is maximum
				else
					leavingWeight = (int)((DateTime.Now - pl.Leaving).TotalDays * 5000 / Faction.LeavePeriod.TotalDays); // range 0 - 5000
			}

			if ( acct != null && acct.LastLogin + TimeSpan.FromDays( 5 ) < DateTime.Now )
				lastLoginWeight = (int)Math.Pow((DateTime.Now - acct.LastLogin).TotalDays * 5.5, 1.5); // range 0 - 8 (about 6000 for 60 days)

			if ( leavingWeight == Int32.MaxValue )
				return max;

			int uselessness;
			try
			{
				uselessness = (skillsWeight + gameTimeWeight + leavingWeight + lastLoginWeight) * max / meanWeight;
				if ( uselessness > 0 && player.Guild == null )
					uselessness += max / 20; // 5% more uselessness
			}
			catch ( OverflowException oe )
			{
				uselessness = max;
			}
			catch
			{
				uselessness = 0;
			}

			if ( uselessness < 0 )
				uselessness = 0;
			if ( uselessness > max )
				uselessness = max;

			return uselessness;
		}

		private static int computeUselessnessPercent( PlayerState pl )
		{
			int max = 327; // see previous method
			return computeUselessness( pl ) * 100 / max;
		}
	}
}