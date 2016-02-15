//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BlackBag : Bag
	{
		[Constructable]
		public BlackBag() : this( 1 )
		{
		}

		[Constructable]
		public BlackBag(int amount)
		{
			Name = "Black Knights Armour";
			Hue = 1;

			DropItem( new BlackHelm());
			DropItem( new BlackGorget());
			DropItem( new BlackArms());
			DropItem( new BlackGloves());
			DropItem( new BlackLegs());
			DropItem( new BlackChest());
			DropItem( new BlackShield());
			DropItem( new BlackShroud());
			DropItem( new BlackthornsBlade());
		}

		public BlackBag( Serial serial ) : base( serial )
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