//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
   public class TokenCheckGump : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "TokenCheckGump", AccessLevel.GameMaster, new CommandEventHandler( TokenCheckGump_OnCommand ) );
      }

      private static void TokenCheckGump_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new TokenCheckGump( e.Mobile ) );
      }

      public TokenCheckGump( Mobile owner ) : base( 200,100 )
      {
this.Closable=true;
this.Disposable=true;
this.Dragable=true;
this.Resizable=false;
this.AddPage(0);
this.AddBackground(64, 132, 185, 30, 3000);
this.AddBackground(11, 8, 250, 345, 9200);
this.AddAlphaRegion(7, 5, 258, 351);
this.AddBackground(20, 20, 225, 29, 3000);
this.AddLabel(87, 25, 150, @"Copper Checks");
this.AddBackground(63, 174, 185, 30, 3000);
this.AddLabel(92, 180, 150, @"100,000 Copper");
this.AddBackground(62, 217, 185, 30, 3000);
this.AddBackground(62, 261, 185, 30, 3000);
this.AddButton(14, 169, 1149, 1147, 3, GumpButtonType.Reply, 0);
this.AddLabel(92, 222, 150, @"250,000 Copper");
this.AddButton(13, 212, 1149, 1147, 4, GumpButtonType.Reply, 0);
this.AddLabel(92, 267, 150, @"500,000 Copper");
this.AddButton(104, 317, 4014, 4015, 1, GumpButtonType.Reply, 0);
this.AddButton(209, 317, 4017, 4018, 0, GumpButtonType.Reply, 0);
this.AddLabel(62, 320, 150, @"Back");
this.AddLabel(165, 319, 150, @"Close");
this.AddImage(103, 62, 5571);
this.AddBackground(64, 132, 185, 30, 3000);
this.AddButton(14, 256, 1149, 1147, 5, GumpButtonType.Reply, 0);
this.AddButton(15, 127, 1149, 1147, 2, GumpButtonType.Reply, 0);
this.AddLabel(93, 137, 150, @"60,000 Copper");


}

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons
      {
         Mobile from = state.Mobile;

         switch ( info.ButtonID )
         {
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //Cancel
               from.SendMessage( "You decide not to change any Copper." );
               break;
            }
		case 1: // Main menu
		{
			from.SendGump( new TokenStoneGump( from) );
			break;
		}

            case 2: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //60k Tokens to Deed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 60000 ) )
		   {
         	   TokensBankCheck TokensBankCheck = new TokensBankCheck(60000);
		   from.AddToBackpack( TokensBankCheck );
               from.SendMessage( "60k Copper have been removed from your pack." );
		   from.SendGump( new TokenCheckGump( from) );
		   }
		   else
		   {
		   from.SendGump( new TokenCheckGump( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;
            }
            case 3:
            {
               //100k Tokens to Deed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 100000 ) )
		   {
         	   TokensBankCheck TokensBankCheck = new TokensBankCheck(100000);
		   from.AddToBackpack( TokensBankCheck );
               from.SendMessage( "100k Copper have been removed from your pack." );
		   from.SendGump( new TokenCheckGump( from) );
		   }
		   else
		   {
		   from.SendGump( new TokenCheckGump( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;
            }
            case 4:
            {
               //250k Tokens to Deed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 250000 ) )
		   {
         	   TokensBankCheck TokensBankCheck = new TokensBankCheck(250000);
		   from.AddToBackpack( TokensBankCheck );
               from.SendMessage( "250k Copper have been removed from your pack." );
		   from.SendGump( new TokenCheckGump( from) );
		   }
		   else
		   {
		   from.SendGump( new TokenCheckGump( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;
            }
            case 5:
            {
               //500k Tokens to Deed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 500000 ) )
		   {
         	   TokensBankCheck TokensBankCheck = new TokensBankCheck(500000);
		   from.AddToBackpack( TokensBankCheck );
               from.SendMessage( "500k Copper have been removed from your pack." );
		   from.SendGump( new TokenCheckGump( from) );
		   }
		   else
		   {
		   from.SendGump( new TokenCheckGump( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;
            }
         }
      }
   }
}