
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SilenceShoes : Shoes
	{
		[Constructable]
		public SilenceShoes()
		{
			ItemID = 5903;
			Weight = 5.0;
			Name = "Boots of Silence";
			Hue = 2125;
		}

		public SilenceShoes( Serial serial ) : base( serial )
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
			Item u = from.Backpack.FindItemByType( typeof(SilenceShoes) );
			if ( u !=null )
			{
				if( this.ItemID == 5903 ) this.ItemID = 5899;
				else if( this.ItemID == 5899 ) this.ItemID = 5905;
				else if( this.ItemID == 5905 ) this.ItemID = 5903;
			}
			else
			{
                               	from.SendMessage( "You must have the item in your pack to morph it." );
                        }
		}
	}
}