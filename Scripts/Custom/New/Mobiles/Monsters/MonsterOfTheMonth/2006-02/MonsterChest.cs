using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class MonsterChest : MetalChest
   {
		[Constructable]
		public MonsterChest() : this( 1 )
		{
			Movable = true;
			Hue = 1109;
			Name = "a chest from the deep";
			Locked = true;
			this.RequiredSkill = this.LockLevel = 100;
		}
		[Constructable]
		public MonsterChest( int amount )
		{
			DropItem( new SpecialFishingNet() );
			DropItem( new MessageInABottle() );
			DropItem( new Gold( 859 ) );
			DropItem( new Gold( 541 ) );
			DropItem( new Gold( 489 ) );
			DropItem( new SpecialHairDye() );
		}

      public MonsterChest( Serial serial ) : base( serial )
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