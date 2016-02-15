using System;
using Server;
using Server.Gumps;

namespace Server.Gumps
{
	public class CiissFinishGump : Gump
	{
		public CiissFinishGump()
			: base( 200, 200 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 353, 118, 9270);
			this.AddAlphaRegion( 2, 2, 349, 114 );
//			this.AddItem(297, 38, 4168);
			this.AddLabel(118, 15, 1149, @"Quest Complete");
			this.AddLabel(48, 39, 255, @"Aaahsss perrrfectss. Thanksss you sssso muchsss!.");
			this.AddLabel(48, 55, 255, @"Pleasssse, take thisss as a sssign of my...");
			this.AddLabel(48, 71, 255, @"grrratitudess. Thankss you... againsss!");
//			this.AddItem(12, 38, 4171);

		}


	}
}