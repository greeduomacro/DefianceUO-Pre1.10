using System;
using Server;
using Server.Gumps;

namespace Server.Gumps
{
	public class GavinFinishGump : Gump
	{
		public GavinFinishGump()
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
			this.AddLabel(48, 39, 255, @"Ah GREAT!.");
			this.AddLabel(48, 55, 255, @"Here, take this gold.");
			this.AddLabel(48, 71, 255, @"Thank ye so much!");
//			this.AddItem(12, 38, 4171);

		}


	}
}