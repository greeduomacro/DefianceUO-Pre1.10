using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using System.Collections;
using Server.Misc;

namespace Server.Gumps
{
	class BMStoneGump : Gump
	{
		private BomberManGame m_Game;

		public BMStoneGump(PlayerMobile from, BomberManGame game)
			: base(100, 100)
		{
			if (from == null || from.AccessLevel < AccessLevel.GameMaster ||game == null)
				return;

			m_Game = game;
			int y = 25;

			AddBackground(0, 0, 300, 270, 2600);
			AddLabel(45, y, 34, "Bomber Man Game");
			y += 30;
			AddLabel(45, y, 902, "Players signed in: " + m_Game.Participants.Count + "/" + m_Game.StartLocations.Count);
			y += 20;
			AddLabel(45, y, 902, "OpenJoin: "+m_Game.OpenJoin);

			y += 30;
			if (m_Game.OpenJoin)
				AddLabel(45, y, 902, "Close sign up");
			else
				AddLabel(45, y, 902, "Open sign up");
			AddButton(200, y, 2152, 2154, 4, GumpButtonType.Reply, 0);

			y += 30;
			AddLabel(45, y, 902, "Start game");
			AddButton(200, y, 2152, 2154, 1, GumpButtonType.Reply, 0);
			y += 30;
			AddLabel(45, y, 902, "End game");
			AddButton(200, y, 2152, 2154, 2, GumpButtonType.Reply, 0);

			if (m_Game.LastGameResults != null && m_Game.LastGameResults.Count > 0)
			{
				y += 30;
				AddLabel(45, y, 902, "Last Game Results");
				AddButton(200, y, 2152, 2154, 5, GumpButtonType.Reply, 0);
			}

			y += 30;
			AddLabel(45, y, 902, "Map Configuration");
			AddButton(200, y, 2152, 2154, 3, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (sender == null || sender.Mobile == null || sender.Mobile.Deleted ||
				info == null || sender.Mobile.AccessLevel < AccessLevel.GameMaster)
				return;

			Mobile from = (Mobile)sender.Mobile;

			switch (info.ButtonID)
			{
				case 1:
					if (m_Game.Running)
					{
						from.SendMessage("The game is already running.");
						return;
					}
					m_Game.StartGame(from);
					break;
				case 2:
					m_Game.EndGame(null);
					break;
				case 3:
					if (m_Game.Running == true)
					{
						from.SendMessage("No configuration while the game is running.");
						return;
					}
					from.SendGump(new BMMapSetupGump((PlayerMobile)from, m_Game));
					break;
				case 4:
					if (m_Game.Running == true)
					{
						from.SendMessage("No configuration while the game is running.");
						return;
					}
					m_Game.OpenJoin = !m_Game.OpenJoin;
					from.SendGump(new BMStoneGump((PlayerMobile)from, this.m_Game));
					break;
				case 5:
					from.CloseGump(typeof(BMResultGump));
					from.SendGump(new BMResultGump(m_Game.LastGameResults, true));
					break;
			}
		}
	}

	class BMMapSetupGump : Gump
	{
		private BomberManGame m_Game;

