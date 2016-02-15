using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Guilds;
using Server.Factions;
using Server.FSPvpPointSystem;

namespace Server.Gumps
{
	public class PvpStatGump : Gump
	{
		private Mobile m_Target;

		public PvpStatGump( Mobile target ) : base( 0, 0 )
		{
			if ( target == null )
				return;
			m_Target = target;

			FSPvpSystem.PvpStats ps = FSPvpSystem.GetPvpStats( target );

			Closable = false;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			string msg = "Viewing " + target.Name + "'s Pvp Record.";

			AddPage(0);

			AddBackground(26, 23, 456, 222, 9250);
			AddAlphaRegion(39, 38, 427, 28);
			AddAlphaRegion(39, 83, 283, 146);
			AddHtml( 39, 38, 427, 28, "<BASEFONT COLOR=WHITE><CENTER>" + msg + "</CENTER></BASEFONT>", (bool)false, (bool)false);
			AddButton(330, 85, 4005, 4006, 1, GumpButtonType.Page, 1);
			AddButton(330, 115, 4005, 4006, 2, GumpButtonType.Page, 2);
			AddButton(330, 145, 4005, 4006, 3, GumpButtonType.Page, 3);
			AddButton(330, 175, 4005, 4006, 0, GumpButtonType.Reply, 0);
			AddLabel(365, 85, 1149, @"Player Info");
			AddLabel(365, 115, 1149, @"Battle History");
			AddLabel(365, 145, 1149, @"Rank Info");
			AddLabel(365, 175, 1149, @"Close");
			//AddLabel(333, 205, 64, @"FSPvp (DFI Mod) v1.1");

			AddPage(1);

			AddLabel(45, 90, 1149, @"Name: " + ps.Owner.Name.ToString() );
			AddLabel(45, 110, 1149, @"ST Murders: " + ps.Owner.ShortTermMurders.ToString() );
			AddLabel(45, 130, 1149, @"LT Murders: " + ps.Owner.Kills.ToString() );

			Guild g = target.Guild as Guild;

			if ( g != null )
			{
				AddLabel(45, 150, 1149, @"Guild: " + g.Name.ToString() );
				AddLabel(45, 170, 1149, @"Guild Abr: " + g.Abbreviation.ToString() );
			}
			else
			{
				AddLabel(45, 150, 1149, @"Guild: N/A" );
				AddLabel(45, 170, 1149, @"Guild Abr: N/A" );
			}

			PlayerState pl = PlayerState.Find( target );

			if ( pl != null )
			{
				Faction faction = pl.Faction;
				AddLabel(45, 190, 1149, @"Faction: " + faction.Definition.FriendlyName.ToString() );
			}
			else
			{
				AddLabel(45, 190, 1149, @"Faction: N/A");
			}

			AddPage(2);

			AddLabel(45, 90, 1149, @"Points: " + ps.Points.ToString() );
			AddLabel(45, 110, 1149, @"Wins: " + ps.Wins.ToString() );
			AddLabel(45, 130, 1149, @"Losses: " + ps.Loses.ToString() );
			AddLabel(45, 150, 1149, @"Res-Kills: " + ps.ResKills.ToString() );
			AddLabel(45, 170, 1149, @"Res-Killed: " + ps.ResKilled.ToString() );

			int totalFought = ps.Wins + ps.Loses;
			AddLabel(45, 190, 1149, @"Total Battles Fought: " + totalFought.ToString() );

			AddPage( 3 );

			PvpRankInfo rank = PvpRankInfo.GetInfo( ps.RankType );

			AddLabel(45, 90, 1149, @"Rank: " + rank.Rank.ToString() );
			AddLabel(45, 110, 1149, @"Rank Title: " + rank.Title );
			AddLabel(45, 130, 1149, @"Rank Abr: " + rank.Abbreviation );
			AddLabel( 45, 150, 1149, String.Format( @"Points Till Next Rank: {0}", rank.Rank >= PvpRankInfo.MaxRank ? "N/A" : ( rank.Required - ps.Points ).ToString() ) );
		}
	}
}