/**
*	Ladder system by Morten Legarth (c)taunted.dk ( legarth@taunted.dk )
*	Version: v0.10 -  26-02-2005
*
*	This system has been written for use at the Blitzkrieg frees-shard
*	http://blitzkrieg.dorijan.de . Unauthorized reproduction or redistribution
*	is prohibited.
*
*							LadderAreaControl.cs
*						-------------------------
*
*	File Description:	The Ladder Area Control is a modified version of
*						the Region Controller. Tailored for use in the
*						ladder system. It is used to set up areas in
*						which you can add arenas.
*
*
*/

using System;
using System.Collections;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Ladder
{

	public class LadderAreaControl : RegionControl
	{
		private ArrayList m_ArenaList;
		private Point3D m_GateSpot;

		public ArrayList Arenas
		{
			get { return m_ArenaList; }
			set { m_ArenaList = value; }
		}

		public Point3D GateSpot
		{
			get { return m_GateSpot; }
			set { m_GateSpot = value; }
		}


		[Constructable]
		public LadderAreaControl() : base()
		{
			Ladder.Arenas.Add(this);
			this.Name = "Ladder Area Controller";
			m_ArenaList = new ArrayList();
			this.ItemID = 5396;

			// Default flags
			this.PlayerLogoutDelay = TimeSpan.FromMinutes(5);
			this.AllowHousing = false;
			this.AllowSpawn = false;
			this.CanMountEthereal = true;
			this.CannotEnter = false;
			this.CanUseStuckMenu = true;
			this.ItemDecay = true;
			this.ShowEnterMessage = true;
			this.ShowExitMessage = true;
			this.CannotLootOwnCorpse = true;
			this.IsGuarded = false;
			this.AllowBenifitPlayer = true;
			this.AllowHarmPlayer = true;
			this.CanBeDamaged = true;
			this.CanHeal = true;
			this.CanRessurect = true;
			this.AllowBenifitNPC = true;
			this.AllowHarmNPC = true;
			this.CanLootPlayerCorpse = true;
			this.CanLootNPCCorpse = true;
			this.CanUsePotions = true;
			this.Priority = CustomRegionPriority.HighPriority;
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (m.AccessLevel >= AccessLevel.GameMaster)
			{

				m.CloseGump(typeof(LadderAreaControlGump));
				m.SendGump(new LadderAreaControlGump(this));

				m.CloseGump(typeof(RemoveAreaGump));
				m.SendGump(new RemoveAreaGump(this));
			}
		}

		public ArenaControl GetArena()
		{
			foreach (ArenaControl a in m_ArenaList)
			{
				if (a != null && !a.InUse)
					return a;
			}
			return null;
		}


		public LadderAreaControl(Serial serial) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)2); // version
			writer.WriteItemList(m_ArenaList);
            writer.Write(m_GateSpot);
        }

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_ArenaList = reader.ReadItemList();
            if (version == 2)
            {
                m_GateSpot = reader.ReadPoint3D();
            }

        }

		public override void OnDelete()
		{
			if (Ladder.Arenas.Contains(this))
				Ladder.Arenas.Remove(this);

			base.OnDelete();
		}
	}
}