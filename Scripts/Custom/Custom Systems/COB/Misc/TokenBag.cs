//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server.Items
{
   public class TokenBag : Bag
   {
   public override int MaxWeight{ get{ return 0; } }
    public override int DefaultDropSound{ get{ return 0x42; } }

      [Constructable]
      public TokenBag() : base( )
      {
  Name = "A Token Bag";
  Movable = true;
  Hue = TokenSettings.Token_Colour;
  LootType = LootType.Blessed;
     }

		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if ( dropped is Tokens)
		{
			ArrayList list = this.Items;

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = (Item)list[i];

				if ( !(item is Container) && item.StackWith( from, dropped, false ) )
					return true;
			}

			DropItem( dropped );

			return true;
		}
		else
		{
				from.SendMessage ("Only tokens of the Realm my be placed in this Bag.");
				return false;
				}
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if ( item is Tokens)
		{
			item.Location = new Point3D( p.X, p.Y, 0 );
			AddItem( item );

			from.SendSound( GetDroppedSound( item ), GetWorldLocation() );

			return true;
		}
		else
		{
				from.SendMessage ("Only tokens of the Realm my be placed in this Bag.");
				return false;
		}
		}


public TokenBag( Serial serial ) : base( serial )
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

}}