using System;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class GameTeamSelector : Gump
	{
		private CTFGame m_Game;
		private int m_TeamSize;
		private bool m_RandomTeam;

		public GameTeamSelector( CTFGame game, bool randomTeam ) : this( game, game.TeamSize, randomTeam )
		{
		}

		public GameTeamSelector( CTFGame game, int teamSize, bool randomTeam ) : base( 50, 50 )
		{
			m_Game = game;
			m_TeamSize = teamSize;
			m_RandomTeam = randomTeam;

			//Closable = false;
			Dragable = false;

			AddPage( 0 );
			AddBackground( 0, 0, 250, 220, 5054 );
			AddBackground( 10, 10, 230, 200, 3000 );

			AddPage( 1 );
			AddLabel( 20, 20, 0, "Select a team:" );

			if ( m_RandomTeam )
			{
				AddButton( 20, 60, 4005, 4006, 1, GumpButtonType.Reply, 0 );
				AddLabel( 55, 60, 0, "Join Random Team" );
			}
			else
			{
				for (int i=0;i<m_Game.Teams.Count;i++)
				{
					CTFTeam team = (CTFTeam)m_Game.Teams[i];
					if ( team.ActiveMemberCount < m_TeamSize )
					{
						AddButton( 20, 60 + i*20, 4005, 4006, i+1, GumpButtonType.Reply, 0 );
						AddLabel( 55, 60 + i*20, 0, "Join Team " + team.Name );
					}
				}
			}

		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			from.CloseGump( typeof( GameTeamSelector ) );

			if ( !GameJoinStone.IsNaked( from ) )
			{
				from.SendMessage( "You must be naked to join." );
				return;
			}

			if ( m_Game.Deleted )
				return;

			if ( info.ButtonID == 0 )
				return;

			CTFTeam team = null;

			if ( m_RandomTeam )
			{
				team = m_Game.RandomTeam();
			}
			else
			{
				team = m_Game.GetTeam( info.ButtonID - 1 );
			}

			if ( team != null && team.ActiveMemberCount < m_TeamSize )
			{
				bool freeze = from.Frozen;

				from.Kill();
				if ( from.Corpse != null && !from.Corpse.Deleted )
					from.Corpse.Delete();
				from.Location = team.Home;
				from.Map = team.Map;
				from.Resurrect();

				from.Frozen = freeze;

				m_Game.SwitchTeams( from, team );

				from.SendMessage( "You have joined team {0}!", team.Name );
			}
			else
			{
				from.SendMessage( "That team is full, please try again." );
				from.SendGump( new GameTeamSelector( m_Game, m_RandomTeam ) );
			}
		}
	}

	public class GameJoinGump : Gump
	{
		private CTFGame m_Game;
		private bool m_RandomTeam;
		public GameJoinGump( CTFGame game, string gameName, bool randomTeam ) : base( 20, 30 )
		{
			m_Game = game;
			m_RandomTeam = randomTeam;

			AddPage( 0 );
			AddBackground( 0, 0, 550, 220, 5054 );
			AddBackground( 10, 10, 530, 200, 3000 );

			AddPage( 1 );
			AddLabel( 20, 20, 0, String.Format( "Welcome to {0}!", gameName ) );
			AddLabel( 20, 80, 0, "Bank your items before joining, supplies" );
			AddLabel( 20, 100, 0, "will be provided.  Enjoy!" );

			AddLabel( 55, 180, 0, "Cancel" );
			AddButton( 20, 180, 4005, 4006, 0, GumpButtonType.Reply, 0 );
			AddLabel( 165, 180, 0, "Okay, Join!" );
			AddButton( 130, 180, 4005, 4006, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			from.CloseGump( typeof( GameJoinGump ) );

			if ( info.ButtonID == 1 )
			{
				from.CloseGump( typeof( GameTeamSelector ) );
				from.SendGump( new GameTeamSelector( m_Game, m_RandomTeam ) );
			}
		}
	}
}