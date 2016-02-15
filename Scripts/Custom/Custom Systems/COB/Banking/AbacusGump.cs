using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Menus;
using Server.Menus.Questions;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server.Gumps
{
	public class AbacusGump : Gump
	{
		public AbacusGump( Mobile owner, string TTokenPack, string TTokenBank, string TToken, string formattotal, string formatbank, string formatpack ) : base( 275,125 )
		{
			owner.CloseGump( typeof( AbacusGump ) );

			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			this.AddBackground(0, 0, 325, 340, 9200);
			this.AddBackground(10, 60, 305, 230, 3500);
			this.AddLabel(100, 20, 0, @"The Abacus of Wisdom");
			this.AddImage(25, 85, 80);
			this.AddItem(35, 160, 2475);
			this.AddLabel(40, 235, 172, @"Total Tokens :");
			this.AddLabel(135, 235, 195, TToken+""+formattotal);
			this.AddLabel(135, 175, 195, TTokenBank+""+formatbank);
			this.AddLabel(135, 105, 195, TTokenPack+""+formatpack);
			this.AddLabel(90, 105, 0, @"Pack :");
			this.AddLabel(90, 175, 0, @"Bank :");
			AddButton( 275, 300, 210, 211, 0, GumpButtonType.Reply, 0 );
			this.AddLabel(225, 298, 32, @"Close");

		}

		public AbacusGump( Mobile owner, int TTokenPack, int TTokenBank, int TToken, string formattotal, string formatbank, string formatpack ) : base( 275,125 )
		{
			owner.CloseGump( typeof( AbacusGump ) );

			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			this.AddBackground(0, 0, 325, 340, 9200);
			this.AddBackground(10, 60, 305, 230, 3500);
			this.AddLabel(100, 20, 0, @"The Abacus of Wisdom");
			this.AddImage(25, 85, 80);
			this.AddItem(35, 160, 2475);
			this.AddLabel(40, 235, 172, @"Total Tokens :");
			this.AddLabel(135, 235, 195, TToken+""+formattotal);
			this.AddLabel(135, 175, 195, TTokenBank+""+formatbank);
			this.AddLabel(135, 105, 195, TTokenPack+""+formatpack);
			this.AddLabel(90, 105, 0, @"Pack :");
			this.AddLabel(90, 175, 0, @"Bank :");
			AddButton( 275, 300, 210, 211, 0, GumpButtonType.Reply, 0 );
			this.AddLabel(225, 298, 32, @"Close");

		}


		public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons
		{

         Mobile from = state.Mobile;

         switch ( info.ButtonID )
         {
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0
            {
		   //Cancel
       	    break;
            }
		}
		}

	}
}