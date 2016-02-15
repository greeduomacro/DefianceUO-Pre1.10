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
   public class TokenPrizeGump2 : Gump
   {
      public static void Initialize()
      {
         Commands.Register( "TokenPrizeGump2", AccessLevel.GameMaster, new CommandEventHandler( TokenPrizeGump2_OnCommand ) );
      }

      private static void TokenPrizeGump2_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendGump( new TokenPrizeGump2( e.Mobile ) );
      }

      public TokenPrizeGump2( Mobile owner ) : base( 200,100 )
      {
this.Closable=true;
this.Disposable=true;
this.Dragable=true;
this.Resizable=false;
this.AddPage(0);
this.AddBackground(5, 2, 250, 475, 9200);
this.AddAlphaRegion(1, 1, 259, 481);
this.AddBackground(20, 20, 225, 32, 3000);
this.AddLabel(114, 26, 150, @"Deeds");
this.AddBackground(60, 129, 185, 30, 3000);
this.AddBackground(60, 217, 185, 30, 3000);
this.AddButton(109, 437, 4014, 4015, 1, GumpButtonType.Reply, 0);
this.AddButton(213, 436, 4017, 4018, 0, GumpButtonType.Reply, 0);
this.AddLabel(71, 440, 150, @"Back");
this.AddLabel(171, 438, 150, @"Close");
this.AddImage(102, 58, 5571);
this.AddButton(15, 213, 1149, 1147, 4, GumpButtonType.Reply, 0);
this.AddButton(15, 125, 1149, 1147, 2, GumpButtonType.Reply, 0);
this.AddBackground(61, 173, 185, 30, 3000);
this.AddButton(16, 169, 1149, 1147, 3, GumpButtonType.Reply, 0);
this.AddLabel(92, 223, 150, @"Holy Blessing - 250k");
this.AddLabel(92, 179, 150, @"Cloth Bless Deed - 100k");
this.AddLabel(93, 135, 150, @"Dragon Trophy - 125k");
this.AddBackground(60, 263, 185, 30, 3000);
this.AddButton(17, 259, 1149, 1147, 5, GumpButtonType.Reply, 0);
this.AddLabel(92, 269, 150, @"Unholy Skull of Blessing - 175k");
this.AddBackground(59, 311, 185, 30, 3000);
this.AddButton(15, 306, 1149, 1147, 6, GumpButtonType.Reply, 0);
this.AddLabel(90, 317, 150, @"Anti Bless Deed - 200k");
this.AddBackground(59, 360, 185, 30, 3000);
this.AddButton(15, 356, 1149, 1147, 7, GumpButtonType.Reply, 0);
this.AddLabel(90, 366, 150, @"Carpet Addon - 200k");
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
            case 2:
            {
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 125000 ) )
		   {
		   from.AddToBackpack(new DragonTrophyDeed());
               from.SendMessage( "A Dragon's Head Deed has been placed in your pack." );
		   }
		   else
		   {
		   from.SendGump( new TokenPrizeGump2( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;
            }

            case 3: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
               // CBD
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 100000 ) )
		   {
		   from.AddToBackpack( new ClothingBlessDeed() );
               from.SendMessage( "A Clothing Bless Deed has been placed in your backpack." );
		   }
		   else
		   {
		   from.SendMessage( "You do not have enough Copper for that." );
		   from.SendGump( new TokenPrizeGump2( from) );
		   }
		   break;
            }

            case 4:
            {
               //Item HolyDeedofBlessing
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 250000 ) )
		   {
		   from.AddToBackpack(new HolyDeedofBlessing());
               from.SendMessage( "A HolyDeed of Blessing has been placed in your pack." );
		   }
		   else
		   {
		   from.SendGump( new TokenPrizeGump2( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;

	   }

          case 5:
            {
               //Item CursedClothingBlessDeed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 175000 ) )
		   {
		   from.AddToBackpack(new CursedClothingBlessDeed());
               from.SendMessage( "A Cursed Clothing Bless Deed has been added to your pack." );
		   }
		   else
		   {
		   from.SendGump( new TokenPrizeGump2( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;

	   }

          case 6:
            {
               //Item AntiBlessDeed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 200000 ) )
		   {
		   from.AddToBackpack(new AntiBlessDeed());
               from.SendMessage( "A Anti Bless Deed has been added to your pack." );
		   }
		   else
		   {
		   from.SendGump( new TokenPrizeGump2( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;

	  }

          case 7:
            {
               //Item CursedClothingBlessDeed
		   Item[] Tokens = from.Backpack.FindItemsByType( typeof( Tokens ) );
		   if ( from.Backpack.ConsumeTotal( typeof( Tokens ), 200000 ) )
		   {
		   from.AddToBackpack(new CarpetAddonDeed());
               from.SendMessage( "A Carpet Addon Deed Deed has been added to your pack." );
		   }
		   else
		   {
		   from.SendGump( new TokenPrizeGump2( from) );
		   from.SendMessage( "You do not have enough Copper for that." );
		   }
		   break;

	   }

		}

   	     }
   	 }
     }