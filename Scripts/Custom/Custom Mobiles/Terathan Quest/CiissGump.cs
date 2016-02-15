using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class CiissGump : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "CiissGump", AccessLevel.GameMaster, new CommandEventHandler( CiissGump_OnCommand ) );
      }

      private static void CiissGump_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new CiissGump( e.Mobile ) );
      }

      public CiissGump( Mobile owner ) : base( 50,50 )
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
			AddLabel( 140, 60, 0x34, "Quest Offer" );


			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>*Ciiss glances at you with a grim look on her face*<BR><BR>Pleasssse, behave - or you will face a bitter dead! Yessss, careful you mussst be!<BR>" +
"<BASEFONT COLOR=YELLOW>I'm nothing but a ssssimple traveller, and today i found sssomething... interesssting - an ancient Ophidian Sssstaff. Soooo pretty.<BR><BR>But asss I picked it up from the ground, a sssneaky little drone took it from me, and sssstole it from under my nosssse!<BR>" +
"<BASEFONT COLOR=YELLOW>Pleassse, will you find the drone which took it and retrieve my ssstaff for me?<BR><BR> I will reward you beyond your wildessst dreamsss. But be careful... Terathansss are... evilsss..." +
						     "</BODY>", false, true);

//			<BASEFONT COLOR=#7B6D20>

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
               from.SendMessage( "Be careful out there..." );
               break;
            }

         }
      }
   }
}