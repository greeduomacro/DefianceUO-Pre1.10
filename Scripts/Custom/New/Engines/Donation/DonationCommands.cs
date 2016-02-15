using System;
using Server.Accounting;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Scripts.Commands
{
	public class DonationCommands
	{
		public static void Initialize()
		{
			Server.Commands.Register( "DonationPanel", AccessLevel.Administrator, new CommandEventHandler( DonationPanel_OnCommand ) );
			Server.Commands.Register( "DonatorsOnline", AccessLevel.GameMaster, new CommandEventHandler( DonatorsOnline_OnCommand ) );
			Server.Commands.Register( "DonationStatus", AccessLevel.Player, new CommandEventHandler( DonationStatus_OnCommand ) );
			Server.Commands.Register( "DonationSettings", AccessLevel.Player, new CommandEventHandler( DonationSettings_OnCommand ) );
			//Server.Commands.Register( "ConvertDonationStatuses", AccessLevel.GameMaster, new CommandEventHandler( ConvertDonationStatuses_OnCommand ) );
		}

		[Usage( "DonationPanel" )]
		[Description( "Brings up a player's donation panel" )]
		private static void DonationPanel_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new DonationPanelTarget();
			e.Mobile.SendMessage( "Select a player to bring up his donation panel." );
		}

		private class DonationPanelTarget : Target
		{
			public DonationPanelTarget() : base( 15, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( !(targ is PlayerMobile) )
				{
					from.SendMessage( "Use this on a player." );
					return;
				}
				else
					from.SendGump( new DonationStatusGump( (PlayerMobile)targ ) );
			}
		}

		[Usage( "DonatorsOnline" )]
		[Description( "Counts online donators" )]
		private static void DonatorsOnline_OnCommand( CommandEventArgs e )
		{
			int donatorsonline = 0;
			foreach ( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if ( m != null && m is PlayerMobile && m.AccessLevel == AccessLevel.Player && ((PlayerMobile)m).HasDonated )
					donatorsonline++;
			}
			e.Mobile.SendMessage( "Donators online: {0}.", donatorsonline );
		}

		[Usage( "DonationStatus" )]
		[Description( "Shows when your subscription expires" )]
		private static void DonationStatus_OnCommand( CommandEventArgs e )
		{
			PlayerMobile m = e.Mobile as PlayerMobile;
			if ( m == null )
				return;

			if ( m.DonationTimeLeft <= TimeSpan.Zero )
				m.SendMessage( "You currently have no subscription. Type [donate for information on how to subscribe." );
			else if ( m.DonationTimeLeft == TimeSpan.MaxValue )
				m.SendMessage( "You have a permanent subscription." );
			else
				m.SendMessage( String.Format( "Your subscription will expire in {0} days.", (int)m.DonationTimeLeft.Days ) );
		}

    [Usage( "DonationSettings" )]
    [Description( "Opens the Donation Settings gump" )]
    private static void DonationSettings_OnCommand( CommandEventArgs e )
    {
      PlayerMobile m = e.Mobile as PlayerMobile;
      if ( m == null )
        return;
	//m.SendMessage( "This is not active yet..." );

      /*if ( m.DonationTimeLeft <= TimeSpan.Zero )
        m.SendMessage( "You currently have no subscription. Type [donate for information on how to subscribe." );
      else*/
				m.SendGump( new DonationSettingsGump( m ) );
    }

		[Usage( "ConvertDonationStatuses" )]
    [Description( "Converts donation statuses." )]
    private static void ConvertDonationStatuses_OnCommand( CommandEventArgs e )
    {
      int converted = 0;
      foreach ( object o in World.Mobiles.Values )
      {
        if ( o == null || !(o is PlayerMobile) )
          continue;
        PlayerMobile m = (PlayerMobile)o;
				if ( m.DonationDuration == TimeSpan.MaxValue )
				{
					converted++;
					m.DonationDuration = TimeSpan.FromDays( 120.0 );
				}
			}
			e.Mobile.SendMessage( "{0} converted", converted );
		}


		/*[Usage( "ConvertDonationStatuses" )]
		[Description( "Converts donation statuses." )]
		private static void ConvertDonationStatuses_OnCommand( CommandEventArgs e )
		{
			int converted = 0, removed = 0;
			foreach ( object o in World.Mobiles.Values )
			{
				if ( o == null || !(o is PlayerMobile) )
					continue;
				PlayerMobile m = (PlayerMobile)o;
				Account acc = null;
				if ( m != null )
					acc = (Account)m.Account;
				if ( acc != null )
				{
					e.Mobile.SendMessage( "Checking..." );
					if ( Insensitive.Equals( acc.GetTag("hasdonated"), "true" ) )
					{
						acc.SetTag( "DonationStart", DateTime.Now.ToString() );
						acc.SetTag( "DonationDuration", TimeSpan.MaxValue.ToString() );
						acc.RemoveTag( "hasdonated" );
						converted++;
					}
					else if ( Insensitive.Equals( acc.GetTag("hasdonated"), "false" ) )
					{
						acc.RemoveTag( "hasdonated" );
						removed++;
					}
					else
						e.Mobile.SendMessage( "No such tag..." );
				}
			}
			e.Mobile.SendMessage( "Donators converted: {0}, tags removed: {1}.", converted, removed );
		}*/
	}
}