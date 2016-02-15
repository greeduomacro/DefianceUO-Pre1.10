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
   public class TokenSpellBookGump : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "TokenSpellBookGump", AccessLevel.GameMaster, new CommandEventHandler( TokenSpellBookGump_OnCommand ) );
      }

      private static void TokenSpellBookGump_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new TokenSpellBookGump( e.Mobile ) );
      }

public TokenSpellBookGump( Mobile owner ) : base( 200,100 )
{
owner.CloseGump( typeof( TokenSpellBookGump ) );
this.Closable=true;
this.Disposable=false;
this.Dragable=true;
this.Resizable=false;

this.AddPage(0);
this.AddImage(10, 10, 2201);
this.AddButton(50, 30, 2298, 2298, 1, GumpButtonType.Reply, 0); //respet
this.AddButton(210, 30, 2268, 2268, 2, GumpButtonType.Reply, 0);//heal evil
this.AddLabel(100, 35, 0, @"Resurrect");
this.AddLabel(100, 50, 0, @"Pet");
this.AddLabel(260, 42, 0, @"Heal Evil");
this.AddButton(110, 90, 21001, 21001, 3, GumpButtonType.Reply, 0); // archery
this.AddLabel(50, 95, 0, @"Infinate");
this.AddLabel(50, 110, 0, @"Arrow");
this.AddButton(270, 90, 20494, 20494, 4, GumpButtonType.Reply, 0); // run scared
this.AddLabel(210, 95, 0, @"Run");
this.AddLabel(210, 110, 0, @"Scared");
this.AddButton(50, 155, 2263, 2263, 5, GumpButtonType.Reply, 0); // risen barrier
this.AddLabel(100, 160, 0, @"Risen");
this.AddLabel(100, 175, 0, @"Barrier");
this.AddButton(210, 155, 20740, 20740, 6, GumpButtonType.Reply, 0); // speed spell
this.AddLabel(260, 160, 0, @"Speed");
this.AddLabel(260, 175, 0, @"Spell");
this.AddImage(32, 239, 2620);
this.AddImage(32, 254, 2626);
this.AddImage(52, 254, 2627);
this.AddImage(52, 239, 2621);
this.AddImage(322, 239, 2622);
this.AddImage(322, 254, 2628);
this.AddButton(78, 250, 2360, 2360, 7, GumpButtonType.Reply, 0); //red
this.AddButton(178, 250, 2361, 2361, 8, GumpButtonType.Reply, 0); // green
this.AddButton(278, 250, 2362, 2362, 9, GumpButtonType.Reply, 0); //blue

}

public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons
      {
         Mobile from = state.Mobile;

         switch ( info.ButtonID )
         {
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               //Cancel
               from.SendMessage( "You choose not to cast a spell." );
               break;
            }

            case 1:
            {
		   //ResPet(from);
		   break;
            }

            case 2:
            {
			//HealEvil(from);
			break;
            }

            case 3:
            {
			//Achery(from);
			break;
            }

            case 4:
            {
			//RunScared(from);
			break;
            }

            case 5:
            {
			//RisenBarrier(from);
			break;
            }

            case 6:
            {
			//SpeedSpell(from);
			break;
            }

            case 7:
            {
			//Red(from);
			break;
            }

          	case 8:
            {
			//Green(from);
			break;
            }

            case 9:
            {
			//Blue(from);
			break;
            }
         }
      }
   }
}