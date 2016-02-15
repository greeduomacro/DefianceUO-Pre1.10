// Org Author:  Lord Elfstone Draconis
// Author:      Darkeyes
// Date:        August 15, 2003
// Date:        Designed for RunUO Beta 34

using System;

namespace Server.Items
{
   public class WhiteFurnitureDyeTub : DyeTub
   {
      public override bool AllowDyables{ get{ return false; } }
      public override bool AllowFurniture{ get{ return true; } }
      public override int TargetMessage{ get{ return 501019; } } // Select the furniture to dye.
      public override int FailMessage{ get{ return 501021; } } // That is not a piece of furniture.

      [Constructable]
      public WhiteFurnitureDyeTub()
      {
                                Name = "White Furniture DyeTub";
                                Hue = DyedHue = 0x481;
                                Redyable = false;
      }

      public WhiteFurnitureDyeTub( Serial serial ) : base( serial )
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