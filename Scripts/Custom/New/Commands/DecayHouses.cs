using System;
using System.Collections;
using Server;
using Server.Multis;
using Server.Accounting;

namespace Server.Scripts.Commands
{
    public class DecayHouses
    {
        public static void Initialize()
        {
            Server.Commands.Register("DecayHouses", AccessLevel.Administrator, new CommandEventHandler(DecayHouses_OnCommand));
        }

        [Usage("DecayHouses")]
        [Description("Decays Houses based on a date time.")]
        public static void DecayHouses_OnCommand( CommandEventArgs e )
        {
			int counter = 0;

			DateTime time = DateTime.MinValue;
			try
			{
				time = DateTime.Parse( e.ArgString );
			}
			catch
			{
				e.Mobile.SendMessage( "Format: [decayhouses <datetime>" );
				return;
			}

			foreach( Account acct in Accounts.Table.Values )
			{
				TimeSpan duration = TimeSpan.Zero;
				try
				{
					duration = TimeSpan.Parse( acct.GetTag( "DonationDuration" ) );
				}
				catch
				{
					duration = TimeSpan.Zero;
				}

				if ( acct.LastLogin < time && duration == TimeSpan.MaxValue )
				{
					for ( int i = 0; i < acct.Length; i++ )
					{
						ArrayList list = BaseHouse.GetHouses( acct[i] );
						for ( int j = 0; j < list.Count; j++ )
						{
							if ( list[j] != null )
							{
								((BaseHouse)list[j]).DecayDonator_Sandbox();
								counter++;
							}
						}
					}
				}
			}

			e.Mobile.SendMessage( "Decayed {0} permanent donation houses.", counter );
        }
    }
}