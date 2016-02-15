using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class DungeonQuestGump4 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "DungeonQuestGump4", AccessLevel.GameMaster, new CommandEventHandler( DungeonQuestGump4_OnCommand ) );
      }

      private static void DungeonQuestGump4_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new DungeonQuestGump4( e.Mobile ) );
      }

      public DungeonQuestGump4( Mobile owner ) : base( 50,50 )
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
"<BASEFONT COLOR=YELLOW>A so indeed it was. I felt a great disturbance in the atmosphere. I was hoping it was the death of Nessa. You did marvelous my friend.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>*After handing the hide to Te'Ar you notice him spreading it om the floor. He examined the hide closely and picks out five hairs at " +
"<BASEFONT COLOR=YELLOW>different locations. Then he peels of a claw that where stuck on the hide. He now binds the hairs around the claw and lays it like that " +
"<BASEFONT COLOR=YELLOW>on his outstretched hands. He speaks a few words and then slightly starts to blow over the claw with the hairs. You slowly see the claw " +
"<BASEFONT COLOR=YELLOW>changing shape. As soon as Te'Ar is done blowing the claw has transformed into a key.*<BR><BR>" +
"<BASEFONT COLOR=YELLOW>There we go. Give me a second to catch my breath. It is getting harder to make the transformation with each key I do. But just one more to go " +
"<BASEFONT COLOR=YELLOW>and then we are done my migthy friend. You have no idea what you allready have achieved and no idea how big the impact shall be once completed. " +
"<BASEFONT COLOR=YELLOW>I am quite sure the Bard holds more prisoners and perhaps we can manage to free them all. A lot of family's will finally be reunited. And " +
"<BASEFONT COLOR=YELLOW>a lot of souls will finally find their peace. The Bard you say? No he left Silence a few years ago. I saw him leave and he never returned. " +
"<BASEFONT COLOR=YELLOW>That is why I have the courage to stand here. Inside the dungeon at the entrance waiting for someone willing to aid me. I am afraid the powers " +
"<BASEFONT COLOR=YELLOW>of the Bard would even be able to harm a old ghost like me. Or perhaps he would imprison me aswell. But either way there are no more perils " +
"<BASEFONT COLOR=YELLOW>then the current inhabitants of this dungeon. And that seems to be enough... *Smiles*. YOu have done well my friend. Just one " +
"<BASEFONT COLOR=YELLOW>more task awaits you and we shall be free at last. Take this bag of gems. Find the Noxious ones. Last I heard they left this dungeon " +
"<BASEFONT COLOR=YELLOW>to inhabit other lands. But I am sure their outbreak left a passage to their current home. I wish I could guide you more but it is all I " +
"<BASEFONT COLOR=YELLOW>know.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Oh yes, after slaying one of these beast. Use the gem to capture his essence. Bring me fifty of those and we shall be able to construct the " +
"<BASEFONT COLOR=YELLOW>final key my friend. Goodluck , for the last time." +
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