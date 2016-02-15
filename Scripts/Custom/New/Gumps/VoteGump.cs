using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Gumps
{
	public class AutoVoteGump : Gump
	{
		public AutoVoteGump() : base( 0, 0 )
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage( 0 );
			AddBackground( 201, 122, 381, 210, 9380 );
			AddHtml( 281, 154, 232, 40, "<CENTER>Please vote for Defiance</CENTER>", false, false );
			AddImage( 542, 91, 10410 );
			//AddImage( 159, 303, 10402 );
			AddLabel( 307, 190, 0, "Just follow the link below:" );
			AddHtml( 273, 212, 232, 40, "<CENTER><a href=\"http://www.gamesites200.com/ultimaonline/in.php?id=1504\">Click Here</a></CENTER>", false, false );
		}
	}
}