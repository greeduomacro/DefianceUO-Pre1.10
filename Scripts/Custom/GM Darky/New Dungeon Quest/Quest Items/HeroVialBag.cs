
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
   	public class HeroVialBag : Bag
   	{
      		[Constructable]
      		public HeroVialBag() : this( 1 )
      		{
			Movable = true;
			Hue = 39;
			Name = "a bag of Vials";
      		}
		[Constructable]
		public HeroVialBag( int amount )
		{
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
			DropItem( new EmptyHeroVial1() );
		}


      		public HeroVialBag( Serial serial ) : base( serial )
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