using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Misc
{

	public class CountShardPlatinum
	{
		public static float WorkHorse()
		{
			float tokencount=0;

			foreach( Item i in World.Items.Values )
			{
				if (i is Tokens)
				{
					tokencount = tokencount + i.Amount;
				}
				else if (i is TokensBankCheck)
				{
					tokencount += ((TokensBankCheck)i).Worth;
				}
			}
		return tokencount;
		}

		public static void Initialize()
		{
			Commands.Register( "CountShardPlatinum", AccessLevel.Administrator, new CommandEventHandler( CountShardPlatinum_OnCommand ) );
		}

                [Usage( "CountShardPlatinum" )]
                [Description( "Checks for the full amount of copper on the shard." )]
		private static void CountShardPlatinum_OnCommand( CommandEventArgs args )
		{
			float tokencount = WorkHorse();
			string formatworld="";
			int decimalworld=0;

			if ( TokenSettings.Currency_Format.ToLower().StartsWith("y"))
			{
				//World
				if ( tokencount < 1000 )
				{
					formatworld="";
				}
				if ( tokencount >= 1000 && tokencount < 1000000 )
				{
					tokencount=tokencount/1000;
					formatworld=" Thousand";
					decimalworld=TokenSettings.Places_Thousand;
				}
				if ( tokencount > 999999 )
				{
					tokencount=tokencount/1000000;
					formatworld=" Million";
					decimalworld=TokenSettings.Places_Million;
				}
			}
			else
			{
			}

			args.Mobile.SendMessage("Tokens in the World : "+String.Format("{0:f"+decimalworld+"}",tokencount)+formatworld);
		}

	}
}