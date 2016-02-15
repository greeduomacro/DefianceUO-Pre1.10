using System;
using System.Collections;
using Server.Accounting;
using Server.Mobiles;
using Server.Network;
using Server.Misc;
using Server.Prompts;
using Server.Targeting;

namespace Server.Gumps
{
	public class FriendListGump : Gump
	{
		Mobile m_Mobile;
		int m_Page;

		public FriendListGump( Mobile m ) : this( m, 0 )
		{
		}

		public FriendListGump( Mobile m, int page )
			: base( 20, 20 )
		{
			if ( m == null || !(m is PlayerMobile) )
				return;

			m_Mobile = m;
			m_Page = page;

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			ArrayList friends = FriendSystem.GetFriendList( m ).MutualFriends;

			this.AddPage(0);
			this.AddBackground(0, 0, 240, 465, 3600);
			AddAlphaRegion(14, 15, 210, 434);
			this.AddLabel(20, 15, 1359, String.Format( "Defiance friend list" ));
			AddButton( 160, 17, 2031, 2032, 9, GumpButtonType.Reply, 0 ); // help
			this.AddLabel(20, 39, 1359, String.Format( "Friends of {0}", m.Name ));
			this.AddLabel(20, 70, 1359, "Name");
			this.AddLabel(146, 70, 1359, "Tell");
			this.AddLabel(182, 70, 1359, "Remove");

			//next page
			if ( friends.Count - 8 * m_Page - 7 > 0 )
				AddButton(200, 44, 9903, 9905, 2, GumpButtonType.Reply, 0);

			//last page
			if ( m_Page > 0 )
				AddButton(180, 44, 9909, 9911, 1, GumpButtonType.Reply, 0);

			this.AddLabel(155, 420, 1359, "Close");
			this.AddButton(190, 420, 4023, 4025, 0, GumpButtonType.Reply, 0);
			this.AddLabel(20, 420, 1359, "Add Friend");
			this.AddButton(90, 420, 4014, 4016, 5, GumpButtonType.Reply, 0);

			for ( int i = 12 * m_Page; i >= 0 && i < friends.Count && 12 * m_Page + 12 - i > 0; i++ )
			{
				Mobile friend = (Mobile)friends[i];
				AddLabel( 20, 90 + 25 * (i % 12), friend.NetState != null ? 80 : 2401, friend.Name );
				AddButton( 150, 90 + 25 * (i % 12), 4014, 4016, 110 + i, GumpButtonType.Reply, 0 );
				AddButton( 185, 90 + 25 * (i % 12), 4017, 4019, 10 + i, GumpButtonType.Reply, 0 );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			if ( from == null )
				return;

			int button = info.ButtonID;

			switch ( button )
			{
				case 0:
					break;
				case 1:
					m_Mobile.SendGump( new FriendListGump( m_Mobile, m_Page - 1 ) );
					break;
				case 2:
					m_Mobile.SendGump( new FriendListGump( m_Mobile, m_Page + 1 ) );
					break;
				case 5:
				{
					m_Mobile.Target = new AddFriendTarget();
					m_Mobile.SendMessage( 133, "Target somebody to make them your friend." );
					break;
				}
				case 9:
				{
					m_Mobile.SendMessage( 133, "To send a message to a friend named Lolipop, type \"[FTell Lolipop Hello friend!\"" );
					m_Mobile.SendMessage( 133, "To write a player whom has spaces in his name, use _ instead of spaces as example: \"[FTell John_Adams I think you're cute!\"." );
                                        m_Mobile.SendMessage( 133, "You don't have to type your friend's whole name to send him a message. The first two letters of the name is enough.");
					m_Mobile.SendMessage( 133, "You cannot send tells to a player unless he/she is on your list." );
					m_Mobile.SendGump( new FriendListGump( m_Mobile ) );
					break;
				}
				default:
				{
					ArrayList friends = FriendSystem.GetFriendList( m_Mobile ).MutualFriends;
					int f = button - 10;
					if ( f >= 100 )
					{
						f -= 100;
						if ( f < 0 || f >= friends.Count )
							return;
						Mobile friend = (Mobile)friends[f];
						m_Mobile.SendMessage( 133, "Enter a message to send {0}.", friend.Name );
						m_Mobile.Prompt = new FriendTellPrompt( m_Mobile, friend );
					}
					else
					{
						if ( f < 0 || f >= friends.Count )
							return;
						FriendSystem.RemoveFriend( m_Mobile, (Mobile)friends[f] );
						m_Mobile.SendGump( new FriendListGump( m_Mobile ) );
					}
					break;
				}
			}
		}
	}

	public class AddFriendTarget : Target
	{
		public AddFriendTarget() : base( 15, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targ )
		{
			if ( !(targ is PlayerMobile) )
			{
				from.SendMessage( 133, "You need to target a player." );
			}
			else if ( (Mobile)targ == from )
			{
				from.SendMessage( 133, "You can't target yourself." );
			}
			else
			{
				if ( FriendSystem.AddFriend( from, (Mobile)targ ) )
				{
					from.SendMessage( 133, "You've added {0} to your friends. He/she will show on your friend list when he/she added you.", ((Mobile)targ).Name );
					((Mobile)targ).SendMessage( 133, "{0} added you to his/her friends. You can add him/her to by typing [FriendList.", from.Name );
				}
				else
					from.SendMessage( 133, "This player is already on your friends list" );
			}
			from.SendGump( new FriendListGump( from ) );
		}
	}

	public class FriendTellPrompt : Prompt
	{
		Mobile m_From;
		Mobile m_To;
		public FriendTellPrompt( Mobile from, Mobile to )
		{
			m_From = from;
			m_To = to;
		}

		public override void OnCancel( Mobile from )
		{
			from.SendGump( new FriendListGump( m_From ) );
		}

		public override void OnResponse( Mobile from, string text )
		{
			text = text.Trim();
			if ( text.Length > 0 && m_From != null && m_To != null )
				FriendSystem.Tell( m_From, m_To, text );

			from.SendGump( new FriendListGump( m_From ) );
		}

	}
}