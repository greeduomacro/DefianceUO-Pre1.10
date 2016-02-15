using System;
using Server.Items;

namespace Server.Factions
{
   public class FactionQuitStone : Item
   {
      [Constructable]
      public FactionQuitStone() : base( 0xEDC )
      {
         Movable = false;
         Name = "a faction quit stone";
      }

      public override void OnDoubleClick( Mobile from )
      {
         PlayerState pl = PlayerState.Find( from );

         if ( pl != null )
         {
            pl.Faction.RemoveMember( from );

            from.SendMessage( "You have been removed from your faction." );
         }
         else
         {
            from.SendMessage( "You are not in a faction." );
         }
      }

      public FactionQuitStone( Serial serial ) : base( serial )
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