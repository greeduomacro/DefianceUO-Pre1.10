
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server;

namespace Server.Items
{
	public class NoxiousEssence : Item
	{
		[Constructable]
		public NoxiousEssence() : this( 1 )
		{
		}


		[Constructable]
		public NoxiousEssence( int amount ) : base( 0xF21 )
		{
			Weight = 1.0;
			Name = "Noxious Essence";
			Stackable = true;
			Amount = amount;
			Hue = 1371;
			LootType = LootType.Newbied;
		}

		public NoxiousEssence( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new NoxiousEssence( amount ), amount );
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