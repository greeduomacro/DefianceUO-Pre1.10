		////////////////////////////////////////////////////////////////////////////////////////
	   /////                                                                            ////////
	  //////    Version: 1.0        Author: Vorspire        Shard: Alternate-PK         ////////
	 ///////    Edited by A_Li_N                                                        ////////
	////////                                                                            ////////
	////////    QuakeNet: #Alternate-PK		MSN: alere_flammas666@hotmail.com           ////////
	////////                                                                            ////////
	////////    Description: This stone allows players to increase skills based on      ////////
	////////                 the settings you chose. This stone is fully custom-        ////////
	////////                 -isable and includes an experience feature, if your        ////////
	////////                 shard uses a similar experience system. Everything in      ////////
	////////                 this script is straight forward and easy to under-         ////////
	////////                 -stand. On behalf of Alternate-PK, I hope you enjoy        ////////
	////////                 this script to its full potential.                         ////////
	////////                                                                            ////////
	////////    Distribution: This script can be freely distributed, as long as the     ////////
	////////                  credit notes are left intact.	This script can also be     ////////
	////////                  modified, as long as the credit notes are left intact.    ///////
	////////                                                                            //////
	/////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class SkillStoneGumpHelp : Gump
	{
		private SkillBuyStone m_Stone;
		private Mobile m_From;

		public SkillStoneGumpHelp( SkillBuyStone stone, Mobile from ) : base( 180, 30 )
		{
			m_Stone = stone;
			m_From = from;

			Closable = true;
			Disposable = false;
			Dragable = false;
			Resizable = false;

			AddPage(0);
			AddBackground( 72, 17, 640, 550, 9270 );	//Main
			if( m_Stone.CoolLooking )
				AddAlphaRegion( 86, 33, 608, 516 );

			AddBackground( 170, 40, 201, 63, 9270 );	//Top-Left
			AddBackground( 410, 40, 201, 63, 9270 );	//Top-Right
			AddBackground( 116, 116, 255, 54, 9270 );	//Middle-Left
			AddBackground( 410, 116, 255, 54, 9270 );	//Middle-Right
			AddImage( 65, 62, 10400 );
			AddImage( 635, 62, 10410 );

			AddBackground( 93, 186, 597, 358, 9270 );	//Main Field
			if( m_Stone.CoolLooking )
				AddAlphaRegion( 86, 33, 607, 153 );

			AddLabel( 194, 61, 53, "Help Page" );

			AddBackground( 673, 0, 57, 57, 9270 );		//Close
			AddButton( 681, 11, 2642, 2643, 0, GumpButtonType.Reply, 0);	//Close Gump

			AddLabel( 486, 61, 53, "Back To Skills" );
			AddButton( 426, 46, 4506, 4506, 1, GumpButtonType.Reply, 0);	//Skills Pages

			AddLabel( 177, 200, 53, "Skill Name" );
			AddLabel( 177, 245, 53, "%" );
			AddLabel( 177, 290, 53, "Gold" );
			AddLabel( 177, 380, 43, "Gold Cost per " + "" + m_Stone.SkillIncrease + "%:" );

			AddLabel( 248, 200, 38, "- Tells you the name of the skill you are raising" );
			AddLabel( 248, 245, 38, "- Shows you the % of that skill you have" );
			AddLabel( 248, 290, 38, "- Click a button under this to purchase the skill for Gold" );
			AddLabel( 310, 380, 38, "- Displays Price in Gold" );

			AddImage( 200, 465, 4506 );
			AddImage( 358, 465, 4502 );
			AddImage( 522, 478, 5526 );

			AddLabel( 180, 508, 38, "Previous Page" );
			AddLabel( 348, 508, 38, "Next Page" );
			AddLabel( 480, 508, 38, "Bring you to this page" );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int BID = info.ButtonID;
			m_From.CloseGump( typeof( SkillStoneGumpHelp ) );

			if( BID == 1 )
				m_From.SendGump( new SkillStoneGump( m_Stone, m_From, 0 ) );
		}
	}
}