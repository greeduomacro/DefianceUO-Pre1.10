using System;
using Server.Items;

namespace Server.Items
{
   public class SkillStonee : Item
   {
      [Constructable]
      public SkillStonee() : base( 0xED4 )
      {
         Movable = false;
         Hue = 0x480;
         Name = "a skill stone";
      }

      public SkillStonee( Serial serial ) : base( serial )
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

      public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target
      {
         from.SendMessage( 0x0, "You must use a Skill Ticket on this." );
      }
   }
}