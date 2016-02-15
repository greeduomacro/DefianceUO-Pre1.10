using System;
using System.Text;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class WebStone : Item
	{
		[Constructable]
		public WebStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1153;
			Name = "Web Stone"; //name this whatever you want, like "Click here for our websites" Or whatever :P
		}

		public override void OnDoubleClick( Mobile from )
		{
		from.SendGump( new WebstoneGump() );
		}

		public WebStone( Serial serial ) : base( serial )
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

namespace Server.Gumps
{

	public class WebstoneGump : Gump
	{


		public WebstoneGump() : base( 0, 0 )
		{
			this.AddPage(0);
			this.AddBackground(213, 131, 234, 258, 5054);

			this.AddButton(241, 165, 4023, 4025, 1, GumpButtonType.Reply, 0);
			this.AddLabel(285, 165, 52, "Defiance Homepage"); //#1 Site's Name

			this.AddButton(241, 195, 4023, 4025, 2, GumpButtonType.Reply, 0);
			this.AddLabel(285, 195, 52, "Forums"); //#2 Site's Name

			this.AddButton(241, 225, 4023, 4025, 3, GumpButtonType.Reply, 0);
			this.AddLabel(285, 225, 52, "Donation Shop"); //#3 Site's Name

			this.AddButton(241, 255, 4023, 4025, 4, GumpButtonType.Reply, 0);
			this.AddLabel(285, 255, 52, "Account Management"); //#4 Site's Name

			this.AddButton(241, 285, 4023, 4025, 5, GumpButtonType.Reply, 0);
			this.AddLabel(285, 285, 52, "Download Center"); //#5 Site's Name

			this.AddButton(241, 315, 4023, 4025, 6, GumpButtonType.Reply, 0);
			this.AddLabel(285, 315, 52, "Stratics"); //#6 Site's Name

			this.AddButton(302, 357, 2453, 4454, 0, GumpButtonType.Reply, 0); // Close Button

			this.AddImage(290, 53, 1417);
			this.AddImage(301, 63, 5608);
			this.AddImage(213, 69, 1419);
			this.AddImage(162, 28, 10440);
			this.AddImage(414, 28, 10441);
			this.AddImage(361, 133, 5217);
			this.AddImage(275, 133, 5217);
			this.AddImage(214, 133, 5217);

		}
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			switch ( info.ButtonID )
			{
				case 1: // #1 Site's Url
					{
						sender.LaunchBrowser( "http://www.defianceuo.com" );
						break;
					}
				case 2: // #2 Site's url
					{
						sender.LaunchBrowser( "http://www.defianceuo.com/forums" );
						break;
					}
				case 3: // #3 Site's url
					{
						sender.LaunchBrowser( "http://www.defianceuo.com/donate" );
						break;
					}
				case 4: // #4 Site's Url
					{
						sender.LaunchBrowser( "http://accounts.defianceuo.com/pwchange.php" );
						break;
					}
				case 5: // #5 Site's url
					{
						sender.LaunchBrowser( "http://www.defianceuo.com/download.php" );
						break;
					}
				case 6: // #6 Site's url
					{
                        sender.LaunchBrowser("http://uo.stratics.com/");
						break;
					}
			}
		}
	}
}