/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							ArenaControlGump.cs
*						-------------------------
*
*	File Description:	This gump is the inteface for the
*						Arena Controller.
*
*
*/
using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;
using Server.Network;

namespace Server.Ladder
{
	public class ArenaControlGump : Gump
	{
		ArenaControl m_Controller;
		public ArenaControlGump(ArenaControl r)	: base( 25, 25 )
		{
			m_Controller = r;

			Closable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			AddBackground(23, 32, 412, 376, 9270);
			AddAlphaRegion(19, 29, 418, 373);
			AddButton(55, 50, 227, 227, (int)Buttons.Location1Button, GumpButtonType.Reply, 0);
			AddButton(55, 120, 227, 227, (int)Buttons.Location2Button, GumpButtonType.Reply, 0);
			AddButton(55, 190, 227, 227, (int)Buttons.GateLocButton, GumpButtonType.Reply, 0);
			AddButton(55, 260, 234, 234, (int)Buttons.AreaButton, GumpButtonType.Reply, 0);
			AddButton(55, 330, 236, 236, (int)Buttons.UpdateArenaButton, GumpButtonType.Reply, 0);

			AddLabel(152, 70, 1152, "Add Start Location Player 1" + "(" + m_Controller.StartLoc1.ToString() + ")");
			AddLabel(152, 140, 1152, "Add Start Location Player 2" + "(" + m_Controller.StartLoc2.ToString() + ")");
			AddLabel(152, 210, 1152, "Add location for Exit Gate" + "(" + m_Controller.GateLoc.ToString() + ")");
			AddLabel(152, 280, 1152, "Add Arena Area");
			AddLabel(152, 350, 1152, "Add Arena to Ladder Area");
			//AddImage(353, 54, 3953);
			//AddImage(353, 180, 3955);

		}

		public enum Buttons
		{
			Location1Button = 1,
			Location2Button,
			GateLocButton,
			AreaButton,
			UpdateArenaButton
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (m_Controller == null || m_Controller.Deleted)
				return;

			Mobile m = sender.Mobile;

			switch (info.ButtonID)
			{
				case 1:
					{
						m.Target = new StartLocTarget(m_Controller, info.ButtonID);

						m.CloseGump(typeof(ArenaControlGump));
						m.SendGump(new ArenaControlGump(m_Controller));
						break;
					}
				case 2:
					{
						m.Target = new StartLocTarget(m_Controller, info.ButtonID);

						m.CloseGump(typeof(ArenaControlGump));
						m.SendGump(new ArenaControlGump(m_Controller));
						break;
					}
				case 3:
					{
						m.Target = new StartLocTarget(m_Controller, info.ButtonID);

						m.CloseGump(typeof(ArenaControlGump));
						m.SendGump(new ArenaControlGump(m_Controller));
						break;
					}
				case 4:
					{
						m.CloseGump(typeof(ArenaControlGump));
						m.SendGump(new ArenaControlGump(m_Controller));

						m.CloseGump(typeof(RemoveAreaGump));

						m.SendGump(new RemoveAreaGump(m_Controller));

						m_Controller.ChooseArea(m);
						break;
					}
				case 5:
					{
						if (m_Controller.StartLoc1 == Point3D.Zero)
						{
							m.SendMessage("Cannot update: Player 1 start location must be set.");
						}
						else if (m_Controller.StartLoc2 == Point3D.Zero)
						{
							m.SendMessage("Cannot update: Player 2 start location must be set.");
						}
						else if (m_Controller.GateLoc == Point3D.Zero)
						{
							m.SendMessage("Cannot update: Gate location must be set.");
						}
						else if (m_Controller.Coords.Count == 0)
						{
							m.SendMessage("Cannot update: Atleast one area must be set.");
						}
						else
						{
							m.SendMessage("Select a Ladder Area Controler to link.");
							m.Target = new LadderAreaTarget(m_Controller);
						}
						break;
					}
			}
		}
	}

	public class LadderAreaTarget : Target
	{
		private ArenaControl m_Ac;

		public LadderAreaTarget(ArenaControl store) : base( -1, true, TargetFlags.None )
		{
			this.m_Ac = store;

		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (o != null && m_Ac != null && o is LadderAreaControl)
			{
				LadderAreaControl control = (LadderAreaControl)o;
				control.Arenas.Add(m_Ac);
				m_Ac.LAC = control;
				from.SendMessage("The arena is added to the Ladder Area Controller");

			}
			else
				from.SendMessage("You must target a valid Ladder Area Controller");

		}
	}

	public class StartLocTarget : Target
	{
		private ArenaControl m_Ac;
		int m_LocID;
		public StartLocTarget(ArenaControl store, int locID) : base( -1, true, TargetFlags.None )
		{
			this.m_Ac = store;
			this.m_LocID = locID;
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (m_Ac == null)
				return;

			IPoint3D p = o as IPoint3D;

			if (p != null)
			{
				if (p is Item)
					p = ((Item)p).GetWorldTop();
				else if (p is Mobile)
					p = ((Mobile)p).Location;

				if (m_LocID == 1)
				{
					m_Ac.StartLoc1 = new Point3D(p);
				}
				else if (m_LocID == 2)
				{
					m_Ac.StartLoc2 = new Point3D(p);
				}
				else if (m_LocID == 3)
				{
					m_Ac.GateLoc = new Point3D(p);
				}

			}
		}
	}
}