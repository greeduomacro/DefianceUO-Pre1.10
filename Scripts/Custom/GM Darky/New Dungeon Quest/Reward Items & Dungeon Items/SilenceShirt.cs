
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SilenceShirt : Surcoat
	{
		[Constructable]
		public SilenceShirt()
		{
			ItemID = 8189;
			Weight = 1.0;
			Name = "Bindings of Silence";
			Hue = 2125;
		}

		public SilenceShirt( Serial serial ) : base( serial )
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
			Item u = from.Backpack.FindItemByType( typeof(SilenceShirt) );
			if ( u !=null )
			{
				if( this.ItemID == 8189 ) this.ItemID = 8097;
				else if( this.ItemID == 8097 ) this.ItemID = 8059;
				else if( this.ItemID == 8059 ) this.ItemID = 5437;
				else if( this.ItemID == 5437 ) this.ItemID = 8095;
				else if( this.ItemID == 8095 ) this.ItemID = 5441;
				else if( this.ItemID == 5441 ) this.ItemID = 8189;
			}
			else
			{
                               	from.SendMessage( "You must have the item in your pack to morph it." );
                        }
		}
	}
}