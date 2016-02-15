
//////////////////////////
//Created by LostSinner//
////////////////////////
using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
   	public class NoxiousGemBag : Bag
   	{
      		[Constructable]
      		public NoxiousGemBag() : this( 1 )
      		{
			Movable = true;
			Hue = 39;
			Name = "a bag of empty Noxious Gems";
      		}
		[Constructable]
		public NoxiousGemBag( int amount )
		{

			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );
			DropItem( new EmptyNoxiousGem1() );

		}


      		public NoxiousGemBag( Serial serial ) : base( serial )
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