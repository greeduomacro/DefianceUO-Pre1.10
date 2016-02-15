
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SilenceHelm : Bandana
	{
		[Constructable]
		public SilenceHelm()
		{
			ItemID = 5440;
			Weight = 5.0;
			Name = "Halo of Silence";
			Hue = 2125;
		}

		public SilenceHelm( Serial serial ) : base( serial )
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
			Item u = from.Backpack.FindItemByType( typeof(SilenceHelm) );
			if ( u !=null )
			{

				if( this.ItemID == 5440 ) this.ItemID = 5910;
				else if( this.ItemID == 5910 ) this.ItemID = 5915;
				else if( this.ItemID == 5915 ) this.ItemID = 5908;
				else if( this.ItemID == 5908 ) this.ItemID = 5916;
				else if( this.ItemID == 5916 ) this.ItemID = 5912;
				else if( this.ItemID == 5912 ) this.ItemID = 5444;
				else if( this.ItemID == 5444 ) this.ItemID = 5911;
				else if( this.ItemID == 5911 ) this.ItemID = 5907;
				else if( this.ItemID == 5907 ) this.ItemID = 5913;
				else if( this.ItemID == 5913 ) this.ItemID = 5909;
				else if( this.ItemID == 5909 ) this.ItemID = 5914;
				else if( this.ItemID == 5914 ) this.ItemID = 5440;
			}
			else
			{
                               	from.SendMessage( "You must have the item in your pack to morph it." );
                        }
		}
	}
}