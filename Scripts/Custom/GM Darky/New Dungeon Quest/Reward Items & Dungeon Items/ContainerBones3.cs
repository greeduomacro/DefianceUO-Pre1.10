using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class ContainerBones3 : Bag
   {
      [Constructable]
      public ContainerBones3() : this( 1 )
      {
		  Name = "An Unknown Mage's Skeleton";
  Movable = true;
  GumpID = 9;
  ItemID = 3792;
      }
	   [Constructable]
	   public ContainerBones3( int amount )
	   {
		   DropItem( new Robe( 0x52B ) );
		   DropItem( new Shoes( 0x6B6 ) );
		   DropItem( new WizardsHat( 0x52B ) );
		   DropItem( new Gold( 260, 350 ) );
	   }


      public ContainerBones3( Serial serial ) : base( serial )
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