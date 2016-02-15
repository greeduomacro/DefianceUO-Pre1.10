
/////////////////////////////////////////////
//Created by LostSinner & Modified by Darky//
/////////////////////////////////////////////
using System;
using Server.Items;

namespace Server.Items
{
   	public class JalindeLetter: Item
   	{

		[Constructable]
		public JalindeLetter()
		{
			ItemID = 7989;
			Weight = 0.1;
			Name = "Letter to Jalinde Summerdrake";
			Hue = 1150;
			LootType = LootType.Newbied;
		}

            	public JalindeLetter( Serial serial ) : base ( serial )
            	{
           	}

           	public override void Serialize( GenericWriter writer )
           	{
              		base.Serialize( writer );
              		writer.Write( (int) 0 );
           	}

           	public override void Deserialize( GenericReader reader )
           	{
              		base.Deserialize( reader );
              		int version = reader.ReadInt();
           	}
        }
}