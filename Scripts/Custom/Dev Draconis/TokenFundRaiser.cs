using System;
using System.Collections;
using System.Text;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Regions;
using Server.EventPrizeSystem;

namespace Server.Items
{
	public class TokenFundRaiser : Barrel
	{
		string m_FundName;
		int m_Fund;
		ArrayList m_TokenContributers;
		bool m_ShowTopList;

        public ArrayList TokenContributers
		{
			get
			{
				if ( m_TokenContributers == null )
					m_TokenContributers = new ArrayList();
				for ( int i = 0; i < m_TokenContributers.Count; i++ )
				{
					TokenContributer contributer = (TokenContributer)m_TokenContributers[i];
					if ( contributer == null || contributer.Mobile == null || contributer.Mobile.Deleted )
					{
						m_TokenContributers.Remove( contributer );
						i--;
					}
				}
				return m_TokenContributers;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool ShowTopList
		{
			get { return m_ShowTopList; }
			set { m_ShowTopList = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string FundName
		{
			get { return m_FundName; }
			set { m_FundName = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int Fund
		{
			get { return m_Fund; }
			set { m_Fund = value; }
		}

		[Constructable]
		public TokenFundRaiser() : base()
		{
			Hue = 2207;
			Movable = false;
			FundName = "Random Fund";
		}

		public TokenFundRaiser( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			writer.Write( m_ShowTopList );
			writer.Write( TokenContributers.Count );
			foreach ( TokenContributer contributer in TokenContributers )
			{
				writer.Write( contributer.Amount );
				writer.Write( contributer.Mobile );
				writer.Write( contributer.Date );
			}
			writer.Write( FundName );
			writer.Write( Fund );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_ShowTopList = reader.ReadBool();
					int c = reader.ReadInt();
					for ( int i = 0; i < c; i++ )
					{
						TokenContributers.Add( new TokenContributer( reader.ReadInt(), reader.ReadMobile() ) );
						((TokenContributer)TokenContributers[TokenContributers.Count - 1]).Date = reader.ReadDateTime();
					}
					goto case 0;
				}
				case 0:
				{
					FundName = reader.ReadString();
					Fund = reader.ReadInt();
					break;
				}
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, String.Format( "Raiser for: {0}. {1} worth of tokens so far", m_FundName, m_Fund ) );
			from.SendMessage( "Golden Prize Token is worth 100" );
			from.SendMessage( "Silver Prize Token is worth 10" );
			from.SendMessage( "Bronze Prize Token is worth 1" );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_ShowTopList )
				from.SendGump( new TokenContributersGump( this, from ) );
			else
				base.OnDoubleClick( from );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is GoldenPrizeToken )
			{
				AddToFund( from, ((GoldenPrizeToken)dropped).Amount *100);
				dropped.Delete();
				return true;
			}
			else if ( dropped is SilverPrizeToken )
			{
				AddToFund( from, ((SilverPrizeToken)dropped).Amount *10);
				dropped.Delete();
				return true;
			}
			else if ( dropped is BronzePrizeToken )
			{
				AddToFund( from, ((BronzePrizeToken)dropped).Amount);
				dropped.Delete();
				return true;
			}
			else
			{
				from.SendMessage( "You can only drop tokens into here." );
				return false;
			}
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			return OnDragDrop( from, item );
		}

		private void AddToFund( Mobile from, int amount )
		{
			bool skip = false;
			foreach ( TokenContributer contributer in TokenContributers )
				if ( contributer.Mobile == from )
				{
					contributer.Amount += amount;
					contributer.Date = DateTime.Now;
					skip = true;
					break;
				}
			if ( !skip )
				TokenContributers.Add( new TokenContributer( amount, from ) );
			Fund += amount;
			PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, false, String.Format( "{0} contributed with {1} worth of tokens. Thanks a lot!", from.Name, amount ) );
		}

		protected class TokenContributer
		{
			int m_Amount;
			Mobile m_TokenContributer;
			DateTime m_TokenContributionDate;
			public int Amount
			{
				get { return m_Amount; }
				set { m_Amount = value; }
			}

			public Mobile Mobile
			{
				get { return m_TokenContributer; }
				set { m_TokenContributer = value; }
			}

			public DateTime Date
			{
				get { return m_TokenContributionDate; }
				set { m_TokenContributionDate = value; }
			}

			public TokenContributer( int amount, Mobile contributer )
			{
				m_Amount = amount;
				m_TokenContributer = contributer;
				m_TokenContributionDate = DateTime.Now;
			}
		}

		private class TokenContributersGump : Gump
		{
			TokenFundRaiser m_TokenFundRaiser;
			Mobile m_Mobile;

			public TokenContributersGump( TokenFundRaiser raiser, Mobile from ) : base( 30, 30 )
			{
				m_TokenFundRaiser = raiser;
				m_Mobile = from;

				AddPage( 0 );
				AddBackground( 0, 0, 250, 360, 9270 );
				AddAlphaRegion( 0, 0, 250, 360 );

				m_TokenFundRaiser.TokenContributers.Sort( new TokenContributionComparer() );

				AddLabel( 50, 10, 2401, String.Format( "Top {0} Contributors", m_TokenFundRaiser.TokenContributers.Count > 20 ? 20 : m_TokenFundRaiser.TokenContributers.Count ) );

				for ( int i = 0; i < m_TokenFundRaiser.TokenContributers.Count && i < 20; i++ )
				{
					TokenContributer contributer = (TokenContributer)m_TokenFundRaiser.TokenContributers[i];
					AddLabel( 30, 30 + i * 15, 2401, (i + 1).ToString() );
					AddLabel( 50, 30 + i * 15, 2401, contributer.Mobile.Name );
					AddLabel( 170, 30 + i * 15, 2401, contributer.Amount.ToString() );
				}

				if ( m_Mobile.AccessLevel >= AccessLevel.GameMaster )
				{
					AddLabel( 280, 10, 2401, "Clear" );
					AddButton( 250, 10, 4023, 4024, 1, GumpButtonType.Reply, 0 );
                    AddLabel(280, 30, 2401, "Full List");
                    AddButton(250, 30, 4023, 4024, 2, GumpButtonType.Reply, 0);
                }
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile m = state.Mobile;
				if ( m_TokenFundRaiser == null || m == null || m_Mobile == null )
					return;
				int button = info.ButtonID;
				if ( button == 1 && m_Mobile.AccessLevel >= AccessLevel.GameMaster )
				{
					m_TokenFundRaiser.TokenContributers.Clear();
					m_Mobile.SendGump( new TokenContributersGump( m_TokenFundRaiser, m_Mobile ) );
				}
                if (button == 2 && m_Mobile.AccessLevel >= AccessLevel.GameMaster)
                {
                    m_Mobile.SendGump(new TokenContributersStaffGump(m_TokenFundRaiser.TokenContributers));
                }
            }

			private class TokenContributionComparer : IComparer
			{
				public int Compare( object a, object b )
				{
					if ( !( a is TokenContributer ) || !( b is TokenContributer ) )
						return 0;
					TokenContributer contributera = (TokenContributer)a;
					TokenContributer contributerb = (TokenContributer)b;
					if ( contributera.Amount > contributerb.Amount )
						return -1;
					else if ( contributera.Amount < contributerb.Amount )
						return 1;
					else if ( contributera.Date > contributerb.Date )
						return -1;
					else if ( contributera.Date < contributerb.Date )
						return 1;
					else
						return 0;
				}
			}
		}

        public class TokenContributersStaffGump : Gump
        {
            public static readonly int MaxEntriesPerPage = 30;
            private int m_Page;
            private ArrayList m_TokenContributers;

            public TokenContributersStaffGump(ArrayList contributers)
                : this(contributers, 0)
            {
            }

            public TokenContributersStaffGump(ArrayList contributers, int page)
                : base(500, 30)
            {
                m_Page = page;
                m_TokenContributers = contributers;

                AddImageTiled(0, 0, 300, 425, 0xA40);
                AddAlphaRegion(1, 1, 298, 423);

                AddHtml(10, 10, 280, 20, "<basefont color=#A0A0FF><center>Fund Raiser [Staff View]</center></basefont>", false, false);

                int lastPage = (m_TokenContributers.Count - 1) / MaxEntriesPerPage;

                string sLog;

                if (page < 0 || page > lastPage)
                {
                    sLog = "";
                }
                else
                {
                    int max = m_TokenContributers.Count - (lastPage - page) * MaxEntriesPerPage;
                    int min = Math.Max(max - MaxEntriesPerPage, 0);

                    StringBuilder builder = new StringBuilder();

                    for (int i = min; i < max; i++)
                    {
                        TokenContributer contributer = contributers[i] as TokenContributer;

                        if (contributer != null && contributer.Mobile != null && contributer.Mobile.Account != null)
                        {
                            if (i != min) builder.Append("<br>");
                            builder.AppendFormat("{0} (<i>{1}</i>): {2}", contributer.Mobile.Name, contributer.Mobile.Account, contributer.Amount);
                        }
                    }

                    sLog = builder.ToString();
                }

                AddHtml(10, 40, 280, 350, sLog, false, true);

                if (page > 0)
                    AddButton(10, 395, 0xFAE, 0xFB0, 1, GumpButtonType.Reply, 0); // Previous page

                AddLabel(45, 395, 0x481, String.Format("Current page: {0}/{1}", page + 1, lastPage + 1));

                if (page < lastPage)
                    AddButton(261, 395, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0); // Next page
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                switch (info.ButtonID)
                {
                    case 1: // Previous page
                        {
                            if (m_Page - 1 >= 0)
                                from.SendGump(new TokenContributersStaffGump(m_TokenContributers, m_Page - 1));

                            break;
                        }
                    case 2: // Next page
                        {
                            if ((m_Page + 1) * MaxEntriesPerPage < m_TokenContributers.Count)
                                from.SendGump(new TokenContributersStaffGump(m_TokenContributers, m_Page + 1));

                            break;
                        }
                }
            }
        }

	}
}