using System;
using Server;
using Server.Network;
using Server.FSPvpPointSystem;

namespace Server.Gumps
{
	public class PvpSelectGump : Gump
	{
		public PvpSelectGump() : base( 25, 25 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(15, 11, 312, 199, 9200);
			AddAlphaRegion(21, 17, 298, 78);
			AddHtml( 21, 17, 298, 78, @"<BASEFONT COLOR=WHITE><CENTER>Please select from one of the following menus.</CENTER></BASEFONT>", (bool)false, (bool)false);
			AddButton(25, 105, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddButton(25, 130, 4005, 4006, 2, GumpButtonType.Reply, 0);
			AddButton(25, 155, 4005, 4006, 3, GumpButtonType.Reply, 0);
			AddButton(25, 180, 4005, 4006, 4, GumpButtonType.Reply, 0);
			AddLabel(60, 105, 1149, @"Most Battles Won Board");
			AddLabel(60, 130, 1149, @"Most Battles Lost Board");
			AddLabel(60, 155, 1149, @"Most Res Kills Board");
			AddLabel(60, 180, 1149, @"Most Res Killed Board");
		}

      		public override void OnResponse( NetState state, RelayInfo info )
      		{
			Mobile from = state.Mobile;

			if ( from == null )
				return;

			Gump gump;


			switch( info.ButtonID )
			{
				default:
				case 1: gump = new MostWinsGump(); break;
				case 2: gump = new MostLosesGump(); break;
				case 3: gump = new MostResKillersGump(); break;
				case 4: gump = new MostResKilliesGump(); break;
			}

			from.SendGump( gump );
		}
	}
}