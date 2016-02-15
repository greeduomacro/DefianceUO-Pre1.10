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
	public class LadderAreaControlGump : Gump
	{
		LadderAreaControl m_Controller;
		public LadderAreaControlGump(LadderAreaControl r)	: base( 25, 25 )
		{
			m_Controller = r;

			Closable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			AddBackground(23, 32, 412, 336, 9270);
			AddAlphaRegion(19, 29, 418, 443);
			AddButton(55, 46, 5569, 5570, (int)Buttons.SpellButton, GumpButtonType.Reply, 0);
			AddButton(55, 128, 5581, 5582, (int)Buttons.SkillButton, GumpButtonType.Reply, 0);
			AddButton(50, 205, 7006, 7006, (int)Buttons.AreaButton, GumpButtonType.Reply, 0);
			AddButton(50, 285, 227, 227, (int)Buttons.GateLocButton, GumpButtonType.Reply, 0);

			AddLabel(152, 70, 1152, "Edit Restricted Spells");
			AddLabel(152, 153, 1152, "Edit Restricted Skills");
			AddLabel(152, 234, 1152, "Add Region Area");
			AddLabel(152, 314, 1152, "Set Gate Location" +"(" + m_Controller.GateSpot.ToString() + ")");
			AddImage(353, 54, 3953);
			AddImage(353, 180, 3955);

		}

		public enum Buttons
		{
			SpellButton = 1,
			SkillButton,
			AreaButton,
			GateLocButton
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
						//m_Controller.SendRestrictGump( m, RestrictType.Spells );
						m.CloseGump(typeof(SpellRestrictGump));
						m.SendGump(new SpellRestrictGump(m_Controller.RestrictedSpells));

						m.CloseGump(typeof(RegionControlGump));
						m.SendGump(new RegionControlGump(m_Controller));
						break;
					}
				case 2:
					{
						//m_Controller.SendRestrictGump( m, RestrictType.Skills );

						m.CloseGump(typeof(SkillRestrictGump));
						m.SendGump(new SkillRestrictGump(m_Controller.RestrictedSkills));

						m.CloseGump(typeof(LadderAreaControlGump));
						m.SendGump(new LadderAreaControlGump(m_Controller));
						break;
					}
				case 3:
					{
						m.CloseGump(typeof(LadderAreaControlGump));
						m.SendGump(new LadderAreaControlGump(m_Controller));

						m.CloseGump(typeof(RemoveAreaGump));
						m.SendGump(new RemoveAreaGump(m_Controller));

						m_Controller.ChooseArea(m);
						break;
					}
				case 4:
					{
						m.CloseGump(typeof(LadderAreaControlGump));
						m.SendGump(new LadderAreaControlGump(m_Controller));

						m.CloseGump(typeof(RemoveAreaGump));
						m.SendGump(new RemoveAreaGump(m_Controller));
						m.Target = new GateLocTarget(m_Controller);
						break;
					}
			}
		}
	}
	public class GateLocTarget : Target
	{
		private LadderAreaControl m_LAC;

		public GateLocTarget(LadderAreaControl store) : base( -1, true, TargetFlags.None )
		{
			this.m_LAC = store;
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (m_LAC == null)
				return;

			IPoint3D p = o as IPoint3D;

			if (p != null)
			{
				if (p is Item)
					p = ((Item)p).GetWorldTop();
				else if (p is Mobile)
					p = ((Mobile)p).Location;


				m_LAC.GateSpot = new Point3D(p);

			}
		}
	}
}