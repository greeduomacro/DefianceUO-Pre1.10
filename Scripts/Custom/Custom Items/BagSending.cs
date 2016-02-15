using System;
using System.Collections;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
   public class BagofSending : Item
   {
      private int i_charges;

      [CommandProperty( AccessLevel.GameMaster )]
      public int Charges
      {
         get { return i_charges; }
         set { i_charges = value; InvalidateProperties(); }
      }


      [Constructable]
      public BagofSending() : base( 0xE76 )
      {
         Hue = Utility.RandomMetalHue();
         Weight = 2.0;
      }

      public BagofSending( Serial serial ) : base( serial )
      {
      }

      public override void OnSingleClick( Mobile from )
      {
         LabelTo( from, 1054105 ); //1041367, "1"
         LabelTo( from, 1054132, Charges.ToString() );
      }
      public override void AddNameProperty( ObjectPropertyList list )
      {
         // list.Add( Name );
         list.Add( 1054105 ); //1041367, "1"
         list.Add( 1054132, Charges.ToString() );
      }

      public override void OnDoubleClick( Mobile from )
      {
         BankBox bank = from.BankBox;

         if ( IsChildOf( from.Backpack ) )
         {
            if( this.Charges > 0 )
            {
               from.Target = new InternalTarget( this );
            }
            else
            {
               from.SendLocalizedMessage( 1019073 );   // That item is out of charges.
            }
         }
         else
         {
            from.SendLocalizedMessage( 1054107 ); // The bag of sending must be in your backpack.
         }
      }

      public override void GetContextMenuEntries( Mobile from, ArrayList list )
      {
         base.GetContextMenuEntries( from, list );

         if ( from.Alive )
            list.Add( new BagSendingEntry( from, this ) );
      }

      private class InternalTarget : Target
      {
         private BagofSending m_BagofSending;

         public InternalTarget( BagofSending bag ) :  base ( 1, false, TargetFlags.None )
         {
            m_BagofSending = bag;
         }

         protected override void OnTarget( Mobile from, object targeted )
         {
            if ( targeted is Container )
            {
               from.SendLocalizedMessage( 1054109 );
            }
            else if ( targeted is PlayerMobile )
            {
               from.SendLocalizedMessage( 1054109 );
       }
            else if ( targeted is BaseCreature )
            {
               from.SendLocalizedMessage( 1054109 );
       }
            else
            {
               Item sendme = (Item)targeted;
               if ( sendme.LootType == LootType.Cursed )
               {
                  from.SendLocalizedMessage( 1054108 );
               }
               else
               {
                  if( sendme.IsChildOf(from.Backpack) )
                  {
                     BankBox bank = from.BankBox;
                     bank.DropItem( sendme );
                     from.SendLocalizedMessage( 1054150 );
                     m_BagofSending.Charges--;
                  }
                  else
                  {
                     from.SendLocalizedMessage( 1054152 );
                  }
               }
            }
         }
      }

      public class BagSendingEntry : ContextMenuEntry
      {
         private BagofSending i_Bag;
         private Mobile m_Sender;

         public BagSendingEntry( Mobile from, BagofSending bag ) : base( 6189, 1 )
         {
            m_Sender = from;
            i_Bag = bag;
         }

         public override void OnClick()
         {
            if( i_Bag.Charges > 0 )
            {
               m_Sender.Target = new InternalTarget( i_Bag );
            }
            else
            {
               m_Sender.SendLocalizedMessage( 1019073 );   // That item is out of charges.
            }
         }
      }

      public override void Serialize( GenericWriter writer )
      {
         base.Serialize( writer );

         writer.Write( (int) 0 ); // version
         writer.Write( (int) i_charges );
      }

      public override void Deserialize( GenericReader reader )
      {
         base.Deserialize( reader );

         int version = reader.ReadInt();
         i_charges = (int)reader.ReadInt();
      }
   }
}