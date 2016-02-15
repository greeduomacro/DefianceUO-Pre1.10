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
   public class TokenPrizeGump1 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "TokenPrizeGump1", AccessLevel.GameMaster, new CommandEventHandler( TokenPrizeGump1_OnCommand ) );
      }

      private static void TokenPrizeGump1_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new TokenPrizeGump1( e.Mobile ) );
      }

      public TokenPrizeGump1( Mobile owner ) : base( 200,100 )
      {
this.Closable=true;
this.Disposable=true;
this.Dragable=true;
this.Resizable=false;
this.AddPage(0);
this.AddBackground(6, 6, 250, 363, 9200);
this.AddAlphaRegion(3, 3, 256, 370);
this.AddImage(98, 50, 5571);
this.AddBackground(66, 116, 175, 30, 3000);
this.AddBackground(16, 17, 225, 28, 3000);
this.AddLabel(93, 20, 150, @"Wearables");
this.AddButton(8, 112, 1149, 1147, 2, GumpButtonType.Reply, 0);
this.AddLabel(88, 120, 150, @"Hooded Shroud - 200k");
this.AddBackground(58, 167, 185, 30, 3000);
this.AddLabel(88, 172, 150, @"Orc Mask - 50k");
this.AddBackground(56, 218, 185, 30, 3000);
this.AddButton(9, 214, 1149, 1147, 4, GumpButtonType.Reply, 0);
this.AddLabel(87, 224, 150, @"No Age Ethereal - 185k");
this.AddButton(112, 331, 4014, 4015, 1, GumpButtonType.Reply, 0);
this.AddButton(201, 330, 4017, 4018, 0, GumpButtonType.Reply, 0);
this.AddLabel(74, 333, 150, @"Back");
this.AddLabel(160, 333, 150, @"Close");
this.AddButton(7, 162, 1149, 1147, 3, GumpButtonType.Reply, 0);
this.AddBackground(56, 270, 185, 30, 3000);
this.AddLabel(84, 276, 150, @"Black Dye Tub - 200k");
this.AddButton(8, 266, 1149, 1147, 5, GumpButtonType.Reply, 0);


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
               //Hooded Shroud
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 200000 ) )
		   {
		   from.AddToBackpack( new HoodedShroudOfShadows() );
               from.SendMessage( "A Mysterious Shroud has been placed in your backpack." );
		   }
		   else
		   {
		   from.SendMessage( "You do not have enough Copper for that." );
		   from.SendGump( new TokenPrizeGump1( from) );
		   }
		   break;
            }
            case 3: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //White Knight
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 50000 ) )
		   {
		   from.AddToBackpack( new OrcMask() );
               from.SendMessage( "A Mysterious Orc Mask has been placed in your backpack." );
		   }
		   else
		   {
		   from.SendMessage( "You do not have enough Copper for that." );
		   from.SendGump( new TokenPrizeGump1( from) );
		   }
		   break;
            }

            case 4: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //Black Knight
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 185000 ) )
		   {
		   from.AddToBackpack( new EtherealHorse() );
               from.SendMessage( "A Mysterious Ethereal has been placed in your backpack." );
		   }
		   else
		   {
		   from.SendMessage( "You do not have enough Copper for that." );
		   from.SendGump( new TokenPrizeGump1( from) );
		   }
		   break;
	}

            case 5: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //White Knight
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 200000 ) )
		   {
		   from.AddToBackpack( new BlackDyeTub() );
               from.SendMessage( "A Black Dye Tub has been placed in your backpack." );
		   }
		   else
		   {
		   from.SendMessage( "You do not have enough Copper for that." );
		   from.SendGump( new TokenPrizeGump1( from) );
		   }
		   break;

            }
         }
      }
   }
}