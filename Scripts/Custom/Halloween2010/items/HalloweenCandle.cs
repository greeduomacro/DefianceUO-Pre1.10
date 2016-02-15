using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class HalloweenCandle : SelfDestructingItem
	{
		[Constructable]
		public HalloweenCandle() : base()
		{
			switch (Utility.Random( 5 ))
			{
				case 0:	Name = "Candle of Fear";
						Hue = 1193; break;
				case 1:	Name = "Candle of Blood";
						Hue = 1194; break;
				case 2:	Name = "Candle of Superstition";
						Hue = 1195; break;
				case 3:	Name = "Candle of Poison";
						Hue = 1196; break;
				case 4:	Name = "Candle of Horror";
						Hue = 1175; break;
			}

			ItemID= 0x1853;
			Weight = 9;
            	ShowTimeLeft = true;

			TimeLeft = 5356800; //2 months
			Running = true;
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "the light is slowly fading away" );
			base.OnSingleClick(from);
		}

		public HalloweenCandle( Serial serial ) : base( serial )
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