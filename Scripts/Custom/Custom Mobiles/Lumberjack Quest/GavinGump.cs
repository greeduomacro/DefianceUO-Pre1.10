using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class GavinGump : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "GavinGump", AccessLevel.GameMaster, new CommandEventHandler( GavinGump_OnCommand ) );
      }

      private static void GavinGump_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new GavinGump( e.Mobile ) );
      }

      public GavinGump( Mobile owner ) : base( 50,50 )
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
			AddLabel( 140, 60, 0x34, "The Quest of the Flesh Golem Crafter" );


			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>*Gavin looks up and smiles*<BR><BR>Oh, er, 'allo there. Did nay hear ye walkin' up.<BR>" +
"<BASEFONT COLOR=YELLOW>Say, could ye do meh a small favour? <BR><BR> Ye see, I got to clear out a few tree in this area to allow t'others to grow!<BR>" +
"<BASEFONT COLOR=YELLOW>Problem is, my axe is blunt - and I've promised the local Blacksmith that I'll sell the wood to them. <BR><BR> If you bring get me...say...500 boards, I'll give ye half the gold!" +
						     "</BODY>", false, true);

//			<BASEFONT COLOR=#7B6D20>

//			AddLabel( 113, 135, 0x34, "Noriar looks at you in disbelief..." );
//			AddLabel( 113, 150, 0x34, "Noriar looks at you in disbelief..." );
//			AddLabel( 113, 165, 0x34, "one you seek. The Greatest Smite Cleric in the land!" );
//			AddLabel( 113, 180, 0x34, "The Shield of Peril Draws you near. So you wish" );
//			AddLabel( 113, 195, 0x34, "a Shield of Peril do you? will you " );
//			AddLabel( 113, 210, 0x34, "bring me the required items?" );
//			AddLabel( 113, 235, 0x34, "This is a VERY dangerous yet rewarding Test of Faith." );
//			AddLabel( 113, 250, 0x34, "The materials I require are 75 Diamond Shards," );
//			AddLabel( 113, 265, 0x34, "The shards can be found on Dartmoor Ponies." );
//			AddLabel( 113, 280, 0x34, "These are RARE and not easily killed" );
//			AddLabel( 113, 295, 0x34, "felled. Should you retreive these shards I shall" );
//			AddLabel( 113, 310, 0x34, "craft you your Shield of Peril!" );
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
               from.SendMessage( "Thanks a lot!" );
               break;
            }

         }
      }
   }
}