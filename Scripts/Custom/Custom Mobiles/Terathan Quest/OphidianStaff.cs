using System;
using Server;

namespace Server.Items
{
   public class OphidianStaff : Item
   {
      [Constructable]
      public OphidianStaff() : this( 1 )
      {
      }

      [Constructable]
      public OphidianStaff( int amount ) : base( 0x255A )
      {
	 Name = "Ophidian Staff";
	 Hue = 2213;
       	 Weight = 0.5;
      }

      public OphidianStaff( Serial serial ) : base( serial )
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