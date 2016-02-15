
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SilenceLeggings : Skirt
	{
		[Constructable]
		public SilenceLeggings()
		{
			ItemID = 5398;
			Weight = 5.0;
			Name = "Leggings of Silence";
			Hue = 2125;
		}

		public SilenceLeggings( Serial serial ) : base( serial )
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
			Item u = from.Backpack.FindItemByType( typeof(SilenceLeggings) );
			if ( u !=null )
			{
				if( this.ItemID == 5398 ) this.ItemID = 5422;
				else if( this.ItemID == 5422 ) this.ItemID = 5433;
				else if( this.ItemID == 5433 ) this.ItemID = 5431;
				else if( this.ItemID == 5431 ) this.ItemID = 5398;
			}
			else
			{
                               	from.SendMessage( "You must have the item in your pack to morph it." );
                        }
		}
	}
}