		public BMMapSetupGump(PlayerMobile from, BomberManGame game) : base(100, 100)
		{
			m_Game = game;

			if (from == null || from.Deleted || from.AccessLevel < AccessLevel.GameMaster)
				return;

			AddBackground(0, 0, 300, 530, 2600);
			int y = 25;

			AddLabel(45, y, 34, "Bomber Man Map Configuration");
			y += 30;

			AddLabel(45, y, 8, "Walls");
			y += 30;
			AddLabel(45, y, 902, "Save a  wall");
			AddButton(250, y, 2152, 2154, 1, GumpButtonType.Reply, 0);

			y += 30;
			AddLabel(45, y, 902, "Save all walls in arena");
			AddButton(250, y, 2152, 2154, 6, GumpButtonType.Reply, 0);

			y += 30;
			AddLabel(45, y, 902, "Remove saved wall");
			AddButton(250, y, 2152, 2154, 2, GumpButtonType.Reply, 0);
			y += 40;
			AddLabel(45, y, 902, "Show all walls");
			AddButton(250, y, 2152, 2154, 3, GumpButtonType.Reply, 0);
			y += 30;
			AddLabel(45, y, 902, "Remove all shown walls");
			AddButton(250, y, 2152, 2154, 4, GumpButtonType.Reply, 0);
			y += 40;
			AddLabel(45, y, 902, "Wipe all saved walls (!)");
			AddButton(250, y, 2152, 2154, 5, GumpButtonType.Reply, 0);

			y += 40;
			AddLabel(45, y, 8, "Start Locations");

			y += 30;
			AddLabel(45, y, 902, "Add start location");
			AddButton(250, y, 2152, 2154, 8, GumpButtonType.Reply, 0);

			y += 30;
			AddLabel(45, y, 902, "Remove start location");
			AddButton(250, y, 2152, 2154, 9, GumpButtonType.Reply, 0);

			y += 30;
			AddLabel(45, y, 902, "Show start locations");
			AddButton(250, y, 2152, 2154, 10, GumpButtonType.Reply, 0);

			y += 30;
			AddLabel(45, y, 902, "Wipe start locations (!)");
			AddButton(250, y, 2152, 2154, 11, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (sender == null || sender.Mobile == null || sender.Mobile.Deleted ||
				info == null || sender.Mobile.AccessLevel < AccessLevel.GameMaster ||
				m_Game.Running)
				return;

			Mobile from = (Mobile)sender.Mobile;

			switch (info.ButtonID)
			{
				case 1:
					if (m_Game.Running)
						return;
					from.SendMessage("Target the wall you want to save.");
					from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(SaveWall_OnTarget));
					from.SendGump(this);
					break;
				case 2:
					if (m_Game.Running)
						return;
					from.SendMessage("Target the saved wall you want to delete.");
					from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(RemoveSavedWall_OnTarget));
					from.SendGump(this);
					break;
				case 3:
					if (m_Game.Running)
						return;
					m_Game.PlaceWalls();
					from.SendGump(this);
					break;
				case 4:
					if (m_Game.Running)
						return;
					m_Game.RemoveWalls();
					from.SendGump(this);
					break;
				case 5:
					if (m_Game.Running)
						return;
					m_Game.WhipeWalls();
					from.SendGump(this);
					break;
				case 6:
					if (m_Game.Running)
						return;
					SaveWallsInArena();
					from.SendMessage("You saved the wall setup.");
					from.SendGump(this);
					break;
				case 8:
					if (m_Game.Running)
						return;
					from.SendMessage("Target a mobile that is standing on the new start location.");
					from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(AddStartLocation_OnTarget));
					from.SendGump(this);
					break;
				case 9:
					if (m_Game.Running)
						return;
					from.SendMessage("Target a start point mark (show start locations) to delete that start location.");
					from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(RemoveStartLocation_OnTarget));
					break;
				case 10:
					if (m_Game.Running)
						return;
					ShowStartLocatons(from);
					from.SendGump(this);
					break;
				case 11:
					if (m_Game.Running)
						return;
					WhipeStartLocations(from);
					from.SendGump(this);
					break;
			}
		}

		private void SaveWallsInArena()
		{
			IPooledEnumerable eable = null;
			ArrayList addedWalls = new ArrayList();
			foreach (Rectangle2D rec in m_Game.RegionControler.MyRegion.Coords)
			{
				eable = m_Game.RegionControler.MyRegion.Map.GetItemsInBounds(rec);
				foreach (Item wall in eable)
				{
					if (wall is BMwall && !m_Game.WallCoordinates.Contains(wall.Location))
					{
						m_Game.WallCoordinates.Add(wall.Location);
						addedWalls.Add((BMwall)wall);
					}
				}
			}
			foreach (BMwall wall in addedWalls)
				wall.Delete();

			if (eable != null)
				eable.Free();
		}

		public void RemoveStartLocation_OnTarget(Mobile from, object o)
		{
			if (from == null || o == null)
				return;

			Item mark = o as Item;

			if (mark == null || mark.ItemID == 4810)
			{
				if (m_Game.StartLocations.Contains(mark.Location))
				{
					m_Game.StartLocations.Remove(mark.Location);
					from.SendMessage("You deleted a start location.");
					mark.Delete();
				}
			}
			else
			{
				from.SendMessage("This is not a start location mark.");
			}

		}

		public void SaveWall_OnTarget(Mobile from, object o)
		{
			if (from == null || !(from is Mobile) || o == null ||
				m_Game == null || m_Game.WallCoordinates == null)
				return;

			BMwall wall = o as BMwall;
			if (wall == null)
			{
				from.SendMessage("This is not a BMwall.");
				return;
			}

			bool IsInRegion = false;
			// XXX optimize to contains
			foreach (Rectangle2D rectangle in m_Game.RegionControler.MyRegion.Coords)
			{
				if (rectangle.Contains(wall.Location))
					IsInRegion = true;
			}
			if (!IsInRegion)
			{
				from.SendMessage("This wall is not in the game area.");
				return;
			}

			if (m_Game.WallCoordinates.Contains(wall.Location))
			{
				from.SendMessage("A wall was saved already on that position.");
				return;
			}

			m_Game.WallCoordinates.Add(wall.Location);
			wall.Delete();
			from.SendMessage("You successfully saved a wall.");
		}

		public void RemoveSavedWall_OnTarget(Mobile from, object o)
		{
			if (from == null || !(from is Mobile) || o == null ||
				m_Game == null || m_Game.WallCoordinates == null)
				return;

			BMwall wall = o as BMwall;
			if (wall == null)
			{
				from.SendMessage("This is not a BMwall.");
				return;
			}

			if (m_Game.WallCoordinates.Contains(wall.Location))
			{
				m_Game.WallCoordinates.Remove(wall.Location);
				wall.Delete();
				from.SendMessage("You successfully deleted a saved wall.");
			}
			else
			{
				from.SendMessage("This is not a saved wall.");
			}
		}

		public void AddStartLocation_OnTarget(Mobile from, object o)
		{
			if (from == null || !(from is Mobile) || o == null ||
				m_Game == null || m_Game.StartLocations == null)
				return;

			Mobile mob = o as Mobile;
			if (mob == null)
				return;

			if (m_Game.StartLocations.Contains(mob.Location))
			{
				from.SendMessage("That is already a startpoint.");
				return;
			}
			else
			{
				m_Game.StartLocations.Add(mob.Location);
				from.SendMessage("You successfully added a start location.");
			}
		}

		public void ShowStartLocatons(Mobile from)
		{
			if (m_Game == null)
				return;

			ArrayList markers = new ArrayList();

			foreach (Point3D loc in m_Game.StartLocations)
			{
				Item marker = new Item(4810);
				marker.Hue = 34;
				marker.Movable = false;
				marker.MoveToWorld(loc, m_Game.RegionControler.MyRegion.Map);
				markers.Add(marker);
				Timer.DelayCall( TimeSpan.FromSeconds(5), new TimerStateCallback(RemoveLocationMarkers_Callback), markers);
			}
		}

		public void WhipeStartLocations(Mobile from)
		{
			if (m_Game == null || from == null)
				return;

			if (m_Game.Running)
			{
				from.SendMessage("You cant do that while the game is running.");
				return;
			}

			m_Game.StartLocations.Clear();
		}

		public void RemoveLocationMarkers_Callback(object o)
		{
			if (o == null || !(o is ArrayList))
				return;

			ArrayList markers = o as ArrayList;

			foreach (Item marker in markers)
				marker.Delete();

			markers.Clear();
		}
	}

	public class BMGump : Gump
	{
		public BMGump(int x, int y)	: base(x, y)
		{
		}

		public void AddBlackAlpha(int x, int y, int width, int height)
		{
			AddImageTiled(x, y, width, height, 2624);
			AddAlphaRegion(x, y, width, height);
		}
	}


	class BMResultGump : BMGump
	{
		private PlayerMobile m_winner;

		public BMResultGump(ArrayList playerstats, bool staffrequest) : base(100,100)
		{
			if (playerstats == null || playerstats.Count == 0)
				return;

			int i = playerstats.Count;
			int y = 105;

			AddBlackAlpha(0, 0, 425, 110+(20*i));
			AddHtml(12, 13, 400, 25, "<center>Game Results</center>", (bool)true, (bool)false);

			AddHtml(12, 80, 400, 25, "Name             Kills  UpgradesTaken  WallsDestroyed", (bool)true, (bool)false);

			foreach (BMPlayerScore stat in playerstats)
			{
				if (stat.Owner != null && !stat.Owner.Deleted)
				{
					if (stat.Wins == 1)
						m_winner = stat.Owner;
					AddLabel(16, y, stat.Hue, ""+stat.Owner.Name);
					AddLabel(155, y, stat.Hue, ""+stat.Kills);
					AddLabel(195, y, stat.Hue, ""+stat.GimmicksTaken);
					AddLabel(295, y, stat.Hue, ""+stat.DestroyedWalls);
					y += 20;
				}
			}

			if (m_winner != null)
			{
				AddLabel(15, 47, 34, "Winner: " + m_winner.Name);
				if (staffrequest)
				{
					AddLabel(265, 47, 902, "Get");
					AddButton(290, 45, 4005, 4006, 2, GumpButtonType.Reply, 0);

					AddLabel(335, 47, 902, "Go to");
					AddButton(375, 45, 4005, 4006, 1, GumpButtonType.Reply, 0);
				}
			}
			else
				AddLabel(15, 47, 34, "This was a draw.");
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (sender == null || sender.Mobile.AccessLevel < AccessLevel.GameMaster || info == null || m_winner == null)
				return;

			switch (info.ButtonID)
			{
				case 1:
					sender.Mobile.MoveToWorld(m_winner.Location, m_winner.Map);
					sender.Mobile.SendMessage("You were moved to the winner of the last game.");
					break;
				case 2:
					m_winner.MoveToWorld(sender.Mobile.Location, sender.Mobile.Map);
					sender.Mobile.SendMessage("You moved the winner of the last game to your position.");
					break;
			}
		}

	}

	public class BMscoreboardgump : BMGump
	{
		// expects an ordered arraylist of BMPlayerScores to show
		public BMscoreboardgump(ArrayList scores) : base(100,100)
		{
			if (scores == null)
				return;

			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);

			AddBlackAlpha(0, 0, 490, 400);
			AddHtml(10, 10, 470, 25, "<center>Bomberman scores</center>", (bool)true, (bool)false);

			int y = 40;
			int x = 15;
			int rank = 0;
			int headcolor = 53;
			int scoreshue_equal = 48;
			int scoreshue_unequal = 243;

			AddLabel(x, y, headcolor, "Rank");
			AddLabel(x + 40, y, headcolor, "Name");
			AddLabel(x + 170, y, headcolor, "Guild");
			AddLabel(x + 210, y, headcolor, "Games");
			AddLabel(x + 260, y, headcolor, "Wins");
			AddLabel(x + 310, y, headcolor, "Kills");
			AddLabel(x + 360, y, headcolor, "Walls");
			AddLabel(x + 410, y, headcolor, "Upgrades");

			y += 5;

			foreach (BMPlayerScore ps in scores)
			{
				if (ps.Owner == null || ps.Owner.Deleted)
					// TODO: delete row vom db
					continue;

				int hue;
				if ((rank++ % 2) == 0)
					hue = scoreshue_equal;
				else
					hue = scoreshue_unequal;

				y += 16;

				AddLabel(x, y, hue, "" + rank);
				AddLabel(x + 40, y, hue, "" + ps.Owner.Name);

				if (ps.Owner.Guild != null)
					AddLabel(x + 170, y, hue, "" + ps.Owner.Guild.Abbreviation);
				else
					AddLabel(x + 170, y, hue, "n/a");

				AddLabel(x + 210, y, hue, "" + ps.GamesPlayed);
				AddLabel(x + 260, y, hue, "" + ps.Wins);
				AddLabel(x + 310, y, hue, "" + ps.Kills);
				AddLabel(x + 360, y, hue, "" + ps.DestroyedWalls);
				AddLabel(x + 410, y, hue, "" + ps.GimmicksTaken);
			}
		}
	}
}