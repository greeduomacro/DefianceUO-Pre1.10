using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Menus;
using Server.Menus.Questions;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
	public class Abacus : Item
	{
		[Constructable]
		public Abacus() : base( 5383 )
		{
			Hue = TokenSettings.Token_Colour;
			Movable = true;
			Name = "an Abacus";
		}

		public Abacus( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			int TTokenPack=0;//total tokens ON the player
			int TTokenBank=0;//total tokens in the players bank
			int TToken=0;//Total Tokens

      		float TTokenPack2=0;//total tokens ON the player
			float TTokenBank2=0;//total tokens in the players bank
			float TToken2=0;//Total Tokens

			string formatbank="";
			string formatpack="";
			string formattotal="";

			int decimalbank=0;
			int decimalpack=0;
			int decimaltotal=0;

			foreach ( Item item in from.Backpack.Items )
			{
				if ( item is Tokens )
				{
					TTokenPack += item.Amount;
				}
				else if ( item is Container )
				{
					ArrayList list = item.Items;
					for ( int i = 0; i < list.Count; ++i )
					{
						Item inbag = (Item)list[i];

						if ( inbag is Tokens )
						{
							TTokenPack += inbag.Amount;
						}

					}
				}
			}

			foreach( Item item in from.BankBox.Items )
			{
				if( item is Tokens )
				{
					TTokenBank += item.Amount;
				}
				else if( item is Container )
				{
					ArrayList list = item.Items;

					for ( int i = 0; i < list.Count; ++i )
					{
						Item inbag = (Item)list[i];

						if ( inbag is Tokens )
						{
							TTokenBank += inbag.Amount;
						}
					}
				}
			}

			TToken = TTokenPack + TTokenBank;

			if ( TokenSettings.Currency_Format.ToLower().StartsWith("y") )
			{
				//Pack
				if ( TTokenPack < 1000 )
				{
					formatpack="";
				}
				if ( TTokenPack >= 1000 && TTokenPack < 1000000 )
				{
					TTokenPack2=TTokenPack;
					TTokenPack2=TTokenPack2/1000;
					formatpack=" Thousand";
					decimalpack=TokenSettings.Places_Thousand;
				}
				if ( TTokenPack > 999999 )
				{
					TTokenPack2=TTokenPack;
					TTokenPack2=TTokenPack2/1000000;
					formatpack=" Million";
					decimalpack=TokenSettings.Places_Million;
				}
				//Bank
				if ( TTokenBank < 1000 )
				{
					formatbank="";
				}
				if ( TTokenBank >= 1000 && TTokenBank < 1000000 )
				{
					TTokenBank2=TTokenBank;
					TTokenBank2=TTokenBank2/1000;
					formatbank=" Thousand";
					decimalbank=TokenSettings.Places_Thousand;
				}
				if ( TTokenBank > 999999 )
				{
					TTokenBank2=TTokenBank;
					TTokenBank2=TTokenBank2/1000000;
					formatbank=" Million";
					decimalbank=TokenSettings.Places_Million;
				}

				//Total
				if ( TToken < 1000 )
				{
					formattotal="";
				}
				if ( TToken >= 1000 && TToken < 1000000 )
				{
					TToken2=TToken;
					TToken2=TToken2/1000;
					formattotal=" Thousand";
					decimaltotal=TokenSettings.Places_Thousand;
				}
				if ( TToken > 999999 )
				{
					TToken2=TToken;
					TToken2=TToken2/1000000;
					formattotal=" Million";
					decimaltotal=TokenSettings.Places_Million;
				}
			}
			else
			{
			}

			if ( TokenSettings.Use_Abacus_Gump.ToLower().StartsWith("y") && TokenSettings.Currency_Format.ToLower().StartsWith("y"))
			{
				//int valuef = 2;
				from.SendGump( new AbacusGump( from, String.Format("{0:f"+decimalpack+"}",TTokenPack2), String.Format("{0:f"+decimalbank+"}",TTokenBank2), String.Format("{0:f"+decimaltotal+"}",TToken2), formattotal, formatbank, formatpack ) );
			}

			if ( TokenSettings.Use_Abacus_Gump.ToLower().StartsWith("y") && TokenSettings.Currency_Format.ToLower().StartsWith("n"))
			{
				from.SendGump( new AbacusGump( from, TTokenPack, TTokenBank, TToken, formattotal, formatbank, formatpack ) );
			}

			if ( TokenSettings.Use_Abacus_Gump.ToLower().StartsWith("n") && TokenSettings.Currency_Format.ToLower().StartsWith("y"))
			{
				from.SendMessage("Backpack: "+String.Format("{0:f"+decimalpack+"}",TTokenPack2)+formatpack+" Tokens");
				from.SendMessage("Bank: "+String.Format("{0:f"+decimalbank+"}",TTokenBank2)+formatbank+" Tokens");
				from.SendMessage("Total: "+String.Format("{0:f"+decimaltotal+"}",TToken2)+formattotal+" Tokens");
			}

			if ( TokenSettings.Use_Abacus_Gump.ToLower().StartsWith("n") && TokenSettings.Currency_Format.ToLower().StartsWith("n"))
			{
				from.SendMessage("Backpack: "+TTokenPack+formatpack+" Tokens");
				from.SendMessage("Bank: "+TTokenBank+formatbank+" Tokens");
				from.SendMessage("Total: "+TToken+formattotal+" Tokens");
			}
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