using System;
using Server;
using Server.Gumps;

namespace Server.Gumps
{
       public class CollectGump : Gump
       {
               private static string[] Links = new string[]
                       {
                               "http://www.defianceuo.com/vote.htm",
                               "http://www.defianceuo.com/donationcart/cart.php",
                               "http://www.defianceuo.com/forum/viewforum.php?f=3",
               };

               private static string[] Descriptions = new string[]
                       {
                               "Vote For Defiance",
                               "Defiance Donations",
                               "Defiance UOR Forums",
               };

               public CollectGump()
                       : base( 200, 200 )
               {
this.Closable=false;
this.Disposable=false;
this.Dragable=true;
this.Resizable=false;
this.AddPage(0);
this.AddBackground(4, 19, 304, 188, 9380);
this.AddLabel(49, 78, 255, @"Please visit our links below:");
this.AddLabel(49, 61, 255, @"This may take up to 25 seconds.....");
//this.AddLabel(82, 25, 1132, @"The World is Collecting...");
this.AddLabel(83, 24, 1149, @"The World is Saving...");
this.AddImage(231, 133, 92);

for ( int i = 0; i < Links.Length; i++ )
this.AddHtml( 49, 104 + 16 * i, 300, 30, String.Format( "<a href=\"{0}\">{1}</a>", Links[i], Descriptions[i] ), false, false );

		}


	}
}