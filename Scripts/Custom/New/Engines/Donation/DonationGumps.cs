using System;
using Server.Accounting;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server.Gumps
{
  public delegate int GetHueMethod( Account acc );
	public delegate void SetHueMethod( Account acc, int hue );


	public class DonationSettingsGump : Gump
	{
		Account m_Account;
		public DonationSettingsGump( Mobile m ) : base( 20, 20 )
		{
			if ( !(m is PlayerMobile ) || m == null || m.Account == null )
				return;

			m_Account = (Account)m.Account;

			AddPage( 0 );



			AddBackground( 0, 0, 300, 250, 2600 );

			AddLabel( 60, 30, 2401, "Donation Settings" );

			int errors = 0;

			TryButton( 30, 50, m, 90, 1, ref errors );
			AddLabel( 65, 50, 2401, "Set Horse Hue" );
			TryButton( 30, 70, m, 30, 2, ref errors );
			AddLabel( 65, 70, 2401, "Set Sandal Hue" );
			TryButton( 30, 90, m, 30, 3, ref errors );
			AddLabel( 65, 90, 2401, "Set Bandana Hue" );
			TryButton( 30, 110, m, 180, 4, ref errors );
			AddLabel( 65, 110, 2401, "Set Robe Hue" );

			TimeSpan timeleft = ((PlayerMobile)m).DonationTimeLeft;

			if ( timeleft == TimeSpan.MaxValue )
				AddLabel( 30, 140, 2401, "You have a permanent membership" );
			else
			{
				if ( timeleft > TimeSpan.FromSeconds( 0.0 ) )
					AddLabel( 30, 140, 2401, String.Format( "Your membership expires in {0} days", (int)timeleft.TotalDays ) );
				if ( errors > 0 )
					AddLabel( 30, 160, 37, "* required membership length" );
				AddLabel( 30, 180, 2401, "Type [donate to donate" );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			if ( (Account)m.Account != m_Account )
				return;
			switch ( info.ButtonID )
			{
				case 1:
				{
					m.SendGump( new DonationHuePickerGump( m, DonationSystem.DonationHorseHues, new GetHueMethod( DonationSystem.GetHorseHue ), new SetHueMethod( DonationSystem.SetHorseHue ), "Pick Horse Hue" ) );
					break;
				}
        case 2:
        {
          m.SendGump( new DonationHuePickerGump( m, DonationSystem.DonationSandalHues, new GetHueMethod( DonationSystem.GetSandalHue ), new SetHueMethod( DonationSystem.SetSandalHue ), "Pick Sandal Hue" ) );
          break;
        }
        case 3:
        {
          m.SendGump( new DonationHuePickerGump( m, DonationSystem.DonationBandanaHues, new GetHueMethod( DonationSystem.GetBandanaHue ), new SetHueMethod( DonationSystem.SetBandanaHue ), "Pick Bandana Hue" ) );
          break;
        }
				case 4:
				{
					m.SendGump( new DonationHuePickerGump( m, DonationSystem.DonationRobeHues, new GetHueMethod( DonationSystem.GetRobeHue ), new SetHueMethod( DonationSystem.SetRobeHue ), "Pick Robe Hue" ) );
					break;
				}
			}
		}

		private void TryButton( int x, int y, Mobile m, int days, int id, ref int errors )
		{
			//Al: +1 day to enable people using a 90 day ticket to use the feature set to 90 days
			if ( m is PlayerMobile && ((int)((PlayerMobile)m).DonationTimeLeft.TotalDays+1) >= days )
				AddButton( x, y, 4005, 4007, id, GumpButtonType.Reply, 0 );
			else
			{
				errors++;
				AddLabel( x, y, 37, String.Format( "{0}*", days ) );
			}
		}
	}


  public class DonationHuePickerGump : Gump
  {
    Account m_Account;
		int [] m_Hues;
		GetHueMethod m_GetHueMethod;
		SetHueMethod m_SetHueMethod;
		string m_Caption;

    public DonationHuePickerGump( Mobile m, int [] hues, GetHueMethod gethuemethod, SetHueMethod sethuemethod, string caption ) : base( 20, 20 )
    {
      if ( m == null || m.Account == null || hues == null || hues.Length <= 0 || gethuemethod == null || sethuemethod == null || caption == null )
        return;

      m_Account = (Account)m.Account;
			m_Hues = hues;
			m_GetHueMethod = gethuemethod;
			m_SetHueMethod = sethuemethod;
			m_Caption = caption;

      int cushue = m_GetHueMethod( m_Account );

      AddPage( 0 );

			AddBackground( 0, 0, 200, 120 + m_Hues.Length * 30, 2600 );

			AddLabel( 50, 30, 2401, m_Caption );

      int c = 0;
      foreach ( int hue in m_Hues )
      {
        AddRadio( 30, 50 + c * 30, 9727, 9730, (cushue == hue), c );
        AddLabel( 65, 55 + c * 30, (hue == 0) ? 2401 : (hue == 1177) ? 52 : hue - 1, (hue == 0) ? "Regular Hue" : (hue == 1177) ? "Blaze Hue" : "*****" );
        c++;
      }
      AddButton( 30, 60 + c * 30, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddLabel( 65, 60 + c * 30, 2401, "Apply" );
    }

    public override void OnResponse( NetState state, RelayInfo info )
    {
      Mobile m = state.Mobile;
      if ( (Account)m.Account != m_Account )
        return;
      if ( info.ButtonID == 1 )
      {
        int [] switches = info.Switches;
        if ( switches != null && switches.Length == 1 )
				{
					int selected = switches[0];
					if ( selected >= 0 && selected < m_Hues.Length )
	          m_SetHueMethod( m_Account, m_Hues[selected] );
				}
      }
			//m.SendGump( new DonationSettingsGump( m ) );
    }
  }


	public class DonationStatusGump : Gump
	{
		PlayerMobile m_Mobile;
		Account m_Account;
		public DonationStatusGump( PlayerMobile m )
			: base( 20, 20 )
		{
			if ( m == null || m.Account == null )
				return;

			m_Mobile = m;
			m_Account = (Account)m.Account;

			DateTime DonationStart;
			TimeSpan DonationDuration;

			try
			{
				DonationStart = DateTime.Parse( m_Account.GetTag( "DonationStart" ) );
				DonationDuration = TimeSpan.Parse( m_Account.GetTag( "DonationDuration" ) );
			}
			catch
			{
				DonationStart = DateTime.MinValue;
				DonationDuration = TimeSpan.FromMinutes( 0.0 );
			}

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 300, 185, 5150);
			this.AddLabel(27, 26, 0, String.Format( "Donation status for account {0}", m.Account.ToString() ) );
			this.AddLabel(27, 49, 0, "Current subscription valid until");
			if ( DateTime.Now - DonationStart > DonationDuration )
				this.AddLabel(27, 69, 0, String.Format( "No current subscription" ) );
			else if ( DonationDuration == TimeSpan.MaxValue )
				this.AddLabel(27, 69, 0, String.Format( "Permanent subscription" ) );
			else
				this.AddLabel(27, 69, 0, (DonationStart + DonationDuration).ToString() );
			this.AddLabel(57, 94, 0, "New subscription");

			this.AddRadio(26, 95, 210, 211, true, 0 );
			this.AddTextEntry( 57, 110, 50, 20, 0, 0, "");
			this.AddLabel(111, 115, 0, "Days" );
			this.AddRadio(27, 131, 210, 211, false, 1 );
			this.AddLabel(57, 134, 0, "Permanent subscription" );
			this.AddButton(233, 110, 4023, 4024, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			if ( info.ButtonID != 1 || from == null || from.AccessLevel < AccessLevel.Administrator )
				return;

			int[] switches = info.Switches;
			if ( switches.Length > 0 )
			{
				int index = switches[0];
				if ( index < 0 || index > 1 )
					return;

				switch ( index )
				{
					case 0:
					{

						TextRelay relay = info.GetTextEntry( 0 );
						string text = ( relay == null ? null : relay.Text.Trim() );
						if ( text != null && text.Length > 0 )
						{
							int days = 0;
							try
							{
								days = Int32.Parse( text );
							}
							catch
							{
								from.SendMessage( "Invalid format." );
								from.SendGump( new DonationStatusGump( m_Mobile ) );
								return;
							}
							if ( days < 0 )
								days = 0;

							DateTime DonationStart = DateTime.Now;
							TimeSpan DonationDuration = TimeSpan.FromDays( (double)days );

							try
							{
								m_Account.SetTag( "DonationStart", DonationStart.ToString() );
								m_Account.SetTag( "DonationDuration", DonationDuration.ToString() );
							}
							catch
							{
								from.SendMessage( "Invalid format." );
								from.SendGump( new DonationStatusGump( m_Mobile ) );
								return;
							}

						}
						break;
					}
					case 1:
					{
						try
						{
							m_Account.SetTag( "DonationStart", DateTime.Now.ToString() );
							m_Account.SetTag( "DonationDuration", TimeSpan.MaxValue.ToString() );
						}
						catch
						{
							from.SendMessage( "Invalid format." );
							from.SendGump( new DonationStatusGump( m_Mobile ) );
							return;
						}
						break;
					}
				}
			}
			from.SendGump( new DonationStatusGump( m_Mobile ) );
		}
	}
}