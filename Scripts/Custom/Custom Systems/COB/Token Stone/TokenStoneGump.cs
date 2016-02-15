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
   public class TokenStoneGump : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "TokenStoneGump", AccessLevel.GameMaster, new CommandEventHandler( TokenStoneGump_OnCommand ) );
      }

      private static void TokenStoneGump_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new TokenStoneGump( e.Mobile ) );
      }

public TokenStoneGump( Mobile owner ) : base( 200,100 )
{
owner.CloseGump( typeof( TokenStoneGump ) );
this.Closable=true;
this.Disposable=true;
this.Dragable=true;
this.Resizable=false;
this.AddPage(0);
this.AddBackground(41, 40, 250, 300, 9200);
this.AddAlphaRegion(36, 34, 257, 310);
this.AddBackground(98, 168, 160, 25, 3000);
this.AddBackground(101, 216, 160, 25, 3000);
this.AddBackground(104, 262, 160, 25, 3000);
this.AddBackground(53, 55, 225, 37, 3000);
this.AddLabel(102, 63, 150, @"Copper Reward Stone");
this.AddButton(239, 303, 4017, 4018, 0, GumpButtonType.Reply, 0);
this.AddLabel(195, 305, 150, @"Close");
this.AddLabel(127, 170, 150, @"Deed Rewards");
this.AddLabel(126, 218, 150, @"Wearable Rewards");
this.AddButton(44, 255, 1149, 1147, 1, GumpButtonType.Reply, 0);
this.AddButton(45, 208, 1149, 1147, 2, GumpButtonType.Reply, 0);
this.AddButton(45, 160, 1149, 1147, 3, GumpButtonType.Reply, 0);
this.AddImage(137, 97, 5571);
this.AddLabel(127, 264, 150, @"Copper Checks");


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

            case 1: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //Checks
		   from.SendGump( new TokenCheckGump( from) );
		   break;
            }
            case 2:
            {
               //Rewards
			from.SendGump( new TokenPrizeGump1( from ) );
		   break;
            }

            case 3:
            {
               //Rewards
			from.SendGump( new TokenPrizeGump2( from ) );
		   break;
            }
         }
      }
   }
}