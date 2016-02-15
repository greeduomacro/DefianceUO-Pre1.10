
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server;

namespace Server.Items
{
	public class BloodOfHeroes : Item
	{
		[Constructable]
		public BloodOfHeroes() : this( 1 )

		{
		}

		[Constructable]
		public BloodOfHeroes( int amount )
		{
			ItemID = 3620;
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Name = "Blood of Heroes";
			Hue = 1157;
			LootType = LootType.Newbied;
		}

		public BloodOfHeroes( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new BloodOfHeroes( amount ), amount );
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