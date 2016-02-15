//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WhiteBag : Bag
	{
		[Constructable]
		public WhiteBag() : this( 1 )
		{
		}

		[Constructable]
		public WhiteBag(int amount)
		{
			Name = "White Knights Armour";
			Hue = 1153;

			DropItem( new WhiteHelm());
			DropItem( new WhiteGorget());
			DropItem( new WhiteArms());
			DropItem( new WhiteGloves());
			DropItem( new WhiteLegs());
			DropItem( new WhiteChest());
			DropItem( new WhiteShield());
			DropItem( new WhiteShroud());
			DropItem( new DupresBlade());
		}

		public WhiteBag( Serial serial ) : base( serial )
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