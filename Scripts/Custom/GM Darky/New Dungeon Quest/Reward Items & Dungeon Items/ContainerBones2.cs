using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class ContainerBones2 : Bag
   {
      [Constructable]
      public ContainerBones2() : this( 1 )
      {
		  Name = "An Unknown Rogue's Skeleton";
  Movable = true;
  GumpID = 9;
  ItemID = 3789;
      }
	   [Constructable]
	   public ContainerBones2( int amount )
	   {
		   DropItem( new Cloak( 0x66B ) );
		   DropItem( new LongPants( 0x6B6 ) );
		   DropItem( new Lantern( ) );
		   DropItem( new Gold( 260, 350 ) );
	           DropItem( new Dagger( ) );

            }


      public ContainerBones2( Serial serial ) : base( serial )
      {
      }

      public override void Serialize( GenericWriter writer )
      {
         base.Serialize( writer );

         writer.Write( (int) 0 ); // version
      }

      public override void Deserialize( GenericReader reader )
      {
         base.Deserialize( reader );

         int version = reader.ReadInt();
      }
   }
}