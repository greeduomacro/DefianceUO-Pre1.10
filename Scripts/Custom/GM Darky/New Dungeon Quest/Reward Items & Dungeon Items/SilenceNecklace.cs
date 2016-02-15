
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SilenceNecklace : Necklace
	{
		[Constructable]
		public SilenceNecklace()
		{
			ItemID = 7941;
			Weight = 1.0;
			Name = "Choker of Silence";
			Hue = 2125;
			LootType = LootType.Newbied;
		}

		public SilenceNecklace( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 1.0 )
				Weight = 5.0;
		}
		public override void OnDoubleClick( Mobile from )
		{
			Item u = from.Backpack.FindItemByType( typeof(SilenceNecklace) );
			if ( u !=null )
			{
				if( this.ItemID == 7941 ) this.ItemID = 7944;
				else if( this.ItemID == 7944 ) this.ItemID = 4232;
				else if( this.ItemID == 4232 ) this.ItemID = 4233;
				else if( this.ItemID == 4233 ) this.ItemID = 7941;
			}
			else
			{
                               	from.SendMessage( "You must have the item in your pack to morph it." );
                        }
		}
	}
}