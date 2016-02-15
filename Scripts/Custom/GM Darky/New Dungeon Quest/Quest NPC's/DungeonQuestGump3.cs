using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class DungeonQuestGump3 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "DungeonQuestGump3", AccessLevel.GameMaster, new CommandEventHandler( DungeonQuestGump3_OnCommand ) );
      }

      private static void DungeonQuestGump3_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new DungeonQuestGump3( e.Mobile ) );
      }

      public DungeonQuestGump3( Mobile owner ) : base( 50,50 )
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
"<BASEFONT COLOR=YELLOW>This is beyond my wildest dreams... You know those heros once belonged to one of the greatest army's to ever walk this realm? They where all " +
"<BASEFONT COLOR=YELLOW>hand picked soldiers by a great king. Each of them a better warrior then any that have lived since that day. You see... eventho at first " +
"<BASEFONT COLOR=YELLOW>the king was a good king. And he used his army only for good. To protect the weak. But power went to his head... and the force that granted " +
"<BASEFONT COLOR=YELLOW>these warriors their unbelievable skills noticed. He then spoke a curse upon them all. For following this evil king knowing he was up to no good. " +
"<BASEFONT COLOR=YELLOW>But enough of these ancient fairy tales... Hand me the vials and I will try to craft the third key my friend.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*As Te'Ar starts to work you notice he casts a spell. After that the pours the potions empty on the ground. But the blood never reaches " +
"<BASEFONT COLOR=YELLOW>the ground. Instead it forms twenty balls floating in the air at eye hight. Right next to eachother and right infront of Te'Ar. Whom then " +
"<BASEFONT COLOR=YELLOW>starts to wirl his hands around the balls. Making big circles with both his arms around and around the floating blood. As he slowly starts " +
"<BASEFONT COLOR=YELLOW>to make smaller and small circles he starts to humm. When he touches the outer balls the vanish. Making his way to the middle each of the " +
"<BASEFONT COLOR=YELLOW>floating blood balls dissapear on touch. Once he reached the last ball his hands touch excactly when he touches the last ball. And he stops moving..*<BR><BR> " +
"<BASEFONT COLOR=YELLOW>What is this? I seem.. there is no key? What... ah silly me. *ADRUM!* There we go. Here take it. Also take this knife. For the fourth key I will " +
"<BASEFONT COLOR=YELLOW>need only a hide of the Beast. Just one of them will be enough. The beast is not on of the easier ones to slay I warn you. But I am sure " +
"<BASEFONT COLOR=YELLOW>a mighty one like you will manage. I see strength in your aura my friend. And it seems to be growing. A fine one you will become one day.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Now go, and get me the hide, we are allmost there my great friend. Finally I shall have peache and Jalinde will be free!" +
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