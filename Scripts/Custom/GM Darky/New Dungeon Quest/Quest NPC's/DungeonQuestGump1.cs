using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class DungeonQuestGump1 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "DungeonQuestGump1", AccessLevel.GameMaster, new CommandEventHandler( DungeonQuestGump1_OnCommand ) );
      }

      private static void DungeonQuestGump1_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new DungeonQuestGump1( e.Mobile ) );
      }

      public DungeonQuestGump1( Mobile owner ) : base( 50,50 )
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
"<BASEFONT COLOR=YELLOW>*As you approach Te'Ar you see he is eagerly waiting, summoning you to hurry*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Did you get them? Did you manage? Have you succeeded my friend? Oh pardon my impatience... speak please.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>But this... this is beyond all my hopes... You, you managed to do it? Hold on please.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*You see Te'Ar holding all the Dark Iron Wire's in his hands. As he slowly starts to mumble something you can not understand you see " +
"<BASEFONT COLOR=YELLOW>a red glow appear around his hands. First transparant but it gets darker and darker. Untill you see nothing but a red ball around where once " +
"<BASEFONT COLOR=YELLOW>used to be his hands holding the iron. When the red is so dark it's about to turn into another colour you hear a *pop* and you see the ghosts " +
"<BASEFONT COLOR=YELLOW>hands again. As he slowly opens his hands, inside them you see the Dark Iron Wire's are gone. Instead he is holding a red key.*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>There we go. As I said my friend this is just the start. We need in total five of these keys to unlock all the doors to get to Jalinde. " +
"<BASEFONT COLOR=YELLOW>If you choose to aid me more, which would be beyond all my dreams, then please take this key. Go deeper into the dungeon and find for me" +
"<BASEFONT COLOR=YELLOW>ten pieces of enchanted wood. I can use them to create the second key. How can I ever reward you?" +
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
               				from.SendMessage( "Good Luck..." );
               				break;
            			}
         		}
      		}
   	}
}