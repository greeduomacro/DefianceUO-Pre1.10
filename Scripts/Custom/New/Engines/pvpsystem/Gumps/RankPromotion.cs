using System;
using Server;
using Server.FSPvpPointSystem;

namespace Server.Gumps
{
	public class RankPromotion : Gump
	{
		public RankPromotion( Mobile from, string title, bool demoted ) : base( 25, 0 )
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage( 0 );
			AddBackground( 0, 0, 200, 150, 9250 );

			AddImage( 70, 75, 5587 );
			AddHtml( 0, 10, 200, 20, String.Format( @"<center>You have been {0} to:</center>", demoted ? "demoted" : "promoted" ), false, false);
			AddHtml( 0, 40, 200, 20, String.Format( @"<center>{0}</center>", title ), false, false);
		}
	}
}