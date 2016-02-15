using System;
using System.Collections;
using Server;
using Server.Guilds;
using Server.Network;

namespace Server.Gumps
{
	public class ViewGuildRosterGump : GuildMobileListGump
	{
		public ViewGuildRosterGump( Mobile from, Guild guild ) : base( from, guild, false, guild.Members )
		{
		}

		protected override void Design()
		{
			AddButton( 20, 400, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtml( 55, 400, 300, 35, "Return to list", false, false );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
			{
				GuildGump.EnsureClosed( m_Mobile );
				m_Mobile.SendGump( new GuildWarInfoGump( m_Mobile, m_Guild ));
			}
		}
	}
}