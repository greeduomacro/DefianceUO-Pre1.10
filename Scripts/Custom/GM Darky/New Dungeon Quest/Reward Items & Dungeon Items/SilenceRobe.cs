
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SilenceRobe : Robe
	{
		[Constructable]
		public SilenceRobe()
		{
			ItemID = 7939;
			Weight = 5.0;
			Name = "Robes of Silence";
			Hue = 2125;
		}

		public SilenceRobe( Serial serial ) : base( serial )
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
			Item u = from.Backpack.FindItemByType( typeof(SilenceRobe) );
			if ( u !=null )
			{
				if( this.ItemID == 7939 ) this.ItemID = 7937;
				else if( this.ItemID == 7937 ) this.ItemID = 7936;
				else if( this.ItemID == 7936 ) this.ItemID = 7939;
			}
			else
			{
                               	from.SendMessage( "You must have the item in your pack to morph it." );
                        }
		}
	}
}