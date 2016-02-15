using System;
using Server;
using Server.Gumps;
using System.Collections;
using Server.Network;
using Server.Targeting;

namespace Server.Guilds
{
	public class GuildWarInfoGump : Gump
	{
		public static void Initialize()
		{
			Commands.Register( "GInfo", AccessLevel.GameMaster, new CommandEventHandler( GuildWarInfo_OnCommand ) );
		}
		[Usage( "GInfo" )]
		[Description( "Gets guild information of target. If you are guildmaster, you can chose to war the target's guild." )]
		private static void GuildWarInfo_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new InternalTarget();
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					Mobile to = targeted as Mobile;
					if ( !to.Player )
						from.SendMessage("You must target a player.");
					else if ( to.Guild != null )
						from.SendGump( new GuildWarInfoGump( from, (to.Guild as Guild) ) );
					else
						from.SendMessage("This player is not in a guild" );
				}
				else
					from.SendMessage("You must target a player.");
			}
		}

		private Mobile m_Mobile;
		private Guild m_ThierGuild;
		private Guild m_Guild;

		public GuildWarInfoGump( Mobile from, Guild guild ): base( 20, 30 )
		{
			m_Mobile = from;
			m_Guild = from.Guild as Guild;
			m_ThierGuild = guild;

			if ( m_ThierGuild == null )
				return;

			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			m_Mobile.CloseGump( typeof (GuildWarInfoGump));

			AddPage( 0 );

			AddBackground(0, 0, 280, 240, 5054);
			AddBackground(10, 10, 260, 220, 3000);

			AddLabel( 80, 20, 0, "Guild War Information" );
			AddLabel( 20,  60, 0, "Name");
			AddLabel( 20, 80, 0, "Type");
			AddLabel( 20, 100, 0, "Enemies");
			AddLabel( 20, 120, 0, "Members");
			AddLabel( 40, 140, 0, "View Members");

			AddLabel( 125, 60, 0, m_ThierGuild.Name);
			AddLabel( 125, 80, 0, m_ThierGuild.Type.ToString());
			AddLabel( 125, 100, 0, m_ThierGuild.Enemies.Count.ToString());
			AddLabel( 125, 120, 0, m_ThierGuild.Members.Count.ToString());
			AddButton( 20, 140, 2117, 2118, 1, GumpButtonType.Reply, 0);

			if ( m_Guild != null && m_Guild != m_ThierGuild && (m_Guild.Leader == m_Mobile || m_Mobile.AccessLevel >= AccessLevel.GameMaster) )
			{
				AddButton( 20, 160, 2117, 2118, 2, GumpButtonType.Reply, 0);
				AddLabel( 40, 160, 0, "Declare War");
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			int cat = info.ButtonID;

			switch ( cat )
			{
				case 0:
					return;
				case 1:
				{
					GuildGump.EnsureClosed( m_Mobile );
					m_Mobile.SendGump( new ViewGuildRosterGump( m_Mobile, m_ThierGuild ));
					return;
				}
				case 2:
				{
					if ( m_Guild == null )
						m_Mobile.SendMessage("You are not in a guild");
					else if ( m_ThierGuild == null )
						m_Mobile.SendMessage("You have selected an invalid guild");
					else if ( m_Guild == m_ThierGuild )
						m_Mobile.SendMessage("You cannot declare war with your own guild");
					else if ( m_Guild.Leader != m_Mobile )
						m_Mobile.SendMessage("You are not the leader of this guild");
					else if ( (m_Guild.WarInvitations.Contains( m_ThierGuild ) && m_ThierGuild.WarDeclarations.Contains( m_Guild )) || m_Guild.IsWar( m_ThierGuild ) )
						m_Mobile.SendMessage("You are already at war with that guild.");
					else
					{
						if ( !m_Guild.WarDeclarations.Contains( m_ThierGuild ) )
						{
							m_Guild.WarDeclarations.Add( m_ThierGuild );
							m_Guild.GuildMessage( 1018019, "{0} ({1})", m_ThierGuild.Name, m_ThierGuild.Abbreviation ); // Guild Message: Your guild has sent an invitation for war:
						}

						if ( !m_ThierGuild.WarInvitations.Contains( m_Guild ) )
						{
							m_ThierGuild.WarInvitations.Add( m_Guild );
							m_ThierGuild.GuildMessage( 1018021, "{0} ({1})", m_Guild.Name, m_Guild.Abbreviation ); // Guild Message: Your guild has received an invitation to war:
						}
					}
					break;
				}
			}
			m_Mobile.SendGump( new GuildWarInfoGump( m_Mobile, m_Guild ) );
		}
	}
}