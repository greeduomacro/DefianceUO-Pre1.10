//**************************************************************************//
//*** Script by k0t (messiah@planet-akc.de) - 24.09.03 - http://thandor.lichtentown.de ****//
//*************************************************************************//

using System;
using Server;
using Server.Network;

namespace Server.Custom.Regions
{
   public class weatherTimer : Timer
   {
      public static void Initialize()
      {
         new weatherTimer();
      }
      public weatherTimer() : base(TimeSpan.FromSeconds(10),TimeSpan.FromDays(1))
      {
         this.Start();
      }
      protected override void OnTick()
      {
         switch(DateTime.Now.Month)
         {
            case 1:
            case 2:
            case 11:
            case 12:
            {
               Map.Felucca.Season = 1;
               Map.Trammel.Season = 1;
               Map.Ilshenar.Season = 1;
               Map.Malas.Season = 1;
               Console.WriteLine( "Season: Desolation" );
            }
               break;
            case 3:
            case 4:
            case 5:
            {
               Map.Felucca.Season = 1;
               Map.Trammel.Season = 1;
               Map.Ilshenar.Season = 1;
               Map.Malas.Season = 1;
               Console.WriteLine( "Season: Spring" );
            }
               break;
            case 6:
            case 7:
            case 8:
            {
               Map.Felucca.Season = 1;
               Map.Trammel.Season = 1;
               Map.Ilshenar.Season = 1;
               Map.Malas.Season = 1;
               Console.WriteLine( "Season: Summer" );
            }
               break;
            case 9:
            case 10:
            {
               Map.Felucca.Season = 1;
               Map.Trammel.Season = 1;
               Map.Ilshenar.Season = 1;
               Map.Malas.Season = 1;
               Console.WriteLine( "Season: Fall" );
            }
               break;
            default:
               Console.WriteLine( "Season: End of the world man, unknown month." );
               break;
         }
      }
   }
}