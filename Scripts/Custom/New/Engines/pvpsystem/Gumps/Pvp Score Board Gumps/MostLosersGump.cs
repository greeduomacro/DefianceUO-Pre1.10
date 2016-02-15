using System;
using Server;
using Server.Gumps;
using Server.Guilds;
using System.Collections;
using Server.FSPvpPointSystem;

namespace Server.Gumps
{
	public class MostLosesGump : Gump
	{
		public MostLosesGump() : base( 0, 0 )
		{
			FSPvpSystem.CheckTopLosers();

			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(31, 13, 449, 504, 9250);
			AddAlphaRegion(44, 26, 423, 37);
			AddAlphaRegion(44, 70, 252, 23);
			AddAlphaRegion(303, 70, 77, 23);
			AddAlphaRegion(388, 70, 79, 23);
			AddAlphaRegion(44, 100, 252, 404);
			AddAlphaRegion(303, 100, 77, 404);
			AddAlphaRegion(388, 100, 79, 404);
			AddHtml( 44, 26, 423, 37, @"<BASEFONT COLOR=WHITE><CENTER>Player vs. Player Scoreboard<BR>Top 20 Loses</CENTER></BASEFONT>", (bool)false, (bool)false);
			AddHtml( 44, 70, 252, 23, @"<BASEFONT COLOR=WHITE><CENTER>Player Names</CENTER></BASEFONT>", (bool)false, (bool)false);
			AddHtml( 303, 70, 77, 23, @"<BASEFONT COLOR=WHITE><CENTER>Loses</CENTER></BASEFONT>", (bool)false, (bool)false);
			AddHtml( 388, 70, 79, 23, @"<BASEFONT COLOR=WHITE><CENTER>Guild</CENTER></BASEFONT>", (bool)false, (bool)false);

			ArrayList list = FSPvpSystem.Losers;

         		int k = 0;
			int listPage = 0;

         		for ( int i = 0, j = 0, index=( ( listPage * 20 ) + k ) ; i < 20 && index >= 0 && index < list.Count && j >= 0; ++i, ++j, ++index )
         		{
            			FSPvpSystem.PvpStats ps = list[index] as FSPvpSystem.PvpStats;

				int offset = 105 + (i * 20);

				AddLabel(55, offset, 1149, ps.Owner.Name.ToString() );
				AddLabel(310, offset, 1149, ps.Loses.ToString() );

                  		Guild g = ps.Owner.Guild as Guild;

                  		if ( g != null )
                 		{
                    			string abb;

					abb = "[" + g.Abbreviation + "]";

					AddLabel(395, offset, 1149, abb );
				}
				else
				{
					AddLabel(395, offset, 1149, @"N/A");
				}
			}
		}
	}
}