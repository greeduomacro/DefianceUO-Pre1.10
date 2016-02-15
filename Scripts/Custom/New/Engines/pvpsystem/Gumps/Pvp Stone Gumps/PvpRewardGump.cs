using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.FSPvpPointSystem;

namespace Server.Gumps
{
	public class PvpRewardGump : Gump
	{
		public PvpRewardGump() : base( 25, 25 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(15, 15, 298, 421, 9250);
			AddAlphaRegion(25, 25, 276, 399);
			//AddButton(30, 50, 4011, 4012, 1, GumpButtonType.Reply, 0);
			AddButton(30, 75, 4011, 4012, 2, GumpButtonType.Reply, 0);
			//AddButton(30, 100, 4011, 4012, 3, GumpButtonType.Reply, 0);
			AddButton(30, 125, 4011, 4012, 4, GumpButtonType.Reply, 0);
			//AddButton(30, 150, 4011, 4012, 5, GumpButtonType.Reply, 0);
			AddButton(30, 175, 4011, 4012, 6, GumpButtonType.Reply, 0);
			//AddButton(30, 200, 4011, 4012, 7, GumpButtonType.Reply, 0);
			AddButton(30, 225, 4011, 4012, 8, GumpButtonType.Reply, 0);
			//AddButton(30, 250, 4011, 4012, 9, GumpButtonType.Reply, 0);
			AddButton(30, 275, 4011, 4012, 10, GumpButtonType.Reply, 0);
			//AddButton(30, 300, 4011, 4012, 11, GumpButtonType.Reply, 0);
			AddButton(30, 325, 4011, 4012, 12, GumpButtonType.Reply, 0);
			AddButton(30, 350, 4011, 4012, 13, GumpButtonType.Reply, 0);
			//AddButton(30, 375, 4011, 4012, 14, GumpButtonType.Reply, 0);
			//AddButton(30, 400, 4011, 4012, 15, GumpButtonType.Reply, 0);
			for( int i = 1; i < 14; i++ )
				AddLabel( 65, 50 + 25 * (i-1), 1149, PvpRankInfo.GetInfo( i ).Title );
			AddLabel(78, 26, 1149, @"Please Select A Catagory");
		}

      		public override void OnResponse( NetState state, RelayInfo info )
      		{
			Mobile from = state.Mobile;
			if ( from == null )
				return;

			FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( from );
			string msg = "You lack the rank required to buy from this list.";
			Gump gump = null;
			int buttonid = info.ButtonID;
			switch ( buttonid )
			{
				case 1: gump = new PvpRewardGump1(); break;
				case 2: gump = new PvpRewardGump2(); break;
				case 3: gump = new PvpRewardGump3(); break;
				case 4: gump = new PvpRewardGump4(); break;
				case 5: gump = new PvpRewardGump5(); break;
				case 6: gump = new PvpRewardGump6(); break;
				case 7: gump = new PvpRewardGump7(); break;
				case 8: gump = new PvpRewardGump8(); break;
				case 9: gump = new PvpRewardGump9(); break;
				case 10: gump = new PvpRewardGump10(); break;
				case 11: gump = new PvpRewardGump11(); break;
				case 12: gump = new PvpRewardGump12(); break;
				case 13: gump = new PvpRewardGump13(); break;
				case 14: gump = new PvpRewardGump14(); break;
			}

			if ( gump != null && ps.RankType >= buttonid )
			{
				from.CloseGump( gump.GetType() );
				from.SendGump( gump );
			}
			else
				from.SendMessage( msg );
		}
	}
}