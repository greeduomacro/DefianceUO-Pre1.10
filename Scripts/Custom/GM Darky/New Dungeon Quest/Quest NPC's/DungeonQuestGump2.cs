using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class DungeonQuestGump2 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "DungeonQuestGump2", AccessLevel.GameMaster, new CommandEventHandler( DungeonQuestGump2_OnCommand ) );
      }

      private static void DungeonQuestGump2_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new DungeonQuestGump2( e.Mobile ) );
      }

      public DungeonQuestGump2( Mobile owner ) : base( 50,50 )
      {
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------

			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Dungeon Quest by Alfa" );


			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>You have returned! Must not have been easy judging by the likes of you... Did you get the Enchanted Wood my friend?<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*After you handed the Enchanted Wood to Te'Ar you notice him stacking them up into a pile balancing them in one hand. As he starts " +
"<BASEFONT COLOR=YELLOW>speak with a rapid tongue in a language you fail to grasp you notice the velocity and volume of his tone increase. At the point you feel " +
"<BASEFONT COLOR=YELLOW>he is just screaming one tone he suddenly slaps his other hand ontop of the pile of Enchaned Wood. With a loud *bang* his palms meet and the " +
"<BASEFONT COLOR=YELLOW>wood is either gone or completely crushed between his hands. As he slowly opens his hands he, instead, holds a Wooden key.*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Oh my... this ancient magic wears me out more then expected. But we succeeded my friend. I hope the rewards giving in treasure can somehow " +
"<BASEFONT COLOR=YELLOW>pay you back for this quest I have send you on. I remember at the time Jalinde and me hunted here we managed to get quite good a loot. " +
"<BASEFONT COLOR=YELLOW>Once we managed to slay this evil critter... I think the Bard called it Chan or something. He seemed to be quite rewarding. But first " +
"<BASEFONT COLOR=YELLOW>I will tell you what to do to get the third key. Take this bag of vials. If you go a level deeper into Silence you will find Fallen Heros. " +
"<BASEFONT COLOR=YELLOW>Slay twenty of them and use those vials to tap their blood. Once you filled them all I can make us the third key.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>I truly am thankfull for all this my friend. And surely so will be my beloved Sister... I hope one day I can repay you..." +
			"</BODY>", false, true);


			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 );

//--------------------------------------------------------------------------------------------------------------
      	}

      		public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons
      		{
         		Mobile from = state.Mobile;

         		switch ( info.ButtonID )
         		{
            			case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            			{
               				//Cancel
               				from.SendMessage( "Safe Travels..." );
               				break;
            			}
         		}
      		}
   	}
}