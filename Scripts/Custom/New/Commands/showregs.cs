using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Scripts.Commands
{
   public class Goods
   {
      public static void Initialize()
      {
         Server.Commands.Register( "regs", AccessLevel.Player, new CommandEventHandler( Goods_OnCommand ) );
      }

      [Usage( "regs" )]
      [Description( "Display the amount of Goods" )]
      public static void Goods_OnCommand( CommandEventArgs e )
      {
         Mobile somemobile = e.Mobile;
         somemobile.SendGump( new GoodsGump(somemobile) );
      }

   }


   public class GoodsGump : Gump
   {

      public GoodsGump ( Mobile from ) : base ( 20, 30 )
      {
         Container mobpack = from.Backpack;

         AddPage ( 0 );
         AddBackground( 0, 0, 260, 450, 5054 );

         AddImageTiled( 10, 10, 240, 23, 0x52 );
         AddImageTiled( 11, 11, 238, 21, 0xBBC );

         AddLabel( 95, 11, 0, "Reagents" );

         AddImageTiled( 10, 32 , 240, 23, 0x52 );
         AddImageTiled( 11, 33 , 238, 21, 0xBBC );
         AddItem(13, 33, 0xF7A);
         AddLabelCropped( 53, 33 , 150, 21, 0, "Black Pearl" );
         AddImageTiled( 180, 34 , 50, 19, 0x52 );
         AddImageTiled( 181, 35 , 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 , 234, 21, 0, from.Backpack.GetAmount(typeof(BlackPearl)).ToString() );

         AddImageTiled( 10, 32 + 22, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 22, 238, 21, 0xBBC );
         AddItem(13, 55, 0xF7B);
         AddLabelCropped( 53, 33 + 22, 150, 21, 0, "Bloodmoss" );
         AddImageTiled( 180, 34 + 22, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 22, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 22, 234, 21, 0, from.Backpack.GetAmount(typeof(Bloodmoss)).ToString() );

         AddImageTiled( 10, 32 + 44, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 44, 238, 21, 0xBBC );
         AddItem(13, 77, 0xF84);
         AddLabelCropped( 53, 33 + 44, 150, 21, 0, "Garlic" );
         AddImageTiled( 180, 34 + 44, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 44, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 44, 234, 21, 0, from.Backpack.GetAmount(typeof(Garlic)).ToString() );

         AddImageTiled( 10, 32 + 66, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 66, 238, 21, 0xBBC );
         AddItem(13, 99, 0xF85);
         AddLabelCropped( 53, 33 + 66, 150, 21, 0, "Ginseng" );
         AddImageTiled( 180, 34 + 66, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 66, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 66, 234, 21, 0, from.Backpack.GetAmount(typeof(Ginseng)).ToString() );

         AddImageTiled( 10, 32 + 88, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 88, 238, 21, 0xBBC );
         AddItem(13, 121, 0xF86);
         AddLabelCropped( 53, 33 + 88, 150, 21, 0, "Mandrake Root" );
         AddImageTiled( 180, 34 + 88, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 88, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 88, 234, 21, 0, from.Backpack.GetAmount(typeof(MandrakeRoot)).ToString() );

         AddImageTiled( 10, 32 + 110, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 110, 238, 21, 0xBBC );
         AddItem(13, 143, 0xF88);
         AddLabelCropped( 53, 33 + 110, 150, 21, 0, "Night Shade" );
         AddImageTiled( 180, 34 + 110, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 110, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 110, 234, 21, 0, from.Backpack.GetAmount(typeof(Nightshade)).ToString() );

         AddImageTiled( 10, 32 + 132, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 132, 238, 21, 0xBBC );
         AddItem(13, 165, 0xF8C);
         AddLabelCropped( 53, 33 + 132, 150, 21, 0, "Sulfurous Ash" );
         AddImageTiled( 180, 34 +  132, 50, 19, 0x52 );
         AddImageTiled( 181, 35 +  132, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 132, 234, 21, 0, from.Backpack.GetAmount(typeof(SulfurousAsh)).ToString() );

         AddImageTiled( 10, 32 + 154, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 154, 238, 21, 0xBBC );
         AddItem(13, 187, 0xF8D);
         AddLabelCropped( 53, 33 + 154, 150, 21, 0, "Spider's Silk" );
         AddImageTiled( 180, 34 + 154, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 154, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 154, 234, 21, 0, from.Backpack.GetAmount(typeof(SpidersSilk)).ToString() );

         AddImageTiled( 10, 32 + 176, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 176, 238, 21, 0xBBC );
         AddItem(13, 209, 0xF3F);
         AddLabelCropped( 53, 33 + 176, 150, 21, 0, "Arrow" );
         AddImageTiled( 180, 34 + 176, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 176, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 176, 234, 21, 0, from.Backpack.GetAmount(typeof(Arrow)).ToString() );


         AddImageTiled( 10, 32 + 198, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 198, 238, 21, 0xBBC );
         AddItem(13, 231, 0x1BFB);
         AddLabelCropped( 53, 33 + 198, 150, 21, 0, "Bolt" );
         AddImageTiled( 180, 34 + 198, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 198, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 198, 234, 21, 0, from.Backpack.GetAmount(typeof(Bolt)).ToString() );

         AddImageTiled( 10, 32 + 220, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 220, 238, 21, 0xBBC );
         AddItem(13, 253, 0xE21);
         AddLabelCropped( 53, 33 + 220, 150, 21, 0, "Bandies" );
         AddImageTiled( 180, 34 + 220, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 220, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 220, 234, 21, 0, from.Backpack.GetAmount(typeof(Bandage)).ToString() );

         AddImageTiled( 10, 32 + 242, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 242, 238, 21, 0xBBC );
         AddItem(13, 275, 0xF0C);
         AddLabelCropped( 53, 33 + 242, 150, 21, 0, "Greater heal potion" );
         AddImageTiled( 180, 34 + 242, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 242, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 242, 234, 21, 0, from.Backpack.GetAmount(typeof(GreaterHealPotion)).ToString() );

         AddImageTiled( 10, 32 + 264, 240, 23, 0x52 );
         AddImageTiled( 11, 33 + 264, 238, 21, 0xBBC );
         AddItem(13, 297, 0xF07);
         AddLabelCropped( 53, 33 + 264, 150, 21, 0, "Greater cure potion" );
         AddImageTiled( 180, 34 + 264, 50, 19, 0x52 );
         AddImageTiled( 181, 35 + 264, 48, 17, 0xBBC );
         AddLabelCropped( 182, 35 + 264, 234, 21, 0, from.Backpack.GetAmount(typeof(GreaterCurePotion)).ToString() );




      }
   }
}